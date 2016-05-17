// This code is distributed under MIT license. 
// Copyright (c) 2014 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

#region usings

using System;

#endregion

namespace Gma.Netmf.Hardware.Lego.PowerFunctions.Control
{
    public static class PwmSpeedExtensions
    {
        private static readonly PwmSpeed[] AscendingSpeeds =
        {
            PwmSpeed.BackwardStep7,
            PwmSpeed.BackwardStep6,
            PwmSpeed.BackwardStep5,
            PwmSpeed.BackwardStep4,
            PwmSpeed.BackwardStep3,
            PwmSpeed.BackwardStep2,
            PwmSpeed.BackwardStep1,
            PwmSpeed.BreakThenFloat,
            PwmSpeed.ForwardStep1,
            PwmSpeed.ForwardStep2,
            PwmSpeed.ForwardStep3,
            PwmSpeed.ForwardStep4,
            PwmSpeed.ForwardStep5,
            PwmSpeed.ForwardStep6,
            PwmSpeed.ForwardStep7
        };

        public static PwmSpeed FromPercent(this int percent)
        {
            if (percent < -100 || percent > 100) throw new ArgumentOutOfRangeException("percent");
            var index = (int) Math.Round(percent/15.0 + 7);
            return AscendingSpeeds[index];
        }

        public static PwmSpeed FromAngle(this int angle)
        {
            if (angle < 0 || angle > 180) throw new ArgumentOutOfRangeException("angle");
            var index = (int) Math.Round(angle/180.0*14);
            return AscendingSpeeds[index];
        }

        public static int ToPercent(this PwmSpeed speed)
        {
            var index = Array.IndexOf(AscendingSpeeds, speed);
            var percent = (index - AscendingSpeeds.Length/2)*100;
            return percent;
        }
    }
}