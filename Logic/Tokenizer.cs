using System.Collections.Generic;
using System.Linq;

namespace TathamOddie.RegexAnalyzer.Logic
{
    public class Tokenizer
    {
        readonly IEnumerable<TokenizerRule> tokenizerRules;
        readonly string input;

        internal Tokenizer(string input)
        {
            tokenizerRules = new[]
            {
                // ( starts a group
                new TokenizerRule(
                    new[] { TokenizerState.GroupContents, TokenizerState.GroupContentsStart }, "(",
                    TokenType.GroupStart, TokenizerStateChange.PushState(TokenizerState.GroupContentsStart)),

                // ) closes a group
                new TokenizerRule(
                    new[] { TokenizerState.GroupContents, TokenizerState.GroupContentsStart }, ")",
                    TokenType.GroupEnd, TokenizerStateChange.RetainState),

                // ? after ( is the start of a group directive
                new TokenizerRule(
                    TokenizerState.GroupContentsStart, "?",
                    TokenType.GroupDirectiveStart, TokenizerStateChange.ReplaceState(TokenizerState.GroupDirectiveContents)),

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

                // a-z after (? is an option
                new TokenizerRule(
                    TokenizerState.GroupDirectiveContents, TokenizerRule.LetterData,
                    TokenType.GroupOption, TokenizerStateChange.ReplaceState(TokenizerState.GroupOption)),

                // a-z after (?[a-z]+ is another option
                new TokenizerRule(
                    TokenizerState.GroupOption, TokenizerRule.LetterData,
                    TokenType.GroupOption, TokenizerStateChange.RetainState),

                // : after (?[a-z]+ is the end of the options
                new TokenizerRule(
                    TokenizerState.GroupOption, ":",
                    TokenType.GroupOptionEnd, TokenizerStateChange.PopState),

                // Whatever is left is a literal
                new TokenizerRule(
                    new[] { TokenizerState.GroupContents, TokenizerState.GroupContentsStart }, TokenizerRule.AnyData,
                    TokenType.Literal, TokenizerStateChange.RetainState)
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

            rule.StateChange(states);

            return new Token(rule.Type, data, index);
        }

        internal static IEnumerable<Token> ReduceTokens(IEnumerable<Token> tokens)
        {
            var tokenQueue = new Queue<Token>(tokens);

            while (tokenQueue.Any())
            {
                var currentToken = tokenQueue.Dequeue();

                switch (currentToken.Type)
                {
                    case TokenType.Literal:
                        var data = tokenQueue
                            .DequeueWhile(t => t.Type == currentToken.Type)
                            .Aggregate(currentToken.Data, (current, token) => current + token.Data);

                        yield return new Token(currentToken.Type, data, currentToken.StartIndex);
                        break;

                    default:
                        yield return currentToken;
                        break;
                }
            }
        }
    }
}