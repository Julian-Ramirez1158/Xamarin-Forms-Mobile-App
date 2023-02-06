using C971.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermDetails : ContentPage
    {
        public Term selectedTerm;
        public TermDetails(Term selectedTerm)
        {
            InitializeComponent();

            this.selectedTerm = selectedTerm;

            navigationTitle.Text = $"{selectedTerm.Title} Courses";
            termDates.Text = $"Start Date: {selectedTerm.Start:MM-dd-yyyy}\nEnd Date: {selectedTerm.End:MM-dd-yyyy}";

            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Term>();
            connection.CreateTable<Course>();
            // Allows us to return the table query and turn it into a list
            List<Term> terms = connection.Query<Term>($"SELECT * FROM Term WHERE Title =  \"{selectedTerm.Title}\"");
            List<Course> entries = connection.Query<Course>($"SELECT * FROM Course WHERE TermId = {terms[0].Id}").ToList();
                //Table<Course.Where<Course.Equals(SelectedTerm.Id)>().ToList();

            listView.ItemsSource = entries;

            connection.Close();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Term>();
            connection.CreateTable<Course>();
            // Allows us to return the table query and turn it into a list
            List<Term> terms = connection.Query<Term>($"SELECT * FROM Term WHERE Title =  '{selectedTerm.Title}'");
            List<Course> entries = connection.Query<Course>($"SELECT * FROM Course WHERE TermId = {terms[0].Id}").ToList();
            //Table<Course.Where<Course.Equals(SelectedTerm.Id)>().ToList();

            listView.ItemsSource = entries;

            connection.Close();

            listView.ItemsSource = entries.Where(s => //s.CourseTitle.ToLower().StartsWith(e.NewTextValue) ||
                                                      //s.CourseTitle.StartsWith(e.NewTextValue))
                                                      s.CourseTitle.ToUpper().Contains(searchBarText.Text.ToUpper()));
        }

        private void updateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UpdateTermPage(this));
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Term>();

            // delete data
            int rowsDeleted = connection.Delete(selectedTerm);

            // close the connection
            connection.Close();

            // TODO: add some actual validation here you twit
            if (rowsDeleted > 0)
            {
                DisplayAlert("Success!", "Term succesffuly deleted", "Close");
                Navigation.PushAsync(new TermHomePage());
            }
            else
            {
                DisplayAlert("Failure!", "Term not deleted", "Close");
            }
        }

        private void addCourseButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
                // Creates table if one doesn't already exists
                connection.CreateTable<Term>();
                connection.CreateTable<Course>();
                // Allows us to return the table query and turn it into a list
                List<Term> terms = connection.Query<Term>($"SELECT * FROM Term WHERE Title =  '{selectedTerm.Title}'");
                List<Course> courses = connection.Query<Course>($"SELECT * FROM Course WHERE TermId = {terms[0].Id}").ToList();

                if (courses.Count >= 6)
                {
                    throw new Exception("A term may only have up to six courses.");
                }

                Navigation.PushAsync(new AddCoursePage(selectedTerm));
            }
            catch (Exception exception)
            {
                DisplayAlert("ERROR:", $"{exception.Message}", "Close");
            }
            
        }

        private void homeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermHomePage());
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCourse = listView.SelectedItem as Course;

            if (selectedCourse != null)
            {
                Navigation.PushAsync(new CourseDetails(selectedCourse, selectedTerm));
            }
        }

    }
    
}