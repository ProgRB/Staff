﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManningTable
{
    /// <summary>
    /// Interaction logic for WorkPlaceUserControl.xaml
    /// </summary>
    public partial class WorkPlaceUserControl : UserControl
    {
        static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(WorkPlaceUserControl),
            new PropertyMetadata(false));
        public WorkPlaceUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Является ли контрол только чтением данных
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }
    }
}