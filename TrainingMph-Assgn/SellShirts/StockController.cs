using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Collections.ObjectModel;

namespace ConcurrentCollections.SellShirts
{
	public class StockController
	{
        //-------------------------------------------------------------------------------------------------------------------

        //You need ConcurrentDictionary because your stock is being accessed and
		//modified by multiple threads at the same time. A normal Dictionary cannot handle that safely.
        private ConcurrentDictionary<string, TShirt> _stock;

		public StockController(IEnumerable<TShirt> shirts)
		{
			_stock = new ConcurrentDictionary<string, TShirt>(shirts.ToDictionary(x => x.Code));

			//-------------------------------------------------------------------------------------------------------------------
		}
		public void Sell(string code)
		{
            //TryRemove is an atomic function are the functions which finish completely before another thread can interfere.
            _stock.TryRemove(code, out _);
        }
		public TShirt SelectRandomShirt()
		{
			var keys = _stock.Keys.ToList();
			if (keys.Count == 0)
				return null;    // all shirts sold

			Thread.Sleep(Rnd.NextInt(10));
			string selectedCode = keys[Rnd.NextInt(keys.Count)];
			//return _stock[selectedCode];
//-------------------------------------------------------------------------------------------------------------------
			for (int i = 0; i < 3; i++)
            {
                if (_stock.TryGetValue(selectedCode, out var shirt))
                    return shirt;


                // refresh keys and pick another code
                keys = _stock.Keys.ToList();
                if (keys.Count == 0)
                    return null;
                selectedCode = keys[Rnd.NextInt(keys.Count)];
            }


            _stock.TryGetValue(selectedCode, out var finalShirt);
            return finalShirt;

			//-------------------------------------------------------------------------------------------------------------------
		}
		public void DisplayStock()
		{
			Console.WriteLine($"\r\n{_stock.Count} items left in stock:");
			foreach (TShirt shirt in _stock.Values)
				Console.WriteLine(shirt);
		}
	}
}
