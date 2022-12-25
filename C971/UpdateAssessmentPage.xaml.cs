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
            try
            {
                if (assessmentTitle.Text == "" || assessmentTitle.Text == null)
                {
                    throw new Exception("Assessment Title is required.");
                }

                if (new DateTime(startDateEntered.Date.Year, startDateEntered.Date.Month, startDateEntered.Date.Day) > new DateTime(endDateEntered.Date.Year, endDateEntered.Date.Month, endDateEntered.Date.Day))
                {
                    throw new Exception("The assessment start date cannot be scheduled after the assessment end date");
                }

                if (assessmentType.SelectedItem == null)
                {
                    throw new Exception("An assessment type is required.");
                }

                SQLiteConnection assessmentConnection = new SQLiteConnection(App.DatabaseLocation);
                List<Course> courses = assessmentConnection.Query<Course>($"SELECT * FROM Course WHERE CourseTitle =  '{SelectedCourse.CourseTitle}'");
                List<Assessment> Assessments = assessmentConnection.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseId = {courses[0].Id}").ToList();


                if (AssessmentDetails.SelectedAssessment.Id != Assessments[0].Id)
                {
                    if (Assessments.First().AssessmentType == assessmentType.SelectedItem.ToString())
                    {
                        throw new Exception("Only one of each type of assessment is accepted.");
                    }
                }
                if (AssessmentDetails.SelectedAssessment.Id != Assessments[1].Id)
                {
                    if (Assessments.Last().AssessmentType == assessmentType.SelectedItem.ToString())
                    {
                        throw new Exception("Only one of each type of assessment is accepted.");
                    }
                }

                assessmentConnection.Close();

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

                DisplayAlert("Success!", "Assessment succesffuly updated", "Close");
                Navigation.PushAsync(new AssessmentDetails(AssessmentDetails.SelectedAssessment, AssessmentDetails.SelectedCourse, AssessmentDetails.SelectedTerm));

            }
            catch (Exception exception)
            {
                DisplayAlert("ERROR:", $"{exception.Message}\nAssessment not updated", "Close");
            }

        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}