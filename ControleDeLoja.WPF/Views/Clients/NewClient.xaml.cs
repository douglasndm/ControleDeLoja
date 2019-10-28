using ControleDeLoja.Functions;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace ControleDeLoja.WPF.Views.Clients
{
    public partial class NewClient : Window
    {
        private Clients referenceClients;
        // FAZ A REFERENCIA PARA A INSTANCIA DA PAGINA DE TODOS OS CLIENTES PARA QUANDO ESTA PAGINA FOR FECHADA 
        // FOR POSSIVEL ACESSAR O METODO PUBLICO DE ATULIZAR A LISTA DE CLIENTES DA PAGINA DOS CLIENTES POR AQUI
        public NewClient(Clients referenceClients)
        {
            InitializeComponent();

            this.referenceClients = referenceClients;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_name.Text != "")
            {
                string phone = new string(tb_phone.Text.Where(x => char.IsDigit(x) || x == '(' || x == ')' || x == '-').ToArray());
                string cpf = new string(tb_cpf.Text.Where(x => char.IsDigit(x)).ToArray());
                string rg = new string(tb_rg.Text.Where(x => char.IsDigit(x)).ToArray());

                string renda = new string(tb_renda.Text.Where(x => char.IsNumber(x) || x == '.' || x == ',').ToArray());

                if (String.IsNullOrEmpty(renda) || Convert.ToDouble(renda) < 3.0)
                    renda = 3.0.ToString();

                SQLiteConnection connection = Database.ConnectionDB();
                try
                {
                    connection.Open();
                    SQLiteCommand sqLiteCommand = new SQLiteCommand(connection);

                    sqLiteCommand.CommandText = "INSERT INTO Clients (Name, LastName, Phone, Andress, Renda, CPF, RG, AvailableLimit, Blocked) VALUES (@name, @lastname, @phone, @andress, @renda, @CPF, @RG, @limit, @Blocked)";

                    sqLiteCommand.Parameters.AddWithValue("@name", tb_name.Text.Trim());
                    sqLiteCommand.Parameters.AddWithValue("@lastname", tb_lastname.Text != "" ? tb_lastname.Text.Trim() : "");
                    sqLiteCommand.Parameters.AddWithValue("@phone", phone);
                    sqLiteCommand.Parameters.AddWithValue("@andress", tb_andress.Text.Trim());
                    sqLiteCommand.Parameters.AddWithValue("@renda", renda);
                    sqLiteCommand.Parameters.AddWithValue("@CPF", cpf);
                    sqLiteCommand.Parameters.AddWithValue("@RG", rg);
                    sqLiteCommand.Parameters.AddWithValue("@limit", !String.IsNullOrEmpty(renda) ? (Convert.ToDouble(renda) / 3) : 3);
                    sqLiteCommand.Parameters.AddWithValue("@Blocked", 0);

                    MessageBox.Show(renda);

                    sqLiteCommand.ExecuteNonQuery();


                    MessageBox.Show(tb_name.Text + " " + tb_lastname.Text + " foi adicionando com sucesso!", "Sucesso!", MessageBoxButton.OK, MessageBoxImage.None);
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
