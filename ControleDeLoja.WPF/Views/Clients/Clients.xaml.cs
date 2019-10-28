using ControleDeLoja.Functions;
using ControleDeLoja.Functions.Users;
using ControleDeLoja.Models;
using ControleDeLoja.Models.Users;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ControleDeLoja.WPF.Views.Clients
{
    public partial class Clients : Page
    {
        private ObservableCollection<Client> listaDeClientes = new ObservableCollection<Client>();
        private Client client;
        private User user;

        public Clients(User user)
        {
            InitializeComponent();
            this.user = user;

            DetailStack.Opacity = 0.0;
            DetailStack.IsEnabled = false;
            listaClients.ItemsSource = listaDeClientes;

            AtualizarClientes();
        }

        private void ListViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            client = ((ListViewItem)sender).Content as Client;

            DetailsName.Content = client.Name;
            DetailsLastName.Content = client.LastName;
            DetailsPhone.Content = client.Phone;
            DetailsAndress.Content = client.Andress;
            DetailsCPF.Content = String.Format("{0: ###.###.###-##}", client.CPF);
            DetailsRG.Content = String.Format("{0: ##.###.###-#}", client.RG);
            DetailsRenda.Content = String.Format("{0:c}", client.Renda);
            DetailsLimiteDisponivel.Content = String.Format("{0:c}", client.Limit);

            DetailsBloqueado.Content = client.Blocked ? "Cliente bloqueado para fazer novas compras" : "Cliente liberado para fazer novas compras";
            StackSituacao.Background = client.Blocked ? (Brush)Brushes.Red : (Brush)Brushes.Green;

            LbSituacao.Foreground = (Brush)Brushes.White;
            DetailsBloqueado.Foreground = (Brush)Brushes.White;

            DetailStack.Opacity = 100.0;
            DetailStack.IsEnabled = true;


            Administrator admin = Administrators.GetAdministrator(user.UserId);

            if (admin != null)
            {
                BtnEditClient.IsEnabled = admin.AcessLevel >= 2;
                BtnDeleteClient.IsEnabled = admin.AcessLevel >= 3;
            }


            var AllSales = Sales.GetAllSalesByAClient(Convert.ToInt16(client.Id));
            listViewComprasClient.ItemsSource = AllSales;

            if(AllSales.Count > 0)
            {
                listaDeComprasDetalhes.Content = "Lista de todas as compras do cliente";
                listViewComprasClient.Opacity = 100;
                listViewComprasClient.IsEnabled = true;
            }
            else
            {
                listaDeComprasDetalhes.Content = "Cliente ainda não realizou nenhuma compra";
                listViewComprasClient.Opacity = 0;
                listViewComprasClient.IsEnabled = false;
            }
        }
        
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AtualizarClientes();
        }

        public void AtualizarClientes()
        {
            listaDeClientes.Clear();

            foreach (var item in Functions.Clients.GetAllClients())
            {
                listaDeClientes.Add(item);
            }
        }

        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            new EditClient(client, this).Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new NewClient(this).Show();
        }

        private void BtnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja apagar todos os dados do cliente " + client.Name + " " + client.LastName + "?", "ATENÇÃO", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
                return;
            try
            {
                SQLiteConnection connection = Database.ConnectionDB();
                connection.Open();

                SQLiteCommand sqLiteCommand = new SQLiteCommand(connection);

                sqLiteCommand.CommandText = "DELETE FROM Clients WHERE Id = @id";
                sqLiteCommand.Parameters.AddWithValue("@id", client.Id);

                sqLiteCommand.ExecuteNonQuery();
                connection.Close();

                DetailStack.Opacity = 0;
                DetailStack.IsEnabled = false;
                MessageBox.Show("Cliente apagado com sucesso");
                AtualizarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
