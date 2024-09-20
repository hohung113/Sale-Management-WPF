using AutoMapper;
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
    /// Interaction logic for ViewStaticAdminWindow.xaml
    /// </summary>
    public partial class ViewStaticAdminWindow : Window
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;
        public ViewStaticAdminWindow(IOrderDetailRepository orderDetailRepository, IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            InitializeComponent();
            LoadData();
            UpdateTotalSales();
        }

        // Load Data For Member Or Admin 
        private void LoadData()
        {
            List<OrderDetail> orders = new List<OrderDetail>();


            bool isAdmin = _memberRepository.IsAdmin();
            if (isAdmin)
            {
                orders = _orderDetailRepository.GetAllOrdersDetail();
            }
            else
            {
                int memberID = _memberRepository.CurrentMember.MemberId;
                var resultList = _orderDetailRepository.GetAllOrderDetailByMember();
                orders = resultList.Where(o => o.Order.MemberId == memberID).ToList();
            }
            var listOrderDetailObject = _mapper.Map<List<OrderDetailObject>>(orders);
            OrdersListView.ItemsSource = listOrderDetailObject;
        }

        private void UpdateTotalSales()
        {
            DateTime startDate = new DateTime(2024, 1, 1);
            DateTime endDate = new DateTime(2024, 12, 31);
            //.Where(p => p.Order.OrderDate >= startDate && p.Order.OrderDate <= endDate)
            if (OrdersListView.ItemsSource is List<OrderDetail> orders)
            {
                var totalSales = orders.Where(p => p.Order.OrderDate >= startDate && p.Order.OrderDate <= endDate)
                    .Sum(o => o.Quantity * o.UnitPrice * (1 - (decimal)o.Discount));
                TotalSalesTextBlock.Text = $"Total Sales: {totalSales:C}";
            }
        }
    }

}

