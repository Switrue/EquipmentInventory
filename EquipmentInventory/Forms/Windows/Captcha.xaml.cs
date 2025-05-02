using EquipmentInventory.Classes.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EquipmentInventory.Forms.Windows;

public partial class Captcha : Window
{
    private readonly CaptchaGenerator _captcha = new CaptchaGenerator();
    private readonly Random _random = new Random();
    private const string NoiseChars = "!@#$%^&*()_+-=[]{};:'\",.<>/?\\|~`";

    public Captcha()
    {
        InitializeComponent();
        Loaded += OnWindowLoaded;
    }

    private void OnWindowLoaded(object sender, RoutedEventArgs e) => GenerateCaptcha();

    private void GenerateCaptcha()
    {
        if (!IsLoaded) return;

        captchaCanvas.Children.Clear();
        _captcha.GenerateNew();

        GenerateCaptchaText();
        GenerateNoise();

        inputTextBox.Clear();
    }

    private void GenerateCaptchaText()
    {
        double x = 30;

        foreach (char c in _captcha.GeneratedText)
        {
            var textBlock = new TextBlock
            {
                Text = c.ToString(),
                FontSize = _random.Next(20, 30),
                FontWeight = FontWeights.Bold,
                RenderTransform = new TransformGroup
                {
                    Children = new TransformCollection
                    {
                        new RotateTransform(_random.Next(-15, 15)),
                        new SkewTransform(_random.Next(-5, 5), _random.Next(-5, 5))
                    }
                },
                Foreground = new SolidColorBrush(GetRandomColor(100, 200))
            };

            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, _random.Next(5, 40));
            captchaCanvas.Children.Add(textBlock);
            x += textBlock.FontSize - _random.Next(5, 10);
        }
    }

    private void GenerateNoise()
    {
        AddNoiseDots(100);
        AddNoiseCircles(15);
        AddNoiseSymbols(30);
        AddWavyLine();
    }

    private void AddNoiseDots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var ellipse = new Ellipse
            {
                Width = _random.Next(1, 3),
                Height = _random.Next(1, 3),
                Fill = new SolidColorBrush(GetRandomColor(150, 220)),
                Opacity = _random.Next(5, 8) / 10.0
            };

            PositionElementRandomly(ellipse);
            captchaCanvas.Children.Add(ellipse);
        }
    }

    private void AddNoiseCircles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var circle = new Ellipse
            {
                Width = _random.Next(5, 20),
                Height = _random.Next(5, 20),
                Stroke = new SolidColorBrush(GetRandomColor(150, 200, 30, 80)),
                StrokeThickness = 1,
                Fill = Brushes.Transparent
            };

            PositionElementRandomly(circle);
            captchaCanvas.Children.Add(circle);
        }
    }

    private void AddNoiseSymbols(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var noise = new TextBlock
            {
                Text = NoiseChars[_random.Next(NoiseChars.Length)].ToString(),
                FontSize = _random.Next(8, 14),
                Foreground = new SolidColorBrush(GetRandomColor(100, 200, 50, 120)),
                RenderTransform = new RotateTransform(_random.Next(0, 360))
            };

            PositionElementRandomly(noise);
            captchaCanvas.Children.Add(noise);
        }
    }

    private void AddWavyLine()
    {
        var path = new Path
        {
            Stroke = new SolidColorBrush(GetRandomColor(150, 200, 30, 80)),
            StrokeThickness = 1,
            Data = new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    CreateWavyPathFigure()
                }
            }
        };
        captchaCanvas.Children.Add(path);
    }

    private PathFigure CreateWavyPathFigure()
    {
        var figure = new PathFigure
        {
            StartPoint = new Point(0, _random.Next(10, (int)captchaCanvas.ActualHeight - 10))
        };

        double x = 0;
        while (x < captchaCanvas.ActualWidth)
        {
            x += _random.Next(10, 30);
            figure.Segments.Add(new QuadraticBezierSegment(
                new Point(x - 5, _random.Next(10, (int)captchaCanvas.ActualHeight - 10)),
                new Point(x, _random.Next(10, (int)captchaCanvas.ActualHeight - 10)),
                true));
        }

        return figure;
    }

    private Color GetRandomColor(int min, int max, int alphaMin = 255, int alphaMax = 255)
    {
        return Color.FromArgb(
            (byte)_random.Next(alphaMin, alphaMax),
            (byte)_random.Next(min, max),
            (byte)_random.Next(min, max),
            (byte)_random.Next(min, max));
    }

    private void PositionElementRandomly(FrameworkElement element)
    {
        if (captchaCanvas.ActualWidth > 0 && captchaCanvas.ActualHeight > 0)
        {
            Canvas.SetLeft(element, _random.Next(0, (int)captchaCanvas.ActualWidth));
            Canvas.SetTop(element, _random.Next(0, (int)captchaCanvas.ActualHeight));
        }
    }

    private void OnCheckClick(object sender, RoutedEventArgs e)
    {
        _captcha.UserInput = inputTextBox.Text;

        if (_captcha.Validate())
        {
            DialogResult = true;
        }
        else
        {
            GenerateCaptcha();
        }
    }

    private void OnRefreshClick(object sender, RoutedEventArgs e) => GenerateCaptcha();

    private void CloseWindow_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    private void Window_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();
}