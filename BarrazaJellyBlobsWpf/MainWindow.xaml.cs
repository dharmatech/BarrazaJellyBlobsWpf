using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

using System.Windows.Threading;

namespace BarrazaJellyBlobsWpf
{
    static class Utils 
    { 
        public static Random random;
        public static Canvas canvas;
    }

    class BlobElement
    {
        double x, y, vx, vy, ex, ey, strokeSize, fillSize;

        Ellipse strokeEllipse, fillEllipse;

        public BlobElement()
        {
            x = Utils.random.Next(500);
            y = Utils.random.Next(500);

            ex = 0.02 + Utils.random.NextDouble() * 0.10;
            ey = 0.02 + Utils.random.NextDouble() * 0.10;

            strokeSize = 100 + Utils.random.Next(160);
            fillSize = strokeSize - 20;

            strokeEllipse = new Ellipse()
            {
                Width = strokeSize,
                Height = strokeSize,
                Fill = new SolidColorBrush(new Color() { R = 255, G = 0, B = 0, A = 255 })
            };

            Utils.canvas.Children.Add(strokeEllipse);

            Panel.SetZIndex(strokeEllipse, 1);

            fillEllipse = new Ellipse()
            {
                Width = fillSize,
                Height = fillSize,
                Fill = new SolidColorBrush(new Color() { R = 255, G = 255, B = 255, A = 255 })
            };

            Utils.canvas.Children.Add(fillEllipse);

            Panel.SetZIndex(fillEllipse, 2);
        }

        public void Update()
        {
            var point = Mouse.GetPosition(Utils.canvas);
            vx += (point.X - x) * ex;
            vy += (point.Y - y) * ey;
            vx *= 0.92;
            vy *= 0.92;
            x += vx;
            y += vy;

            Canvas.SetLeft(strokeEllipse, x - strokeSize / 2);
            Canvas.SetTop(strokeEllipse, y - strokeSize / 2);

            Canvas.SetLeft(fillEllipse, x - fillSize / 2);
            Canvas.SetTop(fillEllipse, y - fillSize / 2);
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Utils.random = new Random();

            Utils.canvas = canvas;

            var blobElements = new List<BlobElement>();

            for (var i = 0; i < 10; i++)
                blobElements.Add(new BlobElement());

            var timer = new DispatcherTimer();

            timer.Tick += (s, e) => blobElements.ForEach(elt => elt.Update());
                
            timer.Interval = TimeSpan.FromMilliseconds(1000 * 0.01);

            timer.Start();
        }
    }
}
