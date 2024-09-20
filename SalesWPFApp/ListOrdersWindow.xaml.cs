using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SalesWPFApp
{
    public partial class ListOrdersWindow : Window
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceProvider _serviceProvider;
        public ListOrdersWindow(IOrderRepository orderRepository, IServiceProvider serviceProvider)
        {
            _orderRepository = orderRepository; 
            _serviceProvider = serviceProvider;
            InitializeComponent();
            LoadCart();
        }

        public void LoadCart()
        {
            var cartItems = _orderRepository.CartItems.ToList();
            decimal totalMoney = 0;
            foreach (var cartItem in cartItems)
            {
                totalMoney += cartItem.TotalValue;
            }
            tblTotalMoney.Text = $"TOTAL BILL {totalMoney.ToString()}";
            CartListView.ItemsSource = cartItems;
        }

        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            var homeWindow = _serviceProvider.GetService<HomeWindow>();
            if (homeWindow != null)
            {
                homeWindow.btnCheckout_Click(sender, e);
            }
            this.Close();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            // Bug here
            // this.Close();
            this.Visibility = Visibility.Collapsed;
            //HomeWindow homeWindow = _serviceProvider.GetService<HomeWindow>();

            //if (homeWindow != null)
            //{
            //    // Show the HomeWindow
            //    homeWindow.LoadItem();
            //    //homeWindow.Show();
            //    //homeWindow.Activate();
            //}
        }
    }
}
