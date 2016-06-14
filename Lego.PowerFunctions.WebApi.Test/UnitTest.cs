using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Devkoes.Restup.WebServer.Rest;
using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Contracts;
using Devkoes.HttpMessage.Models.Schemas;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Text;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Communication;
using Gma.Netmf.Hardware.Lego.PowerFunctions.Control;

namespace Lego.PowerFunctions.WebApi.Test
{



    public class TestStream : IInputStream
    {
        private int indexCounter = 0;
        private IEnumerable<byte[]> _byteStreamParts;

        public TestStream(IEnumerable<byte[]> byteStreamParts)
        {
            _byteStreamParts = byteStreamParts;
        }

        public void Dispose()
        {
        }

        public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
        {
            return AsyncInfo.Run<IBuffer, uint>((token, progress) =>
            {
                IBuffer buff = new byte[0].AsBuffer();
                if (indexCounter < _byteStreamParts.Count())
                {
                    buff = _byteStreamParts.ElementAt(indexCounter).AsBuffer();
                }

                indexCounter++;
                return Task.FromResult(buff);
            });
        }
    }


    [TestClass]
    public class UnitTest1
    {



        [TestMethod]
        public void TestPingController()
        {
            var handler = new RestRouteHandler();
            handler.RegisterController<PingController>(3);
            var request = Create("/ping");
            var result = handler.HandleRequest(request).Result;
            Assert.AreEqual(result.ResponseStatus, HttpResponseStatus.OK);
        }

        [TestMethod]
        public void Test_set_on_LegoController()
        {
            var sender = new DummySender();
            var trandmitter = new Transmitter(sender);

            var handler = new RestRouteHandler();
            handler.RegisterController<LegoController>(trandmitter, "ch1");
            var request = Create($"/setspeedone?output=red&value=ForwardStep1");
            var result = handler.HandleRequest(request).Result;
            Assert.AreEqual(result.ResponseStatus, HttpResponseStatus.OK);
        }



        private static IHttpServerRequest Create(string uri)
        {
            var streamedRequest = $"GET {uri} HTTP/1.1\r\nContent-Length: 4\r\n\r\ndata";
            var byteStreamParts = new List<byte[]>();
            byteStreamParts.Add(Encoding.UTF8.GetBytes(streamedRequest));
            return MutableHttpServerRequest.Parse(new TestStream(byteStreamParts)).Result;
        }
    }

    public class DummySender : ISender
    {
        public void Write(byte[] buffer)
        {
            
        }
    }
}
