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
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboard = new DashboardWindow();
            if(File.Exists("users.json"))
            {
                string jsonData = File.ReadAllText("users.json");
                List<User> users = JsonSerializer.Deserialize<List<User>>(jsonData);
                User foundUser = users.FirstOrDefault(u => u.Username == txtUsername.Text);

                PasswordHelper helper = new PasswordHelper();

                string hashedPassword = helper.HashPassword(txtPassword.Password);

                if (foundUser == null)
                    MessageBox.Show("User not found");
                else
                {
                    if (foundUser.Password == hashedPassword)
                    {
                        dashboard.Show();
                        this.Close();
                    }
                    else
                        MessageBox.Show("Wrong Password");
                }
            }
            
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            this.Close();
        }
    }
}