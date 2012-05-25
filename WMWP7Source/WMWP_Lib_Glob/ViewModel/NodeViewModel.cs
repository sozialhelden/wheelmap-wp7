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
    public class NodeViewModel : INotifyPropertyChanged
    {
        private bool _Wheelchair;

        public bool Wheelchair
        {
            get { return _Wheelchair; }
            set
            {
                if (value != _Wheelchair)
                {
                    _Wheelchair = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Wheelchair"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _Street;

        public string Street
        {
            get { return _Street; }
            set
            {
                if (value != _Street)
                {
                    _Street = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Street"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _Housenumber;

        public string Housenumber
        {
            get { return _Housenumber; }
            set
            {
                if (value != _Housenumber)
                {
                    _Housenumber = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Housenumber"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _City;

        public string City
        {
            get { return _City; }
            set
            {
                if (value != _City)
                {
                    _City = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("City"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _Postcode;

        public string Postcode
        {
            get { return _Postcode; }
            set
            {
                if (value != _Postcode)
                {
                    _Postcode = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Postcode"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _Website;

        public string Website
        {
            get { return _Website; }
            set
            {
                if (value != _Website)
                {
                    _Website = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Website"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set
            {
                if (value != _Phone)
                {
                    _Phone = value;
                    if (PropertyChanged != null)
                    {
                        try
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace);
                        }
                    }
                }
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<CategoryViewModel> _categories;
        public System.Collections.ObjectModel.ObservableCollection<CategoryViewModel> Categories
        {
            get
            {
                if (_categories == null) _categories = new System.Collections.ObjectModel.ObservableCollection<CategoryViewModel>();
                return _categories;
            }
            private set { _categories = value; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
