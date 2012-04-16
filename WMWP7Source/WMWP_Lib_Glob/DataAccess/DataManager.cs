using System;
using System.Net;
using System.Windows;
using Sozialhelden.Wheelmap.Lib.ViewModel;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
            //WebRequest.RegisterPrefix("http://", SharpGIS.WebRequestCreator.GZip);
            //WebRequest.RegisterPrefix("https://", SharpGIS.WebRequestCreator.GZip);
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
                catch (Exception)
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
        public void GetCategoriesAsync(Action<List<Model.Category>> callback)
        {
            validateAPIKey();
            try
            {
                string url = Config.UCategories + getQueryString(paramAPIKey(), paramLocale());

                SharpGIS.GZipWebClient wc = new SharpGIS.GZipWebClient();
                //wc.OpenReadCompleted +=
                //    delegate(object sender, System.Net.OpenReadCompletedEventArgs e)
                wc.DownloadStringCompleted +=
                delegate(object sender, System.Net.DownloadStringCompletedEventArgs e)
                    {
                        List<Model.Category> retVal = new List<Model.Category>();
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

                            string jsonstr = e.Result;

                            JObject json = (JObject)JsonConvert.DeserializeObject(jsonstr);
                            foreach (var cat in json["categories"])
                            {
                                Debug.WriteLine("categorie {0} ({1}, \"{2}\"",
                                    cat["id"], cat["identifier"], cat["localized_name"]);

                                retVal.Add(new Model.Category() { 
                                    id = cat["id"].Value<string>(), 
                                    identifier = cat["identifier"].ToString(),
                                    localized_name = cat["localized_name"].ToString() 
                                });
                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                            throw new Exception("load categories failed: " + ex.Message, ex);
                        }
                        if (callback != null) callback(retVal);
                    };

                Debug.WriteLine(url);
                wc.DownloadStringAsync(new Uri(url, UriKind.Absolute));
                //wc.OpenReadAsync(new Uri(url, UriKind.Absolute));

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        #region Helper
        string getQueryString(params string[] querys)
        {
            string querystring = "?";// "?format=xml&";
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
