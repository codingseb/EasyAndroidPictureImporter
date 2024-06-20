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
using System.Windows.Shapes;

namespace EasyAndroidPictureImporter.UI.Dialogs;
/// <summary>
/// Interaction logic for ConfigWindow.xaml
/// </summary>
public partial class ConfigWindow : Window
{
    public ConfigWindow()
    {
        InitializeComponent();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
        }
    }

    private void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {

    }
}
