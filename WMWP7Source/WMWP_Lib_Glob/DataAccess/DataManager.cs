using System;
using System.Net;
using System.Windows;
using Sozialhelden.Wheelmap.Lib.ViewModel;
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

        /// <summary>
        /// Gets or sets the API key to access server data
        /// </summary>
        /// <value>The API key.</value>
        public string APIKey { private get; set; }

        string _locale;
        /// <summary>
        /// Gets or sets the Language of Server Results.
        /// Default: CurrentCulture or if invalid english
        /// </summary>
        /// <value>A two letter language code</value>
        public string Locale
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(_locale)) _locale = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
                }
                catch (Exception ex)
                {
                    _locale = "en";
                }
                return _locale;
            }
            set
            {
                _locale = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets the categories and put them to the viewmodel.
        /// </summary>
        /// <exception cref="APIKeyEception">if ther is no api - key</exception>
        public System.Collections.Generic.List<Object> GetCategories()
        {
            validateAPIKey();
            try
            {

                string url = Config.UCategories + getQueryString(paramAPIKey(), paramLocale());

                SharpGIS.GZipWebClient wc = new SharpGIS.GZipWebClient();
                wc.DownloadStringCompleted +=
                    delegate(object sender, System.Net.DownloadStringCompletedEventArgs e)
                    {
                        try
                        {
                            if (e.Error != null)
                            {
                                if (e.Error is WebException)
                                {
                                    WebException wex = (WebException)e.Error;
                                    Debug.WriteLine(wex.ToString());
                                }
                                throw e.Error;
                            }
                            string json = e.Result;

                            //TODO P!: JsonArray users = (JsonArray)JsonArray.Load(new System.IO.MemoryStream(json.ToCharArray()));


                            //foreach (JsonObject member in users)
                            //{
                            //    string name = member["Name"];
                            //    int age = member["Age"];
                            //}


                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine(ex.ToString());
                            throw new Exception("load categories failed: " + ex.Message, ex);
                        }
                    };

                Debug.WriteLine(url);
                wc.DownloadStringAsync(new Uri(url, UriKind.Absolute));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
            return new System.Collections.Generic.List<object>();
        }

        #region Helper
        string getQueryString(params string[] querys)
        {
            string querystring = "?";
            foreach (string s in querys)
            {
                querystring += s + "&";
            }
            return querystring;
        }

        string paramAPIKey()
        {
            return string.Format("{0}={1}", Config.PAPIKey, APIKey);
        }

        string paramLocale()
        {
            return string.Format("{0}={1}", Config.PLanguage, Locale);
        }
        #endregion

        #region Validations
        /// <summary>
        /// Validates the API key. And throw an Exception if it failed
        /// </summary>
        private void validateAPIKey()
        {
            if (string.IsNullOrEmpty(APIKey)) throw new APIKeyException("api key not found");
        }
        #endregion

    }
}
