// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using System;
using System.Threading;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;
using Windows.Devices.Spi;
using System.Threading.Tasks;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Communication
{
    public class Transmitter : IDisposable
    {
        private const int MessageResendCount = 5;
        private readonly bool m_DisposeSpi;
        private readonly SpiDevice m_Spi;

        public Transmitter(SpiDevice spi, bool disposeSpi)
        {
            m_Spi = spi;
            m_DisposeSpi = disposeSpi;
        }

        public void Dispose()
        {
            if (m_DisposeSpi && m_Spi != null)
            {
                m_Spi.Dispose();
            }
        }



        internal void Send(Message message)
        {
            var rawData = message.GetData();
            var channel = message.Channel;

            var data = IrPulseEncoder.Encode(rawData);

            for (byte resendIndex = 0; resendIndex <= MessageResendCount; resendIndex++)
            {
                Pause(channel, resendIndex);
                SendData(data);
            }
        }

        protected virtual void SendData(ushort[] data)
        {
            var buffer = new byte[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                buffer[i * 2] = (byte)(data[i] >> 8 & 0xFF);
                buffer[i * 2 + 1] = (byte)(data[i] & 0xFF);
            }
            m_Spi.Write(buffer);
        }

        protected virtual async void Pause(Channel channel, byte resendIndex)
        {
            var milliseconds = 0;
            // delay for first message (4 - Ch) * Tm
            switch (resendIndex)
            {
                case 0:
                    milliseconds = 4 - (int)channel + 1;
                    break;
                case 2:
                case 1:
                    milliseconds = 5;
                    break;
                case 4:
                case 3:
                    milliseconds = 5 + ((int)channel + 1) * 2;
                    break;
            }

            // Tm = 16 ms (in theory 13.7 ms)
            await Task.Delay(milliseconds * 16);
        }
    }
}