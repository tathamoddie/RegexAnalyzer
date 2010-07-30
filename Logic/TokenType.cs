﻿namespace TathamOddie.RegexAnalyzer.Logic
{
    public enum TokenType
    {
        ParseFailure,
        Literal,
        GroupStart,
        GroupEnd,
        GroupDirectiveStart,
        GroupOption,
        GroupOptionQualifier,
        GroupOptionEnd,
        NamedIdentifierStart,
        NamedIdentifierEnd,
        NonCapturingGroupMarker,
        Quantifier,
        StartOfStringAssertion,
        EndOfStringAssertion,
        PositiveLookAheadMarker,
        ConditionalExpressionStart,
        ConditionalExpressionEnd,
        OrOperator,
        CharacterEscapeMarker,
        CharacterEscapeData,
        CharacterSetStart,
        CharacterSetEnd
    }
}