namespace TathamOddie.RegexAnalyzer.Logic.Tokens
{
    enum TokenizerState
    {
        GroupContents,
        GroupContentsStart,
        GroupDirectiveContents,
        GroupOption,
        NamedIdentifierOrNegativeLookBehind,
        ConditionalExpressionPredicate,
        ConditionalExpressionContents,
        EscapedCharacter,
        EscapedCharacterHex,
        CharacterSetContents,
        CharacterSetContentsStart,
        ParametizedQuantifierContents,
        InlineComment
    }
}