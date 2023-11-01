# MainWindow.xaml 
```xaml
<Window x:Class="_2023_WpfApp3.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:xctk="http://schemas.xceed.com/wpf/xaml/toolkit"
        xmlns:local="clr-namespace:_2023_WpfApp3"
        mc:Ignorable="d"
        Title="2023 WPF Painter" Height="575" Width="995">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>
        <Menu Grid.Row="0" Background="#FF3874AB">
            <MenuItem Header="檔案" Height="25" VerticalAlignment="Center" VerticalContentAlignment="Center">
                <MenuItem Header="新畫布" />
                <MenuItem Header="儲存畫布" />
                <MenuItem x:Name="clearMenuItem" Header="清除畫布" Click="clearMenuItem_Click" />
            </MenuItem>
            <MenuItem Header="形狀"/>
        </Menu>
        <ToolBarTray Grid.Row="1" Background="#FF45628E">
            <ToolBar Width="500" Height="35">
                <Label Content="筆刷色彩" VerticalAlignment="Center" Foreground="Black" />
                <xctk:ColorPicker x:Name="strokeColorPicker" Width="100" HorizontalAlignment="Center" DisplayColorAndName="True" SelectedColorChanged="strokeColorPicker_SelectedColorChanged" />
                <Label Content="填滿色彩" VerticalAlignment="Center" Foreground="Black" />
                <xctk:ColorPicker x:Name="fillColorPicker" Width="100" HorizontalAlignment="Center" DisplayColorAndName="True" SelectedColorChanged="fillColorPicker_SelectedColorChanged" />
                <Slider x:Name="thicknessSlider" Width="100" VerticalAlignment="Center" Minimum="1" Maximum="10" Value="1" IsSnapToTickEnabled="True" ValueChanged="thicknessSlider_ValueChanged" />
                <Label Width="30" VerticalAlignment="Center" FontSize="14" FontWeight="Bold" Content="{Binding Value, ElementName=thicknessSlider}" HorizontalAlignment="Center" HorizontalContentAlignment="Center" Foreground="Black" />
            </ToolBar>
            <ToolBar Width="400" Background="#FF5D7FA3">
                <RadioButton Content="直線" Width="50" VerticalAlignment="Center" Click="ShapeButton_Click" IsChecked="True" Tag="Line" Foreground="Black" />
                <RadioButton Content="矩形" Width="50" VerticalAlignment="Center" Click="ShapeButton_Click" Tag="Rectangle" Foreground="Black" />
                <RadioButton Content="橢圓形" Width="50" VerticalAlignment="Center" Click="ShapeButton_Click" Tag="Ellipse" Foreground="Black" />
            </ToolBar>
        </ToolBarTray>
        <Canvas x:Name="myCanvas" Grid.Row="2" Background="#FFC8D7E5" MouseLeftButtonDown="myCanvas_MouseLeftButtonDown" MouseMove="myCanvas_MouseMove" MouseLeftButtonUp="myCanvas_MouseLeftButtonUp"/>
        <StatusBar Grid.Row="3" Background="#FF3874AB">
            <Label x:Name="coordinateLabel" Content="座標點" Width="275" Margin="60,0,0,0" Foreground="White" />
            <Label x:Name="shapeLabel" Content="" Width="260" Foreground="White" />
        </StatusBar>
    </Grid>
</Window>
```

# MainWindow.xaml.cs
```csharp
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
```
