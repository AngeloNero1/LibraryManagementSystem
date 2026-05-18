# 📚 Library Management System

A desktop-based Library Management System developed with **C#**, **WPF**, and **.NET 8**.  
This application provides a role-based environment for managing books and users in a library system.

---

## Overview

The project is designed to simulate a simple real-world library system with different user roles:

- **Admin** – full system control
- **Staff** – manages books and borrowing operations
- **User** – searches and borrows books

The application uses a clean WPF interface and stores data locally using JSON files.

---

## Features

### Authentication
- User registration
- User login
- Password hashing with SHA256
-- Role-based access control

### Admin Functions
- Add new books
- Update book information
- Delete books
- Manage users
- Access admin dashboard

### Staff Functions
- View all books
- Update book details
- Remove books
- Manage borrowing operations

### User Functions
- Browse available books
- Search books
- Borrow books
- View borrowed books

---

## Project Structure

```text
LibraryManagementSystem/
│
├── Models/           # Data models (Book, User)
├── Services/         # Helper services (Password hashing)
├── Views/            # WPF windows and UI screens
├── App.xaml          # Application entry point
└── LibraryManagementSystem.csproj
