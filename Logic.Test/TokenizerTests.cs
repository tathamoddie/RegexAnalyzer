using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TathamOddie.RegexAnalyzer.Logic.Test
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeLiteralCharacter()
        {
            // Arrange
            const string input = "a";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeMultipleLiteralCharacters()
        {
            // Arrange
            const string input = "abc";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Literal, "b", 1),
                    new Token(TokenType.Literal, "c", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeEscapeSequence()
        {
            // Arrange
            const string input = @"ab\+c";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Literal, "b", 1),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 2),
                    new Token(TokenType.CharacterEscapeData, "+", 3),
                    new Token(TokenType.Literal, "c", 4)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeGroupDirectiveStart()
        {
            // Arrange
            const string input = "(?";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldReturnParseFailureWhenClosingGroupImmediatelyAfterGroupDirectiveStartToken()
        {
            // Arrange
            const string input = "(?)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ParseFailure, ")", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeGroup()
        {
            // Arrange
            const string input = "(foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.Literal, "f", 1),
                    new Token(TokenType.Literal, "o", 2),
                    new Token(TokenType.Literal, "o", 3),
                    new Token(TokenType.GroupEnd, ")", 4)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNonCapturingGroup()
        {
            // Arrange
            const string input = "(?:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.NonCapturingGroupMarker, ":", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.GroupEnd, ")", 6)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNamedGroup()
        {
            // Arrange
            const string input = "(?<foo>bar)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.NamedIdentifierStart, "<", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.NamedIdentifierEnd, ">", 6),
                    new Token(TokenType.Literal, "b", 7),
                    new Token(TokenType.Literal, "a", 8),
                    new Token(TokenType.Literal, "r", 9),
                    new Token(TokenType.GroupEnd, ")", 10)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOption()
        {
            // Arrange
            const string input = "(?m:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOption, "m", 2),
                    new Token(TokenType.GroupOptionEnd, ":", 3),
                    new Token(TokenType.Literal, "f", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.GroupEnd, ")", 7)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeMultipleOptions()
        {
            // Arrange
            const string input = "(?mis:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOption, "m", 2),
                    new Token(TokenType.GroupOption, "i", 3),
                    new Token(TokenType.GroupOption, "s", 4),
                    new Token(TokenType.GroupOptionEnd, ":", 5),
                    new Token(TokenType.Literal, "f", 6),
                    new Token(TokenType.Literal, "o", 7),
                    new Token(TokenType.Literal, "o", 8),
                    new Token(TokenType.GroupEnd, ")", 9)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNestedOptions()
        {
            // Arrange
            const string input = "(?m:(?i:(?s:foo)))";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOption, "m", 2),
                    new Token(TokenType.GroupOptionEnd, ":", 3),
                    new Token(TokenType.GroupStart, "(", 4),
                    new Token(TokenType.GroupDirectiveStart, "?", 5),
                    new Token(TokenType.GroupOption, "i", 6),
                    new Token(TokenType.GroupOptionEnd, ":", 7),
                    new Token(TokenType.GroupStart, "(", 8),
                    new Token(TokenType.GroupDirectiveStart, "?", 9),
                    new Token(TokenType.GroupOption, "s", 10),
                    new Token(TokenType.GroupOptionEnd, ":", 11),
                    new Token(TokenType.Literal, "f", 12),
                    new Token(TokenType.Literal, "o", 13),
                    new Token(TokenType.Literal, "o", 14),
                    new Token(TokenType.GroupEnd, ")", 15),
                    new Token(TokenType.GroupEnd, ")", 16),
                    new Token(TokenType.GroupEnd, ")", 17)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeNegativeQualifiedOption()
        {
            // Arrange
            const string input = "(?-m:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOptionQualifier, "-", 2),
                    new Token(TokenType.GroupOption, "m", 3),
                    new Token(TokenType.GroupOptionEnd, ":", 4),
                    new Token(TokenType.Literal, "f", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.Literal, "o", 7),
                    new Token(TokenType.GroupEnd, ")", 8)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizePositiveQualifiedOption()
        {
            // Arrange
            const string input = "(?+m:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOptionQualifier, "+", 2),
                    new Token(TokenType.GroupOption, "m", 3),
                    new Token(TokenType.GroupOptionEnd, ":", 4),
                    new Token(TokenType.Literal, "f", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.Literal, "o", 7),
                    new Token(TokenType.GroupEnd, ")", 8)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeMultipleQualifiedOptions()
        {
            // Arrange
            const string input = "(?+m-si:foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.GroupOptionQualifier, "+", 2),
                    new Token(TokenType.GroupOption, "m", 3),
                    new Token(TokenType.GroupOptionQualifier, "-", 4),
                    new Token(TokenType.GroupOption, "s", 5),
                    new Token(TokenType.GroupOption, "i", 6),
                    new Token(TokenType.GroupOptionEnd, ":", 7),
                    new Token(TokenType.Literal, "f", 8),
                    new Token(TokenType.Literal, "o", 9),
                    new Token(TokenType.Literal, "o", 10),
                    new Token(TokenType.GroupEnd, ")", 11)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOnePartConditional()
        {
            // Arrange
            const string input = "(?(foo)bar)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ConditionalExpressionStart, "(", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 6),
                    new Token(TokenType.Literal, "b", 7),
                    new Token(TokenType.Literal, "a", 8),
                    new Token(TokenType.Literal, "r", 9),
                    new Token(TokenType.GroupEnd, ")", 10)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOnePartConditionalWithNestedNamedGroup()
        {
            // Arrange
            const string input = "(?(foo)(?<bar>baz))";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ConditionalExpressionStart, "(", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 6),
                    new Token(TokenType.GroupStart, "(", 7),
                    new Token(TokenType.GroupDirectiveStart, "?", 8),
                    new Token(TokenType.NamedIdentifierStart, "<", 9),
                    new Token(TokenType.Literal, "b", 10),
                    new Token(TokenType.Literal, "a", 11),
                    new Token(TokenType.Literal, "r", 12),
                    new Token(TokenType.NamedIdentifierEnd, ">", 13),
                    new Token(TokenType.Literal, "b", 14),
                    new Token(TokenType.Literal, "a", 15),
                    new Token(TokenType.Literal, "z", 16),
                    new Token(TokenType.GroupEnd, ")", 17),
                    new Token(TokenType.GroupEnd, ")", 18)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeTwoPartConditional()
        {
            // Arrange
            const string input = "(?(foo)bar|baz)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ConditionalExpressionStart, "(", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 6),
                    new Token(TokenType.Literal, "b", 7),
                    new Token(TokenType.Literal, "a", 8),
                    new Token(TokenType.Literal, "r", 9),
                    new Token(TokenType.OrOperator, "|", 10),
                    new Token(TokenType.Literal, "b", 11),
                    new Token(TokenType.Literal, "a", 12),
                    new Token(TokenType.Literal, "z", 13),
                    new Token(TokenType.GroupEnd, ")", 14)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeTwoPartConditionalWithNestedNamedGroups()
        {
            // Arrange
            const string input = "(?(foo)(?<bar>baz)|(?<bar>baz))";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.ConditionalExpressionStart, "(", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.ConditionalExpressionEnd, ")", 6),
                    new Token(TokenType.GroupStart, "(", 7),
                    new Token(TokenType.GroupDirectiveStart, "?", 8),
                    new Token(TokenType.NamedIdentifierStart, "<", 9),
                    new Token(TokenType.Literal, "b", 10),
                    new Token(TokenType.Literal, "a", 11),
                    new Token(TokenType.Literal, "r", 12),
                    new Token(TokenType.NamedIdentifierEnd, ">", 13),
                    new Token(TokenType.Literal, "b", 14),
                    new Token(TokenType.Literal, "a", 15),
                    new Token(TokenType.Literal, "z", 16),
                    new Token(TokenType.GroupEnd, ")", 17),
                    new Token(TokenType.OrOperator, "|", 18),
                    new Token(TokenType.GroupStart, "(", 19),
                    new Token(TokenType.GroupDirectiveStart, "?", 20),
                    new Token(TokenType.NamedIdentifierStart, "<", 21),
                    new Token(TokenType.Literal, "b", 22),
                    new Token(TokenType.Literal, "a", 23),
                    new Token(TokenType.Literal, "r", 24),
                    new Token(TokenType.NamedIdentifierEnd, ">", 25),
                    new Token(TokenType.Literal, "b", 26),
                    new Token(TokenType.Literal, "a", 27),
                    new Token(TokenType.Literal, "z", 28),
                    new Token(TokenType.GroupEnd, ")", 29),
                    new Token(TokenType.GroupEnd, ")", 30)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizePositiveLookAhead()
        {
            // Arrange
            const string input = "(?=foo)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.GroupDirectiveStart, "?", 1),
                    new Token(TokenType.PositiveLookAheadMarker, "=", 2),
                    new Token(TokenType.Literal, "f", 3),
                    new Token(TokenType.Literal, "o", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.GroupEnd, ")", 6)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOrOperator()
        {
            // Arrange
            const string input = "cat|dog|tiger";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "c", 0),
                    new Token(TokenType.Literal, "a", 1),
                    new Token(TokenType.Literal, "t", 2),
                    new Token(TokenType.OrOperator, "|", 3),
                    new Token(TokenType.Literal, "d", 4),
                    new Token(TokenType.Literal, "o", 5),
                    new Token(TokenType.Literal, "g", 6),
                    new Token(TokenType.OrOperator, "|", 7),
                    new Token(TokenType.Literal, "t", 8),
                    new Token(TokenType.Literal, "i", 9),
                    new Token(TokenType.Literal, "g", 10),
                    new Token(TokenType.Literal, "e", 11),
                    new Token(TokenType.Literal, "r", 12)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOrOperatorInGroup()
        {
            // Arrange
            const string input = "(cat|dog|tiger)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.Literal, "c", 1),
                    new Token(TokenType.Literal, "a", 2),
                    new Token(TokenType.Literal, "t", 3),
                    new Token(TokenType.OrOperator, "|", 4),
                    new Token(TokenType.Literal, "d", 5),
                    new Token(TokenType.Literal, "o", 6),
                    new Token(TokenType.Literal, "g", 7),
                    new Token(TokenType.OrOperator, "|", 8),
                    new Token(TokenType.Literal, "t", 9),
                    new Token(TokenType.Literal, "i", 10),
                    new Token(TokenType.Literal, "g", 11),
                    new Token(TokenType.Literal, "e", 12),
                    new Token(TokenType.Literal, "r", 13),
                    new Token(TokenType.GroupEnd, ")", 14)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeZeroOrMoreQuantifier()
        {
            // Arrange
            const string input = "a*";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "*", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeZeroOrMoreLazyQuantifier()
        {
            // Arrange
            const string input = "a*?";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "*", 1),
                    new Token(TokenType.Quantifier, "?", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeZeroOrOneQuantifier()
        {
            // Arrange
            const string input = "a?";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "?", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeZeroOrOneQuantifierInGroup()
        {
            // Arrange
            const string input = "(a?)";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupStart, "(", 0),
                    new Token(TokenType.Literal, "a", 1),
                    new Token(TokenType.Quantifier, "?", 2),
                    new Token(TokenType.GroupEnd, ")", 3)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOneOrMoreQuantifier()
        {
            // Arrange
            const string input = "a+";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "+", 1)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_GetTokens_ShouldTokenizeOneOrMoreLazyQuantifier()
        {
            // Arrange
            const string input = "a+?";

            // Act
            var result = new Tokenizer(input).GetTokens();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.Quantifier, "+", 1),
                    new Token(TokenType.Quantifier, "?", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleConsecutiveLiteralTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.Literal, "b", 1),
                new Token(TokenType.Literal, "c", 2)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "abc", 0)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldNotCombineMultipleConsecutiveGroupEndTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.GroupEnd, "a", 0),
                new Token(TokenType.GroupEnd, "b", 1),
                new Token(TokenType.GroupEnd, "c", 2)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.GroupEnd, "a", 0),
                    new Token(TokenType.GroupEnd, "b", 1),
                    new Token(TokenType.GroupEnd, "c", 2)
                },
                result.ToArray()
            );
        }

        [TestMethod]
        public void Tokenizer_ReduceTokens_ShouldCombineMultipleConsecutiveLiteralAndQuantifierTokens()
        {
            // Arrange
            var input = new[]
            {
                new Token(TokenType.Literal, "a", 0),
                new Token(TokenType.GroupStart, "(", 1),
                new Token(TokenType.GroupStart, "(", 2),
                new Token(TokenType.Literal, "b", 3),
                new Token(TokenType.Literal, "c", 4),
                new Token(TokenType.Quantifier, "*", 5),
                new Token(TokenType.Quantifier, "?", 6),
                new Token(TokenType.Literal, "d", 7),
                new Token(TokenType.GroupEnd, ")", 8),
                new Token(TokenType.GroupEnd, ")", 9),
                new Token(TokenType.Literal, "e", 10)
            };

            // Act
            var result = Tokenizer.ReduceTokens(input);

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new Token(TokenType.Literal, "a", 0),
                    new Token(TokenType.GroupStart, "(", 1),
                    new Token(TokenType.GroupStart, "(", 2),
                    new Token(TokenType.Literal, "bc", 3),
                    new Token(TokenType.Quantifier, "*?", 5),
                    new Token(TokenType.Literal, "d", 7),
                    new Token(TokenType.GroupEnd, ")", 8),
                    new Token(TokenType.GroupEnd, ")", 9),
                    new Token(TokenType.Literal, "e", 10)
                },
                result.ToArray()
            );
        }

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
                    new Token(TokenType.NamedIdentifierStart, "<", 20),
                    new Token(TokenType.Literal, "param", 21),
                    new Token(TokenType.NamedIdentifierEnd, ">", 26),
                    new Token(TokenType.Literal, @"url", 27),
                    new Token(TokenType.CharacterEscapeMarker, @"\", 30),
                    new Token(TokenType.CharacterEscapeData, "(", 31),
                    new Token(TokenType.GroupEnd, ")", 32),
                    new Token(TokenType.Quantifier, "?", 33),
                    new Token(TokenType.GroupStart, "(", 34),
                    new Token(TokenType.GroupDirectiveStart, "?", 35),
                    new Token(TokenType.NamedIdentifierStart, "<", 36),
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
    }
}