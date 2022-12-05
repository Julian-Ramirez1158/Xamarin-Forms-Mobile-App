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
            TermDetails.selectedTerm.Title = termTitle.Text;
            TermDetails.selectedTerm.Start = startDateEntered.Date;
            TermDetails.selectedTerm.End = endDateEntered.Date;
            

            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Term>();

            // update data
            int rowsInserted = connection.Update(TermDetails.selectedTerm);

            // close the connection
            connection.Close();

            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Term succesffuly updated", "Close");
                // TODO fix navigation problem where back button takes you to previously unsaved entry
                
                Navigation.PushAsync(new TermDetails(TermDetails.selectedTerm));
            }
            else
            {
                DisplayAlert("Failure!", "Term not updated", "Close");
            }
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}