using System;
using System.Collections.Generic;
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
using Morpion;

namespace MorpionTester
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MikMak.Interfaces.IGameManager manager;
        public MainWindow()
        {
            InitializeComponent();
            manager = new MorpionManager();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var a = manager.GetState("test");
            ShowResult(a);
        }

        private String GetString(MikMak.Commons.GridState a)
        {
            char[] allData = new char[9];
            for (int i = 0; i < 9; i++)
            {
                allData[i] = 'O';
            }

            foreach (var item in a.PawnLocations)
            {
                int i = item.Coord.x - 1 + 3 * (item.Coord.y - 1);
                allData[i] = item.Name;
            }
            return new string(allData);
        }

        private void buttonpl_Click(object sender, RoutedEventArgs e)
        {
            int joueur, x, y;

            if (Int32.TryParse(textBoxjoueur.Text, out joueur) && Int32.TryParse(textBoxx.Text, out x) && Int32.TryParse(textBoxy.Text, out y))
            {
                var move = new MikMak.Commons.Move()
                {
                    PlayerId = joueur,
                    Positions = new List<MikMak.Commons.Pawn>(){
                        new MikMak.Commons.Pawn('e', x , y)
                    }
                };
                var d = manager.Play("test", move);
                ShowResult(d);

            }
            else
            {

                this.textBox1.Text = "Err";
            }
        }

        private void ShowResult(MikMak.Commons.GridState d)
        {
            this.textBox1.Text = GetString(d);
            this.textBoxshowRes.Text = d.CurrentMessage.Information;
        }
    }
}
