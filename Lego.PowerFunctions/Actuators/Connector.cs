// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators
{
    public class Connector
    {
        private readonly Output m_Output;
        private readonly RemoteControl m_RemoteControl;

        internal Connector(RemoteControl remoteControl, Output output)
        {
            m_RemoteControl = remoteControl;
            m_Output = output;
        }

        internal RemoteControl RemoteControl
        {
            get { return m_RemoteControl; }
        }

        public Output Output
        {
            get { return m_Output; }
        }
    }
}