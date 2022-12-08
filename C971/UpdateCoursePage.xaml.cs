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
    public partial class UpdateCoursePage : ContentPage
    {
        public CourseDetails CourseDetails;
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

        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            CourseDetails.selectedCourse.CourseTitle = courseTitle.Text;
            CourseDetails.selectedCourse.Start = startDateEntered.Date;
            CourseDetails.selectedCourse.End = endDateEntered.Date;
            CourseDetails.selectedCourse.CourseStatus = (string)courseStatus.SelectedItem;
            CourseDetails.selectedCourse.InstructorName = instructorName.Text;
            CourseDetails.selectedCourse.InstructorPhone = instructorPhone.Text;
            CourseDetails.selectedCourse.InstructorEmail = instructorEmail.Text;

            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Course>();

            // update data
            int rowsInserted = connection.Update(CourseDetails.selectedCourse);

            // close the connection
            connection.Close();

            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Course succesffuly updated", "Close");
                // TODO fix navigation problem where back button takes you to previously unsaved entry

                Navigation.PushAsync(new CourseDetails(CourseDetails.selectedCourse));
            }
            else
            {
                DisplayAlert("Failure!", "Course not updated", "Close");
            }


        }
    }
}