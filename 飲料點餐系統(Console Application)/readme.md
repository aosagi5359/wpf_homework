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
# 詳細說明
當然，以下是您的程式碼和相應的說明內容，以Markdown格式：

```markdown
# 1. 初始化飲料清單和訂單，以及呼叫相關方法

```csharp
// 創建兩個空的 List，用於存儲飲料和訂單。
List<Drink> drinks = new List<Drink>();
List<OrderItem> orders = new List<OrderItem>();

// 呼叫以下幾個方法來初始化飲料清單、顯示菜單、下訂單、計算價格。
AddNewDrink(drinks);
DisplayDrinkMenu(drinks);
PlaceOrder(orders, drinks);
CountPrice(orders, drinks);
```

在這個部分，您創建了兩個空的 `List`，`drinks` 用於存儲飲料資訊，`orders` 用於存儲訂單資訊。然後，您呼叫了一系列方法，包括初始化飲料清單 (`AddNewDrink`)、顯示飲料菜單 (`DisplayDrinkMenu`)、處理訂單 (`PlaceOrder`) 以及計算價格和折扣 (`CountPrice`)。

# 2. 計算總價格和折扣

```csharp
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
```

這個部分負責計算訂單的總價格和應用折扣。首先，它初始化 `total` 變數用於累積總價格，並初始化 `message` 和 `sellPrice` 變數以存儲折扣相關資訊。接著，它遍歷 `myOrders` 中的每個項目，計算總價格。最後，根據總價格的不同範圍應用折扣，然後顯示訂單摘要。

# 3. 輸入訂單

```csharp
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
```

這個部分負責讓用戶輸入訂單。它使用 `Console` 來與用戶進行互動。用戶可以輸入品項編號和杯數，然後程序會檢查輸入的有效性。如果輸入無效，它會顯示錯誤訊息，否則，它會計算小計並添加訂單項目。

# 4. 顯示飲料菜單

```csharp
// 顯示飲料菜單。
private static void DisplayDrinkMenu(List<Drink> myDrinks)
{
    Console.WriteLine("飲料清單");
    Console.WriteLine("----------------------------------");
    int i = 0;
    Console.WriteLine(String.Format

("{0,-4}{1,-5}{2,-5}{3,-5}", "編號", "品名", "大小", "價格"));
    foreach (Drink drink in myDrinks)
    {
        Console.WriteLine($"{i,-6}{drink.Name,-5}{drink.Size,-5}{drink.Price,5:C0}");
        i++;
    }
    Console.WriteLine("----------------------------------");
}
```

這個部分負責顯示飲料菜單。它使用 `Console` 來列出每種飲料的詳細資訊，包括品名、大小和價格。這樣用戶可以看到可訂購的選項。

# 5. 新增飲料到飲料清單

```csharp
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
```

這個部分負責初始化飲料清單，並將不同種類的飲料添加到清單中。每種飲料都有特定的品名、大小和價格。
