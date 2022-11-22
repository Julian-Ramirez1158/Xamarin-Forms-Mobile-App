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
        Term selectedTerm;
        public TermDetails(Term selectedTerm)
        {
            InitializeComponent();

            this.selectedTerm = selectedTerm;

            navigationTitle.Text = selectedTerm.Title;
            termDates.Text = $"Start Date: {selectedTerm.Start:MM-dd-yyyy}\nEnd Date: {selectedTerm.End:MM-dd-yyyy}";
        }

        private void updateButton_Clicked(object sender, EventArgs e)
        {
            selectedTerm.Title = termEntry.Text;

            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Term>();

            // update data
            int rowsInserted = connection.Update(selectedTerm);

            // close the connection
            connection.Close();

            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Term succesffuly updated", "Close");
            }
            else
            {
                DisplayAlert("Failure!", "Term not updated", "Close");
            }
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Term>();

            // delete data
            int rowsInserted = connection.Delete(selectedTerm);

            // close the connection
            connection.Close();

            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Experience succesffuly deleted", "Close");
            }
            else
            {
                DisplayAlert("Failure!", "Experience not deleted", "Close");
            }
        }

        private void addCourseButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}