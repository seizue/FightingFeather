using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace FightingFeather
{
    public partial class DashBoardForm : MetroFramework.Forms.MetroForm
    {
        public DashBoardForm()
        {
            InitializeComponent();
            LoadChartPlasada();
            LoadChartFight();
            LoadChartTax();
            LoadChartGate();
        }

        private void LoadChartPlasada()
        {
            // Define the connection string for SQLite
            string connectionString = "Data Source=munton_summa.db;Version=3;";

            // Define the query to retrieve data from the database
            string query = "SELECT Date, Total FROM Plasada_Summary";

            // Create a new SeriesCollection to hold the chart data
            SeriesCollection series = new SeriesCollection();
            ChartValues<double> values = new ChartValues<double>();
            ChartValues<string> labels = new ChartValues<string>();

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command to execute the query
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Execute the query and create a data reader
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Iterate through the results and add them to the chart series
                        while (reader.Read())
                        {
                            // Extract the date and total values from the database
                            string dateString = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                            double total = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1);

                            // Parse the date string using the specified format if it's not empty
                            DateTime date;
                            if (!string.IsNullOrEmpty(dateString))
                            {
                                date = DateTime.ParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                labels.Add(date.ToString("MM/dd/yyyy"));
                            }

                            // Add the data to the chart series
                            values.Add(total);
                        }
                    }
                }
            }

            // Bind the chart series to the CartesianChart control
            series.Add(new ColumnSeries
            {
                Title = "Total",
                Values = values
            });

            cartesianChart_Plasada.Series = series;
            cartesianChart_Plasada.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = labels
            });
        }


        private void LoadChartFight()
        {
            // Define the connection string for SQLite
            string connectionString = "Data Source=munton_summa.db;Version=3;";

            // Define the query to retrieve data from the database for Fight
            string query = "SELECT TotalFight, Draw FROM Plasada_Summary";

            // Create a new ChartValues list to hold the data for the pie chart
            ChartValues<double> pieChartData = new ChartValues<double>();

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command to execute the query
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Execute the query and create a data reader
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Iterate through the results
                        while (reader.Read())
                        {
                            // Extract the values from the database
                            double totalFight = reader.IsDBNull(0) ? 0.0 : reader.GetDouble(0);
                            double draw = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1);

                            // Calculate the total value (sum of TotalFight and Draw)
                            double total = totalFight + draw;

                            // Add the total to the pie chart data
                            pieChartData.Add(total);
                        }
                    }
                }
            }

            // Bind the pie chart data to the pieChart_Fight control
            pieChart_Fight.Series = new SeriesCollection
    {
        new PieSeries
        {
            Title = "Total",
            Values = pieChartData
        }
    };
        }


        private void LoadChartTax()
        {
            // Define the connection string for SQLite
            string connectionString = "Data Source=munton_summa.db;Version=3;";

            // Define the query to retrieve data from the database for Tax
            string query = "SELECT Date, CityTax FROM Plasada_Summary";

            // Create a new SeriesCollection to hold the chart data for Tax
            SeriesCollection series = new SeriesCollection();
            ChartValues<double> values = new ChartValues<double>();
            ChartValues<string> labels = new ChartValues<string>();

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command to execute the query
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Execute the query and create a data reader
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Iterate through the results and add them to the chart series
                        while (reader.Read())
                        {
                            // Extract the date and cityTax values from the database
                            string dateString = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                            double cityTax = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1);

                            // Parse the date string using the specified format
                            DateTime date;
                            if (!string.IsNullOrEmpty(dateString))
                            {
                                date = DateTime.ParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                labels.Add(date.ToString("MM/dd/yyyy"));
                            }

                            // Add the data to the chart series
                            values.Add(cityTax);
                        }
                    }
                }
            }

            // Bind the chart series to the CartesianChart control for Tax
            series.Add(new LineSeries
            {
                Title = "CityTax",
                Values = values
            });

            cartesianChart_Tax.Series = series;
            cartesianChart_Tax.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = labels
            });
        }


        private void LoadChartGate()
        {
            // Define the connection string for SQLite
            string connectionString = "Data Source=munton_summa.db;Version=3;";

            // Define the query to retrieve data from the database for Gate
            string query = "SELECT Date, Gate FROM Plasada_Summary";

            // Create a new SeriesCollection to hold the chart data for Gate
            SeriesCollection series = new SeriesCollection();
            ChartValues<double> values = new ChartValues<double>();
            ChartValues<string> labels = new ChartValues<string>();

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command to execute the query
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Execute the query and create a data reader
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Iterate through the results and add them to the chart series
                        while (reader.Read())
                        {
                            // Extract the date and gate values from the database
                            string dateString = reader.GetString(0);
                            object gateValue = reader.GetValue(1); // Retrieve as object to handle potential null values

                            double gate;
                            if (gateValue != DBNull.Value && double.TryParse(gateValue.ToString(), out gate))
                            {
                                // Parse the date string using the specified format
                                DateTime date = DateTime.ParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                                // Add the data to the chart series
                                values.Add(gate);
                                labels.Add(date.ToString("MM/dd/yyyy"));
                            }
                        }
                    }
                }
            }

            // Bind the chart series to the CartesianChart control for Gate
            series.Add(new LineSeries
            {
                Title = "Gate",
                Values = values
            });

            cartesianChart_Gate.Series = series;
            cartesianChart_Gate.AxisX.Add(new Axis
            {
                Title = "Date",
                Labels = labels
            });
        }


    }
}

