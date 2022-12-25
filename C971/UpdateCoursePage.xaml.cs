using C971.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateCoursePage : ContentPage
    {
        public CourseDetails CourseDetails;
        public Course SelectedCourse;
        public UpdateCoursePage(CourseDetails courseDetails)
        {
            InitializeComponent();

            CourseDetails = courseDetails;
            courseTitle.Text = courseDetails.selectedCourse.CourseTitle;
            startDateEntered.Date = courseDetails.selectedCourse.Start;
            endDateEntered.Date = courseDetails.selectedCourse.End;
            courseStatus.SelectedItem = courseDetails.selectedCourse.CourseStatus;
            instructorName.Text = courseDetails.selectedCourse.InstructorName;
            instructorPhone.Text = courseDetails.selectedCourse.InstructorPhone;
            instructorEmail.Text = courseDetails.selectedCourse.InstructorEmail;
            courseNotes.Text = courseDetails.selectedCourse.CourseNotes;
            notificationButton.IsToggled = courseDetails.selectedCourse.NotificationsOn;


            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public bool ValidateEmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {

            try
            {

                bool validateEmail = ValidateEmailAddress(instructorEmail.Text);

                if (courseTitle.Text == null || courseTitle.Text == "")
                {
                    throw new Exception("Course title is required.");
                }

                if (new DateTime(startDateEntered.Date.Year, startDateEntered.Date.Month, startDateEntered.Date.Day) > new DateTime(endDateEntered.Date.Year, endDateEntered.Date.Month, endDateEntered.Date.Day))
                {
                    throw new Exception("The course start date cannot be scheduled after the course end date");
                }

                if (courseStatus.SelectedItem == null)
                {
                    throw new Exception("Course status is required.");
                }

                if (
                        instructorName.Text == null || instructorName.Text == "" ||
                        instructorPhone.Text == null || instructorPhone.Text == "" ||
                        instructorEmail.Text == null || instructorEmail.Text == ""
                    )
                {
                    throw new Exception("Instructor name, email, and phone number are required.");
                }

                if (!validateEmail)
                {
                    throw new Exception("A valid email is required.");
                }

                if (courseNotes.Text == null)
                {
                    courseNotes.Text = "";
                }

                CourseDetails.selectedCourse.CourseTitle = courseTitle.Text;
                CourseDetails.selectedCourse.Start = startDateEntered.Date;
                CourseDetails.selectedCourse.End = endDateEntered.Date;
                CourseDetails.selectedCourse.CourseStatus = (string)courseStatus.SelectedItem;
                CourseDetails.selectedCourse.InstructorName = instructorName.Text;
                CourseDetails.selectedCourse.InstructorPhone = instructorPhone.Text;
                CourseDetails.selectedCourse.InstructorEmail = instructorEmail.Text;
                CourseDetails.selectedCourse.CourseNotes = courseNotes.Text;
                CourseDetails.selectedCourse.NotificationsOn = notificationButton.IsToggled;

                SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

                connection.CreateTable<Course>();

                // update data
                int rowsInserted = connection.Update(CourseDetails.selectedCourse);

                // close the connection
                connection.Close();

                DisplayAlert("Success!", "Course succesffuly updated", "Close");
                Navigation.PushAsync(new CourseDetails(CourseDetails.selectedCourse, CourseDetails.SelectedTerm));

            }
            catch (Exception exception)
            {
                DisplayAlert("ERROR:", $"{exception.Message}\nCourse not updated", "Close");
            }

        }
    }
}