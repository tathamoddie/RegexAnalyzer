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
        EscapedHexCharacter,
        CharacterSetContents,
        CharacterSetContentsStart,
        ParametizedQuantifierContents,
        InlineComment
    }
}