using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            (DataContext as WordsList).readElements();
            categoryComboBox.SelectedIndex = 0;
            search.TextChanged += Search_TextChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow objLoginWindow = new LoginWindow(this.DataContext);
            this.Close();
            objLoginWindow.Show();
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            (DataContext as WordsList).filter(search.Text, categoryComboBox.SelectedItem as string);
            ShowPopupIfFiltered();

            if (string.IsNullOrEmpty(search.Text))
            {
                MyPopup.IsOpen = false;
            }
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCategory = categoryComboBox.SelectedItem as string;
            (DataContext as WordsList).filter(search.Text, selectedCategory);
            ShowPopupIfFiltered();
        }


        private void ShowPopupIfFiltered()
        {
            if ((DataContext as WordsList).FilterWords.Count > 0 && !string.IsNullOrEmpty(categoryComboBox.SelectedItem as string))
            {
                MyPopup.IsOpen = true;
            }
            else
            {
                MyPopup.IsOpen = false;
            }
        }


        private void MyPopup_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            if (view_list.SelectedItem != null)
            {
               
                Word selectedWord = (Word)view_list.SelectedItem;

               
                search.Text = selectedWord.Name;

               
                categoryComboBox.SelectedItem = selectedWord.Category;

               
                MyPopup.IsOpen = false;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = search.Text;
            w_Image.Stretch = System.Windows.Media.Stretch.Uniform;
            w_Image.StretchDirection = System.Windows.Controls.StretchDirection.Both;
            Word foundWord = (DataContext as WordsList).Words.FirstOrDefault(w => w.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (foundWord != null)
            {
                wordDesctiption.Text = foundWord.Description;

                if (foundWord.Image == "Load Image")
                {
                   
                    w_Image.Source = new BitmapImage(new Uri("\\Images\\default.jpg", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    w_Image.Source = new BitmapImage(new Uri(System.IO.Path.Combine(appDirectory, $"Images/{foundWord.Image}"), UriKind.Absolute));
                }
            }
            else
            {
                MessageBox.Show("Cuvântul nu a fost găsit.");
            }
            search.Text = String.Empty;
            categoryComboBox.SelectedIndex = 0;
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            Game objLoginWindow = new Game(this.DataContext);
            this.Close();
            objLoginWindow.Show();
        }

    }
}
