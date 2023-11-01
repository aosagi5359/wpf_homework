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
