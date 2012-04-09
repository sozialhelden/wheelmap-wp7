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
using Sozialhelden.Wheelmap.Lib.Global;

namespace Sozialhelden.Wheelmap.Lib.Storage
{
    /// <summary>
    /// load and save cached data and settings from 
    /// </summary>
    public sealed class PersistenceManager
    {
        
        #region Singelton

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceManager"/> class.
        /// </summary>
        private PersistenceManager()
        {
            //TODO: Load Settings
        }

        private static PersistenceManager _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static PersistenceManager Instance
        {
            get
            {
                if (_instance == null) _instance = new PersistenceManager();
                return _instance;
            }
        }

        #endregion

        #region Load
        internal Settings LoadSettings()
        {
            //TODO: implement
            return new Settings();
        }     
        #endregion

        #region Save
        #endregion


    }
}
