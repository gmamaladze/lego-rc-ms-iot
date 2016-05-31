using System.Collections.Generic;
using Windows.Foundation.Collections;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

namespace Lego.PowerFunctions.WebApi
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class ActuatorController
    {
        private static readonly Dictionary<int, Channel> Nr2Channel = new Dictionary<int, Channel>
        {
            {1, Channel.Ch1 },
            {2, Channel.Ch2 },
            {3, Channel.Ch3 },
            {4, Channel.Ch4 }
        };

        private static readonly Dictionary<string, Output> Color2Output = new Dictionary<string, Output>
        {
            {"red", Output.Red },
            {"blue", Output.Blue }
        };

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

        private readonly Transmitter _transmitter;

        public ActuatorController(object transmitter)
        {
            _transmitter = (Transmitter)transmitter;
        }

        [UriFormat("{channelNr}/{color}/set/{speed}")]
        public IGetResponse Set(int channelNr, string color, string speed)
        {
            bool isOk;
            Channel channel;
            isOk = Nr2Channel.TryGetValue(channelNr, out channel);
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);

            Output output;
            isOk = Color2Output.TryGetValue(color, out output);
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);
            
            PwmSpeed pwm;
            isOk = Speed2Pwm.TryGetValue(speed, out pwm);
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);
            
            var rc = new RemoteControl(_transmitter, channel);
            rc.Execute(output, pwm);

            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/break")]
        public IGetResponse Berak(int channelNr, string color)
        {
         //   _connector.RemoteControl.Execute(_connector.Output, PwmSpeed.BreakThenFloat);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/inc")]
        public IGetResponse Inc(int channelNr, string color)
        {
           // _connector.RemoteControl.Execute(_connector.Output, IncDec.IncrementPwm);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/dec")]
        public IGetResponse Dec(int channelNr, string color)
        {
            //_connector.RemoteControl.Execute(_connector.Output, IncDec.DecrementNumericalPwm);
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }
    }
}