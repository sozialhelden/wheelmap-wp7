using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using Sozialhelden.Wheelmap.Lib.Global;
using System.Windows.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace Sozialhelden.Wheelmap.Lib.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        Settings _settings;
        DataAccess.DataManager _data;
        Storage.PersistenceManager _store;

        public MainViewModel()
        {
            _data = DataAccess.DataManager.Instance;
            _store = Storage.PersistenceManager.Instance;


            Categories = new ObservableCollection<CategoryViewModel>();

            _settings = _store.LoadSettings();

            if (string.IsNullOrEmpty(_data.APIKey)) _data.APIKey = _settings.APIKey;
            if (string.IsNullOrEmpty(_settings.Locale) == false) _data.Locale = _settings.Locale;

            actionFindCurrentPosition();
        }


        public bool IsDataLoaded { get; protected set; }

        public void LoadData()
        {
            actionLoadCategories();
        }


        ReadOnlyCollection<CommandViewModel> _commands;

        /// <summary>
        /// LIst of Commands to render in an automatic menue
        /// </summary>
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_commands == null)
                {
                    List<CommandViewModel> cmds = this.InitCommands();
                    _commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _commands;
            }
        }

        #region Categories
        public ObservableCollection<CategoryViewModel> Categories { get; private set; }

        /// <summary>
        /// Find a Category ind the Categories Collection. Id it does not exist we create a new object
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public CategoryViewModel GetCategorie(string identifier)
        {
            identifier = identifier.ToLower().Trim();
            foreach (CategoryViewModel cat in Categories)
            {
                if (cat.Identifier == identifier)
                {
                    return cat;
                }
            }
            //if we are here the categorie was not found
            CategoryViewModel newcat = new CategoryViewModel(identifier, identifier, identifier);
            Categories.Add(newcat);
            return newcat;
        }

        /// <summary>
        /// load alle categories from Server and fill into Categories Collection
        /// the collection will be updated and not cleared
        /// </summary>
        public void actionLoadCategories()
        {
            try
            {
                _data.GetCategoriesAsync(
                    (List<DataAccess.Model.Category> data) =>
                    {
                        Application.Current.RootVisual.Dispatcher.BeginInvoke(() =>
                        {
                            foreach (var cat in data)
                            {
                                CategoryViewModel cvm = GetCategorie(cat.identifier);
                                cvm.ID = cat.id;
                                cvm.Identifier = cat.identifier;
                                cvm.LocalizedName = cat.localized_name;
                            }
                        });
                    });
            }
            catch (System.Exception ex)
            {
                HandleError(ex);
            }
        }
        #endregion

        #region GeoLocation

        bool _currentPositionInitialized = false;
        System.Device.Location.GeoCoordinate _currentPosition;
        System.Device.Location.GeoCoordinateWatcher _currentPositionWatcher;
        System.Windows.Threading.DispatcherTimer _currentPositionRefreshTimer;

        /// <summary>
        /// Occurs when [current position changed].
        /// </summary>
        public event CurrentPositionChangedDelegate CurrentPositionChanged;
        private void RaiseCurrentPositionChanged(System.Device.Location.GeoCoordinate gc)
        {
            try
            {
                if (CurrentPositionChanged != null) CurrentPositionChanged(gc);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }


        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>The current position.</value>
        public System.Device.Location.GeoCoordinate CurrentPosition
        {
            get
            {
                if (_currentPositionInitialized == false)
                {
                    _currentPosition = new System.Device.Location.GeoCoordinate();
                    initGeoWatcher();
                    _currentPositionInitialized = true;
                }
                return _currentPosition;
            }
            set
            {
                if (value != _currentPosition)
                {
                    _currentPosition = value;
                    RaiseCurrentPositionChanged(_currentPosition);
                    if (PropertyChanged != null)
                    {
                        try { PropertyChanged(this, new PropertyChangedEventArgs("")); }
                        catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace); }
                    }
                }
            }
        }

        /// <summary>
        /// starts a geoposition watcher
        /// </summary>
        private void initGeoWatcher()
        {
            if (_currentPositionWatcher == null)
            {
                _currentPositionWatcher = new System.Device.Location.GeoCoordinateWatcher(System.Device.Location.GeoPositionAccuracy.High);

                _currentPositionWatcher.StatusChanged += new EventHandler<System.Device.Location.GeoPositionStatusChangedEventArgs>(_currentPositionWatcher_StatusChanged);
                _currentPositionWatcher.PositionChanged += new EventHandler<System.Device.Location.GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate>>(_currentPositionWatcher_PositionChanged);
            }
            if (_currentPositionRefreshTimer == null) ;
            {
                _currentPositionRefreshTimer = new DispatcherTimer();
                _currentPositionRefreshTimer.Tick += new EventHandler(_currentPositionRefreshTimer_Tick);
                _currentPositionRefreshTimer.Interval = new TimeSpan(0, 0, 60);
            }
            _currentPositionWatcher.Start();
        }

        void _currentPositionRefreshTimer_Tick(object sender, EventArgs e)
        {
            _currentPositionRefreshTimer.Stop();
            _currentPositionWatcher.Start();
        }

        void _currentPositionWatcher_PositionChanged(object sender, System.Device.Location.GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate> e)
        {
            Debug.WriteLine("new position. {0}", e.Position.Location.ToString());
            Application.Current.RootVisual.Dispatcher.BeginInvoke(() =>
            {
                CurrentPosition = e.Position.Location;
            });
            _currentPositionWatcher.Stop();
            _currentPositionRefreshTimer.Start();
        }

        void _currentPositionWatcher_StatusChanged(object sender, System.Device.Location.GeoPositionStatusChangedEventArgs e)
        {
            Debug.WriteLine("PositionWatcher_StatusChanged: {0}", e.Status);
            Application.Current.RootVisual.Dispatcher.BeginInvoke(() =>
            {
                if (e.Status == System.Device.Location.GeoPositionStatus.NoData)
                {
                    //TODO Glob
                    Error = "Ortungsdienst verfügbar aber keine Daten";
                    MessageBox.Show(Error);

                }
                else if (e.Status == System.Device.Location.GeoPositionStatus.Disabled)
                {
                    //TODO Glob
                    Error = "kein  Ortingsdienst";
                    MessageBox.Show(Error);
                }
                Error = e.Status.ToString();
            });

        }


        private void actionFindCurrentPosition()
        {

        }
        #endregion

        #region Commands

        void actionCmd1()
        {
            MessageBox.Show("Test 1");
        }

        void actionCmd2()
        {
            MessageBox.Show("Test 2");
        }

        #endregion

        private string _Error;
        public string Error
        {
            get { return _Error; }
            set
            {
                if (value != _Error)
                {
                    _Error = value;
                    if (PropertyChanged != null)
                    {
                        try { PropertyChanged(this, new PropertyChangedEventArgs("Error")); }
                        catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Eventfehler\n" + ex.StackTrace); }
                    }
                }
            }
        }

        #region Helper
        /// <summary>
        /// Inits the commands.
        /// </summary>
        /// <returns>a list of command objects</returns>
        List<CommandViewModel> InitCommands()
        {
            List<CommandViewModel> cmds = new List<CommandViewModel>();

            //Eigentlich keine Aktion oder??
            cmds.Add(new CommandViewModel
            (
                "Load Categories", //TODO Language
                new VirtualCommand(param => this.actionLoadCategories())
            ));

            cmds.Add(new CommandViewModel
                        (
                        "Test 1",
                        new VirtualCommand(param => this.actionCmd1())
                        ));

            cmds.Add(new CommandViewModel
                        (
                        "Test 2",
                        new VirtualCommand(param => this.actionCmd2())
                        ));

            return cmds;
        }
        #endregion


        protected void HandleError(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
#if DEBUG
            MessageBox.Show(ex.ToString());
#endif
        }



        #region INotifyPropertyChanged Member

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
