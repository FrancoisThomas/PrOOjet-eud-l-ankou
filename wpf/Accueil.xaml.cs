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

namespace wpf
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : UserControl
    {
        private Smallworld mainWindow;


        public Accueil()
        {
            InitializeComponent();
        }



        public void ajoutReference(Smallworld main)
        {
         mainWindow = main;
        }

        public void nouvellePartie(object sender, RoutedEventArgs e) 
        {
            Visibility = Visibility.Collapsed;
            mainWindow.EcranLancement.Visibility = Visibility.Visible;
        }
    }
}
