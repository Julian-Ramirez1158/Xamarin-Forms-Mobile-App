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
        public Term SelectedTerm;
        public AddAssessmentPage(Course selectedCourse, Term selectedTerm)
        {
            InitializeComponent();
            SelectedCourse = selectedCourse;
            SelectedTerm = selectedTerm;



            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
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

                //SQLiteConnection assessmentConnection = new SQLiteConnection(App.DatabaseLocation);
                //var Assessments = assessmentConnection.Table<Assessment>().ToList();
                SQLiteConnection assessmentConnection = new SQLiteConnection(App.DatabaseLocation);
                List<Course> courses = assessmentConnection.Query<Course>($"SELECT * FROM Course WHERE CourseTitle =  '{SelectedCourse.CourseTitle}'");
                List<Assessment> Assessments = assessmentConnection.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseId = {courses[0].Id}").ToList();

                if (Assessments.Any())
                {
                    if (Assessments.First().AssessmentType == assessmentType.SelectedItem.ToString())
                    {
                        throw new Exception("Only one of each type of assessment is accepted.");
                    }
                }
                assessmentConnection.Close();

                Assessment assessment = new Assessment()
                {
                    AssessmentTitle = assessmentTitle.Text,
                    CourseId = SelectedCourse.Id,
                    Start = startDateEntered.Date,
                    End = endDateEntered.Date,
                    AssessmentType = (string)assessmentType.SelectedItem,
                    NotificationsOn = notificationButton.IsToggled
                };

                // open connection to db
                SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

                // creates a type Assessment table
                connection.CreateTable<Assessment>();


                // insert data
                int rowsInserted = connection.Insert(assessment);

                // close the connection
                connection.Close();

                DisplayAlert("Success!", "Assessment succesffuly inserted", "Close");
                Navigation.PushAsync(new AssessmentsPage(SelectedCourse, SelectedTerm));

            }
            catch (Exception exception)
            {
                DisplayAlert("ERROR:", $"{exception.Message}\nAssessment not inserted", "Close");
            }
            
            
        }
    }
}