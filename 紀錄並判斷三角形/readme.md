# MainWindow.xaml
```xaml
<Window x:Class="TriangleChecker.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Triangle Checker" Height="300" Width="400">
    <Grid>
        <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
            <TextBlock Text="輸入三個邊長：" Margin="0,0,0,10"/>
            <StackPanel Orientation="Horizontal">
                <TextBox x:Name="side1TextBox" Width="80" Margin="0,0,10,0"/>
                <TextBox x:Name="side2TextBox" Width="80" Margin="0,0,10,0" TextChanged="side2TextBox_TextChanged"/>
                <TextBox x:Name="side3TextBox" Width="80" TextChanged="side3TextBox_TextChanged"/>
            </StackPanel>
            <Button Content="檢查三角形" Click="CheckTriangleButton_Click" Width="100" Margin="0,10,0,0"/>
            <Label x:Name="resultLabel" Content="" Margin="0,10,0,0" HorizontalContentAlignment="Center" />
            <TextBlock x:Name="testResultsTextBlock" Text="" Margin="0,20,0,0"/>
        </StackPanel>
    </Grid>
</Window>
```
# MainWindow.xaml.cs
```csharp
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace TriangleChecker
{
    public partial class MainWindow : Window
    {
        private List<Triangle> triangles = new List<Triangle>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckTriangleButton_Click(object sender, RoutedEventArgs e)
        {
            double side1, side2, side3;

            if (!double.TryParse(side1TextBox.Text, out side1) ||
                !double.TryParse(side2TextBox.Text, out side2) ||
                !double.TryParse(side3TextBox.Text, out side3) ||
                side1 <= 0 || side2 <= 0 || side3 <= 0)
            {
                MessageBox.Show("請輸入有效的正數邊長。");
                return;
            }

            Triangle triangle = new Triangle(side1, side2, side3);
            triangles.Add(triangle);

            if (triangle.IsTriangle())
            {
                resultLabel.Content = $"邊長 {side1}, {side2}, {side3} 可構成三角形";
                resultLabel.Background = Brushes.Green;
            }
            else
            {
                resultLabel.Content = $"邊長 {side1}, {side2}, {side3} 不可構成三角形";
                resultLabel.Background = Brushes.Red;
            }

            UpdateTestResults();
        }

        private void UpdateTestResults()
        {
            testResultsTextBlock.Text = "測試結果：\n";
            foreach (Triangle triangle in triangles)
            {
                testResultsTextBlock.Text += $"{triangle.ToString()}\n";
            }
        }

        private void side2TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void side3TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }

    public class Triangle
    {
        private double side1, side2, side3;

        public Triangle(double side1, double side2, double side3)
        {
            this.side1 = side1;
            this.side2 = side2;
            this.side3 = side3;
        }

        public bool IsTriangle()
        {
            return (side1 + side2 > side3) &&
                   (side1 + side3 > side2) &&
                   (side2 + side3 > side1);
        }

        public override string ToString()
        {
            return IsTriangle()
                ? $"邊長 {side1}, {side2}, {side3} 可構成三角形"
                : $"邊長 {side1}, {side2}, {side3} 不可構成三角形";
        }
    }
}
```
# 詳細說明
```csharp
 private void CheckTriangleButton_Click(object sender, RoutedEventArgs e)
        {
            double side1, side2, side3;

            if (!double.TryParse(side1TextBox.Text, out side1) ||
                !double.TryParse(side2TextBox.Text, out side2) ||
                !double.TryParse(side3TextBox.Text, out side3) ||
                side1 <= 0 || side2 <= 0 || side3 <= 0)
            {
                MessageBox.Show("請輸入有效的正數邊長。");
                return;
            }
```
CheckTriangleButton_Click 是當使用者按下"檢查三角形"按鈕時觸發的事件處理程序。

它試圖從三個文本框 (side1TextBox, side2TextBox, side3TextBox) 中讀取使用者輸入的三邊長。

如果輸入無效（例如，不能成功轉換為 double 或小於等於0），則會顯示錯誤訊息並結束處理。

```csharp
            Triangle triangle = new Triangle(side1, side2, side3);
            triangles.Add(triangle);

            if (triangle.IsTriangle())
            {
                resultLabel.Content = $"邊長 {side1}, {side2}, {side3} 可構成三角形";
                resultLabel.Background = Brushes.Green;
            }
            else
            {
                resultLabel.Content = $"邊長 {side1}, {side2}, {side3} 不可構成三角形";
                resultLabel.Background = Brushes.Red;
            }

            UpdateTestResults();
        }
```
在這個部分，程式建立一個 Triangle 物件，傳入三邊長的值，然後將此物件加入到 triangles 列表中。

接著，它呼叫 IsTriangle 方法檢查是否可以構成一個有效的三角形，並根據結果更新 resultLabel 控制項的內容和背景色。

最後，它呼叫 UpdateTestResults 方法，更新測試結果的文字區塊。

```csharp
        private void UpdateTestResults()
        {
            testResultsTextBlock.Text = "測試結果：\n";
            foreach (Triangle triangle in triangles)
            {
                testResultsTextBlock.Text += $"{triangle.ToString()}\n";
            }
        }

```
UpdateTestResults 方法更新測試結果的文字區塊 (testResultsTextBlock) 的內容。

它遍歷 triangles 列表中的每個三角形物件，呼叫其 ToString 方法並將結果附加到文字區塊中。

```csharp
 public class Triangle
    {
        private double side1, side2, side3;

        public Triangle(double side1, double side2, double side3)
        {
            this.side1 = side1;
            this.side2 = side2;
            this.side3 = side3;
        }
  public bool IsTriangle()
        {
            return (side1 + side2 > side3) &&
                   (side1 + side3 > side2) &&
                   (side2 + side3 > side1);
        }        
```
Triangle 類別中的 IsTriangle 方法用於檢查三邊長是否可以構成一個有效的三角形。它檢查三角不等式是否成立，即任意兩邊之和必須大於第三邊。

```csharp
        public override string ToString()
        {
            return IsTriangle()
                ? $"邊長 {side1}, {side2}, {side3} 可構成三角形"
                : $"邊長 {side1}, {side2}, {side3} 不可構成三角形";
        }
    }
}
```
ToString 方法用於返回三角形的描述，如果三邊長可以構成三角形，它將返回描述 "邊長 ... 可構成三角形"，否則返回描述 "邊長 ... 不可構成三角形"。
