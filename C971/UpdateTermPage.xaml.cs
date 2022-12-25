using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971.Models;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateTermPage : ContentPage
    {
        TermDetails TermDetails;
        public UpdateTermPage(TermDetails termDetails)
        {
            InitializeComponent();

            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
            

            TermDetails = termDetails;
            termTitle.Text = termDetails.selectedTerm.Title;
            startDateEntered.Date = termDetails.selectedTerm.Start;
            endDateEntered.Date = termDetails.selectedTerm.End;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            try
            {

                if (termTitle.Text == "" || termTitle.Text == null)
                {
                    throw new Exception("Term title is required.");
                }

                if (new DateTime(startDateEntered.Date.Year, startDateEntered.Date.Month, startDateEntered.Date.Day) > new DateTime(endDateEntered.Date.Year, endDateEntered.Date.Month, endDateEntered.Date.Day))
                {
                    throw new Exception("The term start date cannot be schedueld after the term end date");
                }

                TermDetails.selectedTerm.Title = termTitle.Text;
                TermDetails.selectedTerm.Start = startDateEntered.Date;
                TermDetails.selectedTerm.End = endDateEntered.Date;


                SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

                connection.CreateTable<Term>();

                // update data
                int rowsInserted = connection.Update(TermDetails.selectedTerm);

                // close the connection
                connection.Close();

                DisplayAlert("Success!", "Term succesffuly updated", "Close");
                Navigation.PushAsync(new TermDetails(TermDetails.selectedTerm));
            }
            catch (Exception exception)
            {
                DisplayAlert("ERROR:", $"{exception.Message}\nTerm not updated", "Close");
            }

        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}