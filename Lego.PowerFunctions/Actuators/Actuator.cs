// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Actuators
{
    public abstract class Actuator
    {
        private readonly Output m_Output;
        private readonly RemoteControl m_RemoteControl;

        protected Actuator(Connector connector)
        {
            m_RemoteControl = connector.RemoteControl;
            m_Output = connector.Output;
        }

        protected RemoteControl RemoteControl
        {
            get { return m_RemoteControl; }
        }

        protected Output Output
        {
            get { return m_Output; }
        }
    }
}