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
using Sozialhelden.Wheelmap.Lib.Global.ViewModel;
using System.Diagnostics;

namespace Sozialhelden.Wheelmap.Lib.DataAccess
{
    /// <summary>
    /// Present the DataManager API
    /// </summary>
    public sealed class DataManager
    {

        #region Singelton

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        private DataManager()
        {
            _mainViewModel = new MainViewModel();
        }

        private static DataManager _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static DataManager Instance
        {
            get
            {
                if (_instance == null) _instance = new DataManager();
                return _instance;
            }
        }

        MainViewModel _mainViewModel;
        /// <summary>
        /// Gets the main view model.
        /// </summary>
        /// <value>The main view model.</value>
        public MainViewModel MainViewModel
        {
            get
            {
                return _mainViewModel;
            }
        }

        string _apikey;
        public string APIKey
        {
            set
            {
                _apikey = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets the categories and put them to the viewmodel.
        /// </summary>
        /// <exception cref="APIKeyEception">if ther is no api - key</exception>
        public void GetCategories()
        {
            validateAPIKey();
            string url = Config.UCategories + addParams(paramAPIKey());
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += 
                delegate(object sender, System.Net.DownloadStringCompletedEventArgs e) 
            {
                if (e.Error != null) throw e.Error;
            };
            Debug.WriteLine(url);
            wc.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        #region Helper
        string addParams(params string[] querys)
        {
            string querystring = "?";
            foreach (string s in querys)
            {
                querystring += s + "";
            }
            return querystring;
        }

        string paramAPIKey()
        {
            return string.Format("{0}={1}", Config.PAPIKey, _apikey);
        }
        #endregion

        #region Validations
        /// <summary>
        /// Validates the API key. And throw an Exception if it failed
        /// </summary>
        private void validateAPIKey()
        {
            if (string.IsNullOrEmpty(_apikey)) throw new APIKeyException("api key not found");
        }
        #endregion

    }
}
