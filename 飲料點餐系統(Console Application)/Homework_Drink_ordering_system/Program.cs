using Homework_Drink_ordering_system;
using System.Collections.Generic;

namespace Homework_Drink_ordering_system
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Drink> drinks = new List<Drink>();
            List<OrderItem> orders = new List<OrderItem>();

            AddNewDrink(drinks);
            DisplayDrinkMenu(drinks);
            PlaceOrder(orders, drinks);
            CountPrice(orders, drinks);
        }

        private static void CountPrice(List<OrderItem> myOrders, List<Drink> myDrinks)
        {
            var total = 0.0;
            string message = "";
            double sellPrice = 0.0;

            foreach (var orderitem in myOrders) total += orderitem.Subtotal;
            if (total >= 500)
            {
                message = "滿500元以上8折";
                sellPrice = total * 0.8;
            }
            else if (total >= 300)
            {
                message = "滿300元以上85折";
                sellPrice = total * 0.85;
            }
            else if (total >= 200)
            {
                message = "滿200元以上9折";
                sellPrice = total * 0.9;
            }
            else
            {
                message = "未滿200元不打折";
                sellPrice = total;
            }

            Console.WriteLine();
            Console.WriteLine($"您總共訂購{myOrders.Count}項飲料，總計{total}元。{message}，合計需付款{sellPrice}元。");
            Console.WriteLine("訂購完成，按任意鍵結束...");
            Console.ReadLine();
        }
        private static void PlaceOrder(List<OrderItem> myOrders, List<Drink> myDrinks)
        {
            Console.WriteLine("請開始訂購飲料，輸入N/n結束...");
            string s;
            int index, quantity, subtotal;
            while (true)
            {
                Console.Write("請輸入您所要的品項編號? ");
                s = Console.ReadLine();
                if (s =="N") break;
                else if(s == "n") break;
                if (!int.TryParse(s, out index) || index < 0 || index >= myDrinks.Count)
                {
                    Console.WriteLine("無效的品項編號，請重新輸入。");
                    continue;
                }

                else index = Convert.ToInt32(s);
                Console.Write("請輸入您所要的杯數? ");
                s = Console.ReadLine();
                if (s == "x") break;
                if (!int.TryParse(s, out quantity) || quantity <= 0)
                {
                    Console.WriteLine("無效的杯數，請重新輸入。");
                    continue;
                }
                Drink drink = myDrinks[index];
                subtotal = drink.Price * quantity;
                Console.WriteLine($"您訂購{drink.Name}{drink.Size}{quantity}杯，每杯{drink.Price}元，小計{subtotal}元。");
                myOrders.Add(new OrderItem() { Index = index, Quantity = quantity, Subtotal = subtotal });
            }
        }

        private static void DisplayDrinkMenu(List<Drink> myDrinks)
        {
            Console.WriteLine("飲料清單");
            Console.WriteLine("----------------------------------");
            int i = 0;
            Console.WriteLine(String.Format("{0,-4}{1,-5}{2,-5}{3,-5}", "編號", "品名", "大小", "價格"));
            foreach (Drink drink in myDrinks)
            {
                Console.WriteLine($"{i,-6}{drink.Name,-5}{drink.Size,-5}{drink.Price,5:C0}");
                i++;
            }
            Console.WriteLine("----------------------------------");
        }

        private static void AddNewDrink(List<Drink> myDrinks)
        {
            myDrinks.Add(new Drink() { Name = "咖啡", Size = "大杯", Price = 60 });
            myDrinks.Add(new Drink() { Name = "咖啡", Size = "中杯", Price = 50 });
            myDrinks.Add(new Drink() { Name = "紅茶", Size = "大杯", Price = 30 });
            myDrinks.Add(new Drink() { Name = "紅茶", Size = "中杯", Price = 20 });
            myDrinks.Add(new Drink() { Name = "綠茶", Size = "大杯", Price = 25 });
            myDrinks.Add(new Drink() { Name = "綠茶", Size = "中杯", Price = 20 });
        }
    }
}
