using LibraryManagementSystem.Models;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementSystem.Views
{
    public partial class AdminDashboardWindow : Window
    {
        private List<Book> currentBooks = new List<Book>();

        private List<Book> filteredBooks = new List<Book>();

        private List<User> currentUsers = new List<User>();

        public AdminDashboardWindow()
        {
            InitializeComponent();

            LoadBooks();

            LoadUsers();
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

            LoadBorrowedBooks();

            UpdateDashboardStats();
        }

        public void LoadUsers()
        {
            currentUsers = new List<User>();

            if (File.Exists("users.json"))
            {
                string jsonData =
                    File.ReadAllText("users.json");

                currentUsers =
                    JsonSerializer.Deserialize<List<User>>(jsonData)
                    ?? new List<User>();
            }

            lstUsers.Items.Clear();

            foreach (User user in currentUsers)
            {
                lstUsers.Items.Add(
                    $"{user.Username} - {user.Role}");
            }

            UpdateDashboardStats();
        }

        private void LoadBorrowedBooks()
        {
            lstBorrowedBooks.Items.Clear();

            foreach (Book book in currentBooks.Where(b => b.IsBorrowed))
            {
                int days =
                    (DateTime.Now - book.BorrowDate).Days;

                string overdueText = "";

                if (days >= 14)
                {
                    overdueText = " - OVERDUE";
                }

                lstBorrowedBooks.Items.Add(
                    $"{book.Title} - {book.BorrowedBy} - {days} Days{overdueText}");
            }
        }

        private void FilterBooks()
        {
            string searchText =
                txtSearchBook.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                filteredBooks = currentBooks;
            }
            else
            {
                filteredBooks = currentBooks
                    .Where(b =>
                        (b.Title != null && b.Title.ToLower().Contains(searchText)) ||
                        (b.Author != null && b.Author.ToLower().Contains(searchText)) ||
                        (b.Category != null && b.Category.ToLower().Contains(searchText)) ||
                        (b.ISBN != null && b.ISBN.ToLower().Contains(searchText)))
                    .ToList();
            }

            lstBooks.Items.Clear();

            foreach (Book book in filteredBooks)
            {
                lstBooks.Items.Add(
                    $"{book.Title} - {book.Author}");
            }
        }

        private void UpdateDashboardStats()
        {
            txtTotalBooks.Text =
                currentBooks.Count.ToString();

            int borrowedCount =
                currentBooks.Count(b => b.IsBorrowed);

            txtBorrowedBooks.Text =
                borrowedCount.ToString();

            txtTotalUsers.Text =
                currentUsers.Count.ToString();

            txtStaffCount.Text =
                currentUsers.Count(u => u.Role == "Staff").ToString();

            txtAdminCount.Text =
                currentUsers.Count(u => u.Role == "Admin").ToString();
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

        private void SaveUsers()
        {
            string updatedJson =
                JsonSerializer.Serialize(
                    currentUsers,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(
                "users.json",
                updatedJson);
        }

        private void btnAdd_Book_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow =
                new AddBookWindow();

            addBookWindow.ShowDialog();

            LoadBooks();
        }

        private void btnDeleteBook_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex =
                lstBooks.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a book");

                return;
            }

            MessageBoxResult result =
                MessageBox.Show(
                    "Are you sure you want to delete this book?",
                    "Delete Book",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            Book selectedBook =
                filteredBooks[selectedIndex];

            currentBooks.Remove(selectedBook);

            SaveBooks();

            LoadBooks();

            MessageBox.Show(
                "Book deleted successfully");
        }

        private void btnUpdateBook_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex =
                lstBooks.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a book");

                return;
            }

            if (!int.TryParse(
                txtEditStock.Text,
                out int stock))
            {
                MessageBox.Show(
                    "Stock must be a number");

                return;
            }

            Book selectedBook =
                filteredBooks[selectedIndex];

            selectedBook.Title =
                txtEditTitle.Text;

            selectedBook.Author =
                txtEditAuthor.Text;

            selectedBook.ISBN =
                txtEditISBN.Text;

            selectedBook.Category =
                txtEditCategory.Text;

            selectedBook.Stock = stock;

            SaveBooks();

            LoadBooks();

            MessageBox.Show(
                "Book updated successfully");
        }

        private void btnReturnBook_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex =
                lstBorrowedBooks.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a borrowed book");

                return;
            }

            List<Book> borrowedBooks =
                currentBooks.Where(
                    b => b.IsBorrowed)
                .ToList();

            Book selectedBook =
                borrowedBooks[selectedIndex];

            selectedBook.IsBorrowed = false;

            selectedBook.BorrowedBy = null;

            SaveBooks();

            LoadBooks();

            MessageBox.Show(
                "Book returned successfully");
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

            txtEditTitle.Text =
                selectedBook.Title;

            txtEditAuthor.Text =
                selectedBook.Author;

            txtEditISBN.Text =
                selectedBook.ISBN;

            txtEditCategory.Text =
                selectedBook.Category;

            txtEditStock.Text =
                selectedBook.Stock.ToString();
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

            HistoryPanel.Visibility =
                Visibility.Collapsed;

            UsersPanel.Visibility =
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

            HistoryPanel.Visibility =
                Visibility.Collapsed;

            UsersPanel.Visibility =
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

            HistoryPanel.Visibility =
                Visibility.Collapsed;

            UsersPanel.Visibility =
                Visibility.Collapsed;

            LoadBorrowedBooks();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility =
                Visibility.Collapsed;

            BooksPanel.Visibility =
                Visibility.Collapsed;

            BorrowedPanel.Visibility =
                Visibility.Collapsed;

            HistoryPanel.Visibility =
                Visibility.Visible;

            UsersPanel.Visibility =
                Visibility.Collapsed;
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            DashboardPanel.Visibility =
                Visibility.Collapsed;

            BooksPanel.Visibility =
                Visibility.Collapsed;

            BorrowedPanel.Visibility =
                Visibility.Collapsed;

            HistoryPanel.Visibility =
                Visibility.Collapsed;

            UsersPanel.Visibility =
                Visibility.Visible;

            LoadUsers();
        }

        private void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex =
                lstUsers.SelectedIndex;

            if (selectedIndex != -1)
            {
                txtSelectedRole.Text =
                    currentUsers[selectedIndex].Role;
            }
        }

        private void UpdateUserRole(string role)
        {
            int selectedIndex =
                lstUsers.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a user");

                return;
            }

            currentUsers[selectedIndex].Role =
                role;

            SaveUsers();

            LoadUsers();

            txtSelectedRole.Text =
                role;

            MessageBox.Show(
                "Role updated successfully");
        }

        private void btnMakeUser_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserRole("User");
        }

        private void btnMakeStaff_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserRole("Staff");
        }

        private void btnMakeAdmin_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserRole("Admin");
        }
    }
}