using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MyDex
{
    /// <summary>
    /// Interaction logic for ResultsGame.xaml
    /// </summary>
    public partial class ResultsGame : Window, INotifyPropertyChanged
    {
        private Object resurselePentruJoc;
        private List<string> _userAnswers;
        public List<string> UserAnswers
        {
            get { return _userAnswers; }
            set
            {
                _userAnswers = value;
                OnPropertyChanged(nameof(UserAnswers));
            }
        }

        private int _numberOfCorrectAnswers;
        public int NumberOfCorrectAnswers
        {
            get { return _numberOfCorrectAnswers; }
            set
            {
                _numberOfCorrectAnswers = value;
                OnPropertyChanged(nameof(NumberOfCorrectAnswers));
            }
        }


        private List<string> _correctAnswers;
        public List<string> CorrectAnswers
        {
            get { return _correctAnswers; }
            set
            {
                _correctAnswers = value;
                OnPropertyChanged(nameof(CorrectAnswers));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ResultsGame(object dataContext)
        {
            InitializeComponent();

           
            UserAnswers = new List<string>() { "User1", "User2", "User3", "User4", "User5" };
            CorrectAnswers = new List<string>() { "Correct1", "Correct2", "Correct3", "Correct4", "Correct5" };
            NumberOfCorrectAnswers = 0;

            resurselePentruJoc = dataContext;
            DataContext = this;
        }

        
        public void UpdateAnswers(List<string> newUserAnswers, List<string> newCorrectAnswers, int correctAnswerCount)
        {
            
            UserAnswers = newUserAnswers;
            CorrectAnswers = newCorrectAnswers;
            NumberOfCorrectAnswers = correctAnswerCount;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Game game = new Game(resurselePentruJoc);
            this.Visibility = Visibility.Collapsed;
            game.Show();
  
        }
    }
}
