﻿namespace TathamOddie.RegexAnalyzer.Logic.Tokens
{
    enum TokenizerState
    {
        GroupContents,
        GroupContentsStart,
        GroupDirectiveContents,
        GroupOption,
        NamedIdentifier,
        ConditionalExpressionPredicate,
        ConditionalExpressionContents,
        EscapedCharacter,
        CharacterSetContents,
        CharacterSetContentsStart,
        ParametizedQuantifierContents
    }
}