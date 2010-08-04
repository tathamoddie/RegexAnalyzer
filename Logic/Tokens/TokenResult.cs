namespace TathamOddie.RegexAnalyzer.Logic.Tokens
{
    class TokenResult
    {
        readonly Token token;
        readonly TokenizerState newState;

        public TokenResult(Token token, TokenizerState newState)
        {
            this.token = token;
            this.newState = newState;
        }

        public Token Token
        {
            get { return token; }
        }

        public TokenizerState NewState
        {
            get { return newState; }
        }
    }
}