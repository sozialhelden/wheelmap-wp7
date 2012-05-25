using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Sozialhelden.Wheelmap.Lib.ViewModel
{
    public class CommandViewModel : INotifyPropertyChanged
    {
        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                if (value != _DisplayName)
                {
                    _DisplayName = value;
                    if (PropertyChanged != null)
                    {
                        try { PropertyChanged(this, new PropertyChangedEventArgs("DisplayName")); }
                        catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace); }
                    }
                }
            }
        }


        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            DisplayName = displayName;
            this.Command = command;
        }

        public ICommand Command { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
