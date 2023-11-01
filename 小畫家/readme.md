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
using System; // 引入 System 命名空间，其中包含了许多基本的类和功能
using System.Linq; // 引入 LINQ 命名空间，用于查询集合
using System.Windows; // 引入 WPF 中的 Window 类
using System.Windows.Controls; // 引入 WPF 中的控件类
using System.Windows.Input; // 引入 WPF 中的输入相关类
using System.Windows.Media; // 引入 WPF 中的绘图和媒体类
using System.Windows.Shapes; // 引入 WPF 中的形状类

namespace _2023_WpfApp3
{
    public partial class MainWindow : Window
    {
        private string shapeType = "Line"; // 声明一个字符串变量 shapeType，用于存储当前图形类型，默认为 "Line"
        private Color strokeColor = Colors.Red; // 声明一个 Color 类型的变量 strokeColor，用于存储线条颜色，默认为红色
        private Color fillColor = Colors.Yellow; // 声明一个 Color 类型的变量 fillColor，用于存储填充颜色，默认为黄色
        private int strokeThickness = 1; // 声明一个整数变量 strokeThickness，用于存储线条粗细，默认为 1
        private Point start, dest; // 声明 Point 类型的变量 start 和 dest，用于存储绘图起始点和终止点
        private UIElement currentShape; // 声明一个 UIElement 类型的变量 currentShape，用于存储当前绘制的形状

        public MainWindow()
        {
            InitializeComponent(); // 在构造函数中初始化窗口
            strokeColorPicker.SelectedColor = strokeColor; // 设置颜色选择器的选定颜色为初始线条颜色
            fillColorPicker.SelectedColor = fillColor; // 设置颜色选择器的选定颜色为初始填充颜色
        }

        // 下面的方法处理事件和用户交互

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString(); // 当用户单击形状按钮时，更新当前图形类型
        }

        private void strokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = (Color)strokeColorPicker.SelectedColor; // 当用户更改线条颜色时，更新线条颜色
        }

        private void thicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = Convert.ToInt32(thicknessSlider.Value); // 当用户更改线条粗细时，更新线条粗细
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross; // 将鼠标光标设置为十字形
            start = e.GetPosition(myCanvas); // 记录鼠标左键按下时的位置
            DisplayStatus(); // 更新状态信息

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
                myCanvas.Children.Add(currentShape); // 创建并添加线条到画布上
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
                myCanvas.Children.Add(currentShape); // 创建并添加矩形或椭圆到画布上
            }
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas); // 记录鼠标移动时的位置
            DisplayStatus(); // 更新状态信息

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
            currentShape = null; // 当鼠标左键释放时，清空当前形状
        }

        private void fillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = (Color)fillColorPicker.SelectedColor; // 当用户更改填充颜色时，更新填充颜色
        }

        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear(); // 清空画布上的所有元素
            DisplayStatus(); // 更新状态信息
        }

        private void DisplayStatus()
        {
            int lineCount = myCanvas.Children.OfType<Line>().Count();
            int rectCount = myCanvas.Children.OfType<Rectangle>().Count();
            int ellipseCount = myCanvas.Children.OfType<Ellipse>().Count();

            coordinateLabel.Content = $"座標點：({Math.Round(start.X)}, {Math.Round(start.Y)}) - ({Math.Round(dest.X)}, {Math.Round(dest.Y)})"; // 更新坐标信息
            shapeLabel.Content = $"Line: {lineCount}, Rectangle: {rectCount}, Ellipse: {ellipseCount}"; // 更新形状信息
        }
    }
}

```
# 詳細說明
### ShapeButton_Click 是一個事件處理程序，當用戶按下不同形狀的按鈕時觸發。它會根據按鈕的 Tag 屬性來設定 shapeType 變數，以指定正在繪製的形狀類型。
```csharp
private void ShapeButton_Click(object sender, RoutedEventArgs e)
{
    var targetRadioButton = sender as RadioButton;
    shapeType = targetRadioButton.Tag.ToString();
}
```




