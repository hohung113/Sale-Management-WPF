
using AutoMapper;
using DataAccess;
using DataAccess.Entity;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        IProductRepository _productRepository;
        IMemberRepository _memberRepository;
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        public HomeWindow(IProductRepository productRepository, IMemberRepository memberRepository, IServiceProvider serviceProvider, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _memberRepository = memberRepository;
            _serviceProvider = serviceProvider;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            InitializeComponent();
        }
        public async void LoadItem()
        {
            var numberItem = _orderRepository.CartItems.Count();
            tblNumberItem.Text = numberItem.ToString();
            List<Product> products = await _productRepository.GetAll();
            var productObjects = ConvertEntityToObject(products.ToList());
            lvProducts.ItemsSource = productObjects;
        }
        // Load Product
        private void Window_Loaded(object sender, RoutedEventArgs e) => LoadProducts();
        private async void LoadProducts()
        {

            bool IsAdmin = _memberRepository.IsAdmin();
            if (!IsAdmin)
            {
                btnInsert.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
                btnFilterStock.Visibility = Visibility.Collapsed;
                //btnStatic.Visibility = Visibility.Collapsed;
                tblMode.Text = string.Empty;
            }
            else
            {
                btnBuy.Visibility = Visibility.Collapsed;
                btnCart.Visibility = Visibility.Collapsed;
                btnAddToCart.Visibility = Visibility.Collapsed;
                btnProfile.Visibility = Visibility.Collapsed;
                btnCheckout.Visibility = Visibility.Collapsed;
            }

            List<Product> products = await _productRepository.GetAll();
            var productObjects = ConvertEntityToObject(products.ToList());
            lvProducts.ItemsSource = productObjects;
        }
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var requestProductName = txtSearchProductName.Text;

                if (string.IsNullOrWhiteSpace(requestProductName))
                {
                    MessageBox.Show("Please enter a product name to search.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = await _productRepository.GetProductByName(requestProductName);

                if (result == null || !result.Any())
                {
                    MessageBox.Show("No products found matching the search criteria.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    lvProducts.ItemsSource = new List<ProductObject>();
                }
                else
                {
                    var productObjects = ConvertEntityToObject(result.ToList());
                    lvProducts.ItemsSource = productObjects;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnSearchPrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse minimum and maximum price from text boxes
                if (decimal.TryParse(txtMinPrice.Text, out decimal minPrice) &&
                    decimal.TryParse(txtMaxPrice.Text, out decimal maxPrice))
                {
                    if (minPrice > maxPrice)
                    {
                        MessageBox.Show("Minimum price cannot be greater than maximum price.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var result = await _productRepository.GetProductByPriceRange(minPrice, maxPrice);
                    if (result == null || !result.Any())
                    {
                        MessageBox.Show("No products found in the specified price range.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Clear previous results
                        lvProducts.ItemsSource = new List<ProductObject>();
                    }
                    else
                    {
                        var productObjects = ConvertEntityToObject(result.ToList());

                        lvProducts.ItemsSource = productObjects;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter valid numbers for price range.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Gather product details
                var categoryId = int.Parse(txtCategoryID.Text.ToString());
                var productName = txtProductName.Text;
                var weight = txtWeight.Text;
                var unitPrice = decimal.Parse(txtUnitPrice.Text);
                var unitInStock = int.Parse(txtUnitsInStock.Text);

                // Validate product details
                if (string.IsNullOrWhiteSpace(productName) ||
                    string.IsNullOrWhiteSpace(weight) ||
                    unitPrice <= 0 ||
                    unitInStock < 0)
                {
                    MessageBox.Show("Please provide valid product details.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create a new product instance
                var newProduct = new Product
                {
                    CategoryId = categoryId,
                    ProductName = productName,
                    Weight = weight,
                    UnitPrice = unitPrice,
                    UnitInStock = unitInStock
                };
                _productRepository.AddProduct(newProduct);

                MessageBox.Show("Product added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the product. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadProducts();
            }
        }


        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var productId = int.Parse(txtProductID.Text);
                var categoryId = int.Parse(txtCategoryID.Text);
                var productName = txtProductName.Text;
                var weight = txtWeight.Text;
                var unitPrice = decimal.Parse(txtUnitPrice.Text);
                var unitInStock = int.Parse(txtUnitsInStock.Text);
                if (unitPrice <= 0)
                {
                    MessageBox.Show("Unit price must be greater than 0.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (unitInStock < 2)
                {
                    MessageBox.Show("Units in stock cannot be negative.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var updatedProduct = new Product
                {
                    ProductId = productId,
                    CategoryId = categoryId,
                    ProductName = productName,
                    Weight = weight,
                    UnitPrice = unitPrice,
                    UnitInStock = unitInStock
                };

                bool isUpdated = await _productRepository.UpdateProduct(updatedProduct);

                if (isUpdated)
                {
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update the product. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Please enter valid product details.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the product. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadProducts();
            }
        }


        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var productIdText = txtProductID.Text;
                if (int.TryParse(productIdText, out int productId))
                {
                    var product = new Product { ProductId = productId };

                    bool isDeleted = await _productRepository.DeleteProduct(product);
                    if (isDeleted)
                    {
                        MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the product. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the product. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadProducts();
            }
        }


        private async void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GetProductObject())
                {
                    var cProduct = _productRepository.CurrentProduct;
                    var productID = int.Parse(txtProductID.Text);
                    cProduct = await _productRepository.GetProductById(productID);
                    var enterNumberItemWindow = _serviceProvider.GetService<EnterNumberItemWindow>();
                    enterNumberItemWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please Choose Item try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Tạo cửa sổ mới trước khi đóng cửa sổ hiện tại
            this.Close();
            var loginWindow = _serviceProvider.GetService<LoginWindow>();

            if (loginWindow != null)
            {
                    loginWindow.Show();
                //if (loginWindow.IsLoaded)
                //{
                //}
            }
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = _serviceProvider.GetService<ListOrdersWindow>();

            if (cartWindow != null)
            {
                if (cartWindow.IsLoaded)
                {
                    cartWindow.Show();
                }
                else
                {
                    //cartWindow = new ListOrdersWindow(_serviceProvider.GetService<IOrderRepository>());
                    cartWindow.LoadCart();
                    // cartWindow.Visibility = Visibility.Visible;
                    cartWindow.Show();
                }
            }
        }

        // Auto Mapper
        private List<ProductObject> ConvertEntityToObject(List<Product> entitys)
        {
              var productObject = _mapper.Map<List<ProductObject>>(entitys);
            return productObject.ToList();
            //return entitys.Select(p => new ProductObject
            //{
            //    ProductId = p.ProductId,
            //    CategoryId = p.CategoryId,
            //    ProductName = p.ProductName,
            //    Weight = p.Weight,
            //    UnitPrice = p.UnitPrice,
            //    UnitInStock = p.UnitInStock
            //}).ToList();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearchProductName.Text = string.Empty;
            txtMaxPrice.Text = string.Empty;
            txtMinPrice.Text = string.Empty;
            LoadProducts();
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = _serviceProvider.GetService<MemberProfileWindow>();
            profileWindow.Show();
        }

        private bool GetProductObject()
        {
            bool result = false;
            if (lvProducts.SelectedItem != null)
            {
                result = true;
            }
            return result;
        }

        public async void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
             // MessageBox.Show("Do You Want Checkout !", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            try
            {
                List<CartItem> cartItems = _orderRepository.CartItems.ToList();
                if (!cartItems.Any())
                {
                    MessageBox.Show("Dont have any item in cart !", "Fail", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    Member currentMember = _memberRepository.CurrentMember;
                    var memberId = currentMember.MemberId;
                    var orderDate = DateTime.Now;
                    var requiredDate = orderDate.AddDays(2);
                    var shippedDate = orderDate.AddDays(4);
                    var freight = 10;

                    // Add Into Order
                    Order newOrder = new Order
                    {
                        MemberId = memberId,
                        OrderDate = orderDate,
                        RequiredDate = requiredDate,
                        ShippedDate = shippedDate,
                        Freight = freight
                    };

                    await _orderRepository.AddNewOrder(newOrder);
                    int newOrderId = newOrder.OrderId;
                    OrderDetail newOrderDetail = new OrderDetail();
                    foreach (var item in cartItems)
                    {
                        // Add into OrderDetail Table
                        newOrderDetail.OrderId = newOrderId;
                        newOrderDetail.ProductId = item.ProductId;
                        newOrderDetail.UnitPrice = item.UnitPrice;
                        newOrderDetail.Quantity = item.Quantity;
                        newOrderDetail.Discount = 0.1;

                        await _orderDetailRepository.AddNewOrderDetail(newOrderDetail);
                        // Update Quantity For Product
                        var product = await _productRepository.GetProductById(item.ProductId);
                        product.UnitInStock -= item.Quantity;
                        await _productRepository.UpdateProduct(product);
                    }
                    _orderRepository.ClearCartItem();
                    MessageBox.Show("Checkout successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                HomeWindow homeWindow = _serviceProvider.GetService<HomeWindow>();

                if (homeWindow != null)
                {
                    // Show the HomeWindow
                    homeWindow.LoadItem();
                    homeWindow.Activate();
                }
                else
                {
                    MessageBox.Show("Failed to load HomeWindow.");
                }
            }
        }

        private bool _isAscendingOrder = true;
        private async void LoadFilterUnitInStock()
        {
            List<Product> products = await _productRepository.GetAll();
            var productObjects = ConvertEntityToObject(products.ToList());
            if (_isAscendingOrder)
            {
                lvProducts.ItemsSource = productObjects.OrderBy(p => p.UnitInStock);
            }
            else
            {
                lvProducts.ItemsSource = productObjects.OrderByDescending(p => p.UnitInStock);
            }
        }
        private void btnFilterStock_Click(object sender, RoutedEventArgs e)
        {
            _isAscendingOrder = !_isAscendingOrder;
            LoadFilterUnitInStock();
        }

        private void btnViewStatic_Click(object sender, RoutedEventArgs e)
        {
            ViewStaticAdminWindow staticsWindow = _serviceProvider.GetService<ViewStaticAdminWindow>();
            staticsWindow.Show();
        }
    }
}
