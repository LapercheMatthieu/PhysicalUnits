using MatthL.PhysicalUnits.Infrastructure.Repositories;
using System.Windows;
using System.Windows.Controls;

namespace MatthL.PhysicalUnits.UI.Views
{
    /// <summary>
    /// Logique d'interaction pour UnitSystemSelector.xaml
    /// </summary>
    public partial class UnitSystemSelector : UserControl
    {
        public UnitSystemSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Indique si le popup est ouvert
        /// </summary>
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register(
                "IsPopupOpen",
                typeof(bool),
                typeof(UnitSystemSelector),
                new PropertyMetadata(false));

        public bool IsPopupOpen
        {
            get { return (bool)GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }

        private void UnitButton_Click(object sender, RoutedEventArgs e)
        {
            IsPopupOpen = true;
        }

        public bool ShowMetrics
        {
            get { return PhysicalUnitRepository.Settings.ShowMetrics; }
            set { PhysicalUnitRepository.Settings.ShowMetrics = value; }
        }

        public bool ShowImperial
        {
            get { return PhysicalUnitRepository.Settings.ShowImperial; }
            set { PhysicalUnitRepository.Settings.ShowImperial = value; }
        }

        public bool ShowUS
        {
            get { return PhysicalUnitRepository.Settings.ShowUS; }
            set { PhysicalUnitRepository.Settings.ShowUS = value; }
        }

        public bool ShowAstronomic
        {
            get { return PhysicalUnitRepository.Settings.ShowAstronomic; }
            set { PhysicalUnitRepository.Settings.ShowAstronomic = value; }
        }

        public bool ShowOther
        {
            get { return PhysicalUnitRepository.Settings.ShowOther; }
            set { PhysicalUnitRepository.Settings.ShowOther = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsPopupOpen = false;
        }
    }
}