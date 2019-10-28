using ControleDeLoja.Classes;
using ControleDeLoja.Functions;
using ControleDeLoja.Models;
using ControleDeLoja.Models.Users;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControleDeLoja.WPF.Views.Estoque
{
    public partial class Estoque : Page
    {
        private Product item = new Product();
        private ObservableCollection<Product> listItems = new ObservableCollection<Product>();

        public Estoque(User user)
        {
            InitializeComponent();

            DetailStack.Opacity = 0;
            DetailStack.IsEnabled = false;
            listaItems.ItemsSource = listItems;
            AtualizarLista();
        }

        private void ListViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            item = ((ContentControl)sender).Content as Product;

            DetailsEAN.Content = item.EAN;
            DetailsName.Content = item.Name;
            DetailsDescription.Content = item.Description;
            DetailsQnt.Content = item.Count.ToString();
            DetailsPrice.Content = item.Price.ToString("C");
            DetailsPriceAll.Content = (item.Price * item.Count).ToString("C");

            DetailStack.Opacity = 100;

            DetailStack.IsEnabled = true;
            BtnEditItem.IsEnabled = true;

            BtnDeleteItem.IsEnabled = true;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            new NewProduct().Show();
        }

        public void AtualizarLista()
        {
            listItems.Clear();

            foreach (var item in Products.GetAllProducts())
            {
                listItems.Add(item);
            }
        }

        private void BtnEditItem_Click(object sender, RoutedEventArgs e)
        {
            new EditItem(item, this).Show();
        }

        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja apagar o item " + item.Name + " do estoque?", "ATENÇÃO!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                return;

            SQLiteConnection connection = Database.ConnectionDB();

            try
            {               
                connection.Open();
                SQLiteCommand sqLiteCommand = new SQLiteCommand(connection);

                sqLiteCommand.CommandText = "DELETE FROM Estoque WHERE Item_Id = @id";
                sqLiteCommand.Parameters.AddWithValue("@id", item.Id);
                sqLiteCommand.ExecuteNonQuery();


                DetailStack.Opacity = 0;
                DetailStack.IsEnabled = false;

                MessageBox.Show("Item apagado com sucesso do estoque");
                AtualizarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                connection.Close();
            }
        }
    }
}
