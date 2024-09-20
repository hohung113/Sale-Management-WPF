using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for EnterNumberItemWindow.xaml
    /// </summary>
    public partial class EnterNumberItemWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public EnterNumberItemWindow(IProductRepository productRepository , IMemberRepository memberRepository
            , IOrderRepository orderRepository , IServiceProvider serviceProvider)
        {
            _memberRepository = memberRepository;
            _serviceProvider = serviceProvider;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            InitializeComponent();
        }
        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Sử dụng Regular Expression để chỉ cho phép nhập số
        }
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            List<CartItem> itemcart = _orderRepository.CartItems;
            try
            {
                var quantity = int.Parse(QuantityTextBox.Text);
                var unitInStock = _productRepository.CurrentProduct.UnitInStock;
                var productId = _productRepository.CurrentProduct.ProductId;
                var productName = _productRepository.CurrentProduct.ProductName;
                var productPrice = _productRepository.CurrentProduct.UnitPrice;
                Product product = _productRepository.CurrentProduct;

                if (quantity > unitInStock)
                {
                    MessageBox.Show("Quantity exceeds the available stock.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CartItem item = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ProductName = productName,
                    UnitPrice = productPrice,
                    TotalValue = quantity * productPrice
                };
                // add item into cart
                _orderRepository.AddToCart(item);
                
                // Update For Quantity Product
                //product.UnitInStock = unitInStock - quantity;
                //await _productRepository.UpdateProduct(product);

                MessageBox.Show("Order placed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Hide the current window
            // this.Close();
            this.Visibility = Visibility.Hidden;
            HomeWindow homeWindow = _serviceProvider.GetService<HomeWindow>();

            if (homeWindow != null)
            {
                // Show the HomeWindow
                homeWindow.LoadItem();
                //homeWindow.Show();
                //homeWindow.Activate();
            }
            else
            {
                MessageBox.Show("Failed to load HomeWindow.");
            }
        }

    }
}
