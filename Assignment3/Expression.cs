using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3
{
    /// <summary>
    ///     A string representing a simple mathematical calculation to be evaluated
    /// </summary>
    class Expression
    {
        private int parenOpenCount;//keeps track of how many unclosed parentheses there are
        private DisplayString input;
        private StringBuilder expressionString;

        public Expression(DisplayString input)
        {
            expressionString = new StringBuilder();
            this.input = input;
        }

        /// <summary>
        ///     Appends symbols to the expression
        /// </summary>
        /// <param name="appendStr">The string to be appended to the expression string</param>
        /// <param name="displayString">The string bound to the string displayed in the ui</param>
        public string AppendExpression(string appendStr)
        {
            expressionString.Append(appendStr);

            switch (appendStr[appendStr.Length - 1])
            {
                case '(':
                    parenOpenCount++;
                    break;
                case ')':
                    parenOpenCount--;
                    break;
            }

            return expressionString.ToString();
        }

        /// <summary>
        ///     Clears the expression string
        /// </summary>
        /// <param name="displayString">The string bound to the string displayed in the ui</param>
        public void Clear(DisplayString displayString)
        {
            expressionString.Clear();
            displayString.Display = "";
        }

        /// <summary>
        ///     Removes the last expression component from the expresssion
        /// </summary>
        /// <param name="displayString">The string bound to the string displayed in the ui</param>
        public void Delete(DisplayString displayString)
        {
            if (expressionString.Length != 0)
            {   
                switch (expressionString[expressionString.Length - 1])
                {
                    case ')':
                        parenOpenCount++;
                        break;
                    case '(':
                        parenOpenCount--;

                        //check for sin, cos, tan, sqrt and delete more characters is detected
                        if (expressionString.Length > 2)
                        {
                            switch (expressionString[expressionString.Length - 2])
                            {
                                case 'n'://sin and tan
                                case 's'://cos
                                    expressionString.Remove(expressionString.Length - 3, 3);
                                    break;
                                case 't'://sqrt
                                    expressionString.Remove(expressionString.Length - 4, 4);
                                    break;
                            }
                        }
                        break;
                    case '\uDF0B'://pi Technically 2 characters in ascii
                        expressionString.Remove(expressionString.Length - 1, 1);
                        break;
                }
                
                expressionString.Remove(expressionString.Length - 1, 1);
            }

            displayString.Display = expressionString.ToString();
        }

        /// <summary>
        ///     Gets data representing legal buttons to press to add to the expression
        /// </summary>
        /// <returns>
        ///     A tuple consisting of
        ///         An list of ExpressionComponent representing the set of buttons that can legally be pressed to add to the expression
        ///         An list of ExpressionComponent representing the set of buttons that when pressed create a syntactically incorrect expression
        /// </returns>
        public Tuple<List<ExpressionComponent>, List<ExpressionComponent>> GetActiveButtonState()
        {
            //if empty expression
            if(expressionString.Length == 0)
            {
                return new Tuple<List<ExpressionComponent>, List<ExpressionComponent>>(
                    new List<ExpressionComponent>(
                        new ExpressionComponent[]
                        {
                            ExpressionComponent.Constants,
                            ExpressionComponent.Decimal,
                            ExpressionComponent.Digits,
                            ExpressionComponent.Minus,
                            ExpressionComponent.ParenOpen
                        }
                    ), new List<ExpressionComponent>(
                        new ExpressionComponent[]
                        {
                            ExpressionComponent.Operators,
                            ExpressionComponent.ParenClose
                        }
                    )
                );
            }

            //Non empty expression
            switch (expressionString[expressionString.Length - 1])
            {
                //constants
                case 'e':
                case '\uDF0B'://pi
                    Tuple<List<ExpressionComponent>, List<ExpressionComponent>> constState = new Tuple<List<ExpressionComponent>, List<ExpressionComponent>>(
                        new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Minus,
                                ExpressionComponent.Operators
                            }
                        ), new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Constants,
                                ExpressionComponent.Decimal,
                                ExpressionComponent.Digits,
                                ExpressionComponent.ParenOpen
                            }
                        )
                    );

                    determineParenCloseLegal(constState);

                    return constState;
                //decimal separator
                case '.':
                    return new Tuple<List<ExpressionComponent>, List<ExpressionComponent>>(
                        new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Digits
                            }
                        ), new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Constants,
                                ExpressionComponent.Decimal,
                                ExpressionComponent.Minus,
                                ExpressionComponent.Operators,
                                ExpressionComponent.ParenClose,
                                ExpressionComponent.ParenOpen
                            }
                        )
                    );
                //digits
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    Tuple<List<ExpressionComponent>, List<ExpressionComponent>> digitState = new Tuple<List<ExpressionComponent>, List<ExpressionComponent>>(
                        new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Digits,
                                ExpressionComponent.Minus,
                                ExpressionComponent.Operators
                            }
                        ), new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Constants,
                                ExpressionComponent.ParenOpen
                            }
                        )
                    );
                    
                    determineParenCloseLegal(digitState);
                    determineDecimalLegal(digitState);

                    return digitState;
                //Minus sign
                case '-':
                    Tuple<List<ExpressionComponent>, List<ExpressionComponent>> minusState = new Tuple<List<ExpressionComponent>, List<ExpressionComponent>>(
                        new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Constants,
                                ExpressionComponent.Digits
                            }
                        ), new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Operators,
                                ExpressionComponent.ParenClose
                            }
                        )
                    );

                    determineDecimalLegal(minusState);
                    determineMinusLegal(minusState);

                    return minusState;
                //operators
                case '+':
                case '×':
                case '÷':
                case '^':
                //open parentheses
                case '(':
                    return new Tuple<List<ExpressionComponent>, List<ExpressionComponent>>(
                        new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Constants,
                                ExpressionComponent.Decimal,
                                ExpressionComponent.Digits,
                                ExpressionComponent.Minus,
                                ExpressionComponent.ParenOpen
                            }
                        ), new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Operators,
                                ExpressionComponent.ParenClose
                            }
                        )
                    );
                //close parentheses
                case ')':
                    Tuple<List<ExpressionComponent>, List<ExpressionComponent>> parenCloseState = new Tuple<List<ExpressionComponent>, List<ExpressionComponent>>(
                        new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Minus,
                                ExpressionComponent.Operators
                            }
                        ), new List<ExpressionComponent>(
                            new ExpressionComponent[]
                            {
                                ExpressionComponent.Constants,
                                ExpressionComponent.Decimal,
                                ExpressionComponent.Digits,
                                ExpressionComponent.ParenOpen
                            }
                        )
                    );

                    determineParenCloseLegal(parenCloseState);

                    return parenCloseState;
                //Impossible but Nessessary
                default:
                    return null;
            }
        }
        
        /// <summary>
        ///     Tries to solve the expression
        /// </summary>
        public void Evaluate()
        {
            //Detect erroneous expressions
            switch (expressionString[expressionString.Length - 1])
            {
                //expression ends in an operator
                case '+':
                case '-':
                case '×':
                case '÷':
                case '^':
                    throw new FormatException("ERROR: Expression ends with an operator");
                //expression ends in a decimal point
                case '.':
                    throw new FormatException("ERROR: Expression ends in a decimal point");
            }

            //unclosed parentheses
            if (parenOpenCount > 0)
            {
                throw new FormatException("ERROR: Unclosed parentheses pair");
            }

            evaluate(0, expressionString.Length - 1);
        }

        /// <summary>
        ///     Figures out whether a decimal is legally appendable to the expression following a digit
        ///     adds the minus symbol to the appropriate list
        /// </summary>
        /// <param name="legalLists">
        ///     The tuple containing the list of legal moves and list of illegal moves
        /// </param>
        private void determineDecimalLegal(Tuple<List<ExpressionComponent>, List<ExpressionComponent>> legalLists)
        {
            string expression = expressionString.ToString();
            int i;

            for (i = expression.Length - 2; i >= 0 && Char.IsDigit(expression, i); i--)//go to first instance of non digit
            {
                //do nothing
            }

            if (i != -1 && expression[i + 1] == '.')//Deciaml already present in numeric al expression
            {
                legalLists.Item2.Add(ExpressionComponent.Decimal);
            }
            else
            {
                legalLists.Item1.Add(ExpressionComponent.Decimal);
            }
        }

        /// <summary>
        ///     Figures out whether the minus at the end of the expression is an operator or negation
        ///     adds the appropriate symbols to the appropriate lists
        /// </summary>
        /// <param name="legalLists">
        ///     The tuple containing the list of legal moves and list of illegal moves
        /// </param>
        private void determineMinusLegal(Tuple<List<ExpressionComponent>, List<ExpressionComponent>> legalLists)
        {
            string expression = expressionString.ToString();

            if (expression.Length > 1)
            {
                switch (expression[expression.Length - 2])
                {
                    //sign is negation
                    case '+':
                    case '-':
                    case '×':
                    case '÷':
                    case '^':
                        legalLists.Item2.Add(ExpressionComponent.Minus);
                        legalLists.Item2.Add(ExpressionComponent.ParenOpen);
                        break;
                    //sign is an operator
                    default:
                        legalLists.Item1.Add(ExpressionComponent.Minus);
                        legalLists.Item1.Add(ExpressionComponent.ParenOpen);
                        break;
                }
            }
            else
            {
                legalLists.Item2.Add(ExpressionComponent.Minus);
            }
        }

        /// <summary>
        ///     Figures out whether close parentheses is a legal symbol to add to the expression and adds it to the appropriate list
        /// </summary>
        /// <param name="legalLists">
        ///     The tuple containing the list of legal moves and list of illegal moves
        /// </param>
        private void determineParenCloseLegal(Tuple<List<ExpressionComponent>, List<ExpressionComponent>> legalLists)
        {
            if (parenOpenCount > 0)
            {
                legalLists.Item1.Add(ExpressionComponent.ParenClose);
            }
            else
            {
                legalLists.Item2.Add(ExpressionComponent.ParenClose);
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void evaluate(int start, int end)
        {
            //scan evaluation string for evauation priority
            int[] priority = new int[end - start + 1];

            for (int i = start; i <= end; i++)
            {
                switch (expressionString[i])
                {
                    case '+':
                    case '-':
                        priority[i - start] = 5;
                        break;
                    case '×':
                    case '÷':
                        priority[i - start] = 4;
                        break;
                    case '^':
                        priority[i - start] = 3;
                        break;
                    case 'e':
                    case '\uDF0B':
                        priority[i - start] = 2;
                        break;
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        priority[i - start] = 1;
                        break;
                    case 'c':
                    case 's':
                    case 't':
                        while (Char.IsLetter(expressionString[i]))
                        {
                            priority[i - start] = 0;
                            i++;
                        }
                        break;
                    case '(':
                        int parenOpenCount = 0;
                        do
                        {
                            priority[i - start] = 0;
                            if (expressionString[i] == '(')
                            {
                                parenOpenCount++;
                            }

                            if (expressionString[i] == ')')
                            {
                                parenOpenCount--;
                            }

                            i++;
                        } while (parenOpenCount > 0);
                        break;
                }
            }

            int maxIdx = 0;
            for(int i = 0; i < priority.Length; i++)
            {
                if(priority[i] > priority[maxIdx])
                {
                    maxIdx = i;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="operatorIdx"></param>
        private void evaluateAddSubtract(int start, int end, int operatorIdx)
        {

        }
    }
}
