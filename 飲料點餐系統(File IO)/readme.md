# MainWindow.xaml
```xaml
<Window x:Class="WpfApp2.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:WpfApp2"
        mc:Ignorable="d"
        Title="飲品訂購系統" Height="645" Width="790">
    <Grid>
        <GroupBox Header="內用/外帶" Margin="20,10,0,0" Background="#FFE0E0E0" Height="63" VerticalAlignment="Top" HorizontalAlignment="Left" Width="615">
            <StackPanel Height="37" Orientation="Horizontal">
                <RadioButton Content="內用" IsChecked="True" FontSize="18" VerticalContentAlignment="Center" Checked="RadioButton_Checked" Width="100" Height="40"/>
                <RadioButton Content="外帶" FontSize="18" VerticalContentAlignment="Center" Checked="RadioButton_Checked" Width="100" Height="40"/>
            </StackPanel>
        </GroupBox>

        <StackPanel x:Name="stackpanel_DrinkMenu" Margin="20,83,0,0" Background="#FFF0F0F0"
                    HorizontalAlignment="Left" Width="615" VerticalAlignment="Top" RenderTransformOrigin="0.5,0.5">
        </StackPanel>
        
        <Button x:Name="OrderButton" Content="確認訂購" Margin="670,19,20,0" VerticalAlignment="Top"
                Height="45" IsCancel="True" FontSize="18" Click="OrderButton_Click" Background="#FF007ACC" Foreground="White"/>
        <Button x:Name="SaveOrderButton" Content="儲存訂單" Margin="670,70,20,0" VerticalAlignment="Top"
        Height="45" FontSize="18" Click="SaveOrderButton_Click" Background="#FF00B200" Foreground="White"/>

        <TextBlock x:Name="displayTextBlock" Margin="20,365,20,20" TextWrapping="Wrap" Background="#FFF9EBEB" FontSize="18" Padding="10,10,10,10"/>
    </Grid>
</Window>
```
# MainWindow.xaml.cs
```csharp
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> drinks = new Dictionary<string, int>();
        Dictionary<string, int> orders = new Dictionary<string, int>();
        string takeout = "";

        public MainWindow()
        {
            InitializeComponent();

            // 初始化飲料清單
            AddNewDrink(drinks);

            // 顯示飲料清單
            DisplayDrinkMenu(drinks);
        }

        // 顯示飲料菜單
        private void DisplayDrinkMenu(Dictionary<string, int> myDrinks)
        {
            foreach (var drink in myDrinks)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;

                // 建立複選框 (CheckBox) 來選擇飲料項目
                CheckBox cb = new CheckBox();
                cb.Content = $"{drink.Key} : {drink.Value}元";
                cb.Width = 200;
                cb.FontFamily = new FontFamily("Consolas");
                cb.FontSize = 18;
                cb.Foreground = Brushes.Blue;
                cb.Margin = new Thickness(5);

                // 建立滑塊 (Slider) 來選擇數量
                Slider sl = new Slider();
                sl.Width = 100;
                sl.Value = 0;
                sl.Minimum = 0;
                sl.Maximum = 10;
                sl.VerticalAlignment = VerticalAlignment.Center;
                sl.IsSnapToTickEnabled = true;

                // 顯示選擇的數量
                Label lb = new Label();
                lb.Width = 50;
                lb.Content = "0";
                lb.FontFamily = new FontFamily("Consolas");
                lb.FontSize = 18;
                lb.Foreground = Brushes.Red;

                // 加入 UI 元件到 StackPanel
                sp.Children.Add(cb);
                sp.Children.Add(sl);
                sp.Children.Add(lb);

                // 設定數量顯示的繫結 (Binding)
                Binding myBinding = new Binding("Value");
                myBinding.Source = sl;
                lb.SetBinding(ContentProperty, myBinding);

                // 將 StackPanel 加入到 UI 中的 stackpanel_DrinkMenu
                stackpanel_DrinkMenu.Children.Add(sp);
            }
        }

        // 從檔案中讀取飲料資訊並新增到清單中
        private void AddNewDrink(Dictionary<string, int> myDrinks)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "CSV檔案|*.csv|文字檔案|*.txt|所有檔案|*.*";

            if (fileDialog.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(fileDialog.FileName);
                foreach (string line in lines)
                {
                    string[] tokens = line.Split(',');
                    string drinkName = tokens[0];
                    int price = Convert.ToInt32(tokens[1]);
                    myDrinks.Add(drinkName, price);
                }
            }
        }

        // 計算訂單總價
        private double CalculateTotalPrice()
        {
            double total = 0.0;

            foreach (KeyValuePair<string, int> item in orders)
            {
                string drinkName = item.Key;
                int quantity = item.Value;
                int price = drinks[drinkName];
                total += price * quantity;
            }

            return total;
        }

        // 點擊"儲存訂單"按鈕時的事件處理函式
        private void SaveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            SaveOrderToFile();
        }

        // 儲存訂單到檔案
        private void SaveOrderToFile()
        {
            // 彈出儲存檔案對話方塊
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "文字檔案|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        // 將訂單資訊寫入文字檔案
                        writer.WriteLine("訂單清單:");
                        writer.WriteLine();

                        foreach (KeyValuePair<string, int> item in orders)
                        {
                            string drinkName = item.Key;
                            int quantity = item.Value;
                            int price = drinks[drinkName];
                            writer.WriteLine($"品名: {drinkName}, 數量: {quantity}, 小計: {price * quantity}元");
                        }

                        // 添加總價格資訊
                        writer.WriteLine();
                        writer.WriteLine($"總計: {CalculateTotalPrice()}元");
                    }

                    // 顯示儲存成功的訊息框
                    MessageBox.Show("訂單已成功儲存到檔案: " + filePath, "儲存成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // 顯示儲存失敗的訊息框
                    MessageBox.Show("儲存訂單時發生錯誤: " + ex.Message, "儲存錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 點擊"訂購"按鈕時的事件處理函式
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            PlaceOrder(orders);
            DisplayOrderDetail(orders);
        }

        // 顯示訂單詳細資訊
        private void DisplayOrderDetail(Dictionary<string, int> myOrders)
        {
            displayTextBlock.Inlines.Clear();
            Run titleString = new Run();
            titleString.Text = $"您所訂購的飲品：";
            titleString.Foreground = Brushes.Blue;

            Run takeoutString = new Run();
            takeoutString.Text = $"{takeout}";
            takeoutString.FontWeight = FontWeights.Bold;

            displayTextBlock.Inlines.Add(titleString);
            displayTextBlock.Inlines.Add(takeoutString);
            displayTextBlock.Inlines.Add(new Run("，訂購明細如下：\n"));
            string discountString = "";

            double total = 0.0;
            double sellPrice = 0.0;
            int i = 1;
            foreach (KeyValuePair<string, int> item in myOrders)
            {
                string drinkName = item.Key;
                int quantity = myOrders[drinkName];
                int price = drinks[drinkName];
                total += price * quantity;
                Run runDetail = new Run($"訂購品項{i}：{drinkName} X {quantity}杯，每杯{price}元，小計{price * quantity}元。\n");
                displayTextBlock.Inlines.Add(runDetail);
                i++;
            }

            if (total >= 500)
            {
                discountString = "訂購滿500元以上者打8折";
                sellPrice = total * 0.8;
            }
            else if (total >= 300)
            {
                discountString = "訂購滿300元以上者打85折";
                sellPrice = total * 0.85;
            }
            else if (total >= 200)
            {
                discountString = "訂購滿200元以上者打9折";
                sellPrice = total * 0.9;
            }
            else
            {
                discountString = "訂購未滿200元以上者不打折";
                sellPrice = total;
            }

            TextBlock summaryTextBlock = new TextBlock();
            summaryTextBlock.Inlines.Add(new Run($"本次訂購總共{myOrders.Count}項，{discountString}，售價{sellPrice}元"));
            summaryTextBlock.Foreground = Brushes.Red;
            summaryTextBlock.FontWeight = FontWeights.Bold;
            displayTextBlock.Inlines.Add(summaryTextBlock);
        }

        // 建立訂單
        private void PlaceOrder(Dictionary<string, int> myOrders)
        {
            myOrders.Clear();
            for (int i = 0; i < stackpanel_DrinkMenu.Children.Count; i++)
            {
                var sp = stackpanel_DrinkMenu.Children[i] as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var sl = sp.Children[1] as Slider;
                string drinkName = cb.Content.ToString().Substring(0, cb.Content.ToString().IndexOf(':')).Trim();
                int quantity = Convert.ToInt32(sl.Value);

                if (cb.IsChecked == true && quantity != 0)
                {
                    myOrders.Add(drinkName, quantity);
                }
            }
        }

        // 選擇外帶方式的事件處理函式
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.IsChecked == true) takeout = rb.Content.ToString();
        }
    }
}
```
# 詳細說明
### 初始化飲料清單和顯示飲料菜單

