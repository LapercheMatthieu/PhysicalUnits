using MatthL.PhysicalUnits.Infrastructure.Repositories;
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

namespace MatthL.PhysicalUnits.UI.Views.RepositorySettingViews
{
    /// <summary>
    /// Logique d'interaction pour RepositorySettingButtonView.xaml
    /// </summary>
    public partial class RepositorySettingButtonView : UserControl
    {
        public RepositorySettingButtonView()
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
                typeof(RepositorySettingButtonView),
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
