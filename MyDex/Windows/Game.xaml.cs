using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using MyDex.Properties;

namespace MyDex
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        private List<Word> r_Words;
        private List<bool> r_image_description;
        private List<string> answers = Enumerable.Repeat("", 5).ToList();
        private int questNumber;

        public Game(object dContext)
        {
            InitializeComponent();
            DataContext = dContext;
            IncarcareDate();
        }

        private void IncarcareDate()
        {
            Random random = new Random();
            r_Words = (DataContext as WordsList).Words.OrderBy(x => random.Next()).Take(5).ToList();
            answers = Enumerable.Repeat("", 5).ToList();
            my_Answer.Text = String.Empty;
            r_image_description = new List<bool> { };
            foreach (var word in r_Words)
            {
                if (word.Image == "Load Image")
                {
                    r_image_description.Add(true);
                }
                else
                {
                    double sansa = random.NextDouble();
                    if (sansa < 0.5)
                    {
                        r_image_description.Add(false);
                    }
                    else
                    {
                        r_image_description.Add(true);
                    }

                }
            }
            questNumber = 0;
            SetUi(0);
        }
        private void SetUi(int type)
        {
            if (questNumber == 0 && type == -1) return;
            answers[questNumber] = my_Answer.Text;
            if (questNumber == 4 && type == 1)
            {
                ResultsGame resultsGame = new ResultsGame(DataContext);
                int correctAnswer = 0;
                List<string>arrayOfCorrectAnswer = new List<string>();
                for (int i = 0; i < 5; i++)  
                {
                    if (answers[i] == r_Words[i].Name) correctAnswer++;
                    arrayOfCorrectAnswer.Add(r_Words[i].Name);
                }
                resultsGame.UpdateAnswers(answers, arrayOfCorrectAnswer,correctAnswer);
                this.Visibility = Visibility.Collapsed;
                resultsGame.Show();
                return;
            }
            
            questNumber += type;
            AnswerNumber.Text = $"Question {questNumber + 1}";
            my_Answer.Text = answers[questNumber];
            if (r_image_description[questNumber])
            {
                QuesText.Text = "What is the described word ?";
                DescriptionText.Text = r_Words[questNumber].Description;
                ImageBorder.Visibility = Visibility.Collapsed;
                DescriptionText.Visibility = Visibility.Visible;
            }
            else
            {
                QuesText.Text = "What is the word in the picture ?";
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = System.IO.Path.Combine(appDirectory, "Images", r_Words[questNumber].Image);
                ImageBox.Source = new BitmapImage(new Uri(imagePath));
                DescriptionText.Visibility = Visibility.Collapsed;
                ImageBorder.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetUi(1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetUi(-1);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Visibility = Visibility.Collapsed; 
        }
    }
}
