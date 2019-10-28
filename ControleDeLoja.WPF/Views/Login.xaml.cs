using ControleDeLoja.Classes;
using ControleDeLoja.Functions.Exceptions;
using ControleDeLoja.Functions;
using ControleDeLoja.Models;
using System;
using System.Data.SQLite;
using System.Windows;
using ControleDeLoja.Models.Users;

namespace ControleDeLoja.WPF.Views
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

#if DEBUG
            Administrator administrator = new Administrator();
            administrator.Id = 26;
            administrator.UserId = 1;
            administrator.Name = "Núcleo dos Downloads";
            administrator.LastName = "Developer Mode";
            administrator.Email = "nucleodosdownloads@outlook.com";
            administrator.Password = "123";
            administrator.AcessLevel = 5;

            new Home(administrator).Show();
            this.Close();
#endif
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.tb_email.Text != "")
            {
                if (this.pb_password.Password != "")
                {
                    try
                    {
                        User result = Functions.Login.Logon(tb_email.Text.Trim(), pb_password.Password);

                        //new Caixa.Caixa(result).Show();
                        //this.Close();
                    }
                    catch(EmailOrUserNameNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro");
                    }                    
                }
                else
                {
                    MessageBox.Show("Digite sua senha", "Atenção!");
                }
            }
            else
            {
                MessageBox.Show("Digite seu e-mail", "Atenção!");
            }
        }
    }
}
