﻿using System;
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

namespace WpfApp2.View.Citizen
{
    /// <summary>
    /// Interaction logic for VehicleLookup.xaml
    /// </summary>
    public partial class VehicleLookup : UserControl
    {
        public VehicleLookup()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.User.VehicleLookupViewModel();
        }
    }
}
