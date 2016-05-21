using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;

namespace Lego.PowerFunctions.WebApi
{

    /* Uncomment for Raspberry Pi 2 or 3 */

    /* For Raspberry Pi 2 or 3, use SPI0                             */

    /* Line 0 maps to physical pin number 24 on the RPi2 or RPi3        */

    internal static class SpiDeviceFactory
    {
        private const string SPI_CONTROLLER_NAME = "SPI0";
        private const int SPI_CHIP_SELECT_LINE = 0;

        internal static async Task<SpiDevice> InitSpi()
        {
            //Frequency is 38KHz in the protocol
            const double tCarrier = 1/38.0f;
            //Reality is that there is milliseconds 2us difference in the output as there is always milliseconds 2us bit on on SPI using MOSI
            const double tUshort = tCarrier - 2e-3f;
            //Calulate the outpout frenquency. Here = 16/(1/38 -2^-3) = 658KHz
            var freq = Convert.ToInt32(16.0f/tUshort);

            var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE)
            {
                ClockFrequency = 658000,
                Mode = SpiMode.Mode3
            }; /* Create SPI initialization settings                               */
            /* Datasheet specifies maximum SPI clock frequency of 10MHz         */
            /* The display expects an idle-high clock polarity, we use Mode3
                                                                         * to set the clock polarity and phase to: CPOL = 1, CPHA = 1
                                                                         */

            var spiAqs = SpiDevice.GetDeviceSelector(SPI_CONTROLLER_NAME);
            /* Find the selector string for the SPI bus controller          */
            var devicesInfo = await DeviceInformation.FindAllAsync(spiAqs);
            /* Find the SPI bus controller device with our selector string  */
            return await SpiDevice.FromIdAsync(devicesInfo[0].Id, settings);
            /* Create an SpiDevice with our bus controller and SPI settings */
        }
    }
}