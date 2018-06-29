using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Assignment3
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

        public Button[] Buttons
        {
            get
            {
                return buttons;
            }
        }
    }
}
