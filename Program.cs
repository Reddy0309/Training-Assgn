namespace Dates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<DateTime> thirteenthDates = GenerateThirteenthDates(5);

            while (true)
            {
                Console.WriteLine("Press 1 to show all 13th of every months ( for next 5 years)");
                Console.WriteLine("Press 2 to show only Friday && 13th as a date");
                Console.WriteLine("Press 3 to Exit");
                Console.Write("Choose an option (1-3): ");

                string userChoice = Console.ReadLine();

                if (userChoice == "1")
                {
                    Console.WriteLine("All 13th dates for the next 5 years: \n");
                    DisplayDates(thirteenthDates);
                }
                else if (userChoice == "2")
                {
                    List<DateTime> fridayDates = thirteenthDates.FindAll(
                        date => date.DayOfWeek == DayOfWeek.Friday);

                    Console.WriteLine("Friday the 13th dates for the next 5 years:\n");
                    DisplayDates(fridayDates);
                }
                else if (userChoice == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static List<DateTime> GenerateThirteenthDates(int yearsAhead)
        {
            var resultDates = new List<DateTime>();

            DateTime today = DateTime.Today;
            int monthCount = yearsAhead * 12;

            DateTime baseDate = new DateTime(today.Year, today.Month, 13);

            for (int index = 0; index < monthCount; index++)
            {
                DateTime currentDate = baseDate.AddMonths(index);

                if (currentDate < today)
                    continue;

                resultDates.Add(currentDate);
            }

            return resultDates;
        }

        static void DisplayDates(IEnumerable<DateTime> dateList)
        {
            foreach (var date in dateList)
            {
                Console.WriteLine(date.ToString("dd MMMM yyyy (dddd)"));
            }
        }
    }
}
