using MatthL.PhysicalUnits.UI.ViewModels;
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

namespace MatthL.PhysicalUnits.UI.Views.PhysicalUnitViews
{
    /// <summary>
    /// Logique d'interaction pour PhysicalUnitView.xaml
    /// </summary>
    public partial class PhysicalUnitView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty PhysicalUnitViewModelProperty =
            DependencyProperty.Register(
                nameof(PhysicalUnitViewModel),
                typeof(PhysicalUnitViewModel),
                typeof(PhysicalUnitView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register(
                nameof(IsEditing),
                typeof(bool),
                typeof(PhysicalUnitView),
                new PropertyMetadata(false));

        #endregion Dependency Properties

        #region Properties

        public PhysicalUnitViewModel PhysicalUnitViewModel
        {
            get => (PhysicalUnitViewModel)GetValue(PhysicalUnitViewModelProperty);
            set => SetValue(PhysicalUnitViewModelProperty, value);
        }

        public bool IsEditing
        {
            get => (bool)GetValue(IsEditingProperty);
            set => SetValue(IsEditingProperty, value);
        }

        #endregion Properties

        public PhysicalUnitView()
        {
            InitializeComponent();
        }
    }
}
