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

namespace Tetris
{
    /// <summary>
    /// Interaction logic for LeaderboardWindow.xaml
    /// </summary>
    public partial class LeaderboardWindow : Window
    {
        public LeaderboardWindow()
        {
            Resources["scores"] = Leaderboard.LeaderStored;
            InitializeComponent();
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            GameOverWindow gameOver = new GameOverWindow();
            gameOver.Show();
            this.Close();
        }
    }
}