```csharp
Dictionary<string, int> drinks = new Dictionary<string, int>();
Dictionary<string, int> orders = new Dictionary<string, int>();

public MainWindow()
{
    InitializeComponent();

    // 初始化飲料清單
    AddNewDrink(drinks);

    // 顯示飲料清單
    DisplayDrinkMenu(drinks);
}
```

在這個部分，你創建了兩個字典：`drinks` 用於存儲飲料名稱和價格，`orders` 用於存儲用戶的訂單。在 `MainWindow` 的建構函式中，你調用了 `AddNewDrink` 函式，它用於初始化 `drinks` 字典，然後呼叫 `DisplayDrinkMenu` 函式，以在應用程式的 UI 中顯示飲料菜單。

### 顯示飲料菜單

```csharp
private void DisplayDrinkMenu(Dictionary<string, int> myDrinks)
{
    foreach (var drink in myDrinks)
    {
        StackPanel sp = new StackPanel();
        sp.Orientation = Orientation.Horizontal;

        CheckBox cb = new CheckBox();
        cb.Content = $"{drink.Key} : {drink.Value}元";
        // ...

        // 建立 UI 元件，包括複選框 (CheckBox)、滑塊 (Slider) 和標籤 (Label)
        // 將它們添加到 StackPanel，然後將 StackPanel 添加到 stackpanel_DrinkMenu。
        // 這樣用戶就可以選擇飲料並指定數量。
    }
}
```

