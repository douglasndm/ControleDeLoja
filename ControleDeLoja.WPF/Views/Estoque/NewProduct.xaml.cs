using ControleDeLoja.Classes;
using ControleDeLoja.Functions;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace ControleDeLoja.WPF.Views.Estoque
{
    public partial class NewProduct : Window
    {
        public NewProduct()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_name.Text != "")
            {
                string quantidade = new string(tb_qnts.Text.Where(x => char.IsDigit(x)).ToArray());
                string preco = new string(tb_price.Text.Where(x => char.IsDigit(x) || x == '.' || x == ',').ToArray());

                SQLiteConnection connection = Database.ConnectionDB();
                try
                {
                    connection.Open();
                    SQLiteCommand sqLiteCommand = new SQLiteCommand(connection);
                    sqLiteCommand.CommandText = "INSERT INTO Products (EAN, Name, Description, Qnt, Price) VALUES (@ean, @name, @description, @qnt, @price)";

                    sqLiteCommand.Parameters.AddWithValue("@ean", tb_ean.Text.Trim());
                    sqLiteCommand.Parameters.AddWithValue("@name", tb_name.Text.Trim());
                    sqLiteCommand.Parameters.AddWithValue("@description", tb_description.Text.Trim());
                    sqLiteCommand.Parameters.AddWithValue("@qnt", (Convert.ToInt32(quantidade) >= 0 ? Convert.ToInt32(quantidade) : 0));
                    sqLiteCommand.Parameters.AddWithValue("@price", preco);

                    sqLiteCommand.ExecuteNonQuery();


                    MessageBox.Show(tb_name.Text + " adicionando com sucesso", "Sucesso!", MessageBoxButton.OK, MessageBoxImage.None);                    
                    this.Close();
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
            else
            {
                MessageBox.Show("Digite o nome do produto!");
            }
        }
    }
}
