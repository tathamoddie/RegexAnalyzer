using System.Collections.Generic;
using System.Linq;

namespace TathamOddie.RegexAnalyzer.Logic
{
    public class Tokenizer
    {
        readonly IEnumerable<TokenizerRule> tokenizerRules;
        readonly IDictionary<TokenizerState, TokenizerState> oneHitStates;
        readonly string input;

        internal Tokenizer(string input)
        {
            var groupingConstructs = new[]
            {
                TokenizerState.GroupContents,
                TokenizerState.GroupContentsStart,
                TokenizerState.ConditionalExpressionContents
            };

            tokenizerRules = new[]
            {
                // ( starts a group
                new TokenizerRule(
                    groupingConstructs, "(",
                    TokenType.GroupStart, TokenizerStateChange.PushState(TokenizerState.GroupContentsStart)),

                // ) closes a group
                new TokenizerRule(
                    groupingConstructs, ")",
                    TokenType.GroupEnd, TokenizerStateChange.RetainState),

                // ? after ( is the start of a group directive
                new TokenizerRule(
                    TokenizerState.GroupContentsStart, "?",
                    TokenType.GroupDirectiveStart, TokenizerStateChange.ReplaceState(TokenizerState.GroupDirectiveContents)),

                // : after (? is a non-capturing group marker
                new TokenizerRule(
                    TokenizerState.GroupDirectiveContents, ":",
                    TokenType.NonCapturingGroupMarker, TokenizerStateChange.PopState),

                // = after (? is a non-capturing group marker
                new TokenizerRule(
                    TokenizerState.GroupDirectiveContents, "=",
                    TokenType.PositiveLookAheadMarker, TokenizerStateChange.PopState),

#region Conditional Expressions

                // ( after (? is the start of a conditional expression
                new TokenizerRule(
                    TokenizerState.GroupDirectiveContents, "(",
                    TokenType.ConditionalExpressionStart, TokenizerStateChange.ReplaceState(TokenizerState.ConditionalExpressionPredicate)),

                // a-z, 0-9 are valid group identifier characters
                new TokenizerRule(
                    TokenizerState.ConditionalExpressionPredicate, TokenizerRule.LetterAndNumberData,
                    TokenType.Literal, TokenizerStateChange.RetainState),

                // ) ends a conditional expression
                new TokenizerRule(
                    TokenizerState.ConditionalExpressionPredicate, ")",
                    TokenType.ConditionalExpressionEnd, TokenizerStateChange.ReplaceState(TokenizerState.ConditionalExpressionContents)),

                // | divides conditional expression content
                new TokenizerRule(
                    TokenizerState.ConditionalExpressionContents, "|",
                    TokenType.OrOperator, TokenizerStateChange.RetainState),

#endregion

#region Group Names

                // < after (? is the start of a group identifier
                new TokenizerRule(
                    TokenizerState.GroupDirectiveContents, "<",
                    TokenType.NamedIdentifierStart, TokenizerStateChange.ReplaceState(TokenizerState.NamedIdentifier)),

                // a-z, 0-9 are valid group identifier characters
                new TokenizerRule(
                    TokenizerState.NamedIdentifier, TokenizerRule.LetterAndNumberData,
                    TokenType.Literal, TokenizerStateChange.RetainState),

                // > ends a named identifier
                new TokenizerRule(
                    TokenizerState.NamedIdentifier, ">",
                    TokenType.NamedIdentifierEnd, TokenizerStateChange.PopState),

#endregion

#region Group Options

                // a-z after (? is an option
                new TokenizerRule(
                    TokenizerState.GroupDirectiveContents, TokenizerRule.LetterData,
                    TokenType.GroupOption, TokenizerStateChange.ReplaceState(TokenizerState.GroupOption)),

                // + after (? is an option qualifier
                new TokenizerRule(
                    TokenizerState.GroupDirectiveContents, "+",
                    TokenType.GroupOptionQualifier, TokenizerStateChange.ReplaceState(TokenizerState.GroupOption)),

                // - after (? is an option qualifier
                new TokenizerRule(
                    TokenizerState.GroupDirectiveContents, "-",
                    TokenType.GroupOptionQualifier, TokenizerStateChange.ReplaceState(TokenizerState.GroupOption)),

                // a-z after (?[a-z]+ is another option
                new TokenizerRule(
                    TokenizerState.GroupOption, TokenizerRule.LetterData,
                    TokenType.GroupOption, TokenizerStateChange.RetainState),

                // + after (?[a-z]+ is an option qualifier
                new TokenizerRule(
                    TokenizerState.GroupOption, "+",
                    TokenType.GroupOptionQualifier, TokenizerStateChange.RetainState),

                // - after (?[a-z]+ is an option qualifier
                new TokenizerRule(
                    TokenizerState.GroupOption, "-",
                    TokenType.GroupOptionQualifier, TokenizerStateChange.RetainState),

                // : after (?[a-z]+ is the end of the options
                new TokenizerRule(
                    TokenizerState.GroupOption, ":",
                    TokenType.GroupOptionEnd, TokenizerStateChange.PopState),

#endregion

                // \ starts an escape sequence
                new TokenizerRule(
                    groupingConstructs, @"\",
                    TokenType.CharacterEscapeMarker, TokenizerStateChange.PushState(TokenizerState.EscapedCharacter)),

                // Anything immediately after \ is escaped data
                new TokenizerRule(
                    TokenizerState.EscapedCharacter, TokenizerRule.AnyData,
                    TokenType.CharacterEscapeData, TokenizerStateChange.PopState),

                // | is an 'or' operator
                new TokenizerRule(
                    groupingConstructs, "|",
                    TokenType.OrOperator, TokenizerStateChange.RetainState),

                // Basic quantifiers
                new TokenizerRule(
                    groupingConstructs, new[] { "*", "+", "?" },
                    TokenType.Quantifier, TokenizerStateChange.RetainState),

                // Strign end assertion
                new TokenizerRule(
                    groupingConstructs, new[] { "$" },
                    TokenType.EndOfStringAssertion, TokenizerStateChange.RetainState),

                // Whatever is left is a literal
                new TokenizerRule(
                    groupingConstructs, TokenizerRule.AnyData,
                    TokenType.Literal, TokenizerStateChange.RetainState)
            };

            // As soon as the state in the key is hit once, it is replaced by the state in the value
            oneHitStates = new Dictionary<TokenizerState, TokenizerState>
            {
                { TokenizerState.GroupContentsStart, TokenizerState.GroupContents }
            };

            this.input = input;
        }

