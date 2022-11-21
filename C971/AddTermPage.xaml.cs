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

            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Term succesffuly inserted", "Close");
            }
            else
            {
                DisplayAlert("Failure!", "Term not inserted", "Close");
            }
        }
    }
}