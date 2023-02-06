using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentReports : TabbedPage
    {
        public StudentReports()
        {
            InitializeComponent();

            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermHomePage());
        }
    }
}