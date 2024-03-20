using Microsoft.Win32;
using System;
using System.IO;
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
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
namespace MyDex
{
    /// <summary>
    /// Interaction logic for AdminController.xaml
    /// </summary>
    public partial class AdminController : Window

    {

        private string imagePath;
        public AdminController(object dContext)
        {
            InitializeComponent();
            DataContext = dContext;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Word w = new Word(CuvantBox.Text,Descriere.Text,Categorie.Text, ImageB.Content.ToString());
            if ((DataContext as WordsList).IsInWords(w.Name) == false)
            {
                if (w.Image != "Load Image")
                {
                    copyImage(w.Image);
                }
                (DataContext as WordsList).AddWord(w);
                System.Windows.MessageBox.Show("Word added!!");
            }
            else 
            {
                Word updateWord = (DataContext as WordsList).GetWord(w.Name);
                if (updateWord.Name != null)
                {
                    updateWord.Name = CuvantBox.Text;
                    updateWord.Description = Descriere.Text;
                    updateWord.Category = Categorie.Text;
                    updateWord.Image = ImageB.Content.ToString();
                    if (w.Image != "Load Image")
                    {
                        copyImage(w.Image);
                    }
                    (DataContext as WordsList).DeleteWord(updateWord.Name);
                    (DataContext as WordsList).AddWord(updateWord);
                }
            }
            
            CuvantBox.Text = "";
            Descriere.Text = "";
            Categorie.Text = "";
            ImageB.Content = "Load Image";
            
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (.jpg,.jpeg, .png)|.jpg;.jpeg;.png";
            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(imagePath);
                ImageB.Content = fileName;
                System.Windows.MessageBox.Show("Imaginea a fost selectata cu succes!");
            }
        }

        private void copyImage(string imageName)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string destinationFolder = System.IO.Path.Combine(appDirectory, "Images");
            string destinationPath = System.IO.Path.Combine(destinationFolder, imageName);
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }
            try
            {
                if (!File.Exists(destinationPath))
                {
                    File.Copy(imagePath, destinationPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la accesarea imagini: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedItem as Word != null)
            {
                (DataContext as WordsList).SelectedWord = (sender as ListBox).SelectedItem as Word;
                CuvantBox.Text = (DataContext as WordsList).SelectedWord.Name;
                Descriere.Text = (DataContext as WordsList).SelectedWord.Description;
                Categorie.Text = (DataContext as WordsList).SelectedWord.Category;
                ImageB.Content = (DataContext as WordsList).SelectedWord.Image;
            }
        }

        private void Delete_Button(object sender, RoutedEventArgs e)
        {
            if (CuvantBox.Text != "")
            {
                (DataContext as WordsList).DeleteWord(CuvantBox.Text);
                CuvantBox.Text = string.Empty;
                Descriere.Text = string.Empty;
                Categorie.Text = string.Empty;
                ImageB.Content = "Load Image";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void ListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            ((ListView)sender).SelectedItem = null;

            CuvantBox.Text = string.Empty;
            Descriere.Text = string.Empty;
            Categorie.Text = string.Empty;
            ImageB.Content = "Load Image";

        }

    }
}
