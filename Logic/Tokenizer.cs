using System;
using System.Collections.Generic;
using System.Linq;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic
{
    public class Tokenizer
    {
        readonly string input;

        internal Tokenizer(string input)
        {
            this.input = input;
        }

        public static IEnumerable<Token> Tokenize(string input)
        {
            return new Tokenizer(input).Tokenize();
        }

        IEnumerable<Token> Tokenize()
        {
            var tokens = GetTokens();
            return tokens;
        }

        internal IEnumerable<Token> GetTokens()
        {
            return input
                .Select((character, index) => GetToken(character, index));
        }

        Token GetToken(char character, int index)
        {
            var data = character.ToString();
            return new LiteralToken(data, index);
        }
    }
}