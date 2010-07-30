namespace TathamOddie.RegexAnalyzer.Logic
{
    public enum TokenType
    {
        ParseFailure,
        Literal,
        GroupStart,
        GroupDirectiveStart,
        GroupEnd,
        NamedIdentifierStart,
        NamedIdentifierEnd
    }
}