using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Rest;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace Lego.PowerFunctions.WebApi
{
    public sealed class StartupTask : IBackgroundTask
    {
        /* Uncomment for Raspberry Pi 2 or 3 */

        private const string SPI_CONTROLLER_NAME = "SPI0";
            /* For Raspberry Pi 2 or 3, use SPI0                             */

        private const int SPI_CHIP_SELECT_LINE = 0;
            /* Line 0 maps to physical pin number 24 on the RPi2 or RPi3        */

        private BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            //using (
                var device = InitSpi().Result;//)
            //using (
                var rcTransmitter = new Transmitter(device, true);//)
            //using (
                var server = new HttpServer(1390)
            ;//)
            {
                var pingHandler = new RestRouteHandler();
                pingHandler.RegisterController<PingController>(3);
                server.RegisterRoute("test", pingHandler);
                server.RegisterRoute("test/2", pingHandler);

                var receiver = new Receiver(rcTransmitter, Channel.Ch1);
                var handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(receiver.RedConnector);
                server.RegisterRoute("lego/1/red", handler);
                handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(receiver.BlueConnector);
                server.RegisterRoute("lego/1/blue", handler);

                receiver = new Receiver(rcTransmitter, Channel.Ch2);
                handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(receiver.RedConnector);
                server.RegisterRoute("lego/2/red", handler);
                handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(receiver.BlueConnector);
                server.RegisterRoute("lego/2/blue", handler);

                receiver = new Receiver(rcTransmitter, Channel.Ch3);
                handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(receiver.RedConnector);
                server.RegisterRoute("lego/3/red", handler);
                handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(receiver.BlueConnector);
                server.RegisterRoute("lego/3/blue", handler);

                receiver = new Receiver(rcTransmitter, Channel.Ch4);
                handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(receiver.RedConnector);
                server.RegisterRoute("lego/4/red", handler);
                handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(receiver.BlueConnector);
                server.RegisterRoute("lego/4/blue", handler);

                server.StartServerAsync().Wait();
            }
        }

        /* Initialize the SPI bus */

        private async Task<SpiDevice> InitSpi()
        {
            //Frequency is 38KHz in the protocol
            const double tCarrier = 1/38.0f;
            //Reality is that there is milliseconds 2us difference in the output as there is always milliseconds 2us bit on on SPI using MOSI
            const double tUshort = tCarrier - 2e-3f;
            //Calulate the outpout frenquency. Here = 16/(1/38 -2^-3) = 658KHz
            var freq = Convert.ToInt32(16.0f/tUshort);

            var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE)
            {
                ClockFrequency = 658000,
                Mode = SpiMode.Mode3
            }; /* Create SPI initialization settings                               */
            /* Datasheet specifies maximum SPI clock frequency of 10MHz         */
            /* The display expects an idle-high clock polarity, we use Mode3
                                                                         * to set the clock polarity and phase to: CPOL = 1, CPHA = 1
                                                                         */

            var spiAqs = SpiDevice.GetDeviceSelector(SPI_CONTROLLER_NAME);
                /* Find the selector string for the SPI bus controller          */
            var devicesInfo = await DeviceInformation.FindAllAsync(spiAqs);
                /* Find the SPI bus controller device with our selector string  */
            return await SpiDevice.FromIdAsync(devicesInfo[0].Id, settings);
                /* Create an SpiDevice with our bus controller and SPI settings */
        }
    }
}