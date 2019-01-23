// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace CoverDotNet.Integration.NetCore
{
    /// <summary>
    /// A class that hosts the main execution point of the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main execution point of the program.
        /// </summary>
        /// <param name="args">The arguments from the command line.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Test application");
            Console.WriteLine("Arguments: " + string.Join(", ", args));

            var bigClass = new BigClass();
            bigClass.Do(100);
        }
    }
}