在這個部分，你創建了一個 `StackPanel` 用於包含飲料選項的 UI 元件。然後，你建立了一個 `CheckBox` 用於選擇飲料，以及一個 `Slider` 和 `Label` 來指定數量和顯示數量。最後，你將這些 UI 元件添加到 `StackPanel`，再將 `StackPanel` 添加到 `stackpanel_DrinkMenu`，這樣用戶就可以在應用程式中選擇飲料和數量。

### 儲存訂單到檔案

```csharp
private void SaveOrderToFile()
{
    // 彈出儲存檔案對話方塊
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.Filter = "文字檔案|*.txt";

    if (saveFileDialog.ShowDialog() == true)
    {
        // 創建要儲存訂單的文字檔案，寫入訂單詳細資訊
        // 包括品名、數量和小計，以及總價格。
    }
}
```

這個部分包括儲存訂單到檔案的邏輯。首先，你使用 `SaveFileDialog` 來彈出儲存檔案的對話方塊，讓用戶選擇儲存的檔案名稱和位置。然後，你將訂單詳細資訊寫入選擇的檔案，包括品名、數量和小計，以及總價格。如果儲存過程中出現錯誤，你會彈出錯誤訊息。

### 顯示訂單詳細資訊

```csharp
private void DisplayOrderDetail(Dictionary<string, int> myOrders)
{
    // 清空現有的顯示內容
    displayTextBlock.Inlines.Clear();

    // 創建標題和顯示訂單內容，包括飲料品名、數量、小計和總價格。
    // 也包括折扣信息和售價。
}
```

這個部分負責顯示訂單的詳細資訊，包括已訂購的飲料、數量、小計、總價格和折扣信息。它首先清空 `displayTextBlock` 中的內容，然後建立一個標題和訂單內容，最後將它們添加到 `displayTextBlock` 以在應用程式中顯示。

### 建立訂單

```csharp
private void PlaceOrder(Dictionary<string, int> myOrders)
{
    myOrders.Clear();
    for (int i = 0; i < stackpanel_DrinkMenu.Children.Count; i++)
    {
        var sp = stackpanel_DrinkMenu.Children[i] as StackPanel;
        var cb = sp.Children[0] as CheckBox;
        var sl = sp.Children[1] as Slider;
        string drinkName = cb.Content.ToString().Substring(0, cb.Content.ToString().IndexOf(':')).Trim();
        int quantity = Convert.ToInt32(sl.Value);

        if (cb.IsChecked == true && quantity != 0)
        {
            myOrders.Add(drinkName, quantity);
        }
    }
}
```

這個部分負責建立訂單，它清空現有的訂

單 (在 `myOrders` 字典中)，然後遍歷飲料清單中的每個選項。對於每個選項，它檢查是否已選中 (`cb.IsChecked == true`) 並且數量不為零 (`quantity != 0`)，如果是，則將該飲料和數量添加到訂單中。

### 外帶方式的事件處理函式

```csharp
private void RadioButton_Checked(object sender, RoutedEventArgs e)
{
    var rb = sender as RadioButton;
    if (rb.IsChecked == true) takeout = rb.Content.ToString();
}
```

這個事件處理函式處理外帶方式的選擇。它檢查哪個選項被選中，然後將其值存儲在 `takeout` 變數中，以便在後續處理中使用。
