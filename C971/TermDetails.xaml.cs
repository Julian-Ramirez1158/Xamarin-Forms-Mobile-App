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

            navigationTitle.Text = selectedTerm.Title;
            termDates.Text = $"Start Date: {selectedTerm.Start:MM-dd-yyyy}\nEnd Date: {selectedTerm.End:MM-dd-yyyy}";

            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Course>();
            // Allows us to return the table query and turn it into a list
            List<Course> entries = connection.Query<Course>($"SELECT * FROM Course WHERE TermId = {selectedTerm.Id}").ToList();
                //Table<Course.Where<Course.Equals(SelectedTerm.Id)>().ToList();

            listView.ItemsSource = entries;

            connection.Close();
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
            Navigation.PushAsync(new AddCoursePage(selectedTerm));
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
                Navigation.PushAsync(new CourseDetails(selectedCourse));
            }
        }
    }
    
}