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
    public partial class AssessmentDetails : ContentPage
    {
        public Assessment SelectedAssessment;
        public Course SelectedCourse;
        public Term SelectedTerm;
        public AssessmentDetails(Assessment selectedAssessment, Course selectedCourse, Term selectedTerm)
        {
            InitializeComponent();

            SelectedAssessment = selectedAssessment;
            SelectedCourse = selectedCourse;
            SelectedTerm = selectedTerm;
            navigationTitle.Text = selectedAssessment.AssessmentTitle;
            assessmentDates.Text = $"Start Date: {selectedAssessment.Start:MM-dd-yyyy}\nEnd Date: {selectedAssessment.End:MM-dd-yyyy}";
            assessmentType.Text = $"Assessment Type: {selectedAssessment.AssessmentType}";
            notifications.Text = selectedAssessment.NotificationsOn ? "Notifications: Enabled" : "Notifications: Disabled";


            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void courseButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentsPage(SelectedCourse, SelectedTerm));
        }

        private void UpdateAssessmentButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UpdateAssessmentPage(this, SelectedCourse, SelectedTerm));
        }

        private void DeleteAssessmentButton_Clicked(object sender, EventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Assessment>();

            // delete data
            int rowsDeleted = connection.Delete(SelectedAssessment);

            // close the connection
            connection.Close();

            // TODO: add some actual validation here you twit
            if (rowsDeleted > 0)
            {
                DisplayAlert("Success!", "Assessment succesffuly deleted", "Close");
                //work around for non-async navigation
                Navigation.PushAsync(new AssessmentsPage(SelectedCourse, SelectedTerm));
            }
            else
            {
                DisplayAlert("Failure!", "Assessment not deleted", "Close");
            }
        }

        
    }
}