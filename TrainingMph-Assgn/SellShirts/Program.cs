using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcurrentCollections.SellShirts
{
	class Program
	{
		static void Main(string[] args)
		{
			StockController controller = new StockController(TShirtProvider.AllShirts);
			TimeSpan workDay = new TimeSpan(0, 0, 0, 0, 500);

            //new SalesPerson("Kim").Work(workDay, controller);
            //-------------------------------------------------------------------------------------------------------------------
            var names = new[] { "Kim", "Jim", "Tim", "Sam", "Pam", "Liam", "Zoe", "Ana", "Raj", "Dev" };
            var tasks = new List<Task>();


            foreach (var name in names)
            {
                var sp = new SalesPerson(name);
                tasks.Add(Task.Run(() => sp.Work(workDay, controller)));
            }

            //-------------------------------------------------------------------------------------------------------------------
            Task.WaitAll(tasks.ToArray());

            controller.DisplayStock();
		}
	}
}
