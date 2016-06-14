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
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }
    }
}