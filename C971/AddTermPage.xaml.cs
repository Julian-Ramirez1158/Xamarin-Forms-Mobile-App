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

            try
            {

                if(termTitle.Text == "" || termTitle.Text == null)
                {
                    throw new Exception("Term title is required.");
                }

                if (new DateTime(startDateEntered.Date.Year, startDateEntered.Date.Month, startDateEntered.Date.Day) > new DateTime(endDateEntered.Date.Year, endDateEntered.Date.Month, endDateEntered.Date.Day))
                {
                    throw new Exception("The term start date cannot be schedueld after the term end date");
                }

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

                DisplayAlert("Success!", "Term succesffuly inserted", "Close");
                Navigation.PushAsync(new TermHomePage());
 
            }
            catch(Exception exception)
            {
                DisplayAlert("ERROR:", $"{exception.Message}\nTerm not inserted", "Close");
            }
            
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}