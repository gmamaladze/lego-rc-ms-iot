using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using Windows.System.Threading;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators;
using System.Threading.Tasks;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace BackgroundApplication2
{
    public sealed class StartupTask : IBackgroundTask
    {

        /* Uncomment for Raspberry Pi 2 or 3 */
        private const string SPI_CONTROLLER_NAME = "SPI0";  /* For Raspberry Pi 2 or 3, use SPI0                             */
        private const Int32 SPI_CHIP_SELECT_LINE = 0;       /* Line 0 maps to physical pin number 24 on the RPi2 or RPi3        */

        /* Initialize the SPI bus */
        private async Task<SpiDevice> InitSpi()
        {
            //Frequency is 38KHz in the protocol
            const double tCarrier = 1 / 38.0f;
            //Reality is that there is milliseconds 2us difference in the output as there is always milliseconds 2us bit on on SPI using MOSI
            const double tUshort = tCarrier - 2e-3f;
            //Calulate the outpout frenquency. Here = 16/(1/38 -2^-3) = 658KHz
            var freq = Convert.ToInt32(16.0f / tUshort);

            var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE)
            {
                ClockFrequency = 658000,
                Mode = SpiMode.Mode3
            };/* Create SPI initialization settings                               */
            /* Datasheet specifies maximum SPI clock frequency of 10MHz         */
            /* The display expects an idle-high clock polarity, we use Mode3
                                                                         * to set the clock polarity and phase to: CPOL = 1, CPHA = 1
                                                                         */

            string spiAqs = SpiDevice.GetDeviceSelector(SPI_CONTROLLER_NAME);       /* Find the selector string for the SPI bus controller          */
            var devicesInfo = await DeviceInformation.FindAllAsync(spiAqs);         /* Find the SPI bus controller device with our selector string  */
            return await SpiDevice.FromIdAsync(devicesInfo[0].Id, settings);  /* Create an SpiDevice with our bus controller and SPI settings */

        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            while (true)
            {
                var device = InitSpi().Result;
                using (var transmitter = new Transmitter(device, true))
                {

                    //Firts vehiclle
                    var receiverTrain = new Receiver(transmitter, Channel.Ch1);
                    var motor = new Motor(receiverTrain.RedConnector);
                    var light = new Led(receiverTrain.RedConnector);

                    //Rover on another channel
                    var receiverRover = new Receiver(transmitter, Channel.Ch2);
                    var drive = new Motor(receiverRover.BlueConnector);
                    var steeringWheel = new Servo(receiverRover.RedConnector);

                    //Now Control
                    while (true)
                    {
                        motor.IncSpeed();
                        light.TurnOn();

                        drive.SetSpeed(100);
                        steeringWheel.SetAngle180(45);

                        Task.Delay(300).Wait();

                        steeringWheel.Center();
                        motor.Brake();

                        light.TurnOff();
                    }
                }
            }
        }
    }
}
