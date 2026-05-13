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


        private void btnRegister_Click(object sender, RoutedEventArgs e)
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
                    User user = new User();
                    user.Username = txtusername.Text;
                    user.Password = txtPassword.Password;

                    List<User> users = new List<User>();

                    if (File.Exists("users.json"))
                    {
                        string existingJson = File.ReadAllText("users.json");

                        users = JsonSerializer.Deserialize<List<User>>(existingJson);
                    }

                    users.Add(user);

                    string jsonData = JsonSerializer.Serialize(users);

                    File.WriteAllText("users.json", jsonData);

                    MessageBox.Show("User registered successfully");

                }
            }
        }
    }
}