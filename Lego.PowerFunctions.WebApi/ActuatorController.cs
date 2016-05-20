using System.Collections.Generic;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

namespace Lego.PowerFunctions.WebApi
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class ActuatorController
    {
        private static readonly Dictionary<string, PwmSpeed> Speed2Pwm = new Dictionary<string, PwmSpeed>
        {
            {"break", PwmSpeed.BreakThenFloat},
            {"bw1", PwmSpeed.BackwardStep1},
            {"bw2", PwmSpeed.BackwardStep2},
            {"bw3", PwmSpeed.BackwardStep3},
            {"bw4", PwmSpeed.BackwardStep4},
            {"bw5", PwmSpeed.BackwardStep5},
            {"bw6", PwmSpeed.BackwardStep6},
            {"bw7", PwmSpeed.BackwardStep7},
            {"float", PwmSpeed.Float},
            {"fw1", PwmSpeed.ForwardStep1},
            {"fw2", PwmSpeed.ForwardStep2},
            {"fw3", PwmSpeed.ForwardStep3},
            {"fw4", PwmSpeed.ForwardStep4},
            {"fw5", PwmSpeed.ForwardStep5},
            {"fw6", PwmSpeed.ForwardStep6},
            {"fw7", PwmSpeed.ForwardStep7}
        };

        private readonly Connector _connector;

        public ActuatorController(Connector connector)
        {
            _connector = connector;
        }

        [UriFormat("/set/{speed}")]
        public IGetResponse Set(string speed)
        {
            PwmSpeed pwm;
            var isOk = Speed2Pwm.TryGetValue(speed, out pwm);
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);
            _connector.RemoteControl.Execute(_connector.Output, pwm);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/break")]
        public IGetResponse Berak()
        {
            _connector.RemoteControl.Execute(_connector.Output, PwmSpeed.BreakThenFloat);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/inc")]
        public IGetResponse Inc()
        {
            _connector.RemoteControl.Execute(_connector.Output, IncDec.IncrementPwm);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/dec")]
        public IGetResponse Dec()
        {
            _connector.RemoteControl.Execute(_connector.Output, IncDec.DecrementNumericalPwm);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }
    }
}