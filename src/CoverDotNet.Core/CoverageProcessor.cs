// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoverDotNet.Core.Helpers;
using CoverDotNet.Core.Model;
using Polly;

namespace CoverDotNet.Core
{
    /// <summary>
    /// A class which will perform actions related to coverage testing.
    /// </summary>
    public class CoverageProcessor
    {
        /// <summary>
        /// Runs and processes the specified programs.
        /// </summary>
        /// <param name="output">The output where to send the coverage data.</param>
        /// <param name="assemblyPath">The path to the assembly.</param>
        /// <param name="targetToolPath">The path to the target.</param>
        /// <param name="targetToolArgs">Arguments to pass to the target.</param>
        /// <returns>A observable to monitor the progress.</returns>
        public IObservable<Unit> Process(string output, string assemblyPath, string targetToolPath, string targetToolArgs)
        {
            var moduleObservable = ModuleDataHelpers.GetModulesObservable(assemblyPath);

            return moduleObservable.Select(_ => Unit.Default);
        }
    }
}
