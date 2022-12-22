using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Threading;
using System.Windows.Threading;

namespace InvokerGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);
        static int level = 1;
        static int nextSpell = 2500;
        static Random rnd = new Random();
        static int sk1 = 1, sk2 = 2, sk3 = 3;
        static List<skill> Skills = new List<skill>();
        static ImageSource quas = new BitmapImage(new Uri("pack://application:,,,/icons/invoker-quas.jpg"));
        static ImageSource wex = new BitmapImage(new Uri("pack://application:,,,/icons/invoker-wex.jpg"));
        static ImageSource exort = new BitmapImage(new Uri("pack://application:,,,/icons/invoker-exort.jpg"));
		static int score;
        public MainWindow()
        {
            InitializeComponent();
            GameField.Focus();
        }

        class skill
        {
            public ImageBrush ico = new ImageBrush();
            public int QuasCount, WexCount, ExortCount;
            public string name;
            public skill(string name, int quas, int wex, int exort)
            {
	            QuasCount = quas;
	            WexCount = wex;
	            ExortCount = exort;
	            this.name = name;
                ico.ImageSource = new BitmapImage(new Uri("pack://application:,,,/icons/invoker-" + name + ".jpg"));
            }   
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += GameTick;
            generateAllSkills();
            randomOnStart();
            timer.Start();
        }

        private void GameTick(object sender, EventArgs e)
        {
            foreach(var x in GameField.Children.OfType<Rectangle>())
            {
                var speed = 1 * level;
                Canvas.SetTop(x, Canvas.GetTop(x) + speed);
                if(Canvas.GetTop(x) > 480)
                {
                    Canvas.SetTop(x, rnd.Next(-635, -500));
                    Canvas.SetLeft(x, rnd.Next(10, 370));
                    var newspell = rnd.Next(0, 10);
                    x.Fill = Skills[newspell].ico;
                    x.Name = Skills[newspell].name;
                }
				
            }

            //scoreLabel.Content = Canvas.GetTop(GameField.Children.OfType<Rectangle>().ElementAt(3));

        }

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Q)
			{
				skill1.Fill = skill2.Fill;
				sk1 = sk2;
				skill2.Fill = skill3.Fill;
				sk2 = sk3;
				ImageBrush newspellico = new ImageBrush();
				newspellico.ImageSource = quas;
				skill3.Fill = newspellico;
				sk3 = 1;
			}
            else if (e.Key == Key.W)
			{
				skill1.Fill = skill2.Fill;
				sk1 = sk2;
				skill2.Fill = skill3.Fill;
				sk2 = sk3;
				ImageBrush newspellico = new ImageBrush();
				newspellico.ImageSource = wex;
				skill3.Fill = newspellico;
				sk3 = 2;
			}
            else if (e.Key == Key.E)
			{
				skill1.Fill = skill2.Fill;
				sk1 = sk2;
				skill2.Fill = skill3.Fill;
				sk2 = sk3;
				ImageBrush newspellico = new ImageBrush();
				newspellico.ImageSource = exort;
				skill3.Fill = newspellico;
				sk3 = 3;
			}
            else if (e.Key == Key.R)
			{
				int cQuas = 0, cWex = 0, cExort = 0;
				if (sk1 == 1)
				{
					cQuas++;
				}
                else if (sk1 == 2)
				{
					cWex++;
				}
                else if (sk1 == 3)
				{
					cExort++;
				}
				if (sk2 == 1)
				{
					cQuas++;
				}
				else if (sk2 == 2)
				{
					cWex++;
				}
				else if (sk2 == 3)
				{
					cExort++;
				}
				if (sk3 == 1)
				{
					cQuas++;
				}
				else if (sk3 == 2)
				{
					cWex++;
				}
				else if (sk3 == 3)
				{
					cExort++;
				}
                foreach (var x in GameField.Children.OfType<Rectangle>().OrderByDescending(s => Canvas.GetTop(s)))
                {
	                if (Canvas.GetTop(x) > -80 && Canvas.GetTop(x) < 440)
	                {
		                var currentCast = sk1.ToString() + sk2.ToString() + sk3.ToString();
		                var spell = Skills.Find(p => p.name == x.Name);
		                if (cQuas == spell.QuasCount && cWex == spell.WexCount && cExort == spell.ExortCount)
		                {
			                Canvas.SetTop(x, rnd.Next(-635, -400));
			                Canvas.SetLeft(x, rnd.Next(10, 370));
			                var newspell = rnd.Next(0, 10);
			                x.Fill = Skills[newspell].ico;
			                x.Name = Skills[newspell].name;
			                score++;
			                scoreLabel.Content = score.ToString();
			                if (score % 40 == 0) level++;
			                levelTextBox.Text = level.ToString();
                            break;
		                }

	                }
                }

			}
        }



		private void levelTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!char.IsDigit(e.Text,0))
			{
				e.Handled = true;
			}
		}

		private void levelTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				GameField.Focus();
				level = Convert.ToInt32(levelTextBox.Text);
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			AllSpells allSpells = new AllSpells();
			allSpells.Show();
		}

		void randomOnStart()
        {
            foreach (var x in GameField.Children.OfType<Rectangle>())
            {
                Canvas.SetLeft(x, rnd.Next(10, 370));
				var newspell = rnd.Next(0, 10);
				x.Fill = Skills[newspell].ico;
				x.Name = Skills[newspell].name;

			}
        }

        void generateAllSkills()
        {
            Skills.Add(new skill("cold", 3,0,0));
            Skills.Add(new skill("ghost", 2,1,0));
            Skills.Add(new skill("ice", 2,0,1));
            Skills.Add(new skill("tornado", 1,2,0));
            Skills.Add(new skill("emp",0,3,0));
            Skills.Add(new skill("alacrity", 0,2,1));
            Skills.Add(new skill("forge", 1,0,2));
            Skills.Add(new skill("chaos", 0,1,2));
            Skills.Add(new skill("sunstrike", 0,0,3));
            Skills.Add(new skill("blast", 1,1,1));
        }
    }
}
