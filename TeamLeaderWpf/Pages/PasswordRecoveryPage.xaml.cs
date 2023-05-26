using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Web;

namespace TeamLeaderWpf.Pages
{
    public partial class PasswordRecoveryPage : Page
    {
        public PasswordRecoveryPage()
        {
            InitializeComponent();
            EmailTextBox.Text = "Введите электронную почту";
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
            if(EmailTextBox.Text == "Введите электронную почту")
            {
                NavigationService.GoBack();
            }
            else if (!IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Ввведите именно gmail");
            }
            else
            {
                var user = Mongo.MongoDb.FindUserByEmail(EmailTextBox.Text);
                if ( user == null)
                {
                    MessageBox.Show("Неверный email. Проверьте правильность написания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var group = Mongo.MongoDb.FindGroupById(user.GroupId);

                Core.PasswordGenetartor passwordGenetartor = new Core.PasswordGenetartor();
                var pass = passwordGenetartor.GeneratePassword(true,true, true, false, 10);

                SmtpClient client = new SmtpClient();
                client.Credentials = new NetworkCredential("mck-ktits@internet.ru", "Sb16dbUywA1LnUwKWXLJ");
                client.Host = "smtp.mail.ru";
                client.Port = 587;
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("mck-ktits@internet.ru");
                mail.To.Add(new MailAddress(EmailTextBox.Text));
                mail.Subject = "Заявка на восстановление пароля от сервиса МЦК КТИТС";
                mail.IsBodyHtml = true;
                mail.Body = $"Уважаемый староста {group.Name} группы. <br> Благодарим за оставленную заявку на восстановление пароля к нашему сервису." +
                    $"<br> Ваш новый пароль для входа в систему: {pass} <br> Приятного использования!";

                user.Password = pass;

                Mongo.MongoDb.ReplaceOneUser(user);

                client.Send(mail);

                MessageBox.Show("Ответ на обращение будет в течении 6 рабочих дней", "Спасибо за обращение", MessageBoxButton.OK);
                NavigationService.Navigate(new AuthorizationPage());
            }
        }

        bool IsValidEmail(string email)
        {
            if (email.Length < 9)
                return false;

            if (!email.Substring(email.Length - 9).Equals("gmail.com"))
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
