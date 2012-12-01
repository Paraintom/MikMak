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
using MikMak.Main.GamesManagement;
using MikMak.Main.Security;
using MikMak.Commons;

namespace MorpionTester
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string gameID = "test";
        private GameManager manager;
        private SessionManager sessionManager;
        private Session currentSession;

        public MainWindow()
        {
            var persist = new PersistenceManager();
            sessionManager = new SessionManager(persist);
            manager = new GameManager(persist);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = manager.GetState(currentSession);
                ShowResult(a);
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Game"))
                {
                    gameID = manager.GetNewGame(currentSession, 1, PersistenceManager.GetPlayerId("tom"));
                    var newState = manager.GetState(currentSession);
                    ShowResult(newState);
                }
                else
                {
                    MessageBox.Show("Exception" + ex.ToString());
                }
            }
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
            int  x, y;
            try
            {
                if (Int32.TryParse(textBoxx.Text, out x) && Int32.TryParse(textBoxy.Text, out y))
                {
                    var move = new MikMak.Commons.Move()
                    {
                        PlayerNumber = currentSession.PlayerNumber,
                        Positions = new List<MikMak.Commons.Pawn>(){
                        new MikMak.Commons.Pawn('e', x , y)
                    }
                    };
                    var d = manager.Play(currentSession, move);
                    //Play("test", move);
                    ShowResult(d);

                }
                else
                {
                    this.textBox1.Text = "Err of parsing";
                }
            }
            catch (Exception u)
            {
                MessageBox.Show("Exception : " + u.ToString());
            }
        }

        private void ShowResult(MikMak.Commons.GridState d)
        {
            this.textBox1.Text = GetString(d);
            this.textBoxshowRes.Text = d.CurrentMessage.Information;
        }

        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentSession = sessionManager.GetSession(textBoxLogin.Text, textBoxPassword.Text);
                currentSession = sessionManager.GetSession(currentSession, gameID);
                MessageBox.Show("You are connected : " + currentSession.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.ToString());
            }
        }
    }
}
