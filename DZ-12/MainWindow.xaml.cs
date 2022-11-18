using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Contact editContact;
        private string Email
        {
            get => emailTextBox.Text;
            set => emailTextBox.Text = value;
        }

        private string FirstName
        {
            get => firstNameTextBox.Text;
            set => firstNameTextBox.Text = value;
        }

        private string LastName
        {
            get => lastNameTextBox.Text;
            set => lastNameTextBox.Text = value;
        }

        private string Phone
        {
            get => phoneTextBox.Text;
            set => phoneTextBox.Text = value;
        }

        private Contact SelectedContact => (Contact)((ListBoxItem)contactsListBox.SelectedItem)?.Tag;

        public MainWindow()
        {
            InitializeComponent();
            AddContact(new Contact("1", "1", "1", "1"));
            AddContact(new Contact("2", "2", "2", "2"));
            AddContact(new Contact("3", "3", "3", "3"));
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            editContact = null;
            IsEnabled(true);
        }

        private void IsEnabled(bool znah)
        {
            firstNameTextBox.IsEnabled = znah;
            FirstName = "";
            lastNameTextBox.IsEnabled = znah;
            LastName = "";
            phoneTextBox.IsEnabled = znah;
            Phone = "";
            emailTextBox.IsEnabled = znah;
            Email = "";
            CancelButton.IsEnabled = znah;
            NewButton.IsEnabled = !znah;
            saveButton.IsEnabled = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled(false);
        }

        private void TextIsNull(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                foreach (var i in info.Children)
                    if (i is StackPanel)
                        foreach (var j in ((StackPanel)i).Children)
                            if (j is TextBox)
                                if (String.IsNullOrWhiteSpace(((TextBox)j).Text))
                                {
                                    saveButton.IsEnabled = false;
                                    return;
                                }
                saveButton.IsEnabled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = editContact ??
                new Contact(FirstName, LastName, Email, Phone);
            if (editContact is null)
                AddContact(contact);
            else
                EditContact(contact);
            IsEnabled(false);
        }

        private void EditContact(Contact contact)
        {
            int n = contactsListBox.SelectedIndex;
            contact.email = Email;
            contact.phone = Phone;
            for (int i = 0; i < contactsListBox.Items.Count; i++)
                if (((ListBoxItem)contactsListBox.Items[i]).Tag == contact)
                {
                    ListBoxItem listBoxItem = CreateListBoxItem(contact);
                    contactsListBox.Items[i] = listBoxItem;
                    contactsListBox.SelectedIndex = n;
                    break;
                }
        }
        private void SelectContactToView(Contact contact)
        {
            selectedEmailTextBlock.Text = FormatContactEmail(contact);
            selectedNameTextBlock.Text = FormatContactName(contact);
            selectedPhoneTextBlock.Text = FormatContactPhone(contact);
        }

        private void ContactsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contactsListBox.SelectedItem is null)
                ClearContactToView();
            else
                SelectContactToView(SelectedContact);
        }

        private void ClearContactToView()
        {
            selectedEmailTextBlock.Text = string.Empty;
            selectedNameTextBlock.Text = string.Empty;
            selectedPhoneTextBlock.Text = string.Empty;
        }

        private ListBoxItem CreateListBoxItem(Contact contact)
        {
            var nameTextBlock = new TextBlock { FontWeight = FontWeights.Bold, Text = FormatContactName(contact) };

            var emailTextBlock = new TextBlock { Text = FormatContactEmail(contact) };

            var phoneTextBlock = new TextBlock { Text = FormatContactPhone(contact) };

            var editButton = new Button { Content = "Edit" };
            editButton.Click += (sender, e) => SelectContactToEdit(contact);

            var deleteButton = new Button { Content = "Delete" };
            deleteButton.Click += (sender, e) => DeleteContact(contact);

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { SharedSizeGroup = "Name" });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5.0) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { SharedSizeGroup = "Email" });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5.0) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { SharedSizeGroup = "Phone" });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5.0) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50.0) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5.0) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50.0) });
            grid.Children.Add(nameTextBlock);
            grid.Children.Add(emailTextBlock);
            grid.Children.Add(phoneTextBlock);
            grid.Children.Add(editButton);
            grid.Children.Add(deleteButton);

            Grid.SetColumn(nameTextBlock, 0);
            Grid.SetColumn(emailTextBlock, 2);
            Grid.SetColumn(phoneTextBlock, 4);
            Grid.SetColumn(editButton, 6);
            Grid.SetColumn(deleteButton, 8);

            return new ListBoxItem { Content = grid, Tag = contact };
        }

        private void AddContact(Contact contact)
        {
            ListBoxItem listBoxItem = CreateListBoxItem(contact);

            contactsListBox.Items.Add(listBoxItem);
        }

        private void SelectContactToEdit(Contact contact)
        {
            IsEnabled(true);
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;
            saveButton.IsEnabled = true;
            UpdateInputs(contact);
            editContact = contact;
        }

        private void UpdateInputs(Contact contact)
        {
            Email = contact.email;
            FirstName = contact.firstName;
            LastName = contact.lastName;
            Phone = contact.phone;
        }

        private void DeleteContact(Contact contact)
        {
            foreach (ListBoxItem listBoxItem in contactsListBox.Items)
                if (listBoxItem.Tag == contact)
                {
                    contactsListBox.Items.Remove(listBoxItem);
                    break;
                }
        }

        private string FormatContactName(Contact contact)
        {
            return $"{contact.firstName} {contact.lastName}";
        }

        private string FormatContactEmail(Contact contact)
        {
            return contact.email.ToLower();
        }

        private string FormatContactPhone(Contact contact)
        {
            return $" {contact.phone}";
        }
    }
    public class Contact
    {
        public string email = string.Empty;
        public string phone = string.Empty;
        public string firstName = string.Empty;
        public string lastName = string.Empty;

        public Contact(string firstName, string lastName,
            string email, string phone)
        {
            this.email = email;
            this.lastName = lastName;
            this.firstName = firstName;
            this.phone = phone;
        }
    }
}
