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
    public partial class PrefixSelectorView : UserControl
    {
        private List<PrefixView> _allButtons;

        public PrefixSelectorView()
        {
            InitializeComponent();
            Loaded += PrefixSelectorView_Loaded;
        }

        private void PrefixSelectorView_Loaded(object sender, RoutedEventArgs e)
        {
            // Récupérer tous les boutons PrefixView
            _allButtons = new List<PrefixView>
            {
                YoctoButton, ZeptoButton, AttoButton, FemtoButton, PicoButton,
                NanoButton, MicroButton, MilliButton, CentiButton, DeciButton,
                SIButton,
                DekaButton, HectoButton, KiloButton, MegaButton, GigaButton,
                TeraButton, PetaButton, ExaButton, ZettaButton, YottaButton
            };

            UpdateSelection();
        }

        // Propriété SelectedPrefix
        public Prefix SelectedPrefix
        {
            get { return (Prefix)GetValue(SelectedPrefixProperty); }
            set { SetValue(SelectedPrefixProperty, value); }
        }

        public static readonly DependencyProperty SelectedPrefixProperty =
            DependencyProperty.Register(nameof(SelectedPrefix), typeof(Prefix), typeof(PrefixSelectorView),
                new FrameworkPropertyMetadata(Prefix.SI, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedPrefixChanged));

        private static void OnSelectedPrefixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (PrefixSelectorView)d;
            view.UpdateSelection();
        }

        private void PrefixButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is PrefixView button)
            {
                SelectedPrefix = button.Prefix;
            }
        }

        private void UpdateSelection()
        {
            if (_allButtons == null) return;

            foreach (var button in _allButtons)
            {
                button.IsSelected = button.Prefix == SelectedPrefix;
            }
        }
    }
}
