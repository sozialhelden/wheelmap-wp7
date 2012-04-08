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

namespace Sozialhelden.Wheelmap.Lib.DataAccess
{
    internal static partial class Config
    {

#region URLs
        public static readonly string APIURL = "http://wheelmap.org/api/";
        public static readonly string UCategories = APIURL + "categories";

#endregion

        #region Parameters
        /// <summary>
        /// Parametrer API Key
        /// </summary>
        public static readonly string PAPIKey="api_key";
        /// <summary>
        /// Parameter Language
        /// </summary>
        public static readonly string PLanguage="locale";
#endregion

    }
}
