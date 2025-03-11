using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace MusicV1
{
    public partial class Chart: Form
    {
        public Chart()
        {
            InitializeComponent();
        }

        private void Chart_Load(object sender, EventArgs e)
        {

        }
        public void ShowStats(List<Album> albums)
        {
            //  1850 year and above
            var filteredAlbums = albums
                .Where(a => a.release >= 1850)    // skip year < 1850
                .ToList();

            // get release years
            var yearGroups = filteredAlbums
                .Where(a => a.release > 0)
                .GroupBy(a => a.release)
                .OrderBy(g => g.Key)
                .Select(g => new { Year = g.Key, Count = g.Count() })
                .ToList();

            // erorr massageBox
            if (!yearGroups.Any())
            {
                chartReleases.Series.Clear();
                MessageBox.Show("No albums found with year >= 1850.");
                return;
            }
            //clear interface and create new one 
            chartReleases.Series.Clear();
            chartReleases.ChartAreas.Clear();
            chartReleases.ChartAreas.Add(new ChartArea("MainArea"));

            var series = new Series("Releases per Year")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.MediumPurple
            };
            chartReleases.Series.Add(series);

            // Add data 
            foreach (var grp in yearGroups)
                series.Points.AddXY(grp.Year, grp.Count);

            // 3) set X-axis to 1850
            int minYear = yearGroups.First().Year;
            int maxYear = yearGroups.Last().Year;

            // If the first album is after 1850, use that
            if (minYear < 1850) minYear = 1850;

            chartReleases.ChartAreas["MainArea"].AxisX.Minimum = minYear;
            chartReleases.ChartAreas["MainArea"].AxisX.Maximum = maxYear;
            chartReleases.ChartAreas["MainArea"].AxisX.Interval = 4;
            chartReleases.ChartAreas["MainArea"].AxisX.IsStartedFromZero = false;
            chartReleases.ChartAreas["MainArea"].AxisX.ScaleView.Zoom(minYear, maxYear);

            // style
            chartReleases.ChartAreas["MainArea"].BackColor = Color.White;
            chartReleases.BackColor = Color.White;

            // Titles
            chartReleases.Titles.Clear();
            chartReleases.Titles.Add("Releases per Year");
            chartReleases.ChartAreas["MainArea"].AxisX.Title = "Year";
            chartReleases.ChartAreas["MainArea"].AxisY.Title = "Count of Albums";
        }

    }
}