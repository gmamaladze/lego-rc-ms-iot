// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostExtensions
    {
        /// <summary>
        /// Runs a web application and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="host">The <see cref="IWebHost"/> to run.</param>
        public static void Run(this IWebHost host)
        {
        }

        /// <summary>
        /// Runs a web application and block the calling thread until token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="host">The <see cref="IWebHost"/> to run.</param>
        /// <param name="token">The token to trigger shutdown.</param>
        public static void Run(this IWebHost host, CancellationToken token)
        {
            host.Run(token, shutdownMessage: null);
        }

        private static void Run(this IWebHost host, CancellationToken token, string shutdownMessage)
        {
            using (host)
            {
                host.Start();

                var hostingEnvironment = host.Services.GetService<IHostingEnvironment>();
                var applicationLifetime = host.Services.GetService<IApplicationLifetime>();


                var serverAddresses = host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
                if (serverAddresses != null)
                {
                    foreach (var address in serverAddresses)
                    {
                    }
                }

                if (!string.IsNullOrEmpty(shutdownMessage))
                {
                }

                token.Register(state =>
                {
                    ((IApplicationLifetime)state).StopApplication();
                },
                applicationLifetime);

                applicationLifetime.ApplicationStopping.WaitHandle.WaitOne();
            }
        }
    }
}