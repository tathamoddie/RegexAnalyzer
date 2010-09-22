using System;
using System.Collections.Generic;
using System.Linq;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    class TreeBuilder
    {
        public IEnumerable<Node> Build(IEnumerable<Token> tokens)
        {
            var states = new Stack<TreeBuilderState>();
            states.Push(TreeBuilderState.Expression);

            var rules = new TreeBuilderRuleSet();

            var tokenQueue = new Queue<Token>(tokens);

            while (tokenQueue.Any())
            {
                var token = tokenQueue.Dequeue();
                var tokenType = token.Type;

                var currentState = states.Peek();
                var rule = rules
                    .Where(r => r.States == TreeBuilderRule.AnyState || r.States.Contains(currentState))
                    .Where(r => r.TokenType == tokenType)
                    .FirstOrDefault();

                if (rule == null)
                    throw new ApplicationException(string.Format(
                        "No rule is defined for token type {0} in tree builder state {1}.",
                        token.Type,
                        currentState));

                yield return rule.NodeBuilder(token, tokenQueue);
            }
        }
    }
}