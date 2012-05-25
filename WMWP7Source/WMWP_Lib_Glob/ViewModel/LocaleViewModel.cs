using System;
using System.ComponentModel;

namespace Sozialhelden.Wheelmap.Lib.ViewModel
{
    public class LocaleViewModel : INotifyPropertyChanged
    {
        private string _ID;

        public string ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("ID"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _LocalizedName;
        
        public string LocalizedName
        {
            get { return _LocalizedName; }
            set
            {
                if (value != _LocalizedName)
                {
                    _LocalizedName = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("LocalizedName"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
