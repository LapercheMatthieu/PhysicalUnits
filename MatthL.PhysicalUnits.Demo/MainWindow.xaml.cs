
using  PhysicalUnitManagement.Models;
using  PhysicalUnitManagement.Services;
using SQLiteManager.Authorizations;
using SQLiteManager.Managers;
using System.Windows;

namespace  PhysicalUnitManager.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PhysicalUnitStorage.Initialize();
            InitializeComponent();

            Test();
            DataContext = new SimpleVM();
        }
        private async void Test()
        {
            
           // UnitCB.DurationTypeCombobox();
        }


    }
}