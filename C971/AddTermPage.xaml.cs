using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using C971.Models;
using SQLite;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTermPage : ContentPage
    {
        public AddTermPage()
        {
            InitializeComponent();

            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Term post = new Term()
            {
                Title = termTitle.Text,
                Start = startDateEntered.Date,
                End = endDateEntered.Date
            };

            // open connection to db
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            // creates a type Post table
            connection.CreateTable<Term>();


            // insert data
            int rowsInserted = connection.Insert(post);

            // close the connection
            connection.Close();
            

            // TODO: add actual data validation here
            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Term succesffuly inserted", "Close");
                Navigation.PushAsync(new TermHomePage());
            }
            else
            {
                DisplayAlert("Failure!", "Term not inserted", "Close");
            }
            
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}