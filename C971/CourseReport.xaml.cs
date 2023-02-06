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
        private readonly ChartEntry[] entries = new[]
        {
            new ChartEntry(1)
            {
                Label = "Dropped",
                ValueLabel = "1",
                ValueLabelColor = SKColor.Parse("#2c3e50"),
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(3)
            {
                Label = "In Progress",
                ValueLabel = "3",
                ValueLabelColor = SKColor.Parse("#77d065"),
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(12)
            {
                Label = "Completed",
                ValueLabel = "12",
                ValueLabelColor = SKColor.Parse("#b455b6"),
                Color = SKColor.Parse("#b455b6")
            },
            new ChartEntry(8)
            {
                Label = "Plan to take",
                ValueLabel = "8",
                ValueLabelColor = SKColor.Parse("#3498db"),
                Color = SKColor.Parse("#3498db")
            }
        };
        public CourseReport()
        {
            InitializeComponent();
            chartViewPie.Chart = new PieChart { Entries = entries, LabelTextSize = 35 };

        }

        

    }
}