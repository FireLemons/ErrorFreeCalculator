﻿using System.ComponentModel;

namespace ErrorFreeCalculator
{
    /// <summary>
    ///     Class nessessary to bind strings to the display
    /// </summary>
    class DisplayString : INotifyPropertyChanged
    {
        string displayString;

        public string Display
        {
            get
            {
                return this.displayString;
            }
            set
            {
                if (this.displayString != value)
                {
                    this.displayString = value;
                    this.NotifyPropertyChanged("Display");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
