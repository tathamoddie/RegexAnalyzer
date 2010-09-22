using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic.Tokens
{
    class TokenizerRuleSet : List<TokenizerRule>
    {
        public TokenizerRuleSet()
        {
            var groupingConstructs = new[]
            {
                TokenizerState.GroupContents,
                TokenizerState.GroupContentsStart,
                TokenizerState.ConditionalExpressionContents
            };

            AddRange(BuildGroupRules(groupingConstructs));
            AddRange(BuildConditionalExpressionRules());
            AddRange(BuildGroupNameAndLookBehindRules());
            AddRange(BuildGroupOptionRules());
            AddRange(BuildCharacterSetRules(groupingConstructs));
            AddRange(BuildQuantifierRules(groupingConstructs));
            AddRange(BuildBasicExpressionRules(groupingConstructs));
        }

        static IEnumerable<TokenizerRule> BuildGroupRules(IEnumerable<TokenizerState> groupingConstructs)
        {
            // ( starts a group
            yield return new TokenizerRule(
                groupingConstructs, "(",
                TokenType.GroupStart, StateChange<TokenizerState>.PushState(TokenizerState.GroupContentsStart));

            // ) closes a group
            yield return new TokenizerRule(
                groupingConstructs, ")",
                TokenType.GroupEnd, StateChange<TokenizerState>.RetainState);

            // ? after ( is the start of a group directive
            yield return new TokenizerRule(
                TokenizerState.GroupContentsStart, "?",
                TokenType.GroupDirectiveStart, StateChange<TokenizerState>.ReplaceState(TokenizerState.GroupDirectiveContents));

            // # after (? is the start of an inline comment
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "#",
                TokenType.CommentStart, StateChange<TokenizerState>.ReplaceState(TokenizerState.InlineComment));

            // ) ends an inline comment
            yield return new TokenizerRule(
                TokenizerState.InlineComment, ")",
                TokenType.GroupEnd, StateChange<TokenizerState>.PopState);

            // anything other than a ) is part of the inline comment
            yield return new TokenizerRule(
                TokenizerState.InlineComment, TokenizerRule.AnyData,
                TokenType.Literal, StateChange<TokenizerState>.RetainState);

            // : after (? is a non-capturing group marker
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, ":",
                TokenType.NonCapturingGroupMarker, StateChange<TokenizerState>.PopState);

            // = after (? is a positive look ahead
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "=",
                TokenType.PositiveLookAheadMarker, StateChange<TokenizerState>.PopState);

            // ! after (? is a negative look ahead
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "!",
                TokenType.NegativeLookAheadMarker, StateChange<TokenizerState>.PopState);

            // > after (? is a non-backtracking subexpression marker
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, ">",
                TokenType.NonBacktrackingSubexpressionMarker, StateChange<TokenizerState>.PopState);
        }

        static IEnumerable<TokenizerRule> BuildConditionalExpressionRules()
        {
            // ( after (? is the start of a conditional expression
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "(",
                TokenType.ConditionalExpressionStart, StateChange<TokenizerState>.ReplaceState(TokenizerState.ConditionalExpressionPredicate));

            // a-z, 0-9 are valid group identifier characters
            yield return new TokenizerRule(
                TokenizerState.ConditionalExpressionPredicate, TokenizerRule.LetterAndNumberData,
                TokenType.Literal, StateChange<TokenizerState>.RetainState);

            // ) ends a conditional expression
            yield return new TokenizerRule(
                TokenizerState.ConditionalExpressionPredicate, ")",
                TokenType.ConditionalExpressionEnd, StateChange<TokenizerState>.ReplaceState(TokenizerState.ConditionalExpressionContents));

            // | divides conditional expression content
            yield return new TokenizerRule(
                TokenizerState.ConditionalExpressionContents, "|",
                TokenType.OrOperator, StateChange<TokenizerState>.RetainState);
        }

        static IEnumerable<TokenizerRule> BuildGroupNameAndLookBehindRules()
        {
            // < after (? is the start of a group identifier or look behind
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "<",
                TokenType.NamedIdentifierStartOrLookBehindMarker, StateChange<TokenizerState>.ReplaceState(TokenizerState.NamedIdentifierOrNegativeLookBehind));

            // = after (?< is a positive look behind
            yield return new TokenizerRule(
                TokenizerState.NamedIdentifierOrNegativeLookBehind, "=",
                TokenType.PositiveLookBehindMarker, StateChange<TokenizerState>.PopState);

            // ! after (?< is a negative look behind
            yield return new TokenizerRule(
                TokenizerState.NamedIdentifierOrNegativeLookBehind, "!",
                TokenType.NegativeLookBehindMarker, StateChange<TokenizerState>.PopState);

            // a-z, 0-9 are valid group identifier characters
            yield return new TokenizerRule(
                TokenizerState.NamedIdentifierOrNegativeLookBehind, TokenizerRule.LetterAndNumberData,
                TokenType.Literal, StateChange<TokenizerState>.RetainState);

            // - splits the two identifiers in a balancing group
            yield return new TokenizerRule(
                TokenizerState.NamedIdentifierOrNegativeLookBehind, "-",
                TokenType.BalancingGroupNamedIdentifierSeparator, StateChange<TokenizerState>.RetainState);

            // > ends a named identifier
            yield return new TokenizerRule(
                TokenizerState.NamedIdentifierOrNegativeLookBehind, ">",
                TokenType.NamedIdentifierEnd, StateChange<TokenizerState>.PopState);
        }

        static IEnumerable<TokenizerRule> BuildGroupOptionRules()
        {
            // a-z after (? is an option
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, TokenizerRule.LetterData,
                TokenType.GroupOption, StateChange<TokenizerState>.ReplaceState(TokenizerState.GroupOption));

            // + after (? is an option qualifier
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "+",
                TokenType.GroupOptionQualifier, StateChange<TokenizerState>.ReplaceState(TokenizerState.GroupOption));

            // - after (? is an option qualifier
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "-",
                TokenType.GroupOptionQualifier, StateChange<TokenizerState>.ReplaceState(TokenizerState.GroupOption));

            // a-z after (?[a-z]+ is another option
            yield return new TokenizerRule(
                TokenizerState.GroupOption, TokenizerRule.LetterData,
                TokenType.GroupOption, StateChange<TokenizerState>.RetainState);

            // + after (?[a-z]+ is an option qualifier
            yield return new TokenizerRule(
                TokenizerState.GroupOption, "+",
                TokenType.GroupOptionQualifier, StateChange<TokenizerState>.RetainState);

            // - after (?[a-z]+ is an option qualifier
            yield return new TokenizerRule(
                TokenizerState.GroupOption, "-",
                TokenType.GroupOptionQualifier, StateChange<TokenizerState>.RetainState);

            // : after (?[a-z]+ is the end of the options
            yield return new TokenizerRule(
                TokenizerState.GroupOption, ":",
                TokenType.GroupOptionEnd, StateChange<TokenizerState>.PopState);
        }

        static IEnumerable<TokenizerRule> BuildCharacterSetRules(IEnumerable<TokenizerState> groupingConstructs)
        {
            // [ starts a character set
            yield return new TokenizerRule(
                groupingConstructs, "[",
                TokenType.CharacterSetStart, StateChange<TokenizerState>.PushState(TokenizerState.CharacterSetContentsStart));

            // ] closes a character set
            yield return new TokenizerRule(
                new [] { TokenizerState.CharacterSetContents, TokenizerState.CharacterSetContentsStart }, "]",
                TokenType.CharacterSetEnd, StateChange<TokenizerState>.PopState);

            // ^ after [ makes it a negative set
            yield return new TokenizerRule(
                TokenizerState.CharacterSetContentsStart, "^",
                TokenType.NegativeCharacterSetModifier, StateChange<TokenizerState>.ReplaceState(TokenizerState.CharacterSetContents));

            // \ starts an escape sequence
            yield return new TokenizerRule(
                new[] { TokenizerState.CharacterSetContents, TokenizerState.CharacterSetContentsStart }, @"\",
                TokenType.CharacterEscapeMarker, StateChange<TokenizerState>.PushState(TokenizerState.EscapedCharacter));

            // escaped character data is handled by another rule in BuildBasicExpressionRules

            // - is a character range separator
            yield return new TokenizerRule(
                new[] { TokenizerState.CharacterSetContents, TokenizerState.CharacterSetContentsStart }, @"-",
                TokenType.CharacterRangeSeparator, StateChange<TokenizerState>.RetainState);

            // anything else is a character in the set
            yield return new TokenizerRule(
                new[] { TokenizerState.CharacterSetContents, TokenizerState.CharacterSetContentsStart }, TokenizerRule.AnyData,
                TokenType.Character, StateChange<TokenizerState>.RetainState);
        }

        static IEnumerable<TokenizerRule> BuildQuantifierRules(IEnumerable<TokenizerState> groupingConstructs)
        {
            // Basic quantifiers
            yield return new TokenizerRule(
                groupingConstructs, new[] {"*", "+", "?"},
                TokenType.Quantifier, StateChange<TokenizerState>.RetainState);

            // { starts a parametized quantifier
            yield return new TokenizerRule(
                groupingConstructs, "{",
                TokenType.ParametizedQuantifierStart, StateChange<TokenizerState>.PushState(TokenizerState.ParametizedQuantifierContents));

            // } closes a parametized quantifier
            yield return new TokenizerRule(
                TokenizerState.ParametizedQuantifierContents, "}",
                TokenType.ParametizedQuantifierEnd, StateChange<TokenizerState>.PopState);

            // numbers are valid in a parametized quantifier
            yield return new TokenizerRule(
                TokenizerState.ParametizedQuantifierContents, TokenizerRule.NumberData,
                TokenType.Number, StateChange<TokenizerState>.RetainState);

            // , in a parametized quantifier separates ranges
            yield return new TokenizerRule(
                TokenizerState.ParametizedQuantifierContents, ",",
                TokenType.ParametizedQuantifierRangeSeparator, StateChange<TokenizerState>.RetainState);
        }

        static IEnumerable<TokenizerRule> BuildBasicExpressionRules(IEnumerable<TokenizerState> groupingConstructs)
        {
            // \ starts an escape sequence
            yield return new TokenizerRule(
                groupingConstructs, @"\",
                TokenType.CharacterEscapeMarker, StateChange<TokenizerState>.PushState(TokenizerState.EscapedCharacter));

            // x immediately after \ means the escape sequence is in hex (eg, \x20)
            yield return new TokenizerRule(
                TokenizerState.EscapedCharacter, "x",
                TokenType.CharacterEscapeHexMarker, StateChange<TokenizerState>.ReplaceState(TokenizerState.EscapedCharacterHex, 2));

            // u immediately after \ means the escape sequence is in hex (eg, \u0020)
            yield return new TokenizerRule(
                TokenizerState.EscapedCharacter, "u",
                TokenType.CharacterEscapeUnicodeMarker, StateChange<TokenizerState>.ReplaceState(TokenizerState.EscapedCharacterHex, 4));

            // c immediately after \ means an escaped control character (eg, \cC)
            yield return new TokenizerRule(
                TokenizerState.EscapedCharacter, "c",
                TokenType.CharacterEscapeControlMarker, StateChange<TokenizerState>.RetainState);

            // hex digits after \x or \u are the hex content (eg, \x20 or \u0020)
            yield return new TokenizerRule(
                TokenizerState.EscapedCharacterHex, TokenizerRule.HexData,
                TokenType.CharacterEscapeData, StateChange<TokenizerState>.PopState);

            // anything else immediately after \ is escaped data
            yield return new TokenizerRule(
                TokenizerState.EscapedCharacter, TokenizerRule.AnyData,
                TokenType.CharacterEscapeData, StateChange<TokenizerState>.PopState);

            // | is an 'or' operator
            yield return new TokenizerRule(
                groupingConstructs, "|",
                TokenType.OrOperator, StateChange<TokenizerState>.RetainState);

            // String start assertion
            yield return new TokenizerRule(
                groupingConstructs, "^",
                TokenType.StartOfStringAssertion, StateChange<TokenizerState>.RetainState);

            // String end assertion
            yield return new TokenizerRule(
                groupingConstructs, "$",
                TokenType.EndOfStringAssertion, StateChange<TokenizerState>.RetainState);

            // A period is the 'any character' class
            yield return new TokenizerRule(
                groupingConstructs, ".",
                TokenType.AnyCharacter, StateChange<TokenizerState>.RetainState);

            // Whatever is left is a literal
            yield return new TokenizerRule(
                groupingConstructs, TokenizerRule.AnyData,
                TokenType.Literal, StateChange<TokenizerState>.RetainState);
        }
    }
}