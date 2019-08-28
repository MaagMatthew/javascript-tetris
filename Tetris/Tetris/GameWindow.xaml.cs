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
using System.IO;
using Tetris.Game;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public static Mode mode { get => game.GameMode; set => game.GameMode = value; }
        public static double gravityMod { get => game.GravityMod; set => game.GravityMod = value; }

        public static TetrisGame game { get; set; } = new TetrisGame();

        readonly MediaPlayer mediaPlayer = new MediaPlayer();

        public GameWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
            string[] lines = new string[2];
            try
            {
                lines = File.ReadAllLines("settings.set");

            }
            catch (FileNotFoundException)
            {
                using (StreamWriter file = new StreamWriter("settings.set"))
                {
                    string scheme = ColorScheme.schemes.Keys.ToList()[0];
                    bool mute = false;
                    file.Write(scheme + "\n" + mute);
                    lines[0] = scheme;
                    lines[1] = "" + mute;
                }
            }
            if (lines[1].ToLower().Contains("false"))
            {
                mediaPlayer.Open(new Uri("../../Music/Tetris_-_Theme_A_by_Gori_Fater.mp3", UriKind.Relative));
                mediaPlayer.Volume = 100;
                mediaPlayer.MediaEnded += MusicEnded;
                if(mode == Mode.NIGHTMARE)
                {
                    mediaPlayer.SpeedRatio = .5;
                    Background = Brushes.Black;
                    Foreground = Brushes.Red;
                }
                mediaPlayer.Play();

            }

            SetupGrid();
            SetupBindings();
            //addEvents(); // attach keydown and resize events
            game.Start();
        }

        void SetupBindings()
        {
            Binding binding = new Binding()
            {
                Path = new PropertyPath("Rows"),
                Source = game
            };
            TextRows.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding()
            {
                Path = new PropertyPath("Score"),
                Source = game
            };
            TextScore.SetBinding(TextBlock.TextProperty, binding);

        }

        void SetupGrid()
        {
            GameGrid.Children.Clear();

            Cell[,] gameGrid = game.Grid;
            for(int i = 0; i < gameGrid.GetLength(0); i++)
            {
                for(int j = 0; j < gameGrid.GetLength(1); j++)
                {
                    Cell cell = gameGrid[i, j];

                    Rectangle rect = new Rectangle();
                    Binding binding = new Binding()
                    {
                        Path = new PropertyPath("Current"),
                        Source = cell,
                        Converter = new PieceToColorConverter()
                    };
                    rect.SetBinding(Rectangle.FillProperty, binding);

                    rect.SetValue(Grid.ColumnProperty, i);
                    rect.SetValue(Grid.RowProperty, j);

                    GameGrid.Children.Add(rect);
                }
            }
        }

        void MusicEnded(Object sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(0);
            mediaPlayer.Play();
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mediaPlayer.Stop();
            mainWindow.Show();
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    game.ProcessInput(TetrisGame.Action.Left);
                    break;
                case Key.Right:
                    game.ProcessInput(TetrisGame.Action.Right);
                    break;
                case Key.Up:
                    game.ProcessInput(TetrisGame.Action.Up);
                    break;
                case Key.Down:
                    game.ProcessInput(TetrisGame.Action.Down);
                    break;
            }
        }
    }
}
