using C971.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using SkiaSharp;

namespace C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseReport : ContentPage
    {
        public string DateStamp { get; set; }
        public static int InProgress()
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Course>();
            // Allows us to return the table query and turn it into a list
            int inProgressCount = connection.Query<Course>("SELECT * FROM Course WHERE CourseStatus = 'In Progress'").Count();

            connection.Close();
            return inProgressCount;
        }

        public static int Dropped()
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Course>();
            // Allows us to return the table query and turn it into a list
            int droppedCount = connection.Query<Course>("SELECT * FROM Course WHERE CourseStatus = 'Dropped'").Count();

            connection.Close();
            return droppedCount;
        }

        public static int Completed()
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Course>();
            // Allows us to return the table query and turn it into a list
            int completedCount = connection.Query<Course>("SELECT * FROM Course WHERE CourseStatus = 'Completed'").Count();

            connection.Close();
            return completedCount;
        }

        public static int PlanToTake()
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Course>();
            // Allows us to return the table query and turn it into a list
            int pttCount = connection.Query<Course>("SELECT * FROM Course WHERE CourseStatus = 'Plan to take'").Count();

            connection.Close();
            return pttCount;
        }

        private readonly ChartEntry[] entries = new[]
        {
            new ChartEntry(Dropped())
            {
                Label = "Dropped",
                ValueLabel = $"{Dropped()}",
                ValueLabelColor = SKColor.Parse("#2c3e50"),
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(InProgress())
            {
                Label = "In Progress",
                ValueLabel = $"{InProgress()}",
                ValueLabelColor = SKColor.Parse("#77d065"),
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(Completed())
            {
                Label = "Completed",
                ValueLabel = $"{Completed()}",
                ValueLabelColor = SKColor.Parse("#b455b6"),
                Color = SKColor.Parse("#b455b6")
            },
            new ChartEntry(PlanToTake())
            {
                Label = "Plan to take",
                ValueLabel = $"{PlanToTake()}",
                ValueLabelColor = SKColor.Parse("#3498db"),
                Color = SKColor.Parse("#3498db")
            }
        };
        public CourseReport()
        {
            InitializeComponent();
            DateStamp = "Course statuses as of:\n" + DateTime.Now.ToString("MM-dd-yyyy | HH:mm");
            BindingContext = this;
            chartViewPie.Chart = new PieChart { Entries = entries, LabelTextSize = 30 };

        }

        

    }
}