namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IServiceProvider _serviceProvider;
        public LoginWindow(IServiceProvider serviceProvider, IMemberRepository memberRepository)
        {
            _serviceProvider = serviceProvider;
            _memberRepository = memberRepository;
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            var txtUName = txtUserName.Text.Trim();
            var txtPW = txtPassword.Password;
            var result = await _memberRepository.Login(txtUName, txtPW);
            var isAdmin = _memberRepository.LoginAdmin(txtUName, txtPW);
            var homeWindow = _serviceProvider.GetService<HomeWindow>();

            if (isAdmin || result)
            {
                if (isAdmin)
                {
                    _memberRepository.IsAdmin();
                }
                homeWindow.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login Faild");
            }
        }


        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var email = txtUserName.Text.Trim();
                var pass = txtPassword.Password.Trim();
                Member member = new Member
                {
                    Email = email,
                    Password = pass,
                    City = "VN",
                    CompanyName = "FPT",
                    Country ="VN"
                };
                _memberRepository.AddMemeber(member);
                MessageBox.Show($"Register account {member.Email}  successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e) => this.Close();

    }
}