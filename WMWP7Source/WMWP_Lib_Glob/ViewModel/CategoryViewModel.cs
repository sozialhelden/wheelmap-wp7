using System;
using System.ComponentModel;

namespace Sozialhelden.Wheelmap.Lib.ViewModel
{
    public class CategoryViewModel : INotifyPropertyChanged
    {

        public CategoryViewModel(string id, string name)
        {
            Identifier = id;
            LocalizedName = name;
        }

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

        private string _Identifier;

        public string Identifier
        {
            get { return _Identifier; }
            set
            {
                if (value != _Identifier)
                {
                    _Identifier = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Identifier"));
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
