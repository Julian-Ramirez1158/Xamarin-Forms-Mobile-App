using C971.Models;
using SQLite;
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
    public partial class AddAssessmentPage : ContentPage
    {
        public Course SelectedCourse;
        public AddAssessmentPage(Course selectedCourse)
        {
            InitializeComponent();
            SelectedCourse = selectedCourse;




            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void SaveAssessmentButton_Clicked(object sender, EventArgs e)
        {
            Assessment assessment = new Assessment()
            {
                AssessmentTitle = assessmentTitle.Text,
                Start = startDateEntered.Date,
                End = endDateEntered.Date,
                AssessmentType = (string)assessmentType.SelectedItem,
                NotificationsOn = notificationButton.IsToggled
            };

            // open connection to db
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            // creates a type Post table
            connection.CreateTable<Assessment>();


            // insert data
            int rowsInserted = connection.Insert(assessment);

            // close the connection
            connection.Close();


            // TODO: add actual data validation here
            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Assessment succesffuly inserted", "Close");
                Navigation.PushAsync(new AssessmentsPage(SelectedCourse));
            }
            else
            {
                DisplayAlert("Failure!", "Assessment not inserted", "Close");
            }
        }
    }
}