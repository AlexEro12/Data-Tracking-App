using System;
using System.Collections.Generic;
using System.IO;

namespace DataTrackingApp
{
    class Program
    {
        static List<DailyMetrics> metricsList = new List<DailyMetrics>(); // List to store the daily metrics
        static string dataFilePath = "metrics.txt"; // Path to the data file

        static void Main(string[] args)
        {
            LoadData(); // Load previously saved data

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("Data Tracking Application");
                Console.WriteLine("-------------------------");
                Console.WriteLine("1. Enter Daily Metrics");
                Console.WriteLine("2. View Daily Metrics");
                Console.WriteLine("3. Save and Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EnterDailyMetrics();
                        break;
                    case "2":
                        ViewDailyMetrics();
                        break;
                    case "3":
                        SaveData(); // Save the data before exiting
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void EnterDailyMetrics()
        {
            Console.WriteLine("Enter Daily Metrics");
            Console.WriteLine("-------------------");

            DailyMetrics metrics = new DailyMetrics();

            Console.Write("Enter weight (in kilograms): ");
            metrics.Weight = double.Parse(Console.ReadLine());

            Console.Write("Enter calories consumed: ");
            metrics.CaloriesConsumed = int.Parse(Console.ReadLine());

            Console.Write("Enter protein intake: ");
            metrics.ProteinIntake = double.Parse(Console.ReadLine());

            Console.Write("Did you workout today? (Y/N): ");
            string workoutInput = Console.ReadLine().ToUpper();
            metrics.Workout = workoutInput == "Y";

            CalculateProgress(metrics);

            metricsList.Add(metrics);

            Console.WriteLine("Daily metrics added successfully!");
        }

        static void ViewDailyMetrics()
        {
            Console.WriteLine("View Daily Metrics");
            Console.WriteLine("------------------");

            if (metricsList.Count == 0)
            {
                Console.WriteLine("No daily metrics recorded.");
                return;
            }

            Console.WriteLine("Date\t\tWeight (kg)\tCalories\tProtein\tWorkout\tWeight Progress\tCalories Progress\tProtein Progress");

            foreach (var metrics in metricsList)
            {
                Console.WriteLine($"{metrics.Date.ToShortDateString()}\t{metrics.Weight.ToString("F2"),10}\t{metrics.CaloriesConsumed,8}\t{metrics.ProteinIntake.ToString("F2"),7}\t{(metrics.Workout ? "Yes" : "No"),7}\t{metrics.WeightProgress.ToString("+0.00;-0.00"),15}\t{metrics.CaloriesProgress.ToString("+0;-0"),18}\t{metrics.ProteinProgress.ToString("+0.00;-0.00"),16}");
            }
        }

        static void CalculateProgress(DailyMetrics metrics)
        {
            if (metricsList.Count > 0)
            {
                var previousMetrics = metricsList[metricsList.Count - 1];
                metrics.WeightProgress = metrics.Weight - previousMetrics.Weight;
                metrics.CaloriesProgress = metrics.CaloriesConsumed - previousMetrics.CaloriesConsumed;
                metrics.ProteinProgress = metrics.ProteinIntake - previousMetrics.ProteinIntake;
            }
            else
            {
                metrics.WeightProgress = 0;
                metrics.CaloriesProgress = 0;
                metrics.ProteinProgress = 0;
            }
        }

        static void SaveData()
        {
            using (StreamWriter writer = new StreamWriter(dataFilePath))
            {
                foreach (var metrics in metricsList)
                {
                    string dataLine = $"{metrics.Date.ToShortDateString()},{metrics.Weight},{metrics.CaloriesConsumed},{metrics.ProteinIntake},{(metrics.Workout ? "1" : "0")}";
                    writer.WriteLine(dataLine);
                }
            }

            Console.WriteLine("Data saved successfully!");
        }

        static void LoadData()
        {
            if (File.Exists(dataFilePath))
            {
                using (StreamReader reader = new StreamReader(dataFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');

                        DailyMetrics metrics = new DailyMetrics();
                        metrics.Date = DateTime.Parse(data[0]);
                        metrics.Weight = double.Parse(data[1]);
                        metrics.CaloriesConsumed = int.Parse(data[2]);
                        metrics.ProteinIntake = double.Parse(data[3]);
                        metrics.Workout = data[4] == "1";

                        metricsList.Add(metrics);
                    }
                }
            }
        }
    }

    class DailyMetrics
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public double Weight { get; set; }
        public int CaloriesConsumed { get; set; }
        public double ProteinIntake { get; set; }
        public bool Workout { get; set; }
        public double WeightProgress { get; set; }
        public int CaloriesProgress { get; set; }
        public double ProteinProgress { get; set; }
    }
}
