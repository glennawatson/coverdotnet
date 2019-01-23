// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using CoverDotNet.Core.Helpers;
using CoverDotNet.Core.Model;

namespace CoverDotNet.Core
{
    /// <summary>
    /// This class will inject the template into our target assembly.
    /// </summary>
    public class InstrumentationInjector
    {
        private volatile uint _currentMethodId;

        /// <summary>
        /// Injects our monitor into the assembly.
        /// </summary>
        /// <param name="module">The module to inject.</param>
        public void Inject(ModuleData module)
        {
        }
    }
}
