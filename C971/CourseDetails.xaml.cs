﻿using C971.Models;
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
    public partial class CourseDetails : ContentPage
    {
        public Course selectedCourse;
        public CourseDetails(Course selectedCourse)
        {
            InitializeComponent();

            this.selectedCourse = selectedCourse;
            navigationTitle.Text = selectedCourse.CourseTitle;
            courseDates.Text = $"Start Date: {selectedCourse.Start:MM-dd-yyyy}\nEnd Date: {selectedCourse.End:MM-dd-yyyy}";
            courseStatus.Text = $"Course Status: {selectedCourse.CourseStatus}";
            instructorName.Text = $"Instructor Name: {selectedCourse.InstructorName}";
            instructorPhone.Text = $"Instructor Phone: {selectedCourse.InstructorPhone}";
            instructorEmail.Text = $"Instructor Email: {selectedCourse.InstructorEmail}";


            // Hide default android navbar back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private void termButton_Clicked(object sender, EventArgs e)
        {
            //TODO fix this navigation --> needs to point to appropriate term details page
            Navigation.PopAsync();
        }

        private void AssessmentButton_Clicked(object sender, EventArgs e)
        {
            
        }

        private void updateButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UpdateCoursePage(this));
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);

            connection.CreateTable<Term>();

            // delete data
            int rowsDeleted = connection.Delete(selectedCourse);

            // close the connection
            connection.Close();

            // TODO: add some actual validation here you twit
            if (rowsDeleted > 0)
            {
                DisplayAlert("Success!", "Course succesffuly deleted", "Close");
                //work around for non-async navigation
                Navigation.RemovePage(this);
            }
            else
            {
                DisplayAlert("Failure!", "Course not deleted", "Close");
            }

        }
    }
}