// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Commands
{
    public class CommandProcessor
    {
        private readonly Channel m_Channel;
        private readonly Transmitter m_Transmitter;
        private Toggle m_Toggle;

        public CommandProcessor(Transmitter transmitter, Channel channel)
        {
            m_Transmitter = transmitter;
            m_Channel = channel;
            m_Toggle = Toggle.Even;
        }

        public void Execute(ICommand command)
        {
            var message = MessageFactory.GetMessage(command, m_Channel, m_Toggle);
            m_Transmitter.Send(message);
            TriggerToggle();
        }

        private void TriggerToggle()
        {
            m_Toggle = (m_Toggle == Toggle.Even) ? Toggle.Odd : Toggle.Even;
        }
    }
}