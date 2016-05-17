// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators
{
    public class Receiver
    {
        private readonly Connector m_BlueConnector;
        private readonly Connector m_RedConnector;

        public Receiver(Transmitter transmitter, Channel channel)
            : this(new RemoteControl(transmitter, channel))
        {
        }

        internal Receiver(RemoteControl remoteControl)
        {
            m_RedConnector = new Connector(remoteControl, Output.Red);
            m_BlueConnector = new Connector(remoteControl, Output.Blue);
        }

        public Connector RedConnector
        {
            get { return m_RedConnector; }
        }


        public Connector BlueConnector
        {
            get { return m_BlueConnector; }
        }
    }
}