        public static IEnumerable<Token> Tokenize(string input)
        {
            return new Tokenizer(input).Tokenize();
        }

        internal IEnumerable<Token> Tokenize()
        {
            var tokens = GetTokens();
            tokens = ReduceTokens(tokens);
            return tokens;
        }

        internal IEnumerable<Token> GetTokens()
        {
            var index = 0;
            
            var states = new Stack<TokenizerState>();
            states.Push(TokenizerState.GroupContents);

            foreach (var character in input)
            {
                var token = GetToken(character.ToString(), index, states);
                yield return token;
                
                if (token.Type == TokenType.ParseFailure)
                    break;

                index++;
            }
        }

        Token GetToken(string data, int index, Stack<TokenizerState> states)
        {
            var currentState = states.Peek();
            var rule = tokenizerRules
                .Where(r => r.ApplicableStates.Contains(currentState))
                .Where(r => r.ApplicableData == null || r.ApplicableData.Contains(data))
                .FirstOrDefault();

            if (rule == null)
                return new Token(TokenType.ParseFailure, data, index);

            // If this state is only meant to be hit once,
            // automatically transition to the corresponding state
            // for further parsing
            if (oneHitStates.ContainsKey(currentState))
            {
                states.Pop();
                states.Push(oneHitStates[currentState]);
            }

            rule.StateChange(states);

            return new Token(rule.Type, data, index);
        }

        internal static IEnumerable<Token> ReduceTokens(IEnumerable<Token> tokens)
        {
            var typesToCombine = new[] {TokenType.Literal, TokenType.Quantifier, TokenType.GroupOption};

            var tokenQueue = new Queue<Token>(tokens);

            while (tokenQueue.Any())
            {
                var currentToken = tokenQueue.Dequeue();

                if (typesToCombine.Contains(currentToken.Type))
                {
                    var combinedData = tokenQueue
                    .DequeueWhile(t => t.Type == currentToken.Type)
                    .Aggregate(currentToken.Data, (current, token) => current + token.Data);

                    yield return new Token(currentToken.Type, combinedData, currentToken.StartIndex);
                }
                else
                {
                    yield return currentToken;
                }
            }
        }
    }
}