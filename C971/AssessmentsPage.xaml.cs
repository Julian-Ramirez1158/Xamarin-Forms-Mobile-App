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
    public partial class AssessmentsPage : ContentPage
    {
        public Course SelectedCourse;
        public Term SelectedTerm;
        public AssessmentsPage(Course selectedCourse, Term selectedTerm)
        {
            InitializeComponent();
            SelectedCourse = selectedCourse;
            SelectedTerm = selectedTerm;
            navigationTitle.Text = $"{selectedCourse.CourseTitle} Assessments";


            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Assessment>();
            // Allows us to return the table query and turn it into a list
            List<Course> courses = connection.Query<Course>($"SELECT * FROM Course WHERE CourseTitle =  \"{SelectedCourse.CourseTitle}\"");
            List<Assessment> entries = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseId = {courses[0].Id}").ToList();

            listView.ItemsSource = entries;

            connection.Close();
        }

        private void backButton_Clicked(object sender, EventArgs e)
        {
            //TODO fix navigation to take to appropriate course details page 
            Navigation.PushAsync(new CourseDetails(SelectedCourse, SelectedTerm));
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedAssessment = listView.SelectedItem as Assessment;

            if (selectedAssessment != null)
            {
                Navigation.PushAsync(new AssessmentDetails(selectedAssessment, SelectedCourse, SelectedTerm));
            }
        }

        private void AddAssessmentButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
                // Creates table if one doesn't already exists
                connection.CreateTable<Assessment>();
                // Allows us to return the table query and turn it into a list
                List<Course> courses = connection.Query<Course>($"SELECT * FROM Course WHERE CourseTitle =  \"{SelectedCourse.CourseTitle}\"");
                List<Assessment> assessments = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseId = {courses[0].Id}").ToList();

                if (assessments.Count >= 2)
                {
                    throw new Exception("A course may only have up to two assessments.");
                }

                Navigation.PushAsync(new AddAssessmentPage(SelectedCourse, SelectedTerm));
            }

            catch (Exception exception)
            {
                DisplayAlert("ERROR:", $"{exception.Message}", "Close");
            }
        }

    }
}