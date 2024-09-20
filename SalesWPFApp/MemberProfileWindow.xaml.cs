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
    /// Interaction logic for MemberProfileWindow.xaml
    /// </summary>
    public partial class MemberProfileWindow : Window
    {
        private readonly IMemberRepository _memberRepository;
        public MemberProfileWindow(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e) => LoadInformation();

        private void LoadInformation()
        {
            var member = _memberRepository.CurrentMember;
            if (member is not null)
            {
                txtEmail.Text = member.Email.ToString();
                txtCompanyName.Text = member.CompanyName.ToString();
                txtCity.Text = member.City.ToString();
                txtCountry.Text = member.Country.ToString();
                txtPassword.Text = member.Password.ToString();
            }
        }

        private void btnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            // Display confirmation dialog
            var result = MessageBox.Show(
                "Are you sure you want to update your profile?",
                "Confirm Update",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result == MessageBoxResult.Yes)
            {
                var member = _memberRepository.CurrentMember;
                member.Email = txtEmail.Text;
                member.CompanyName = txtCompanyName.Text;
                member.Country = txtCountry.Text;
                member.City = txtCity.Text;
                member.Password = txtPassword.Text;

                _memberRepository.UpdateMember(member);
                MessageBox.Show("Profile updated successfully!"); 
            }
            LoadInformation();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
