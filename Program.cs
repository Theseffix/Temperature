using System;
using Microsoft.EntityFrameworkCore;
using temperature.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace temperature
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                bool redo = true;
                bool outside = true;
                DateTime Date = new DateTime(2016, 11, 08);
                int MenyChoice = MainMeny();

                switch (MenyChoice)
                {
                    case 1:
                        Console.Clear();
                        while (redo)
                        {
                            Console.WriteLine("Date? (ex. 2016-11-08)");
                            string DateS = Console.ReadLine();
                            string[] DateA = DateS.Split('-');
                            try
                            {
                                Date = new DateTime(int.Parse(DateA[0]), int.Parse(DateA[1]), int.Parse(DateA[2]));
                                redo = false;
                            }
                            catch { Console.WriteLine("Error: Userinput incorrect, please follow ex."); }
                        }

                        Console.Clear();
                        Console.WriteLine("1. Outside");
                        Console.WriteLine("2. Inside");

                        int Choice = GetInt32Input(1, 2);
                        if (Choice == 2)outside = false;
                        AverageTemp(Date, outside);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("1. Outside");
                        Console.WriteLine("2. Inside");

                        int Choice2 = GetInt32Input(1, 2);
                        if (Choice2 == 2)outside = false;
                        TopTemp(outside);
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("1. Outside");
                        Console.WriteLine("2. Inside");

                        int Choice3 = GetInt32Input(1, 2);
                        if (Choice3 == 2)outside = false;
                        TopMoist(outside);
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("1. Outside");
                        Console.WriteLine("2. Inside");

                        int Choice4 = GetInt32Input(1, 2);
                        if (Choice4 == 2) outside = false;
                        TopChanceMold(outside);
                        break;
                    case 5:
                        MeteorologicalAutumn();
                        break;
                    case 6:
                        MeteorologicalWinter();
                        break;
                    case 7:
                        TopDoorOpen();
                        break;
                    case 8:
                        Console.Clear();
                        Console.WriteLine("1. Highest difference");
                        Console.WriteLine("2. Lowest difference");

                        int Choice5 = GetInt32Input(1, 2);
                        if (Choice5 == 2) outside = false;
                        TopDifference(outside);
                        break;
                }
            }
        }
        public static int MainMeny()
        {
            Console.Clear();
            Console.WriteLine("1. Average Temperature on specific date");
            Console.WriteLine("2. Top temperatures");
            Console.WriteLine("3. Top moisture level");
            Console.WriteLine("4. Top mold index");
            Console.WriteLine("5. Meteorological autumn");
            Console.WriteLine("6. Meteorological winter");
            Console.WriteLine("7. Top time door open");
            Console.WriteLine("8. Top temperature difference");

            return GetInt32Input(1, 8);
        }
        public static void ReadFile()
        {
            using (var reader = new StreamReader(@"C:\Users\denni\source\repos\temperature\TemperaturData.csv"))
            {
                List<TempData> insideList = new List<TempData>();
                List<TempData> outsideList = new List<TempData>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values[1].Equals("Inne"))
                    {
                        string temp = values[2].Replace('.', ',');
                        TempData tempData = new TempData(DateTime.Parse(values[0]), float.Parse(temp), int.Parse(values[3]));
                        insideList.Add(tempData);
                    }
                    else if (values[1].Equals("Ute"))
                    {
                        string temp = values[2].Replace('.', ',');
                        TempData tempData = new TempData(DateTime.Parse(values[0]), float.Parse(temp), int.Parse(values[3]));
                        outsideList.Add(tempData);
                    }
                }

                using (var context = new EFContext())
                {
                    foreach (TempData tempData in outsideList)
                    {
                        Outside toAdd = new Outside();
                        toAdd.Date = tempData.Date;
                        toAdd.Moisture = tempData.Moisture;
                        toAdd.Temperature = tempData.Temperature;
                        context.Add(toAdd);
                    }

                    foreach (TempData tempData in insideList)
                    {
                        Inside toAdd = new Inside();
                        toAdd.Date = tempData.Date;
                        toAdd.Moisture = tempData.Moisture;
                        toAdd.Temperature = tempData.Temperature;
                        context.Add(toAdd);
                    }
                    context.SaveChanges();
                }


            }
        }
        public static void Checkdups()
        {
            using (var reader = new StreamReader(@"C:\Users\denni\source\repos\temperature\TemperaturData.csv"))
            {
                List<TempDataDubs> Lista = new List<TempDataDubs>();
                int x = 0;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',').ToList();

                    string temp = values[2].Replace('.', ',');
                    Lista.Add(new TempDataDubs(DateTime.Parse(values[0]), values[1], float.Parse(temp), int.Parse(values[3])));
                }

                x = Lista.Distinct().Count();
                Console.WriteLine(Lista.Count());
                Console.WriteLine(x);                

            }
        }
        public static void AverageTemp(DateTime date, bool Outside)
        {
            Console.Clear();
            Console.WriteLine("Loading...");
            using (var context = new EFContext())
            {
                if (Outside)
                {
                    try
                    {
                        var query = context.Outside
                            .Where(x => x.Date.Date == date.Date)
                            .Average(x => x.Temperature);
                        Console.Clear();
                        Console.WriteLine("Average Temperature Outside for " + date.Date.ToString("yyyy-MM-dd") + " was " + Math.Round(query, 1));
                    }
                    catch { Console.Clear(); Console.Write("No data found, press any key to continue:"); }
                }
                else
                {
                    try
                    {
                        var query = context.Inside
                            .Where(x => x.Date.Date == date.Date)
                            .Average(x => x.Temperature);
                        Console.Clear();
                        Console.WriteLine("Average Temperature Outside for " + date.Date.ToString("yyyy-MM-dd") + " was " + Math.Round(query, 1));
                    }
                    catch { Console.Clear(); Console.Write("No data found, press any key to continue:"); }
                }

                //Possible Bias where Many Measurements in one time of day, and few in another time.
            }

            Console.ReadKey();
        }
        public static void TopTemp(bool Outside)
        {
            Console.Clear();
            Console.WriteLine("Loading...");
            using (var context = new EFContext())
            {
                if (Outside)
                {
                    var query = context.Outside
                        .GroupBy(l => l.Date.Date)
                        .Select(cl => new { Date = cl.Key, AverageTemp = cl.Average(c => c.Temperature) })
                        .ToList()
                        .OrderByDescending(x => x.AverageTemp);

                    Console.Clear();
                    int top = 1;
                    foreach (var day in query)
                    {
                        Console.WriteLine($"{top}. Date: {day.Date.ToString("yyyy-MM-dd")} {('\n')}Average Temperature: {Math.Round(day.AverageTemp,1)}");
                        Console.WriteLine();
                        top++;
                    }
                }
                else
                {
                    var query = context.Inside
                        .GroupBy(l => l.Date.Date)
                        .Select(cl => new { Date = cl.Key, AverageTemp = cl.Average(c => c.Temperature) })
                        .ToList()
                        .OrderByDescending(x => x.AverageTemp);

                    Console.Clear();
                    int top = 1;
                    foreach (var day in query)
                    {
                        Console.WriteLine($"{top}. Date: {day.Date.ToString("yyyy-MM-dd")} {('\n')}Average Temperature: {Math.Round(day.AverageTemp, 1)}");
                        Console.WriteLine();
                        top++;
                    }
                }
                Console.ReadKey();
            }
        }
        public static void TopMoist(bool Outside)
        {
            using (var context = new EFContext())
            {
                Console.Clear();
                Console.WriteLine("Loading...");
                if (Outside)
                {
                    var query = context.Outside
                        .GroupBy(l => l.Date.Date)
                        .Select(cl => new { Date = cl.Key, AverageMoist = cl.Average(c => c.Moisture) })
                        .ToList()
                        .OrderByDescending(x => x.AverageMoist);

                    Console.Clear();
                    int top = 1;
                    foreach (var day in query)
                    {
                        Console.WriteLine($"{top}. Date: {day.Date.ToString("yyyy-MM-dd")} {('\n')}Average Moisture: {Math.Round(day.AverageMoist, 1)}");
                        Console.WriteLine();
                        top++;
                    }
                }
                else
                {
                    var query = context.Inside
                        .GroupBy(l => l.Date.Date)
                        .Select(cl => new { Date = cl.Key, AverageMoist = cl.Average(c => c.Moisture) })
                        .ToList()
                        .OrderByDescending(x => x.AverageMoist);
                    Console.Clear();
                    int top = 1;
                    foreach (var day in query)
                    {
                        Console.WriteLine($"{top}. Date: {day.Date.ToString("yyyy-MM-dd")} {('\n')}Average Moisture: {Math.Round(day.AverageMoist, 1)}");
                        Console.WriteLine();
                        top++;
                    }
                }

                Console.ReadKey();
            }
        }
        public static void TopChanceMold(bool Outside)
        {
            //Shows top 10 dates with highest chance of mold.

            List<MouldData> MouldRiskList = new List<MouldData>();
            using (var context = new EFContext())
            {
                Console.Clear();
                Console.WriteLine("Loading...");
                if (Outside)
                {
                    var query = context.Outside
                        .GroupBy(l => l.Date.Date)
                        .Select(cl => new { Date = cl.Key, AverageTemp = cl.Average(c => c.Temperature), AverageMold = cl.Average(c => c.Moisture), })
                        .ToList();

                    foreach (var day in query)
                    {
                        double RHCrit = 80;
                        double MouldIndex;
                        double Temperature = day.AverageTemp;
                        double RH = day.AverageMold;

                        if (Temperature <= 20)
                        {
                            RHCrit = -0.00267 * Math.Pow(Temperature, 3) + 0.160 * Math.Pow(Temperature, 2) - 3.13 * Temperature + 100;
                        }
                        if (Temperature <= 0 || Temperature >= 50)
                        {
                            MouldIndex = 0;
                        }
                        else
                        {
                            MouldIndex = 1 + 7 * ((RHCrit - RH) / (RHCrit - 100)) - 2 * Math.Pow(((RHCrit - RH) / (RHCrit - 100)), 2);
                            if (MouldIndex < 0)
                            {
                                MouldIndex = 0;
                            }
                        }
                        MouldData mouldData = new MouldData(day.Date, day.AverageTemp, day.AverageMold, MouldIndex);
                        MouldRiskList.Add(mouldData);
                    }
                }
                else
                {
                    var query = context.Inside
                        .GroupBy(l => l.Date.Date)
                        .Select(cl => new { Date = cl.Key, AverageTemp = cl.Average(c => c.Temperature), AverageMold = cl.Average(c => c.Moisture), })
                        .ToList();

                    foreach (var day in query)
                    {
                        double RHCrit = 80;
                        double MouldIndex;
                        double Temperature = day.AverageTemp;
                        double RH = day.AverageMold;

                        if (Temperature <= 20)
                        {
                            RHCrit = -0.00267 * Math.Pow(Temperature, 3) + 0.160 * Math.Pow(Temperature, 2) - 3.13 * Temperature + 100;
                        }
                        if (Temperature <= 0 || Temperature >= 50)
                        {
                            MouldIndex = 0;
                        }
                        else
                        {
                            MouldIndex = 1 + 7 * ((RHCrit - RH) / (RHCrit - 100)) - 2 * Math.Pow(((RHCrit - RH) / (RHCrit - 100)), 2);
                            if (MouldIndex < 0)
                            {
                                MouldIndex = 0;
                            }
                        }
                        MouldData mouldData = new MouldData(day.Date, day.AverageTemp, day.AverageMold, MouldIndex);
                        MouldRiskList.Add(mouldData);
                    }
                }
                var q2 = MouldRiskList.OrderByDescending(m => m.MouldIndex).Take(10);
                int number = 1;
                Console.Clear();
                foreach (var day in q2)
                {
                    Console.WriteLine($"{number}. Date: {day.Date.ToString("yyyy-MM-dd")}");
                    Console.Write($"MouldIndex: {Math.Round(day.MouldIndex,0)}, ");
                    if (Math.Round(day.MouldIndex, 0) != 0) Console.WriteLine($"{Math.Round((((day.AverageMoisture - 78) * (day.AverageTemperature / 15)) / 0.22), 1)}%");
                    else Console.WriteLine("0%");
                    Console.WriteLine(MouldIndexReturn(Math.Round(day.MouldIndex,0)));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Average Temperature: {Math.Round(day.AverageTemperature,2)}");
                    Console.WriteLine($"Average Moisture: {Math.Round(day.AverageMoisture,2)}");
                    Console.WriteLine();
                    number++;

                }
            }
            Console.ReadKey();
        }
        public static void MeteorologicalAutumn()
        {
            // Meteorological Autumn occurs when the average temperature of a given day is 10C or lower.

            Console.Clear();
            Console.WriteLine("Loading...");

            using (var context = new EFContext())
            {
                var query = context.Outside
                    .GroupBy(l => l.Date.Date)
                    .Select(cl => new { Date = cl.Key, AverageTemp = cl.Average(c => c.Temperature) })
                    .ToList()
                    .OrderBy(x => x.Date);


                int counter = 0;
                DateTime date = DateTime.Now;
                Console.Clear();
                foreach (var day in query)
                {
                    if(day.AverageTemp <= 10)
                    {
                        if(counter == 0)
                        {
                            date = day.Date;
                        }
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }

                    if(counter == 5)
                    {
                        Console.WriteLine($"Meteorological Autumn start: {date.ToString("yyyy-MM-dd")}");
                        break;
                    }

                }
                Console.ReadKey();
            }

        }
        public static void MeteorologicalWinter()
        {
            // Meteorological Autumn occurs when the average temperature of a given day is 0C or lower.

            Console.Clear();
            Console.WriteLine("Loading...");

            using (var context = new EFContext())
            {
                var query = context.Outside
                    .GroupBy(l => l.Date.Date)
                    .Select(cl => new { Date = cl.Key, AverageTemp = cl.Average(c => c.Temperature) })
                    .ToList()
                    .OrderBy(x => x.Date);


                int counter = 0;
                DateTime date = DateTime.Now;

                Console.Clear();
                foreach (var day in query)
                {
                    int AverageTemp = (int)day.AverageTemp; //Cast to int to get a result, otherwise 2016 never below averagetemp 0 five days in a row. == No Meteorological Winter.
                    if (AverageTemp <= 0)
                    {
                        if (counter == 0)
                        {
                            date = day.Date;
                        }
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }

                    if (counter == 5)
                    {
                        Console.WriteLine($"Meteorological winter start: {date.ToString("yyyy-MM-dd")}");
                        break;
                    }

                }
                Console.ReadKey();
            }
        }
        static void TopDoorOpen()
        {
            using (var context = new EFContext())
            {

                //Create InsideList
                Console.Clear();
                Console.WriteLine("Loading...");
                var insideList = context.Inside
                .AsEnumerable().GroupBy(x =>
                {
                    var stamp = x.Date;
                    stamp = stamp.AddMinutes(-(stamp.Minute % 5));
                    stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                    return stamp;
                })
                .Select(g => new { Date = g.Key, AvergeTemperature = g.Average(s => s.Temperature) })
                .OrderBy(x => x.Date)
                .ToList();

                //Create OutsideList
                var outsideList = context.Outside
                .AsEnumerable().GroupBy(x =>
                {
                    var stamp = x.Date;
                    stamp = stamp.AddMinutes(-(stamp.Minute % 5));
                    stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                    return stamp;
                })
                .Select(g => new { Date = g.Key, AvergeTemperature = g.Average(s => s.Temperature) })
                .OrderBy(x => x.Date)
                .ToList();


                List<(DateTime, int)> doorOpenTime = new List<(DateTime, int)>();

                (DateTime, double) lastInsideData = (DateTime.Now, 0);
                (DateTime, double) lastOutsideData = (DateTime.Now, 0);

                foreach (var insideData in insideList)
                {
                    var outsideData = outsideList.Find(x => x.Date == insideData.Date);
                    if (outsideData != null)
                    {
                        if (insideData.Date == lastInsideData.Item1.AddMinutes(5))
                        {
                            if (insideData.AvergeTemperature < lastInsideData.Item2 && outsideData.AvergeTemperature > lastOutsideData.Item2)
                            {
                                (DateTime, int) toAdd = (insideData.Date, 5);
                                doorOpenTime.Add(toAdd);
                            }
                        }
                        (DateTime, double) toAddInside = (insideData.Date, insideData.AvergeTemperature);
                        (DateTime, double) toAddOutside = (outsideData.Date, outsideData.AvergeTemperature);
                        lastInsideData = toAddInside;
                        lastOutsideData = toAddOutside;
                    }
                }


                var query = doorOpenTime
                    .GroupBy(l => l.Item1.Date)

                    .Select(cl => new
                    {
                        Date = cl.Key,
                        TimeOpen = cl.Sum(c => c.Item2)
                    }).ToList().OrderByDescending(x => x.TimeOpen);

                int top = 1;
                Console.Clear();
                foreach (var thing in query)
                {
                    Console.WriteLine($"{top}. Date: {thing.Date.ToString("yyyy-MM-dd")}");
                    Console.WriteLine($"Average Time door open: {thing.TimeOpen} minutes.");
                    Console.WriteLine();
                    top++;
                }
            }
            Console.ReadKey();

        }
        static void TopDifference(bool highest)
        {
            Console.Clear();
            Console.WriteLine("Loading...");
            using (var context = new EFContext())
            {
                //Create InsideList
                var insideList = context.Inside
                    .AsEnumerable().GroupBy(x =>
                    {
                        var stamp = x.Date;
                        stamp = stamp.AddMinutes(-(stamp.Minute % 5));
                        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                        return stamp;
                    })
                    .Select(g => new { Date = g.Key, AvergeTemperature = g.Average(s => s.Temperature) })
                    .OrderBy(x => x.Date)
                    .ToList();

                //Create OutsideList
                var outsideList = context.Outside
                    .AsEnumerable().GroupBy(x =>
                    {
                        var stamp = x.Date;
                        stamp = stamp.AddMinutes(-(stamp.Minute % 5));
                        stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                        return stamp;
                    })
                    .Select(g => new { Date = g.Key, AvergeTemperature = g.Average(s => s.Temperature) })
                    .OrderBy(x => x.Date)
                    .ToList();

                List<DoorData> differenceList = new List<DoorData>();

                foreach (var insideData in insideList)
                {
                    var outsideData = outsideList.Find(x => x.Date == insideData.Date);
                    if (outsideData != null)
                    {
                        double difference = Math.Abs(insideData.AvergeTemperature - outsideData.AvergeTemperature);
                        DoorData toAdd = new DoorData(insideData.Date, difference, insideData.AvergeTemperature, outsideData.AvergeTemperature);
                        differenceList.Add(toAdd);
                    }
                }

                var list = differenceList.OrderBy(x => x.Temperature).Take(10).ToList();
                if (highest)
                {
                    list = differenceList.OrderByDescending(x => x.Temperature).Take(10).ToList();
                }

                Console.Clear();
                int top = 1;
                foreach (var data in list)
                {
                    Console.WriteLine($"{top}. Date: {data.Date}");
                    Console.WriteLine("Average Temperature difference: " + Math.Round(data.Temperature, 1));
                    Console.WriteLine($"Outside Temperature: {Math.Round(data.OutsideTemperature,1)}");
                    Console.WriteLine($"Inside Temperature: {Math.Round(data.InsideTemperature,1)}");
                    Console.WriteLine();
                    top++;
                }
            }
            Console.ReadKey();
        }
        public static int GetInt32Input(int minNumber, int maxNumber)
        {
            bool success = false;
            int input = 0;

            while (!success)
            {
                success = int.TryParse(Console.ReadLine(), out input);
                if (!success) { Console.WriteLine($"Error: UserInput must be a number between {minNumber}-{maxNumber}."); }
                else { success = true; if (input < minNumber || input > maxNumber) { Console.WriteLine($"Error: UserInput must be a number between {minNumber}-{maxNumber}."); success = false; } }
            }

            return input;
        }
        public static string MouldIndexReturn(double MouldIndex)
        {
            string indexInfo = "Error with IndexInfo - should be between 0-6.";

            switch (MouldIndex)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Green;
                    indexInfo = "Ingen tillväxt.";
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    indexInfo = "Liten (Microskopisk) början till local tillväxt.";
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    indexInfo = "Flertal locala mögelkolonier på yta.";
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    indexInfo = "Visuel mögeltillväxt på yta. Mellan 10% - 50% täckningsgrad.";
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Red;
                    indexInfo = "Visuel mögeltillväxt på yta. Mellan 10% - 50% täckninggrad.";
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Red;
                    indexInfo = "Stor tillväxt av mögel på ytan, >50% visuel täckningsgrad.";
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    indexInfo = "Stor och tät tillväxt, 100% ytlig täckningsgrad.";
                    break;
            }
            
            return indexInfo;
        }
    }
}

