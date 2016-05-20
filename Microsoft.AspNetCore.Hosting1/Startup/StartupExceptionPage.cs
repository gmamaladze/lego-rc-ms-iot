// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Microsoft.AspNetCore.Hosting
{
    internal static class StartupExceptionPage
    {
        private static readonly string _errorPageFormatString = GetResourceString("GenericError.html", escapeBraces: true);
        private static readonly string _errorMessageFormatString = GetResourceString("GenericError_Message.html");
        private static readonly string _errorExceptionFormatString = GetResourceString("GenericError_Exception.html");
        private static readonly string _errorFooterFormatString = GetResourceString("GenericError_Footer.html");

        private static string GetResourceString(string name, bool escapeBraces = false)
        {
            // '{' and '}' are special in CSS, so we use "[[[0]]]" instead for {0} (and so on).
            var assembly = typeof(StartupExceptionPage).GetTypeInfo().Assembly;
            var resourceName = assembly.GetName().Name + ".compiler.resources." + name;
            var manifestStream = assembly.GetManifestResourceStream(resourceName);
            var formatString = new StreamReader(manifestStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false).ReadToEnd();
            if (escapeBraces)
            {
                formatString = formatString.Replace("{", "{{").Replace("}", "}}").Replace("[[[", "{").Replace("]]]", "}");
            }

            return formatString;
        }

        public static byte[] GenerateErrorHtml(bool showDetails, Exception exception)
        {
            // Build the message for each error
            var builder = new StringBuilder();
            var rawExceptionDetails = new StringBuilder();

            builder.Append("An error occurred while starting the application.");


            // And generate the full markup
            return Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, _errorPageFormatString, builder, rawExceptionDetails, string.Empty));
        }
    }
}
