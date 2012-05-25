using System;
using System.ComponentModel;

namespace Sozialhelden.Wheelmap.Lib.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                if (value != _Email)
                {
                    _Email = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                if (value != _Password)
                {
                    _Password = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }


        #region Interfaces

        public event PropertyChangedEventHandler PropertyChanged;

        //TODO
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Password":
                        if (string.IsNullOrEmpty(Password))
                            return "No Password";

                        if (Password.Length < 3)
                            return "Password must at least 3 Chars";
                        break;
                }
                return "";
            }
        }

        #endregion
    }
}
