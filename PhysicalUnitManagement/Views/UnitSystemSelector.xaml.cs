using  PhysicalUnitManagement.Models;
using  PhysicalUnitManagement.Services;
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
            get { return PhysicalUnitStorage.ShowMetrics; }
            set { PhysicalUnitStorage.ShowMetrics = value;}
        }
        public bool ShowImperial
        {
            get { return PhysicalUnitStorage.ShowImperial; }
            set { PhysicalUnitStorage.ShowImperial = value; }
        }
        public bool ShowUS
        {
            get { return PhysicalUnitStorage.ShowUS; }
            set { PhysicalUnitStorage.ShowUS = value; }
        }
        public bool ShowAstronomic
        {
            get { return PhysicalUnitStorage.ShowAstronomic; }
            set { PhysicalUnitStorage.ShowAstronomic = value; }
        }
        public bool ShowOther
        {
            get { return PhysicalUnitStorage.ShowOther; }
            set { PhysicalUnitStorage.ShowOther = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsPopupOpen = false;
        }
    }
}
