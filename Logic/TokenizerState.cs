namespace TathamOddie.RegexAnalyzer.Logic
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
        EscapedCharacter
    }
}