namespace TathamOddie.RegexAnalyzer.Logic
{
    public enum TokenType
    {
        ParseFailure,
        Literal,
        GroupStart,
        GroupEnd,
        GroupDirectiveStart,
        GroupOption,
        GroupOptionEnd,
        NamedIdentifierStart,
        NamedIdentifierEnd
    }
}