using  PhysicalUnitManagement.Enums;
using  PhysicalUnitManagement.Models;
using  PhysicalUnitManagement.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace  PhysicalUnitManagement.Views
{
    /// <summary>
    /// Vue simplifiée qui délègue toute la logique au ViewModel
    /// </summary>
    public partial class PhysicalUnitModifierView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty PhysicalUnitViewModelProperty =
            DependencyProperty.Register(
                nameof(PhysicalUnitViewModel),
                typeof(PhysicalUnitViewModel),
                typeof(PhysicalUnitModifierView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register(
                nameof(IsEditing),
                typeof(bool),
                typeof(PhysicalUnitModifierView),
                new PropertyMetadata(false));

        #endregion

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

        #endregion

        public PhysicalUnitModifierView()
        {
            InitializeComponent();
        }
    }
}