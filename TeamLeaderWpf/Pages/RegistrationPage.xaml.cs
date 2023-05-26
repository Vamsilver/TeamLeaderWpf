using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamLeaderWpf.Core;
using TeamLeaderWpf.Mongo;

namespace TeamLeaderWpf.Pages
{
    /// <summary>
    /// Interaction logic for PasswordRecoveryPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
            EmailTextBox.Text = "Введите электронную почту";
            GroupTextBox.Text = "Введите номер группы";
        }


        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "Введите электронную почту")
            {
                EmailTextBox.Text = "";
            }
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "")
            {
                EmailTextBox.Text = "Введите электронную почту";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "Введите электронную почту" && GroupTextBox.Text == "Введите номер группы")
            {
                NavigationService.GoBack();
            }
            else if (!IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Ввведите именно gmail");
            }
            else
            {
                SmtpClient client = new SmtpClient();
                client.Credentials = new NetworkCredential("mck-ktits@internet.ru", "Sb16dbUywA1LnUwKWXLJ");
                client.Host = "smtp.mail.ru";
                client.Port = 587;
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("mck-ktits@internet.ru");
                mail.To.Add(new MailAddress(EmailTextBox.Text));
                mail.Subject = "Ваши данные от нового аккаунта";
                mail.IsBodyHtml = true;
                mail.Body = $"Уважаемый староста {GroupTextBox.Text} группы. <br> Благодарим за оставленную заявку на доступ к нашему сервису." +
                    $"<br> Ваши данные для входа в систему: <br> Логин: 212004 <br> Пароль: hWjk1jy6Hs <br> Приятного использования!";

                client.Send(mail);

                MessageBox.Show("Ответ на обращение будет в течении 6 рабочих дней", "Спасибо за обращение",MessageBoxButton.OK);
                NavigationService.Navigate(new AuthorizationPage());
            }
        }

        bool IsValidEmail(string email)
        {
            if(email.Length < 9)
                return false;

            if(!email.Substring(email.Length - 9).Equals("gmail.com"))
            {
                return false;
            }

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void GroupTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GroupTextBox.Text == "Введите номер группы")
            {
                GroupTextBox.Text = "";
            }
        }

        private void GroupTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "")
            {
                EmailTextBox.Text = "Введите номер группы";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
