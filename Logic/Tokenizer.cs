using System.Collections.Generic;
using System.Linq;

namespace TathamOddie.RegexAnalyzer.Logic
{
    public class Tokenizer
    {
        readonly IEnumerable<TokenizerRule> tokenizerRules = new TokenizerRuleSet();
        readonly IDictionary<TokenizerState, TokenizerState> oneHitStates;
        readonly string input;

        internal Tokenizer(string input)
        {
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