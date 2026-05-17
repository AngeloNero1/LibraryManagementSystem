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
using System.Windows.Shapes;
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
            if(File.Exists("users.json"))
            {
                string jsonData = File.ReadAllText("users.json");
                List<User> users = JsonSerializer.Deserialize<List<User>>(jsonData) ?? new List<User>();
                User foundUser = users.FirstOrDefault(u => u.Username == txtusername.Text);

                if (foundUser == null)
                {

                    if (txtusername.Text == "")
                    {
                        MessageBox.Show("Please enter your username");

                    }
                    else
                    {
                        if (txtPassword.Password == "" || txtPassword2.Password == "")
                        {
                            MessageBox.Show("Please enter your password");

                        }
                        else if (txtPassword.Password != txtPassword2.Password)
                        {
                            MessageBox.Show("You entered difference passwords ");
                        }
                        else
                        {

                            PasswordHelper helper = new PasswordHelper();
                            User user = new User();
                            user.Username = txtusername.Text;
                            user.Password = helper.HashPassword(txtPassword.Password);

                            List<User> users2 = new List<User>();

                            if (File.Exists("users.json"))
                            {
                                string existingJson = File.ReadAllText("users.json");

                                users2 = JsonSerializer.Deserialize<List<User>>(existingJson) ?? new List<User>();
                            }

                            users2.Add(user);

                            string jsonData1 = JsonSerializer.Serialize(users2);

                            File.WriteAllText("users.json", jsonData1);

                            MessageBox.Show("User registered successfully");

                            LoginWindow login = new LoginWindow();
                            login.Show();
                            this.Close();

                        }
                    }
                }
                else
                    MessageBox.Show("We Already Have That Username");
            }
            
        }
    }
}