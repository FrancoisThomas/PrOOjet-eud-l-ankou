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
using PrOOjet;

namespace wpf
{
    /// <summary>
    /// Logique d'interaction pour Carte.xaml
    /// </summary>
    public partial class Jeu : UserControl
    {

        ICarte carte;
        IPartie partie;

        public Jeu()
        {
            InitializeComponent();
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            partie = MonteurPartie.INSTANCE.creerPartie(Gaulois.INSTANCE, Viking.INSTANCE, new StrategieNormale());

            // on initialise la Grid (mapGrid défini dans le xaml) à partir de la map du modèle (engine)
            carte = partie.Carte;
            int tailleRectangle = 600 / carte.Taille;
            for (int c = 0; c < carte.Taille; c++)
            {
                mapGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(tailleRectangle, GridUnitType.Pixel) });
                mapGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(tailleRectangle, GridUnitType.Pixel) });
                for (int l = 0; l < carte.Taille; l++)
                {
                    var tile = carte.getCase(c, l);
                    var element = creeRectangle(c, l, tile);
                    mapGrid.Children.Add(element);
                }
            }
        }

        private Rectangle creeRectangle(int c, int l, ICase tile)
        {
            var rectangle = new Rectangle();
            ImageBrush imageBrush = new ImageBrush();
            if (tile is ICaseDesert)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(@"Resources\desert.png", UriKind.Relative));
                rectangle.Fill = imageBrush;
            }
            if (tile is ICaseForet)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(@"Resources\foret.png", UriKind.Relative));
                rectangle.Fill = imageBrush;
            }
            if (tile is ICaseEau)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(@"Resources\eau.png", UriKind.Relative));
                rectangle.Fill = imageBrush;
            }
            if (tile is ICasePlaine)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(@"Resources\plaine.png", UriKind.Relative));
                rectangle.Fill = imageBrush;
            }
            if (tile is ICaseMontagne)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(@"Resources\montagne.png", UriKind.Relative));
                rectangle.Fill = imageBrush;
            }
            // mise à jour des attributs (column et Row) référencant la position dans la grille à rectangle
            Grid.SetColumn(rectangle, c);
            Grid.SetRow(rectangle, l);
            rectangle.Tag = tile; // Tag : ref par defaut sur la tuile logique

            rectangle.Stroke = Brushes.Azure;
            rectangle.StrokeThickness = 1;
            // enregistrement d'un écouteur d'evt sur le rectangle : 
            // source = rectangle / evt = MouseLeftButtonDown / délégué = rectangle_MouseLeftButtonDown
            rectangle.MouseLeftButtonDown += new MouseButtonEventHandler(case_MouseLeftButtonDown);
            return rectangle;
        }

        /// <summary>
        /// Délégué : réponse à l'evt click gauche sur le rectangle, affichage des informations de la tuile
        /// </summary>
        /// <param name="sender"> le rectangle (la source) </param>
        /// <param name="e"> l'evt </param>
        void case_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = sender as Rectangle;
            var tile = rectangle.Tag as ICase;

            int column = Grid.GetColumn(rectangle);
            int row = Grid.GetRow(rectangle);

            // V2 : gestion avec Binding
            // Mise à jour du rectangle selectionné => le label sera mis à jour automatiquement par Binding
            Grid.SetColumn(selectionRectangleMap, column);
            Grid.SetRow(selectionRectangleMap, row);
            selectionRectangleMap.Tag = tile;
            selectionRectangleMap.Width = rectangle.Width;
            selectionRectangleMap.Height = rectangle.Height;
            selectionRectangleMap.Visibility = System.Windows.Visibility.Visible;

            tileImage.Fill = rectangle.Fill;

            updateUnitGrid(partie.selectionneUnites(new Coordonnees(column, row)));

            // on arrête la propagation d'evt : sinon l'evt va jusqu'à la fenetre => affichage via "Window_MouseLeftButtonDown"
            e.Handled = true;
        }


        private void updateUnitGrid(List<IUnite> list)
        {
            if (list != null)
            {
                int i = 0;
                int j = 0;

                foreach (IUnite unit in list)
                {
                    var element = createUnitRectangle(i, j, unit);
                    if (j<5)
                    {
                        j++;
                    }
                    else
                    {
                        i++;
                        j = 0;
                    }
                }
            }
        }

        private Rectangle createUnitRectangle(int c, int l, IUnite unit)
        {
            var rectangle = new Rectangle();
            ImageBrush imageBrush = new ImageBrush();
            if (unit is IUniteGaulois)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(@"Resources\icone_gaulois.png", UriKind.Relative));
                rectangle.Fill = imageBrush;
            }
            if (unit is IUniteNain)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(@"Resources\icone_nain.png", UriKind.Relative));
                rectangle.Fill = imageBrush;
            }
            if (unit is IUniteViking)
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(@"Resources\icone_viking.png", UriKind.Relative));
                rectangle.Fill = imageBrush;
            }
            // mise à jour des attributs (column et Row) référencant la position dans la grille à rectangle
            Grid.SetColumn(rectangle, c);
            Grid.SetRow(rectangle, l);
            rectangle.Tag = unit; // Tag : ref par defaut sur la tuile logique

            rectangle.Stroke = Brushes.Azure;
            rectangle.StrokeThickness = 1;
            // enregistrement d'un écouteur d'evt sur le rectangle : 
            // source = rectangle / evt = MouseLeftButtonDown / délégué = rectangle_MouseLeftButtonDown
            rectangle.MouseLeftButtonDown += new MouseButtonEventHandler(case_MouseLeftButtonDown); // TODO chnager
            return rectangle;
        }

    }
}
