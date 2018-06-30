using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Assignment3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<ExpressionComponent, ButtonGroup> expressionCharGroups;
        private Expression inputExpression;
        private DisplayString errorDisplay, 
                              inputDisplay;

        public MainWindow()
        {
            InitializeComponent();

            expressionCharGroups = new Dictionary<ExpressionComponent, ButtonGroup>();
            errorDisplay = new DisplayString();
            inputDisplay = new DisplayString();
            inputExpression = new Expression(inputDisplay);

            //Create dictionary of sets of buttons
            expressionCharGroups.Add(ExpressionComponent.Constants, new ButtonGroup(new Button[] { btnE, btnPi }));
            expressionCharGroups.Add(ExpressionComponent.Digits, new ButtonGroup(new Button[] { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 }));
            expressionCharGroups.Add(ExpressionComponent.Decimal, new ButtonGroup(new Button[] { btnDecimal }));
            expressionCharGroups.Add(ExpressionComponent.Minus, new ButtonGroup(new Button[] { btnMinus }));
            expressionCharGroups.Add(ExpressionComponent.Operators, new ButtonGroup(new Button[] { btnPlus, btnMultiply, btnDivide, btnExponent }));
            expressionCharGroups.Add(ExpressionComponent.ParenClose, new ButtonGroup(new Button[] { btnParenRight }));
            expressionCharGroups.Add(ExpressionComponent.ParenOpen, new ButtonGroup(new Button[] { btnParenLeft, btnSin, btnCos, btnTan, btnSqrt }));

            displayError.DataContext = errorDisplay;
            displayIn.DataContext = inputDisplay;
            SetButtonStateButtons();
        }

        /// <summary>
        ///     Deletes the last symbol from the expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete(object sender, RoutedEventArgs e)
        {
            inputExpression.Delete(inputDisplay);
            errorDisplay.Display = "";
            SetButtonStateButtons();
        }

        /// <summary>
        ///     Clears the input expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear(object sender, RoutedEventArgs e)
        {
            inputExpression.Clear(inputDisplay);
            errorDisplay.Display = "";
            SetButtonStateButtons();
        }

        /// <summary>
        ///     Appends a symbol to the input expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeChar(object sender, RoutedEventArgs e)
        {
            Button calcButton = (Button)sender;
            inputDisplay.Display = inputExpression.AppendExpression(calcButton.Content.ToString());
            errorDisplay.Display = "";
            SetButtonStateButtons();
        }

        /// <summary>
        ///     Appends a trig function to the input expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeFunction(object sender, RoutedEventArgs e)
        {
            Button calcButton = (Button)sender;
            inputDisplay.Display = inputExpression.AppendExpression(calcButton.Content.ToString() + "(");
            errorDisplay.Display = "";
            SetButtonStateButtons();
        }

        /// <summary>
        ///     Appends the sqrt function to the input expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeSqrt(object sender, RoutedEventArgs e)
        {
            Button calcButton = (Button)sender;
            inputDisplay.Display = inputExpression.AppendExpression("sqrt(");
            errorDisplay.Display = "";
            
            SetButtonStateButtons();
        }
        
        /// <summary>
        ///     Disables buttons that would cause an illegal expression if pressed
        ///     Enables buttons that can add to the input expression legally
        /// </summary>
        private void SetButtonStateButtons() {

            Tuple<List<ExpressionComponent>, List<ExpressionComponent>> legalState = inputExpression.GetActiveButtonState();

            foreach (ExpressionComponent c in legalState.Item1)
            {
                ButtonGroup legalButtons = expressionCharGroups[c];

                if (legalButtons.IsActive == false)
                {
                    foreach (Button b in legalButtons.Buttons)
                    {
                        b.IsEnabled = true;
                    }

                    legalButtons.IsActive = true;
                }
            }

            foreach (ExpressionComponent c in legalState.Item2)
            {
                ButtonGroup illegalButtons = expressionCharGroups[c];

                if(illegalButtons.IsActive == true)
                {
                    foreach (Button b in illegalButtons.Buttons)
                    {
                        b.IsEnabled = false;
                    }

                    illegalButtons.IsActive = false;
                }
            }
        }

        /// <summary>
        ///     Tries to evaluate the expression
        ///     Prints an error to the screen if it fails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Evaluate(object sender, RoutedEventArgs e)
        {
            try
            {
                inputExpression.Evaluate();
            }
            catch (Exception ex)
            {
                errorDisplay.Display = ex.Message;
            }
        }
    }
}
