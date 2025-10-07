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
    public partial class PrefixSelectorButtonView : UserControl
    {
        public PrefixSelectorButtonView()
        {
            InitializeComponent();
        }

        // Propriété SelectedPrefix
        public Prefix SelectedPrefix
        {
            get { return (Prefix)GetValue(SelectedPrefixProperty); }
            set { SetValue(SelectedPrefixProperty, value); }
        }

        public static readonly DependencyProperty SelectedPrefixProperty =
            DependencyProperty.Register(nameof(SelectedPrefix), typeof(Prefix), typeof(PrefixSelectorButtonView),
                new FrameworkPropertyMetadata(Prefix.SI, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedPrefixChanged));

        private static void OnSelectedPrefixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (PrefixSelectorButtonView)d;
            view.PopupButton.ClosePopup();
        }
    }
}
