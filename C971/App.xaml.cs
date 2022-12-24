using C971.Models;
using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971
{
    public partial class App : Application
    {
        public static string DatabaseLocation = "";

        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new MainPage());

        }

        public App(string databaseLocation)
        {
            InitializeComponent();

            DatabaseLocation = databaseLocation;
            Notifications();
            SampleData();
            MainPage = new NavigationPage(new MainPage());

            
        }

        public static void Notifications()
        {
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Create tables
            SQLiteConnection connection = new SQLiteConnection(DatabaseLocation);

            connection.CreateTable<Course>();
            connection.CreateTable<Assessment>();
            
            // Allows us to return the table query and turn it into a list
            var courses = connection.Table<Course>().ToList();
            var assessments = connection.Table<Assessment>().ToList();

            
            courses.ForEach(course =>
            {
                if (course.NotificationsOn && new DateTime(course.Start.Year, course.Start.Month, course.Start.Day) == today)
                {
                    CrossLocalNotifications.Current.Show("Course Start Alert:", $"{course.CourseTitle} is starting today!");
                }
                if (course.NotificationsOn && new DateTime(course.End.Year, course.End.Month, course.End.Day) == today)
                {
                    CrossLocalNotifications.Current.Show("Course End Alert:", $"{course.CourseTitle} is ending today!");
                }
            });
            assessments.ForEach(assessment =>
            {
                if (assessment.NotificationsOn && new DateTime(assessment.Start.Year, assessment.Start.Month, assessment.Start.Day) == today)
                {
                    CrossLocalNotifications.Current.Show("Assessment Start Alert:", $"{assessment.AssessmentTitle} due date start is today!");
                }
                if (assessment.NotificationsOn && new DateTime(assessment.End.Year, assessment.End.Month, assessment.End.Day) == today)
                {
                    CrossLocalNotifications.Current.Show("Assessment End Alert:", $"{assessment.AssessmentTitle} due date end is today!");
                }
            });

            connection.Close();
        }

        private void SampleData()
        {
            var sampleDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            Term term = new Term()
            {
                Title = "Sample Term",
                Start = sampleDate,
                End = sampleDate.AddMonths(6).AddDays(-1)
            };
            
            Course course = new Course()
            {
                CourseTitle = "SampleData Course 1",
                Start = sampleDate,
                End = sampleDate.AddMonths(4).AddDays(-1),
                CourseStatus = "Plan to take",
                InstructorName = "Julian Ramirez",
                InstructorPhone = "561-414-1697",
                InstructorEmail = "jram417@wgu.edu",
                NotificationsOn = true
            };
            Assessment assessment1 = new Assessment()
            {
                AssessmentTitle = "Sample Assessment 1",
                Start = sampleDate.AddMonths(1).AddDays(-1),
                End = sampleDate.AddMonths(2),
                AssessmentType = "Objective Assessment",
                NotificationsOn = true
            };
            Assessment assessment2 = new Assessment()
            {
                AssessmentTitle = "Sample Assessment 2",
                Start = sampleDate.AddMonths(2).AddDays(-1),
                End = sampleDate.AddMonths(3),
                AssessmentType = "Performance Assessment",
                NotificationsOn = true
            };
            // open connection to db
            SQLiteConnection connection = new SQLiteConnection(DatabaseLocation);

            // creates a type Term/Course/Assessment tables
            connection.CreateTable<Term>();
            connection.CreateTable<Course>();
            connection.CreateTable<Assessment>();

            var Terms = connection.Table<Term>().ToList();
            
            if (Terms.Count == 0)
            {
                // insert data
                connection.Insert(term);
                List<Term> terms = connection.Query<Term>($"SELECT * FROM Term WHERE Title =  '{term.Title}'");
                course.TermId = terms[0].Id;
                connection.Insert(course);
                List<Course> courses = connection.Query<Course>($"SELECT * FROM Course WHERE CourseTitle =  '{course.CourseTitle}'");
                assessment1.CourseId = courses[0].Id;
                assessment2.CourseId = courses[0].Id;
                connection.Insert(assessment1);
                connection.Insert(assessment2);

                // close the connection
                connection.Close();
            }
            else
            {
                return;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
