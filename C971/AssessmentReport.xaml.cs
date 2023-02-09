using C971.Models;
using Microcharts;
using SkiaSharp;
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
    public partial class AssessmentReport : ContentPage
    {
        public string DateStamp { get; set; }
        public static int OAcount()
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Assessment>();
            // Allows us to return the table query and turn it into a list
            int oaCount = connection.Query<Assessment>("SELECT * FROM Assessment WHERE AssessmentType = 'Objective Assessment'").Count();

            connection.Close();
            return oaCount;
        }

        public static int PAcount()
        {
            SQLiteConnection connection = new SQLiteConnection(App.DatabaseLocation);
            // Creates table if one doesn't already exists
            connection.CreateTable<Assessment>();
            // Allows us to return the table query and turn it into a list
            int PAcount = connection.Query<Assessment>("SELECT * FROM Assessment WHERE AssessmentType = 'Performance Assessment'").Count();

            connection.Close();
            return PAcount;
        }

        private readonly ChartEntry[] entries = new[]
        {
            new ChartEntry(OAcount())
            {
                Label = "Objective",
                ValueLabel = $"{OAcount()}",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(PAcount())
            {
                Label = "Performance",
                ValueLabel = $"{PAcount()}",
                Color = SKColor.Parse("#77d065")
            }
        };

        public AssessmentReport()
        {
            InitializeComponent();
            DateStamp = "Assessment types as of:\n" + DateTime.Now.ToString("MM-dd-yyyy | HH:mm");
            BindingContext = this;
            chartViewBar.Chart = new BarChart { Entries = entries, ValueLabelOrientation = Orientation.Horizontal, LabelOrientation = Orientation.Horizontal, LabelTextSize = 30 };
        }
    }
}