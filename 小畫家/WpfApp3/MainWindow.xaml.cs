using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2023_WpfApp3
{
    public partial class MainWindow : Window
    {
        private string shapeType = "Line";
        private Color strokeColor = Colors.Red;
        private Color fillColor = Colors.Yellow;
        private int strokeThickness = 1;
        private Point start, dest;
        private UIElement currentShape;

        public MainWindow()
        {
            InitializeComponent();
            strokeColorPicker.SelectedColor = strokeColor;
            fillColorPicker.SelectedColor = fillColor;
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString();
        }

        private void strokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = (Color)strokeColorPicker.SelectedColor;
        }

        private void thicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = Convert.ToInt32(thicknessSlider.Value);
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross;
            start = e.GetPosition(myCanvas);
            DisplayStatus();

            if (shapeType == "Line")
            {
                currentShape = new Line
                {
                    X1 = start.X,
                    Y1 = start.Y,
                    X2 = start.X,
                    Y2 = start.Y,
                    Stroke = new SolidColorBrush(strokeColor),
                    StrokeThickness = strokeThickness,
                };
                myCanvas.Children.Add(currentShape);
            }
            else if (shapeType == "Rectangle" || shapeType == "Ellipse")
            {
                currentShape = shapeType == "Rectangle" ? new Rectangle() : new Ellipse();
                var shape = currentShape as Shape;
                shape.Stroke = new SolidColorBrush(strokeColor);
                shape.StrokeThickness = strokeThickness;
                shape.Fill = new SolidColorBrush(fillColor);
                Canvas.SetLeft(shape, start.X);
                Canvas.SetTop(shape, start.Y);
                myCanvas.Children.Add(currentShape);
            }
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas);
            DisplayStatus();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentShape != null)
                {
                    if (currentShape is Line line)
                    {
                        line.X2 = dest.X;
                        line.Y2 = dest.Y;
                    }
                    else if (currentShape is Shape shape)
                    {
                        double width = Math.Abs(dest.X - start.X);
                        double height = Math.Abs(dest.Y - start.Y);
                        Canvas.SetLeft(shape, Math.Min(start.X, dest.X));
                        Canvas.SetTop(shape, Math.Min(start.Y, dest.Y));
                        shape.Width = width;
                        shape.Height = height;
                    }
                }
            }
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentShape = null;
        }

        private void fillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = (Color)fillColorPicker.SelectedColor;
        }

        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            DisplayStatus();
        }

        private void DisplayStatus()
        {
            int lineCount = myCanvas.Children.OfType<Line>().Count();
            int rectCount = myCanvas.Children.OfType<Rectangle>().Count();
            int ellipseCount = myCanvas.Children.OfType<Ellipse>().Count();

            coordinateLabel.Content = $"座標點：({Math.Round(start.X)}, {Math.Round(start.Y)}) - ({Math.Round(dest.X)}, {Math.Round(dest.Y)})";
            shapeLabel.Content = $"Line: {lineCount}, Rectangle: {rectCount}, Ellipse: {ellipseCount}";
        }
    }
}
