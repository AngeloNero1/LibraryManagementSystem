using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text.Json;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnback(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();

            login.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            List<User> users = new List<User>();

            if (File.Exists("users.json"))
            {
                string jsonData = File.ReadAllText("users.json");

                users = JsonSerializer.Deserialize<List<User>>(jsonData)
                        ?? new List<User>();
            }

            User foundUser =
                users.FirstOrDefault(u => u.Username == txtusername.Text);

            if (foundUser == null)
            {
                if (txtusername.Text == "")
                {
                    MessageBox.Show("Please enter your username");
                }

                else if (txtPassword.Password == "" ||
                         txtPassword2.Password == "")
                {
                    MessageBox.Show("Please enter your password");
                }

                else if (txtPassword.Password != txtPassword2.Password)
                {
                    MessageBox.Show("You entered different passwords");
                }

                else
                {
                    PasswordHelper helper = new PasswordHelper();

                    User user = new User();

                    user.Username = txtusername.Text;

                    user.Password =
                        helper.HashPassword(txtPassword.Password);

                    user.Role = "User";

                    users.Add(user);

                    string updatedJson = JsonSerializer.Serialize(
                        users,
                        new JsonSerializerOptions()
                        {
                            WriteIndented = true
                        });

                    File.WriteAllText("users.json", updatedJson);

                    MessageBox.Show("User registered successfully");

                    LoginWindow login = new LoginWindow();

                    login.Show();

                    this.Close();
                }
            }

            else
            {
                MessageBox.Show("We already have that username");
            }
        }
    }
}