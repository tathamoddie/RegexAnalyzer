using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Test.Tokens
{
    [TestClass]
    public class TokenizerComplexTests
    {
        [TestMethod]
        public void Tokenizer_Tokenize_ShouldTokenizeComplexExpression1()
        {
            // Arrange
            const string input = @"(?+im-s:@import\s+(?<param>url\()?(?<path>.*?)(?(param)\)|)(;|(?=\s*?$)))";

            // Act
            var result = Tokenizer.Tokenize(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOptionQualifier, "+", 2),
                    new Token(TokenType.GroupOption, "im", 3),
                    new Token(TokenType.GroupOptionQualifier, "-", 5),
                    new Token(TokenType.GroupOption, "s", 6),
                    new Token(TokenType.GroupOptionEnd, ":", 7),
                    new Token(TokenType.Literal, "@import", 8),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 15),
                    new Token(TokenType.CharacterEscapeData, "s", 16),
                    new Token(TokenType.Quantifier, "+", 17),
                    new Token(TokenType.GroupStart, "(", 18),
                    new Token(TokenType.GroupDirectiveStart, "?", 19),
                    new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 20),
                    new Token(TokenType.Literal, "param", 21),
                    new Token(TokenType.NamedIdentifierEnd, ">", 26),
                    new Token(TokenType.Literal, @"url", 27),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 30),
                    new Token(TokenType.CharacterEscapeData, "(", 31),
                    new Token(TokenType.GroupEnd, ")", 32),
                    new Token(TokenType.Quantifier, "?", 33),
                    new Token(TokenType.GroupStart, "(", 34),
                    new Token(TokenType.GroupDirectiveStart, "?", 35),
                    new Token(TokenType.NamedIdentifierStartOrLookBehindMarker, "<", 36),
                    new Token(TokenType.Literal, "path", 37),
                    new Token(TokenType.NamedIdentifierEnd, ">", 41),
                    new Token(TokenType.Literal, ".", 42),
                    new Token(TokenType.Quantifier, "*?", 43),
                    new Token(TokenType.GroupEnd, ")", 45),
                    new Token(TokenType.GroupStart, "(", 46),
                    new Token(TokenType.GroupDirectiveStart, "?", 47),
                    new Token(TokenType.ConditionalExpressionStart, "(", 48),
                    new Token(TokenType.Literal, "param", 49),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 54),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 55),
                    new Token(TokenType.CharacterEscapeData, ")", 56),
                    new Token(TokenType.OrOperator, "|", 57),
                    new Token(TokenType.GroupEnd, ")", 58),
                    new Token(TokenType.GroupStart, "(", 59),
                    new Token(TokenType.Literal, ";", 60),
                    new Token(TokenType.OrOperator, "|", 61),
                    new Token(TokenType.GroupStart, "(", 62),
                    new Token(TokenType.GroupDirectiveStart, "?", 63),
                    new Token(TokenType.PositiveLookAheadMarker, "=", 64),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 65),
                    new Token(TokenType.CharacterEscapeData, "s", 66),
                    new Token(TokenType.Quantifier, "*?", 67),
                    new Token(TokenType.EndOfStringAssertion, "$", 69),
                    new Token(TokenType.GroupEnd, ")", 70),
                    new Token(TokenType.GroupEnd, ")", 71),
                    new Token(TokenType.GroupEnd, ")", 72)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_Tokenize_ShouldTokenizeComplexExpression2()
        {
            // Arrange
            const string input = @"^[\d]+([.]\d{0,2})?$";

            // Act
            var result = Tokenizer.Tokenize(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.StartOfStringAssertion, "^", 0),
                    new Token(TokenType.CharacterSetStart, "[", 1),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 2),
                    new Token(TokenType.CharacterEscapeData, "d", 3),
                    new Token(TokenType.CharacterSetEnd, "]", 4),
                    new Token(TokenType.Quantifier, "+", 5),
                    new Token(TokenType.GroupStart, "(", 6),
                    new Token(TokenType.CharacterSetStart, "[", 7),
                    new Token(TokenType.Literal, ".", 8),
                    new Token(TokenType.CharacterSetEnd, "]", 9),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 10),
                    new Token(TokenType.CharacterEscapeData, "d", 11),
                    new Token(TokenType.ParametizedQuantifierStart, "{", 12),
                    new Token(TokenType.Number, "0", 13),
                    new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 14),
                    new Token(TokenType.Number, "2", 15),
                    new Token(TokenType.ParametizedQuantifierEnd, @"}", 16),
                    new Token(TokenType.GroupEnd, ")", 17),
                    new Token(TokenType.Quantifier, "?", 18),
                    new Token(TokenType.EndOfStringAssertion, "$", 19)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_Tokenize_ShouldTokenizeComplexExpression3()
        {
            // Arrange
            const string input = @"(?ni:^[\w\d]*?(?=\d*$))";

            // Act
            var result = Tokenizer.Tokenize(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOption, "ni", 2),
                    new Token(TokenType.GroupOptionEnd, ":", 4),
                    new Token(TokenType.StartOfStringAssertion, "^", 5),
                    new Token(TokenType.CharacterSetStart, "[", 6),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 7),
                    new Token(TokenType.CharacterEscapeData, "w", 8),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 9),
                    new Token(TokenType.CharacterEscapeData, "d", 10),
                    new Token(TokenType.CharacterSetEnd, "]", 11),
                    new Token(TokenType.Quantifier, "*?", 12),
                    new Token(TokenType.GroupStart, "(", 14),
                    new Token(TokenType.GroupDirectiveStart, "?", 15),
                    new Token(TokenType.PositiveLookAheadMarker, "=", 16),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 17),
                    new Token(TokenType.CharacterEscapeData, "d", 18),
                    new Token(TokenType.Quantifier, "*", 19),
                    new Token(TokenType.EndOfStringAssertion, "$", 20),
                    new Token(TokenType.GroupEnd, ")", 21),
                    new Token(TokenType.GroupEnd, ")", 22)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_Tokenize_ShouldTokenizeComplexExpression4()
        {
            // Arrange
            const string input = @"^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$";

            // Act
            var result = Tokenizer.Tokenize(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.StartOfStringAssertion, "^", 0),
                    new Token(TokenType.CharacterSetStart, "[", 1),
                    new Token(TokenType.Literal, "_a-zA-Z0-9-", 2),
                    new Token(TokenType.CharacterSetEnd, "]", 13),
                    new Token(TokenType.Quantifier, "+", 14),
                    new Token(TokenType.GroupStart, "(", 15),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 16),
                    new Token(TokenType.CharacterEscapeData, ".", 17),
                    new Token(TokenType.CharacterSetStart, "[", 18),
                    new Token(TokenType.Literal, "_a-zA-Z0-9-", 19),
                    new Token(TokenType.CharacterSetEnd, "]", 30),
                    new Token(TokenType.Quantifier, "+", 31),
                    new Token(TokenType.GroupEnd, ")", 32),
                    new Token(TokenType.Quantifier, "*", 33),
                    new Token(TokenType.Literal, "@", 34),
                    new Token(TokenType.CharacterSetStart, "[", 35),
                    new Token(TokenType.Literal, "a-zA-Z0-9-", 36),
                    new Token(TokenType.CharacterSetEnd, "]", 46),
                    new Token(TokenType.Quantifier, "+", 47),
                    new Token(TokenType.GroupStart, "(", 48),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 49),
                    new Token(TokenType.CharacterEscapeData, ".", 50),
                    new Token(TokenType.CharacterSetStart, "[", 51),
                    new Token(TokenType.Literal, "a-zA-Z0-9-", 52),
                    new Token(TokenType.CharacterSetEnd, "]", 62),
                    new Token(TokenType.Quantifier, "+", 63),
                    new Token(TokenType.GroupEnd, ")", 64),
                    new Token(TokenType.Quantifier, "*", 65),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 66),
                    new Token(TokenType.CharacterEscapeData, ".", 67),
                    new Token(TokenType.GroupStart, "(", 68),
                    new Token(TokenType.GroupStart, "(", 69),
                    new Token(TokenType.CharacterSetStart, "[", 70),
                    new Token(TokenType.Literal, "0-9", 71),
                    new Token(TokenType.CharacterSetEnd, "]", 74),
                    new Token(TokenType.ParametizedQuantifierStart, "{", 75),
                    new Token(TokenType.Number, "1", 76),
                    new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 77),
                    new Token(TokenType.Number, "3", 78),
                    new Token(TokenType.ParametizedQuantifierEnd, "}", 79),
                    new Token(TokenType.GroupEnd, ")", 80),
                    new Token(TokenType.OrOperator, "|", 81),
                    new Token(TokenType.GroupStart, "(", 82),
                    new Token(TokenType.CharacterSetStart, "[", 83),
                    new Token(TokenType.Literal, "a-zA-Z", 84),
                    new Token(TokenType.CharacterSetEnd, "]", 90),
                    new Token(TokenType.ParametizedQuantifierStart, "{", 91),
                    new Token(TokenType.Number, "2", 92),
                    new Token(TokenType.ParametizedQuantifierRangeSeparator, ",", 93),
                    new Token(TokenType.Number, "3", 94),
                    new Token(TokenType.ParametizedQuantifierEnd, "}", 95),
                    new Token(TokenType.GroupEnd, ")", 96),
                    new Token(TokenType.OrOperator, "|", 97),
                    new Token(TokenType.GroupStart, "(", 98),
                    new Token(TokenType.Literal, "aero", 99),
                    new Token(TokenType.OrOperator, "|", 103),
                    new Token(TokenType.Literal, "coop", 104),
                    new Token(TokenType.OrOperator, "|", 108),
                    new Token(TokenType.Literal, "info", 109),
                    new Token(TokenType.OrOperator, "|", 113),
                    new Token(TokenType.Literal, "museum", 114),
                    new Token(TokenType.OrOperator, "|", 120),
                    new Token(TokenType.Literal, "name", 121),
                    new Token(TokenType.GroupEnd, ")", 125),
                    new Token(TokenType.GroupEnd, ")", 126),
                    new Token(TokenType.EndOfStringAssertion, "$", 127)
                },
                result.ToArray()
            );
        }
    }
}
