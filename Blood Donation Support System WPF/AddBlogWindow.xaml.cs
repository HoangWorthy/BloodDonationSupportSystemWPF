using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DAL.Entities;
using BLL.Services;
using BLL.Services.Implementations;

namespace BloodDonationSupportSystemWPF
{
    public partial class AddBlogWindow : Window
    {
        private readonly IAccountService _accountService;
        private readonly IBlogService _blogService;

        public AddBlogWindow()
        {
            InitializeComponent();
            _accountService = new AccountService();
            _blogService = new BlogService();

            LoadAuthors();
        }

        private void LoadAuthors()
        {
            var accounts = _accountService.GetAccounts();
            cbAuthor.ItemsSource = accounts;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtContent.Text) ||
                cbAuthor.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            var newBlog = new Blog
            {
                Title = txtTitle.Text,
                Content = txtContent.Text,
                AuthorId = (long)cbAuthor.SelectedValue,
                Status = "Draft",
                LastModifiedDate = DateOnly.FromDateTime(DateTime.Now),
                CreationDate = datePicker.SelectedDate.HasValue
                    ? DateOnly.FromDateTime(datePicker.SelectedDate.Value)
                    : DateOnly.FromDateTime(DateTime.Now)
            };

            // Dùng BlogService
            _blogService.AddBlog(newBlog);

            MessageBox.Show("Đã thêm blog thành công!");
            DialogResult = true;    
            this.Close();
        }
    }
}
