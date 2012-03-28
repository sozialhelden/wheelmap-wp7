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
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Sozialhelden.Wheelmap.Lib.Global.ViewModel
{
    public class MainViewModel
    {
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

        #region Helper
        /// <summary>
        /// Inits the commands.
        /// </summary>
        /// <returns>a list of command objects</returns>
        List<CommandViewModel> InitCommands()
        {
            List<CommandViewModel> cmds = new List<CommandViewModel>();
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



    }
}
