using ControleDeLoja.Classes;
using ControleDeLoja.Functions;
using ControleDeLoja.Models;
using ControleDeLoja.Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControleDeLoja.WPF.Views.Caixa
{
    public partial class Caixa : Page, INotifyPropertyChanged
    {
        private List<Product> listAvailableProducts = new List<Product>();
        private ObservableCollection<Product> listProdutsToBuy = new ObservableCollection<Product>();

        private User user;
        private Decimal total;

        public Decimal Total
        {
            get { return this.total;}
            set
            {
                if (value != total)
                {
                    this.total = value;
                    tb_TotalPrice.Text = total.ToString("F2");
                    GridDetalhes.Opacity = total > 0 ? 100: 0;
                    MakePayment.IsEnabled = true;
                    OnPropertyChanged("Total");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public Caixa(User user)
        {
            InitializeComponent();

            this.user = user;

            buyList.ItemsSource = listProdutsToBuy;

            MakePayment.IsEnabled = false;
            GridDetalhes.Opacity = 0;



            //GridDetalhesCliente.Opacity = 0.0;
            //StackDetailsCompra.Opacity = 0.0;
            Total = 0;
            
            AtualizarListaDeProdutos();
        }

        private void AtualizarListaDeProdutos()
        {
            listAvailableProducts.Clear();
            listAvailableProducts = Products.GetAllProducts();
        }


        private void AtualizarTotal()
        {
            Total = 0;

            foreach (Product item in listProdutsToBuy)
                Total += item.Count * item.Price;
        }


        private void BtnAddByEAN_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(addByEAN.Text))
            {
                Product productTemp = null;
                string ean = new string(addByEAN.Text.Trim().Where(x => char.IsDigit(x)).ToArray());

                foreach (var item in listAvailableProducts)
                {
                    if(item.EAN == ean)
                    {
                        productTemp = item;
                        break;
                    }
                }
                if(productTemp == null)
                {
                    MessageBox.Show("Nenhum produto encontrando com esse EAN");
                    return;
                }

                AdicionarProdutoAListaDeCompras(productTemp, 1);
                AtualizarTotal();
                DisplayProductDetails(productTemp);
                addByEAN.Text = "";
            }
            else
            {
                MessageBox.Show("Digite o código EAN");
            }

        }

        private void AdicionarProdutoAListaDeCompras(Product product, short Count)
        {
            foreach (var produto in listAvailableProducts)
            {
                if(produto == product)
                {
                    if(produto.Count > 0)
                    {
                        if(!(Count > produto.Count))
                        {
                            bool estaNaListaDeCompras = false;

                            foreach (var item in listProdutsToBuy)
                            {
                                if(item.Id == product.Id)
                                {
                                    estaNaListaDeCompras = true;

                                    product.Count -= Count;
                                    item.Count += Count;
                                    item.TotalPrice = item.Count * item.Price;
                                    return;
                                }
                            }

                            if(!estaNaListaDeCompras)
                            {
                                Product newProduct = product.Clone() as Product;

                                listProdutsToBuy.Add(newProduct);
                                product.Count -= Count;
                                newProduct.Count = Count;
                                newProduct.TotalPrice = newProduct.Price * newProduct.Count;

                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("O estoque não tem quantidade suficiente para suprir essa quantidade de venda");
                        }
                    }
                    else
                    {
                        MessageBox.Show("O estoque do produto está zerado");
                        return;
                    }
                    return;
                }
            }
            //só chegara aqui se não encontrar nenhum produto pq os return não deixaram chegar aqui
            MessageBox.Show("Nenhum produto encontrado");
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem itemClicked = (sender as ListViewItem);
            Product selectedProduct = itemClicked.Content as Product;

            DisplayProductDetails(selectedProduct);
        }

        private void DisplayProductDetails(Product product)
        {
            tb_ProductName.Text = product.Name;
            tb_ProductEAN.Text = product.EAN;
            tb_ProductPrice.Text = product.Price.ToString("F2");
            tb_ProductCount.Text = product.Count.ToString();
        }

        private void MakePayment_Click(object sender, RoutedEventArgs e)
        {
            if(listProdutsToBuy.Count > 0)
            {
                List<Product> productsToBuyTemp = new List<Product>();

                foreach (var item in listProdutsToBuy)
                {
                    productsToBuyTemp.Add(item);
                }
                new FinalizarCompra(productsToBuyTemp, user, Total).Show();
            }
            else
            {
                MessageBox.Show("Não existe nenhum produto na lista de compra");
            }
        }

        private void MenuItemAdministration_Click(object sender, RoutedEventArgs e)
        {
            new Home(user).Show();
        }
    }
}
