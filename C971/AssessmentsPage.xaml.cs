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
        public AssessmentsPage(Course selectedCourse)
        {
            InitializeComponent();
            SelectedCourse = selectedCourse;
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
            List<Assessment> entries = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseId = {SelectedCourse.Id}").ToList();

            listView.ItemsSource = entries;

            connection.Close();
        }

        private void backButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void AddAssessmentButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddAssessmentPage(SelectedCourse));
        }
    }
}