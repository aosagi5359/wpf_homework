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

# 1. 初始化程式

```csharp
private List<Triangle> triangles = new List<Triangle>();

public MainWindow()
{
    InitializeComponent();
}
```

在這部分，初始化了 `triangles` 列表以存儲三角形的資訊。這個程式看起來是一個 WPF 應用程式，主視窗的初始化和 UI 元素的設定發生在這裡。

# 2. 當"CheckTriangle"按鈕被點擊時

```csharp
private void CheckTriangleButton_Click(object sender, RoutedEventArgs e)
{
    double side1, side2, side3;

    // 檢查並獲取使用者輸入的三條邊長
    if (!double.TryParse(side1TextBox.Text, out side1) ||
        !double.TryParse(side2TextBox.Text, out side2) ||
        !double.TryParse(side3TextBox.Text, out side3) ||
        side1 <= 0 || side2 <= 0 || side3 <= 0)
    {
        MessageBox.Show("請輸入有效的正數邊長。");
        return;
    }

    // 創建 Triangle 物件
    Triangle triangle = new Triangle(side1, side2, side3);
    triangles.Add(triangle);

    // 檢查三條邊是否能構成三角形
    if (triangle.IsTriangle())
    {
        resultLabel.Content = $"邊長 {side1}, {side2}, {side3} 可構成三角形";
        resultLabel.Background = Brushes.Green;
    }
    else
    {
        resultLabel.Content = $"邊長 {side1, side2, side3} 不可構成三角形";
        resultLabel.Background = Brushes.Red;
    }

    // 更新測試結果
    UpdateTestResults();
}
```

這個部分處理當 "CheckTriangle" 按鈕被點擊時的事件。它從使用者輸入獲取三條邊長，然後檢查是否可以構成三角形。如果可以，將結果顯示為綠色，否則顯示為紅色。然後，它呼叫 `UpdateTestResults` 函數，以更新測試結果。

# 3. 更新測試結果

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

這個部分更新測試結果，將所有三角形的測試結果顯示在 `testResultsTextBlock` 文本塊中。

# 4. Triangle 類別

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

    public override string ToString()
    {
        return IsTriangle()
            ? $"邊長 {side1}, {side2}, {side3} 可構成三角形"
            : $"邊長 {side1}, {side2}, {side3} 不可構成三角形";
    }
}
```

這個部分定義了 `Triangle` 類別，它表示一個三角形。這個類別具有檢查三條邊是否能構成三角形的 `IsTriangle` 方法，以及一個 `ToString` 方法，用於返回三角形的描述。如果三條邊能構成三角形，它將返回一個成功的描述，否則將返回一個失敗的描述。

這個程式的功能是檢查使用者輸入的三條邊是否能構成三角形，並將結果以綠色或紅色顯示在 UI 中，同時記錄每次測試的結果。
