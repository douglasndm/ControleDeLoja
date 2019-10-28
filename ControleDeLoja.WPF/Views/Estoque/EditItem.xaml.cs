using ControleDeLoja.Classes;
using ControleDeLoja.Functions;
using ControleDeLoja.Models;
using System;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace ControleDeLoja.WPF.Views.Estoque
{
    public partial class EditItem : Window
    {
        private Product product;
        Estoque estoque; // REFERENCIA DA JANELA ANTERIOR PARA FAZER A ATUALIZAÇÃO

        public EditItem(Product product, Estoque estoque)
        {
            InitializeComponent();

            this.estoque = estoque;

            this.product = product;
            this.tb_ean.Text = product.EAN;
            this.tb_name.Text = product.Name;
            this.tb_description.Text = product.Description != "" ? product.Description : "";
            this.tb_qnts.Text = product.Count >= 0 ? product.Count.ToString() : 0.ToString();
            this.tb_price.Text = product.Price >= 0 ? string.Format("{0:c}", product.Price) : "0";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_name.Text != "")
            {
                char[] charCount = tb_qnts.Text.Where(x => char.IsDigit(x)).ToArray();
                string newCount = new string(charCount);

                string newPrice = new string(tb_price.Text.Where(x => char.IsNumber(x) || x == '.' || x == ',').ToArray());

                SQLiteConnection connection = Database.ConnectionDB();
                try
                {                    
                    connection.Open();
                    SQLiteCommand sqLiteCommand = new SQLiteCommand(connection);
                    sqLiteCommand.CommandText = "UPDATE Products SET EAN = @ean, Name = @itemname, Description = @itemdescription, Qnt = @itemqnt, Price = @itemPrice WHERE Id = @id";

                    sqLiteCommand.Parameters.AddWithValue("@ean", tb_ean.Text.Trim());
                    sqLiteCommand.Parameters.AddWithValue("@itemname", tb_name.Text);
                    sqLiteCommand.Parameters.AddWithValue("@itemdescription", tb_description.Text);
                    sqLiteCommand.Parameters.AddWithValue("@itemqnt", newCount);
                    sqLiteCommand.Parameters.AddWithValue("@itemPrice", newPrice);
                    sqLiteCommand.Parameters.AddWithValue("@id", product.Id);

                    sqLiteCommand.ExecuteNonQuery();

                    MessageBox.Show(tb_name.Text + " atualizado com sucesso!", "Sucesso!", MessageBoxButton.OK, MessageBoxImage.None);

                    estoque.AtualizarLista();
                    this.Close();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Nome do item não pode está em branco", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

    }
}
