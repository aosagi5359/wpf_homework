# Drink.cs
```csharp
namespace Homework_Drink_ordering_system
{
    internal class Drink
    {
        public String Name { get; set; }
        public String Size { get; set; }
        public int Price { get; set; }
    }
}
```
# OrderItem.cs
```csharp
namespace Homework_Drink_ordering_system
{
    internal class OrderItem
    {
        public int Index { get; set; }
        public int Quantity { get; set; }
        public int Subtotal { get; set; }
    }
}
```
Program.cs
```csharp
using Homework_Drink_ordering_system;
using System.Collections.Generic;

namespace Homework_Drink_ordering_system
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 創建兩個空的 List，用於存儲飲料和訂單。
            List<Drink> drinks = new List<Drink>();
            List<OrderItem> orders = new List<OrderItem>();

            // 呼叫以下幾個方法來初始化飲料清單、顯示菜單、下訂單、計算價格。
            AddNewDrink(drinks);
            DisplayDrinkMenu(drinks);
            PlaceOrder(orders, drinks);
            CountPrice(orders, drinks);
        }

        // 計算總價格和折扣。
        private static void CountPrice(List<OrderItem> myOrders, List<Drink> myDrinks)
        {
            var total = 0.0;
            string message = "";
            double sellPrice = 0.0;

            // 遍歷訂單中的每個項目，計算總價格。
            foreach (var orderitem in myOrders) total += orderitem.Subtotal;

            // 根據總價格應用折扣。
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

            // 顯示訂單摘要。
            Console.WriteLine();
            Console.WriteLine($"您總共訂購{myOrders.Count}項飲料，總計{total}元。{message}，合計需付款{sellPrice}元。");
            Console.WriteLine("訂購完成，按任意鍵結束...");
            Console.ReadLine();
        }

        // 輸入訂單。
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

        // 顯示飲料菜單。
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

        // 新增飲料到飲料清單。
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

```
