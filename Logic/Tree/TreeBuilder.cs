﻿using System.Collections.Generic;
using System.Linq;
using TathamOddie.RegexAnalyzer.Logic.Tokens;

namespace TathamOddie.RegexAnalyzer.Logic.Tree
{
    public class TreeBuilder
    {
        readonly TreeBuilderRuleSet rules = new TreeBuilderRuleSet();

        public IEnumerable<Node> Build(IEnumerable<Token> tokens)
        {
            var state = new TreeBuilderState();

            var expressionNode = new ExpressionNode(Token.GetData(tokens), 0);
            state.ProcessingQueue.Enqueue(new KeyValuePair<Node, IEnumerable<Token>>(
                expressionNode,
                tokens));

            while (state.ProcessingQueue.Any())
            {
                var processingStep = state.ProcessingQueue.Dequeue();

                var node = processingStep.Key;
                var tokenQueue = new Queue<Token>(processingStep.Value);

                Process(node, tokenQueue, state);
            }

            return expressionNode.Children;
        }

        void Process(Node targetNode, Queue<Token> tokenQueue, TreeBuilderState state)
        {
            state.ProcessingState = new ProcessingState(targetNode, tokenQueue);

            while (tokenQueue.Any())
            {
                var token = tokenQueue.Dequeue();
                var tokenType = token.Type;

                var rule = rules
                    .Where(r => r.TokenType == tokenType)
                    .FirstOrDefault();

                if (rule == null)
                {
                    targetNode.AddChild(new ParseFailureNode(token, "unexpected token"));
                    break;
                }

                var node = rule.NodeBuilder(token, state);

                if (node != null)
                    targetNode.AddChild(node);
            }
        }
    }
}