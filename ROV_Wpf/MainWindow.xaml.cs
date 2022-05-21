using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ROV_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client client = new();
        public MainWindow()
        {
            InitializeComponent();
            new Thread(() =>
            {
                while (true)
                {
                    Thread.CurrentThread.IsBackground = true;
                    Telemetry data = client.get_data();
                    Dispatcher.Invoke(
                        delegate ()
                        {
                            if (data != null)
                            {
                                telemetry.Content = $"light: {data.light}";
                            }
                        });
                    Thread.Sleep(2000);
                }
            }).Start();
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int speed = Convert.ToInt32(engine.Value);
            int pin = 2;
            var command = new EngineCommand(pin, 3, speed);
            // Debug.WriteLine(engine.Value);
            client.engine(command);
        }
    } 
}