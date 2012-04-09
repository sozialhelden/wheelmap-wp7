using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using Sozialhelden.Wheelmap.Lib.Global;

namespace Sozialhelden.Wheelmap.Lib.ViewModel
{
    public class MainViewModel
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
          
            _data.APIKey = _settings.APIKey;
            if (string.IsNullOrEmpty(_settings.Locale) == false) _data.Locale = _settings.Locale;
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

        public ObservableCollection<CategoryViewModel> Categories { get; private set;}

        #region Commands

        public void actionLoadCategories()
        {
            try
            {
                Categories.Add(new CategoryViewModel("test", "te"));
                return;
                foreach (var cat in _data.GetCategories())
                {
                    Categories.Add(new CategoryViewModel("test","te"));
                }
            }
            catch (System.Exception ex)
            {
                HandleError(ex);
            }
        }

        void actionCmd1()
        {
            MessageBox.Show("Test 1");
        }

        void actionCmd2()
        {
            MessageBox.Show("Test 2");
        }

        #endregion

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
            new VirtualCommand(param => this.actionCmd1())
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


    }
}
