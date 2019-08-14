using System.Windows.Controls;

namespace ErrorFreeCalculator
{
    /// <summary>
    ///     Describes buttons related by what part of an expression they correspond to as well as whether they are active
    /// </summary>
    class ButtonGroup
    {
        private bool isActive;
        private Button[] buttons;

        public ButtonGroup(Button[] buttons)
        {
            this.isActive = true;
            this.buttons = buttons;
        }

        /// <summary>
        ///     Is the set of buttons enabled in the UI
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        } 

        /// <summary>
        ///     Related set of buttons
        /// </summary>
        public Button[] Buttons
        {
            get
            {
                return buttons;
            }
        }
    }
}
