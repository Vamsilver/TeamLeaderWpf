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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamLeaderWpf.Mongo;
using TeamLeaderWpf.Core;

namespace TeamLeaderWpf.Pages
{
    /// <summary>
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        string password = "";

        public AuthorizationPage()
        {
            InitializeComponent();
            LoginTextBox.Text = "Введите логин";
            PasswordPasBox.Password = "sample";

            var isNeedToRememberLoginData = Properties.Settings.Default.isNeedToRememberLoginData;
           
            if (isNeedToRememberLoginData)
                LoginTextBox.Text = Properties.Settings.Default.Login;

            RememberCheckBox.IsChecked = isNeedToRememberLoginData;

        }

        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Введите логин")
            {
                LoginTextBox.Text = "";
            }
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "")
            {
                LoginTextBox.Text = "Введите логин";
            }
        }

        private void LogInClick(object sender, RoutedEventArgs e)
        {
            LogIn();
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordPasBox.Password == "sample")
            {
                PasswordPasBox.Password = "";
            }
        }

        private void PasswordPasBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordPasBox.Password == "")
            {
                PasswordPasBox.Password = "sample";
            }
        }

        private void PasswordPasBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LogIn();
            }
        }

        private void LogIn()
        {
            User? user = MongoDb.FindUserByLogin(LoginTextBox.Text);
            if (!LoginTextBox.Text.Equals("Введите логин") || !PasswordPasBox.Password.Equals("sample"))
            {
                if (user != null)
                {
                    if (user.Password == PasswordPasBox.Password)
                    {
                        MongoDb.currentUser = user;
                        MongoDb.currentGroup = MongoDb.FindGroupById(user.GroupId);

                        var flag = RememberCheckBox.IsChecked;
                        Properties.Settings.Default.isNeedToRememberLoginData = (bool)flag!;
                        
                        if((bool)flag)
                            Properties.Settings.Default.Login = LoginTextBox.Text;
                        else
                            Properties.Settings.Default.Login = "";

                        Properties.Settings.Default.Save();

                        NavigationService.Navigate(new GroupPage());
                    }
                    else
                    {
                        MessageBox.Show("Не правильный пароль");
                    }
                }
                else
                {
                    MessageBox.Show("Такого пользвателя не существует");
                }
            }
            else
            {
                MessageBox.Show("Введите данные для входа");
            }
        }

        //private void AuthoLogin(object sender, RoutedEventArgs e)
        //{
        //    User? user = MongoDb.FindUserByLogin("212004");
        //    MongoDb.currentUser = user;
        //    MongoDb.currentGroup = MongoDb.FindGroupById(user.GroupId);
        //    NavigationService.Navigate(new GroupPage());
        //}

        private void RememberButtonClick(object sender, RoutedEventArgs e)
        {
            RememberCheckBox.IsChecked = !RememberCheckBox.IsChecked;
        }
    }
}
