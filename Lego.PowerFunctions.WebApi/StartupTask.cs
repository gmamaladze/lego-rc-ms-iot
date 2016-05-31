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

        private BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            using (var device = SpiDeviceFactory.InitSpi().Result)
            using (var rcTransmitter = new Transmitter(device, true))
                
            {
                var server = new HttpServer(1390);
                var pingHandler = new RestRouteHandler();
                pingHandler.RegisterController<PingController>(3);
                server.RegisterRoute("test", pingHandler);

                var handler = new RestRouteHandler();
                handler.RegisterController<ActuatorController>(rcTransmitter);
                server.RegisterRoute("lego", handler);
                server.StartServerAsync().Wait();
            }
        }
    }
}