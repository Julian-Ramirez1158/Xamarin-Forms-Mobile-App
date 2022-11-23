using C971.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermDetails : ContentPage
    {
        public Term selectedTerm;
        public TermDetails(Term selectedTerm)
        {
            InitializeComponent();

            this.selectedTerm = selectedTerm;

            navigationTitle.Text = selectedTerm.Title;
            termDates.Text = $"Start Date: {selectedTerm.Start:MM-dd-yyyy}\nEnd Date: {selectedTerm.End:MM-dd-yyyy}";

            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        
        private void updateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UpdateTermPage(this));
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Term>();

            // delete data
            int rowsDeleted = connection.Delete(selectedTerm);

            // close the connection
            connection.Close();

            // TODO: add some actual validation here you twit
            if (rowsDeleted > 0)
            {
                DisplayAlert("Success!", "Experience succesffuly deleted", "Close");
                Navigation.PushAsync(new TermHomePage());
            }
            else
            {
                DisplayAlert("Failure!", "Experience not deleted", "Close");
            }
        }

        private void addCourseButton_Clicked(object sender, EventArgs e)
        {

        }

        private void homeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermHomePage());
        }
    }
}