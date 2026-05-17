using LibraryManagementSystem.Models;
using System.IO;
using System.Text.Json;
using System.Windows;

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

            if (File.Exists("books.json"))
            {
                string jsonData =
                    File.ReadAllText("books.json");

                books = JsonSerializer.Deserialize<List<Book>>(jsonData)
                        ?? new List<Book>();
            }

            books.Add(book);

            string updatedJson = JsonSerializer.Serialize(
                books,
                new JsonSerializerOptions()
                {
                    WriteIndented = true
                });

            File.WriteAllText("books.json", updatedJson);

            MessageBox.Show("Book added successfully");

            File.WriteAllText("books.json", updatedJson);

            MessageBox.Show("Book added successfully");

            this.Close();
        }
    }
}