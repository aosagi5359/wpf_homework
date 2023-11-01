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

            AddNewDrink(drinks);

            DisplayDrinkMenu(drinks);
        }

        private void DisplayDrinkMenu(Dictionary<string, int> myDrinks)
        {
            foreach (var drink in myDrinks)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;

                CheckBox cb = new CheckBox();
                cb.Content = $"{drink.Key} : {drink.Value}元";
                cb.Width = 200;
                cb.FontFamily = new FontFamily("Consolas");
                cb.FontSize = 18;
                cb.Foreground = Brushes.Blue;
                cb.Margin = new Thickness(5);

                Slider sl = new Slider();
                sl.Width = 100;
                sl.Value = 0;
                sl.Minimum = 0;
                sl.Maximum = 10;
                sl.VerticalAlignment = VerticalAlignment.Center;
                sl.IsSnapToTickEnabled = true;

                Label lb = new Label();
                lb.Width = 50;
                lb.Content = "0";
                lb.FontFamily = new FontFamily("Consolas");
                lb.FontSize = 18;
                lb.Foreground = Brushes.Red;

                sp.Children.Add(cb);
                sp.Children.Add(sl);
                sp.Children.Add(lb);

                Binding myBinding = new Binding("Value");
                myBinding.Source = sl;
                lb.SetBinding(ContentProperty, myBinding);

                stackpanel_DrinkMenu.Children.Add(sp);
            }
        }

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
        private void SaveOrderButton_Click(object sender, RoutedEventArgs e)
        {

            SaveOrderToFile();
        }
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
                    MessageBox.Show("訂單已成功儲存到檔案: " + filePath, "儲存成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("儲存訂單時發生錯誤: " + ex.Message, "儲存錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            PlaceOrder(orders);
            DisplayOrderDetail(orders);
        }

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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.IsChecked == true) takeout = rb.Content.ToString();
        }
    }
}
