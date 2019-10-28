using ControleDeLoja.Models.Users;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ControleDeLoja.WPF.Views.Users
{
    public partial class Home : Page
    {
        private ObservableCollection<User> users;

        public Home(User user)
        {
            InitializeComponent();

            users = new ObservableCollection<User>();

            for(int i = 0; i < 10; i++)
            {
                users.Add(new Operator()
                {
                    Name = "Douglas " + i,
                    LastName = "EOQ " + i
                });
            }
        }

        private void ListViewItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
