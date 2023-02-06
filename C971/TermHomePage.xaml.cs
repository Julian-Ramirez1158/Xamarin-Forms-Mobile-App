using C971.Models;
using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermHomePage : ContentPage
    {
        public TermHomePage()
        {
            InitializeComponent();
            
            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Term>();
            // Allows us to return the table query and turn it into a list
            var entries = connection.Table<Term>().ToList();
            listView.ItemsSource = entries;

            connection.Close();
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = listView.SelectedItem as Term;

            if (selectedItem != null)
            {
                Navigation.PushAsync(new TermDetails(selectedItem));
            }
        }

        private void addTermButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTermPage());
        }

        private void studentReportsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StudentReports());
        }

    }
}