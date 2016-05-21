using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

namespace Lego.PowerFunctions.WebApi
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class PingController
    {
        private readonly int _a;

        public PingController(int a)
        {
            _a = a;
        }

        [UriFormat("/ping")]
        public IGetResponse Ping()
        {
            using (var device = SpiDeviceFactory.InitSpi().Result)
            using (var rcTransmitter = new Transmitter(device, true))
            {
                var receiver  = new Receiver(rcTransmitter, Channel.Ch1);
                receiver.BlueConnector.RemoteControl.Execute(receiver.BlueConnector.Output, PwmSpeed.ForwardStep1);
            }

            return new GetResponse(GetResponse.ResponseStatus.OK);
        }
    }
}