using C971.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCoursePage : ContentPage
    {
        public Term SelectedTerm;
        public AddCoursePage(Term selectedTerm)
        {
            InitializeComponent();

            SelectedTerm = selectedTerm;

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


                Course course = new Course()
                {
                    CourseTitle = courseTitle.Text,
                    TermId = SelectedTerm.Id,
                    Start = startDateEntered.Date,
                    End = endDateEntered.Date,
                    CourseStatus = (string)courseStatus.SelectedItem,
                    InstructorName = instructorName.Text,
                    InstructorPhone = instructorPhone.Text,
                    InstructorEmail = instructorEmail.Text,
                    CourseNotes = courseNotes.Text,
                    NotificationsOn = notificationButton.IsToggled
                };

                // open connection to db
                SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

                // creates a type Post table
                connection.CreateTable<Course>();


                // insert data
                int rowsInserted = connection.Insert(course);

                // close the connection
                connection.Close();

                DisplayAlert("Success!", "Course succesffuly inserted", "Close");
                Navigation.PushAsync(new TermDetails(SelectedTerm));
            }
            catch (Exception exception)
            {
                DisplayAlert("ERROR:", $"{exception.Message}\nCourse not inserted", "Close");
            }

            

        }
    }
}