using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace UniversalHelmod.Extractors.Satisfactory.Models
{
    public class FGArrayParser
    {
        public static object Parse(string value)
        {
            FGArrayParser parser = new FGArrayParser();
            object result = parser.Execute(value);
            return result;
        }

        class Token
        {
            public int Index;
            public TokenType Type;
        }

        class Result
        {
            public int Index;
            public object Value;

            public void AddValue(object value, string key = null)
            {
                if(Value is List<object>)
                {
                    ((List<object>)Value).Add(value);
                }
                else if (Value is Dictionary<string, object>)
                {
                    ((Dictionary<string, object>)Value).Add(key, value);
                }
            }
        }
        private object Execute(string value)
        {
            List<Token> tokens = Tokenize(value);
            Result result = ParseTokens(value, tokens);
            if (result == null) return null;
            return result.Value;
        }
        private Result ParseTokens(string text, List<Token> tokens, int index = 0)
        {
            Result result = new Result();
            string key = null;
            if(tokens.Count == 0)
            {
                result.Value = text;
                return result;
            }
            for(;index < tokens.Count; index++)
            {
                Token token = tokens[index];
                switch (token.Type)
                {
                    case TokenType.OpeningBrace:
                        {
                            if (tokens.Count <= index + 1) throw new Exception("Last token can't be opening brace");
                            if (index > 0)
                            {
                                Token previous = tokens[index - 1];
                                if (previous.Type == TokenType.OpeningBrace)
                                {
                                    Result parsed = ParseTokens(text, tokens, index + 1);
                                    if (result.Value == null)
                                    {
                                        result.Value = new List<object>();
                                    }
                                    result.AddValue(parsed.Value);
                                    index = parsed.Index;
                                }
                                else if (previous.Type == TokenType.EqualSign)
                                {
                                    Result parsed = ParseTokens(text, tokens, index + 1);
                                    result.AddValue(parsed.Value, key);
                                    index = parsed.Index;
                                }
                            }
                        }
                        break;
                    case TokenType.Separator:
                        {
                            Token previous = tokens[index - 1];
                            Token next = null;
                            if (tokens.Count > index + 1)
                            {
                                next = tokens[index + 1];
                            }
                            if (next != null && next.Type == TokenType.OpeningBrace)
                            {
                                Result parsed = ParseTokens(text, tokens, index + 1);
                                result.AddValue(parsed.Value);
                                index = parsed.Index;
                            }
                            else if (previous.Type == TokenType.EqualSign)
                            {
                                var value = text.Substring(previous.Index + 1, token.Index - previous.Index - 1);
                                result.AddValue(value, key);
                            }
                            else if (previous.Type == TokenType.ClosingBrace)
                            {
                                if (result.Value is Dictionary<string, object>)
                                {
                                    key = text.Substring(previous.Index + 1, token.Index - previous.Index - 1);
                                }
                            }
                            else
                            {
                                if (result.Value == null)
                                {
                                    result.Value = new List<object>();
                                }
                                var value = text.Substring(previous.Index + 1, token.Index - previous.Index - 1);
                                result.AddValue(value);
                            }
                            key = null;
                        }
                        break;
                    case TokenType.EqualSign:
                        {
                            if (result.Value == null)
                            {
                                result.Value = new Dictionary<string, object>();
                            }
                            Token previous = tokens[index - 1];
                            key = text.Substring(previous.Index + 1, token.Index - previous.Index - 1);
                        }
                        break;
                    case TokenType.ClosingBrace:
                        {
                            Token previous = tokens[index - 1];
                            if (previous.Type == TokenType.OpeningBrace || previous.Type == TokenType.Separator)
                            {
                                if (result.Value == null)
                                {
                                    result.Value = new List<object>();
                                }
                                var value = text.Substring(previous.Index + 1, token.Index - previous.Index - 1);
                                result.AddValue(value);
                            }
                            else if (previous.Type == TokenType.EqualSign)
                            {
                                var value = text.Substring(previous.Index + 1, token.Index - previous.Index - 1);
                                result.AddValue(value, key);
                            }
                            result.Index = index;
                            return result;
                        }
                }
            }
            result.Index = index;
            return result;
        }
        private List<Token> Tokenize(string value)
        {
            List<Token> tokens = new List<Token>();
            int index = 0;
            while (index < value.Length)
            {
                var x = value[index];
                switch (x)
                {
                    case '(':
                        tokens.Add(new Token { Type = TokenType.OpeningBrace, Index = index });
                        break;
                    case '=':
                        tokens.Add(new Token { Type = TokenType.EqualSign, Index = index });
                        break;
                    case ',':
                        tokens.Add(new Token { Type = TokenType.Separator, Index = index });
                        break;
                    case ')':
                        tokens.Add(new Token { Type = TokenType.ClosingBrace, Index = index });
                        break;
                }
                index++;
            }
            return tokens;
        }

        enum TokenType
        {
            OpeningBrace,
            ClosingBrace,
            EqualSign,
            Separator
        }
    }
}
