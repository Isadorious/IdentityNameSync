using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IdentityNameSync
{
    /// <summary>
    /// Interaction logic for IdentityNameSyncControl.xaml
    /// </summary>
    public partial class IdentityNameSyncControl : UserControl
    {
        private IdentityNameSyncPlugin Plugin { get; }
        public IdentityNameSyncControl() {
            InitializeComponent();
        }

        public IdentityNameSyncControl(IdentityNameSyncPlugin plugin) : this() {
            Plugin = plugin;
            DataContext = plugin.Config;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e) {
            Plugin.Save();
        }
    }
}
