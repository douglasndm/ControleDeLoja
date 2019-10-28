using ControleDeLoja.Classes;
using ControleDeLoja.Functions;
using ControleDeLoja.Models;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace ControleDeLoja.WPF.Views.Clients
{
    public partial class EditClient : Window
    {
        private Client client;
        private Clients referenceClients;

        public EditClient(Client client, Clients referenceClients)
        {
            InitializeComponent();

            this.client = client;
            this.referenceClients = referenceClients;

            tb_name.Text = client.Name;
            tb_lastname.Text = client.LastName != "" ? client.LastName : "";

            tb_phone.Text = client.Phone;
            tb_cpf.Text = client.CPF;            
            tb_rg.Text = client.RG;
            tb_Andress.Text = client.Andress;

            tb_renda.Text = client.Renda.ToString();
            tb_limit.Text = client.Limit.ToString();

            if (client.Blocked)
            {
                this.RbBlockedYes.IsChecked = true;
                this.RbBlockedNo.IsChecked = false;
            }
            else
            {
                this.RbBlockedYes.IsChecked = false;
                this.RbBlockedNo.IsChecked = true;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_name.Text != "")
            {
                string phone = new string(tb_phone.Text.Where(x => char.IsDigit(x) || x == '(' || x == ')' || x == '-').ToArray());
                string cpf = new string(tb_cpf.Text.Where(x => char.IsDigit(x)).ToArray());
                string rg = new string(tb_rg.Text.Where(x => char.IsDigit(x)).ToArray());

                string renda = new string(tb_renda.Text.Where(x => char.IsNumber(x) || x == '.' || x == ',').ToArray());
                string limit = new string(tb_limit.Text.Where(x => char.IsNumber(x) || x == '.' || x == ',').ToArray());

                bool blocked = (bool)RbBlockedYes.IsChecked ? true : false;

   
                SQLiteConnection connection = Database.ConnectionDB();
                try
                {
                    connection.Open();

                    SQLiteCommand sqLiteCommand = new SQLiteCommand(connection);
                    sqLiteCommand.CommandText = "UPDATE Clientes SET Client_Name = @name, Client_LastName = @lastname, Client_Phone = @phone, Client_Andress = @andress, Client_Renda = @renda, Client_CPF = @CPF, Client_RG = @RG, Client_AvailableLimit = @limit, Client_IsBlocked = @Blocked  WHERE Client_Id = @id";

                    sqLiteCommand.Parameters.AddWithValue("@name", tb_name.Text.Trim());
                    sqLiteCommand.Parameters.AddWithValue("@lastname", tb_lastname.Text != "" ? tb_lastname.Text.Trim() : "");
                    sqLiteCommand.Parameters.AddWithValue("@phone", phone);
                    sqLiteCommand.Parameters.AddWithValue("@andress", tb_Andress.Text.Trim());
                    sqLiteCommand.Parameters.AddWithValue("@renda", renda);
                    sqLiteCommand.Parameters.AddWithValue("@CPF", cpf);
                    sqLiteCommand.Parameters.AddWithValue("@RG", rg);
                    sqLiteCommand.Parameters.AddWithValue("@limit", limit);
                    sqLiteCommand.Parameters.AddWithValue("@Blocked", blocked);
                    sqLiteCommand.Parameters.AddWithValue("@id", this.client.Id);

                    sqLiteCommand.ExecuteNonQuery();

                    MessageBox.Show(tb_name.Text + " " + tb_lastname.Text + " atualizado com sucesso!", "Sucesso!", MessageBoxButton.OK, MessageBoxImage.None);
                    referenceClients.AtualizarClientes();
                    this.Close();
                }
                catch (Exception ex)
                {
                   MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Digite o nome do cliente", "Está faltando algo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
