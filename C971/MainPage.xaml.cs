using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace C971
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string userName = usernameEntry.Text;
            bool isUsernameEmpty = string.IsNullOrEmpty(userName);

            string password = passwordEntry.Text;
            bool isPasswordEmpty = string.IsNullOrEmpty(password);

            if (isUsernameEmpty == true || isPasswordEmpty == true)
            {
                // prevent navigation to next page
                DisplayAlert("Alert", "Please enter username and password", "Close");
            }
            else
            {
                string greeting = "Hello, " + userName + "!";
                greetingLabel.Text = greeting;
                //buttonClicked.Text = "Clicked!";

                // navigate to next page
                Navigation.PushAsync(new TermHomePage());
            }

        }
    }
}
