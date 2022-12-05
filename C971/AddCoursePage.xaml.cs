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

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            Course course = new Course()
            {
                CourseTitle = courseTitle.Text,
                TermId = SelectedTerm.Id,
                Start = startDateEntered.Date,
                End = endDateEntered.Date,
                CourseStatus = (string)courseStatus.SelectedItem,
                InstructorName = instructorName.Text,
                InstructorPhone = instructorPhone.Text,
                InstructorEmail = instructorEmail.Text
            };

            // open connection to db
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            // creates a type Post table
            connection.CreateTable<Course>();


            // insert data
            int rowsInserted = connection.Insert(course);

            // close the connection
            connection.Close();


            // TODO: add actual data validation here
            if (rowsInserted > 0)
            {
                DisplayAlert("Success!", "Term succesffuly inserted", "Close");
                Navigation.PushAsync(new TermDetails(SelectedTerm));
            }
            else
            {
                DisplayAlert("Failure!", "Term not inserted", "Close");
            }
        }
    }
}