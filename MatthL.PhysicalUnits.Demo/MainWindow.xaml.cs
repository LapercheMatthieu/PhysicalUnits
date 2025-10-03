using MatthL.PhysicalUnits.Infrastructure.Repositories;
using System.Windows;

namespace MatthL.PhysicalUnits.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PhysicalUnitRepository.Initialize();
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