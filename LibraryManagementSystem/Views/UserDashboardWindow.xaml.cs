using LibraryManagementSystem.Models;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementSystem.Views
{
    public partial class UserDashboardWindow : Window
    {
        private List<Book> currentBooks = new List<Book>();

        private List<Book> filteredBooks = new List<Book>();

        private string currentUsername;

        public UserDashboardWindow(string username)
        {
            InitializeComponent();

            currentUsername = username;

            LoadBooks();
        }

        public void LoadBooks()
        {
            currentBooks = new List<Book>();

            if (File.Exists("books.json"))
            {
                string jsonData =
                    File.ReadAllText("books.json");

                currentBooks =
                    JsonSerializer.Deserialize<List<Book>>(jsonData)
                    ?? new List<Book>();
            }

            FilterBooks();

            txtTotalBooks.Text =
                currentBooks.Count.ToString();

            int borrowedCount =
                currentBooks.Count(b => b.IsBorrowed);

            txtBorrowedBooks.Text =
                borrowedCount.ToString();

            LoadBorrowedBooks();
        }

        private void LoadBorrowedBooks()
        {
            lstBorrowedBooks.Items.Clear();

            foreach (Book book in currentBooks.Where(
                b => b.IsBorrowed &&
                b.BorrowedBy == currentUsername))
            {
                lstBorrowedBooks.Items.Add(
                    $"{book.Title} - {book.Author}");
            }
        }

        private void FilterBooks()
        {
            string searchText =
                txtSearchBook.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                filteredBooks =
                    currentBooks
                    .Where(b => !b.IsBorrowed)
                    .ToList();
            }
            else
            {
                filteredBooks = currentBooks
                    .Where(b =>
                        !b.IsBorrowed &&
                        (
                            (b.Title != null && b.Title.ToLower().Contains(searchText)) ||
                            (b.Author != null && b.Author.ToLower().Contains(searchText)) ||
                            (b.Category != null && b.Category.ToLower().Contains(searchText))
                        ))
                    .ToList();
            }

            lstBooks.Items.Clear();

            foreach (Book book in filteredBooks)
            {
                lstBooks.Items.Add(
                    $"{book.Title} - {book.Author}");
            }
        }

        private void SaveBooks()
        {
            string updatedJson =
                JsonSerializer.Serialize(
                    currentBooks,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(
                "books.json",
                updatedJson);
        }

        private void lstBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex =
                lstBooks.SelectedIndex;

            if (selectedIndex == -1)
            {
                return;
            }

            Book selectedBook =
                filteredBooks[selectedIndex];

            txtTitle.Text =
                selectedBook.Title;

            txtAuthor.Text =
                selectedBook.Author;

            txtCategory.Text =
                selectedBook.Category;

            txtStock.Text =
                selectedBook.Stock.ToString();
        }

        private void btnBorrowBook_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex =
                lstBooks.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a book");

                return;
            }

            Book selectedBook =
                filteredBooks[selectedIndex];

            if (selectedBook.IsBorrowed)
            {
                MessageBox.Show(
                    "Book already borrowed");

                return;
            }

            selectedBook.IsBorrowed = true;

            selectedBook.BorrowedBy =
                currentUsername;

            selectedBook.BorrowDate =
                DateTime.Now;

            SaveBooks();

            LoadBooks();

            MessageBox.Show(
                "Book borrowed successfully");
        }

        private void txtSearchBook_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterBooks();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login =
                new LoginWindow();

            login.Show();

            this.Close();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility =
                Visibility.Visible;

            BooksPanel.Visibility =
                Visibility.Collapsed;

            BorrowedPanel.Visibility =
                Visibility.Collapsed;
        }

        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility =
                Visibility.Collapsed;

            BooksPanel.Visibility =
                Visibility.Visible;

            BorrowedPanel.Visibility =
                Visibility.Collapsed;

            LoadBooks();
        }

        private void btnBorrowedBooks_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility =
                Visibility.Collapsed;

            BooksPanel.Visibility =
                Visibility.Collapsed;

            BorrowedPanel.Visibility =
                Visibility.Visible;
        }
    }
}