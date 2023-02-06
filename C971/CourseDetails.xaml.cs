using C971.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseDetails : ContentPage
    {
        public Course selectedCourse;
        public Term SelectedTerm;
        public CourseDetails(Course selectedCourse, Term selectedTerm)
        {
            InitializeComponent();

            this.selectedCourse = selectedCourse;
            SelectedTerm = selectedTerm;
            navigationTitle.Text = selectedCourse.CourseTitle;
            courseDates.Text = $"Start Date: {selectedCourse.Start:MM-dd-yyyy}\nEnd Date: {selectedCourse.End:MM-dd-yyyy}";
            courseStatus.Text = $"Course Status: {selectedCourse.CourseStatus}";
            instructorName.Text = $"Instructor Name: {selectedCourse.InstructorName}";
            instructorPhone.Text = $"Instructor Phone: {selectedCourse.InstructorPhone}";
            instructorEmail.Text = $"Instructor Email: {selectedCourse.InstructorEmail}";
            courseNotes.Text = selectedCourse.CourseNotes;
            notifications.Text = selectedCourse.NotificationsOn ? "Notifications Enabled" : "Notifications Disabled";


            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void termButton_Clicked(object sender, EventArgs e)
        {
            //TODO fix this navigation --> needs to point to appropriate term details page
            Navigation.PushAsync(new TermDetails(SelectedTerm));
        }

        private void AssessmentButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentsPage(selectedCourse, SelectedTerm));
        }

        private void updateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UpdateCoursePage(this));
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            SQLiteConnection connection2 = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection2.CreateTable<Assessment>();
            // Allows us to return the table query and turn it into a list
            List<Course> courses = connection2.Query<Course>($"SELECT * FROM Course WHERE CourseTitle =  \"{selectedCourse.CourseTitle}\"");
            int assessmentsCount = connection2.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseId = {courses[0].Id}").ToList().Count();

            connection2.Close();

            if (assessmentsCount == 0)
            {
                SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

                connection.CreateTable<Term>();

                // delete data
                int rowsDeleted = connection.Delete(selectedCourse);

                // close the connection
                connection.Close();
                DisplayAlert("Success!", "Course succesffuly deleted", "Close");
                //work around for non-async navigation
                Navigation.PushAsync(new TermDetails(SelectedTerm));
            }
            else
            {
                DisplayAlert("Error!", "Course not deleted due to existing associated assessments.", "Close");
            }

        }

        private void shareNotes_Clicked(object sender, EventArgs e)
        {
            Share.RequestAsync($"Notes from {navigationTitle.Text}:\n{courseNotes.Text}");
        }
    }
}