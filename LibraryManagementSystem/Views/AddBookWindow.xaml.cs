using LibraryManagementSystem.Models;
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

namespace LibraryManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            Book book = new Book();
            book.Title = txtTitle.Text;
            book.Author = txtAuthor.Text;
            book.ISBN = txtISBN.Text;
            book.Category = txtCategory.Text;
            book.Stock = Convert.ToInt32(txtStock.Text);

            List<Book> books = new List<Book>();

            if(File.Exists("Books.json"))
            {
                string jsonData = File.ReadAllText("Books.json");
                books = JsonSerializer.Deserialize<List<Book>>(jsonData)
                ?? new List<Book>();

                books.Add(book);
                jsonData = JsonSerializer.Serialize(books);
                File.WriteAllText("Books.json", jsonData);

                MessageBox.Show("Book added successfully");
            }
        }
    }
}
