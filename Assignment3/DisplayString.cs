using System.ComponentModel;

namespace Assignment3
{
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
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
