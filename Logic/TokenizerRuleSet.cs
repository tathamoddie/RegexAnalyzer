﻿using System.Collections.Generic;

namespace TathamOddie.RegexAnalyzer.Logic
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
            AddRange(BuildGroupNameRules());
            AddRange(BuildGroupOptionRules());
            AddRange(BuildBasicExpressionRules(groupingConstructs));
        }

        static IEnumerable<TokenizerRule> BuildGroupRules(IEnumerable<TokenizerState> groupingConstructs)
        {
            // ( starts a group
            yield return new TokenizerRule(
                groupingConstructs, "(",
                TokenType.GroupStart, TokenizerStateChange.PushState(TokenizerState.GroupContentsStart));

            // ) closes a group
            yield return new TokenizerRule(
                groupingConstructs, ")",
                TokenType.GroupEnd, TokenizerStateChange.RetainState);

            // ? after ( is the start of a group directive
            yield return new TokenizerRule(
                TokenizerState.GroupContentsStart, "?",
                TokenType.GroupDirectiveStart, TokenizerStateChange.ReplaceState(TokenizerState.GroupDirectiveContents));

            // : after (? is a non-capturing group marker
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, ":",
                TokenType.NonCapturingGroupMarker, TokenizerStateChange.PopState);

            // = after (? is a non-capturing group marker
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "=",
                TokenType.PositiveLookAheadMarker, TokenizerStateChange.PopState);
        }

        static IEnumerable<TokenizerRule> BuildConditionalExpressionRules()
        {
            // ( after (? is the start of a conditional expression
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "(",
                TokenType.ConditionalExpressionStart, TokenizerStateChange.ReplaceState(TokenizerState.ConditionalExpressionPredicate));

            // a-z, 0-9 are valid group identifier characters
            yield return new TokenizerRule(
                TokenizerState.ConditionalExpressionPredicate, TokenizerRule.LetterAndNumberData,
                TokenType.Literal, TokenizerStateChange.RetainState);

            // ) ends a conditional expression
            yield return new TokenizerRule(
                TokenizerState.ConditionalExpressionPredicate, ")",
                TokenType.ConditionalExpressionEnd, TokenizerStateChange.ReplaceState(TokenizerState.ConditionalExpressionContents));

            // | divides conditional expression content
            yield return new TokenizerRule(
                TokenizerState.ConditionalExpressionContents, "|",
                TokenType.OrOperator, TokenizerStateChange.RetainState);
        }

        static IEnumerable<TokenizerRule> BuildGroupNameRules()
        {
            // < after (? is the start of a group identifier
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "<",
                TokenType.NamedIdentifierStart, TokenizerStateChange.ReplaceState(TokenizerState.NamedIdentifier));

            // a-z, 0-9 are valid group identifier characters
            yield return new TokenizerRule(
                TokenizerState.NamedIdentifier, TokenizerRule.LetterAndNumberData,
                TokenType.Literal, TokenizerStateChange.RetainState);

            // > ends a named identifier
            yield return new TokenizerRule(
                TokenizerState.NamedIdentifier, ">",
                TokenType.NamedIdentifierEnd, TokenizerStateChange.PopState);
        }

        static IEnumerable<TokenizerRule> BuildGroupOptionRules()
        {
            // a-z after (? is an option
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, TokenizerRule.LetterData,
                TokenType.GroupOption, TokenizerStateChange.ReplaceState(TokenizerState.GroupOption));

            // + after (? is an option qualifier
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "+",
                TokenType.GroupOptionQualifier, TokenizerStateChange.ReplaceState(TokenizerState.GroupOption));

            // - after (? is an option qualifier
            yield return new TokenizerRule(
                TokenizerState.GroupDirectiveContents, "-",
                TokenType.GroupOptionQualifier, TokenizerStateChange.ReplaceState(TokenizerState.GroupOption));

            // a-z after (?[a-z]+ is another option
            yield return new TokenizerRule(
                TokenizerState.GroupOption, TokenizerRule.LetterData,
                TokenType.GroupOption, TokenizerStateChange.RetainState);

            // + after (?[a-z]+ is an option qualifier
            yield return new TokenizerRule(
                TokenizerState.GroupOption, "+",
                TokenType.GroupOptionQualifier, TokenizerStateChange.RetainState);

            // - after (?[a-z]+ is an option qualifier
            yield return new TokenizerRule(
                TokenizerState.GroupOption, "-",
                TokenType.GroupOptionQualifier, TokenizerStateChange.RetainState);

            // : after (?[a-z]+ is the end of the options
            yield return new TokenizerRule(
                TokenizerState.GroupOption, ":",
                TokenType.GroupOptionEnd, TokenizerStateChange.PopState);
        }

        static IEnumerable<TokenizerRule> BuildBasicExpressionRules(IEnumerable<TokenizerState> groupingConstructs)
        {
            // \ starts an escape sequence
            yield return new TokenizerRule(
                groupingConstructs, @"\",
                TokenType.CharacterEscapeMarker, TokenizerStateChange.PushState(TokenizerState.EscapedCharacter));

            // Anything immediately after \ is escaped data
            yield return new TokenizerRule(
                TokenizerState.EscapedCharacter, TokenizerRule.AnyData,
                TokenType.CharacterEscapeData, TokenizerStateChange.PopState);

            // | is an 'or' operator
            yield return new TokenizerRule(
                groupingConstructs, "|",
                TokenType.OrOperator, TokenizerStateChange.RetainState);

            // Basic quantifiers
            yield return new TokenizerRule(
                groupingConstructs, new[] {"*", "+", "?"},
                TokenType.Quantifier, TokenizerStateChange.RetainState);

            // String end assertion
            yield return new TokenizerRule(
                groupingConstructs, new[] {"$"},
                TokenType.EndOfStringAssertion, TokenizerStateChange.RetainState);

            // Whatever is left is a literal
            yield return new TokenizerRule(
                groupingConstructs, TokenizerRule.AnyData,
                TokenType.Literal, TokenizerStateChange.RetainState);
        }
    }
}