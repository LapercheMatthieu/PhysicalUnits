using MatthL.PhysicalUnits.Core.Enums;
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

namespace MatthL.PhysicalUnits.UI.Views.PrefixViews
{
    public partial class PrefixView : UserControl
    {
        public PrefixView()
        {
            InitializeComponent();
        }

        // Propriété Prefix
        public Prefix Prefix
        {
            get { return (Prefix)GetValue(PrefixProperty); }
            set { SetValue(PrefixProperty, value); }
        }

        public static readonly DependencyProperty PrefixProperty =
            DependencyProperty.Register(nameof(Prefix), typeof(Prefix), typeof(PrefixView),
                new PropertyMetadata(Prefix.SI));

        // Propriété IsSelected pour l'effet visuel
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(PrefixView),
                new PropertyMetadata(false));

        // Event Click exposé
        public event RoutedEventHandler Click;

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
