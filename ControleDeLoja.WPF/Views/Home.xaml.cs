using ControleDeLoja.Models;
using ControleDeLoja.Models.Users;
using System;
using System.Windows;
using System.Windows.Input;

namespace ControleDeLoja.WPF.Views
{
    public partial class Home : Window
    {
        private User user;

        public Home(User user)
        {
            InitializeComponent();

            ContentFrame.Navigate(new Caixa.Caixa(user));

            this.user = user;
            RealDateTime.Content = DateTime.Now;
            AdminName.Content = string.Format("{0} {1}", user.Name, user.LastName);
        }

       
        private void MenuItemCaixa_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new Caixa.Caixa(user));
        }

        private void MenuItemEstoque_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new Estoque.Estoque(user));
        }

        private void MenuItemClientes_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new Clients.Clients(user));
        }

        private void MenuItemRelatorios_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new Relatorios.Vendas());
        }

        private void MenuItemAdministration_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new Users.Home(user));
        }
    }
}
