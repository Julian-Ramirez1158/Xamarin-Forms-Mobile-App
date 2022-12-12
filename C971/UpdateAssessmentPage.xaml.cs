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
    public partial class UpdateAssessmentPage : ContentPage
    {
        public AssessmentDetails AssessmentDetails;
        public Course SelectedCourse;
        public Term SelectedTerm;

        public UpdateAssessmentPage(AssessmentDetails assessmentDetails, Course selectedCourse, Term selectedTerm)
        {
            InitializeComponent();

            AssessmentDetails = assessmentDetails;
            SelectedCourse = selectedCourse;
            SelectedTerm = selectedTerm;

            assessmentTitle.Text = assessmentDetails.SelectedAssessment.AssessmentTitle;
            startDateEntered.Date = assessmentDetails.SelectedAssessment.Start;
            endDateEntered.Date = assessmentDetails.SelectedAssessment.End;
            assessmentType.SelectedItem = assessmentDetails.SelectedAssessment.AssessmentType;
            notificationButton.IsToggled = assessmentDetails.SelectedAssessment.NotificationsOn;

            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void SaveAssessmentButton_Clicked(object sender, EventArgs e)
        {
            AssessmentDetails.SelectedAssessment.AssessmentTitle = assessmentTitle.Text;
            AssessmentDetails.SelectedAssessment.Start = startDateEntered.Date;
            AssessmentDetails.SelectedAssessment.End = endDateEntered.Date;
            AssessmentDetails.SelectedAssessment.AssessmentType = (string)assessmentType.SelectedItem;
            AssessmentDetails.SelectedAssessment.NotificationsOn = notificationButton.IsToggled;

            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Assessment>();

            // update data
            int rowsInserted = connection.Update(AssessmentDetails.SelectedAssessment);

            // close the connection
            connection.Close();

            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Assessment succesffuly updated", "Close");
                // TODO fix navigation problem where back button takes you to previously unsaved entry

                Navigation.PushAsync(new AssessmentDetails(AssessmentDetails.SelectedAssessment, AssessmentDetails.SelectedCourse, AssessmentDetails.SelectedTerm));
            }
            else
            {
                DisplayAlert("Failure!", "Assessment not updated", "Close");
            }
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}