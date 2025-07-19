using  PhysicalUnitManagement.Models;
using  PhysicalUnitManagement.ViewModels;
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

namespace  PhysicalUnitManagement.Views
{
    /// <summary>
    /// Logique d'interaction pour BaseUnitColumn.xaml
    /// </summary>
    public partial class BaseUnitColumn : UserControl
    {
        public BaseUnitColumn()
        {
            InitializeComponent();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (BaseUnitViewModel)DataContext;
            vm.RaiseAskDeletion();
        }
    }
}
