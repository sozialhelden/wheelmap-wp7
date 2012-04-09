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
using System.Collections.Generic;

namespace Sozialhelden.Wheelmap.Lib.Global
{
    public class Settings
    {

        public Settings()
        {
            APIKey = "";
            Locale = "";
            Username = "";
            Password = "";
            CategoryFilter = new List<string>();
            }

        public string APIKey { get; set; }
        public string Locale { get; set; }

        public string Username { get; set; }
        //TODO: Securestring oder Hash verwenden
        public string Password { get; set; }

        public List<string> CategoryFilter { get; set; }
    }
}
