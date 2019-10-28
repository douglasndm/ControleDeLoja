using ControleDeLoja.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using ControleDeLoja.Functions;
using System.Linq;
using System;
using ControleDeLoja.Models.Users;
using ControleDeLoja.Classes;

namespace ControleDeLoja.WPF.Views.Caixa
{
    public partial class FinalizarCompra : Window
    {
        private List<Product> products;
        User user = null;
        Client client = null;
        decimal Total;

        public FinalizarCompra(List<Product> products, User user, decimal Total)
        {
            this.Total = Total;
            this.products = products;
            this.user = user;

            InitializeComponent();

            Detalhes.Opacity = 0;
            btnFinalizarCompra.IsEnabled = false;
        }

        private void BtnFinalizarCompra_Click(object sender, RoutedEventArgs e)
        {
            if (!client.Blocked)
            {
                try
                {
                    Sale sale = new Sale
                    {
                        ClientId = client.Id,
                        UserId = user.UserId,
                        SaleDate = DateTime.Now,
                        TotalPrice = Total,
                        FormaPagamento = ""
                    };

                    Sales.MakeASale(sale);

                    foreach (var produtoComprado in products)
                    {
                        Products.SellProduct(produtoComprado, sale);
                    }

                    MessageBox.Show("Venda realizada com sucesso!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu um erro. \n" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show(client.Name + " está bloqueado para realizar novas compras");
            }
            
        }

        private void BtnFindClient_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(tb_clientCPf.Text))
            {
                Regex regularExpression = new Regex(@"([0-9]{11})|([0-9]{3}\.[0-9]{3}\.[0-9]{3}\-[0-9]{2})");

                if(regularExpression.IsMatch(tb_clientCPf.Text.Trim()))
                {
                    string cpf = new string(tb_clientCPf.Text.Where(p => char.IsDigit(p)).ToArray());
                    
                    client = Functions.Clients.GetClient(cpf);

                    if (client != null)
                    {
                        ClientName.Text = String.Format("{0} {1}", client.Name, client.LastName);
                        Tb_Total.Text = Total.ToString();
                        btnFinalizarCompra.IsEnabled = true;
                        Detalhes.Opacity = 100;
                    }
                    else
                        MessageBox.Show("Cliente não encontrado");

                }
                else
                {
                    MessageBox.Show("O CPF informado não está em um formato válido");
                }
            }
            else
            {
                MessageBox.Show("Digite o CPF do cliente");
            }
        }
    }
}