using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(object dContext)
        {
            InitializeComponent();
            DataContext = dContext;
            getAdmins();
        }

        string filepath = @"D:\C#\MyDex\LoginTXT\admin.txt";

        //arraylist of admins
        ArrayList alladmins = new ArrayList();

        //dictionary of admins and passwords
        Dictionary<string,string> adminsANDpassword = new Dictionary<string,string>();

        //create a function to get all the admins and passwords
        public void getAdmins()
        {
            string[] line = File.ReadAllLines(filepath);
            string[] values;
            string ad = "";
            string psw = "";

            for (int i = 0; i < line.Length; i++) 
            {
                values = line[i].ToString().Split(':');

                if (values[0].Trim().Equals("Username"))
                {
                    ad = values[1];
                    //add admins to arrayList
                    alladmins.Add(ad);
                }
                else if(values[0].Trim().Equals("Password"))
                {
                    psw = values[1];
                }
                if (ad != "" && psw != "")
                {
                    //add to dictionary
                    adminsANDpassword.Add(ad, psw);
                    //clear admins and password
                    ad = "";
                    psw = "";
                }
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string admin = textBoxUsername.Text;
            string password = textBoxPassword.Password;
            bool adminExist = false;

            if (admin.Trim().Equals("") || password.Equals(""))
            {
                MessageBox.Show("You Need To Enter The Username And Password!");
            }
            else {
                foreach (var uname in adminsANDpassword)
                {
                    if (uname.Key.ToString().Trim().Equals(admin))
                    {
                        if (uname.Value.Trim().Equals(password))
                        { 
                            adminExist = true; 
                            break; 
                        }
                    }
                }
                if (adminExist)
                {
                    AdminController adminController = new AdminController(this.DataContext);
                    this.Visibility = Visibility.Collapsed;
                    adminController.Show();
                }
                else
                {
                    MessageBox.Show("No");
                }
            }
        }
    }
}
