// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using System;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Communication
{
    /// <summary>
    ///     This class is responsible for translating data packages into
    ///     38 kHz infrared light pulses.
    /// </summary>
    internal static class IrPulseEncoder
    {
        private const short MaxEncodedMessageSize = 522;
        private const short HighIrPulseCount = 6;
        private const short LowIrPulseCountFor0 = 10;
        private const short LowIrPulseCountFor1 = 21;
        private const short LowIrPulseCountForStartStop = 39;

        public static ushort[] Encode(ushort data)
        {
            var buffer = new ushort[MaxEncodedMessageSize];
            ushort currentDataMask = 0x8000;
            short currentPulse = 0;
            currentPulse = FillStartStop(buffer, currentPulse);

            while (currentDataMask != 0)
            {
                var currentDataBit = (data & currentDataMask) == 0;
                currentPulse = FillBit(currentDataBit, currentPulse, buffer);
                currentDataMask >>= 1;
            }
            //stop bit
            short finalPulseCount = Fill(buffer, currentPulse, HighIrPulseCount, 39);
            var result = Trim(buffer, finalPulseCount);
            return result;
        }

        private static short FillBit(bool currentDataBit, short currentPulse, ushort[] buffer)
        {
            var lowPulseCount = currentDataBit ? LowIrPulseCountFor0 : LowIrPulseCountFor1;
            currentPulse = Fill(buffer, currentPulse, HighIrPulseCount, lowPulseCount);
            return currentPulse;
        }

        private static short FillStartStop(ushort[] buffer, short startIndex)
        {
            return Fill(buffer, startIndex, HighIrPulseCount, LowIrPulseCountForStartStop);
        }

        private static ushort[] Trim(ushort[] buffer, short finalLength)
        {
            var result = new ushort[finalLength];
            Array.Copy(buffer, result, finalLength);
            return result;
        }

        private static short Fill(ushort[] buffer, short startIndex, short pulseCount, IrPulse irPulse)
        {
            short i;
            for (i = startIndex; i < startIndex + pulseCount; i++)
            {
                buffer[i] = (ushort) irPulse;
            }
            return i;
        }

        private static short Fill(ushort[] buffer, short startIndex, short highPulseCount, short lowPulseCount)
        {
            short currentIndex = startIndex;
            currentIndex = Fill(buffer, currentIndex, highPulseCount, IrPulse.High);
            currentIndex = Fill(buffer, currentIndex, lowPulseCount, IrPulse.Low);
            return currentIndex;
        }
    }
}