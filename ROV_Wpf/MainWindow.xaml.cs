using SharpDX.DirectInput;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

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
                    Thread.Sleep(5000);
                }

            }).Start();
            new Thread(() =>
            {
                var directInput = new DirectInput();

                int mode = 0;

                var joystickGuid = Guid.Empty;

                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
                            DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;

                // If Gamepad not found, look for a Joystick
                if (joystickGuid == Guid.Empty)
                    foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                            DeviceEnumerationFlags.AllDevices))
                        joystickGuid = deviceInstance.InstanceGuid;


                // Instantiate the joystick
                var joystick = new Joystick(directInput, joystickGuid);

                Debug.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

                // Query all suported ForceFeedback effects
                var allEffects = joystick.GetEffects();
                foreach (var effectInfo in allEffects)
                    Debug.WriteLine("Effect available {0}", effectInfo.Name);

                // Set BufferSize in order to use buffered data.
                joystick.Properties.BufferSize = 128;

                // Acquire the joystick
                joystick.Acquire();

                while (true)
                {
                    int speed_value;
                    joystick.Poll();
                    var datas = joystick.GetBufferedData();
                    foreach (var state in datas)
                    {
                        if (state.Offset == JoystickOffset.Y)
                        {
                            speed_value = Control.get_y_value(state);
                            if (speed_value != mode)
                            {
                                Dispatcher.Invoke(
                                   delegate ()
                                   {
                                       engine.Value = speed_value;
                                   });
                                mode = speed_value;
                                Debug.WriteLine(mode);
                            }
                        }
                    }
                    Thread.CurrentThread.IsBackground = true;
                    Thread.Sleep(100);
                }

            }).Start();
        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int speed = Convert.ToInt32(engine.Value);
            int pin;
            if (speed < 0)
            {
                pin = 2;
                speed = Math.Abs(speed);
            }
            else if (speed == 0)
            {
                pin = 2;
                client.engine(new EngineCommand(2, 3, 0));
                client.engine(new EngineCommand(3, 3, 0));
            }
            else
            {
                pin = 3;
            }
            var command = new EngineCommand(pin, 3, speed);
            // Debug.WriteLine(engine.Value);
            client.engine(command);
        }
    }
}