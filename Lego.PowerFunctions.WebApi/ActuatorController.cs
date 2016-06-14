using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Commands;

namespace Lego.PowerFunctions.WebApi
{

    [RestController(InstanceCreationType.Singleton)]
    public sealed class LegoController
    {
        private readonly CommandProcessor _cp;

        public LegoController(object transmitter, string channel)
        {
            var channelvalue = Enum.Parse(typeof(Channel), channel, true);
            _cp = new CommandProcessor((Transmitter)transmitter, (Channel)channelvalue);
        }

        [UriFormat("ext?function={function}")]
        public IGetResponse Ext(string function)
        {
            ExtFunction extFuncValue;
            var isOk = Enum.TryParse(function, true, out extFuncValue);
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);
            _cp.Execute(CommandFactory.Create(extFuncValue));
            return new GetResponse(GetResponse.ResponseStatus.OK);

        }

        [UriFormat("setstate?red={blueState}&blue={redState}")]
        public IGetResponse SetState(string redState, string blueState)
        {
            DirectState blueValue;
            DirectState redValue;
            var isOk = Enum.TryParse(blueState, true, out blueValue); 
            isOk = Enum.TryParse(redState, true, out redValue) && isOk;
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);
            _cp.Execute(CommandFactory.Create(blueValue, redValue));
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("setspeed?red={redSpeed}&blue={blueSpeed}")]
        public IGetResponse SetSpeed(string redSpeed, string blueSpeed)
        {
            PwmSpeed blueValue;
            PwmSpeed redValue;
            var isOk = Enum.TryParse(redSpeed, true, out blueValue);
            isOk = Enum.TryParse(blueSpeed, true, out redValue) && isOk;
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);
            _cp.Execute(CommandFactory.Create(redValue, blueValue));
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("incdecone?output={output}&value={incDec}")]
        public IGetResponse IncDecOne(string output, string incDec)
        {
            Output outputValue;
            IncDec incDecValue;
            var isOk = Enum.TryParse(output, true, out outputValue);
            isOk = Enum.TryParse(incDec, true, out incDecValue) && isOk;
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);
            _cp.Execute(CommandFactory.Create(outputValue, incDecValue));
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("setspeedone?output={output}&value={speed}")]
        public IGetResponse SetSpeedOne(string output, string speed)
        {
            Output outputValue;
            PwmSpeed speedValue;
            var isOk = Enum.TryParse(output, true, out outputValue);
            isOk = Enum.TryParse(speed, true, out speedValue) && isOk;
            if (!isOk) return new GetResponse(GetResponse.ResponseStatus.NotFound);
            _cp.Execute(CommandFactory.Create(outputValue, speedValue));
            return new GetResponse(GetResponse.ResponseStatus.OK);
        }
    }


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