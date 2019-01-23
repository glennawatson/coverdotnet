// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CoverDotNet.Core;
using CoverDotNet.Core.Helpers;
using DynamicData;
using Xunit;

namespace CoverDotNet.Tests
{
    /// <summary>
    /// The tests for the <see cref="InstrumentationInjector"/> class.
    /// </summary>
    public class InjectorTests
    {
#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif

        /// <summary>
        /// Tests to make sure it works.
        /// </summary>
        /// <param name="targetFramework">The target framework we are testing.</param>
        /// <param name="extension">The extension of the assembly to test.</param>
        [Theory]
        [InlineData("net461", ".exe")]
        [InlineData("netcoreapp2.1", ".dll")]
        public void CanLoadAssembly(string targetFramework, string extension)
        {
            string assemblyFileName = GetAssemblyPath(targetFramework, extension);
            var assembly = AssemblyDefinitionHelper.LoadAssembly(assemblyFileName);
            Assert.NotNull(assembly);
        }

        /// <summary>
        /// Tests to make sure it works.
        /// </summary>
        /// <param name="targetFramework">The target framework we are testing.</param>
        /// <param name="extension">The extension of the assembly to test.</param>
        [Theory]
        [InlineData("net461", ".exe")]
        [InlineData("netcoreapp2.1", ".dll")]
        public void CanInjectAssembly(string targetFramework, string extension)
        {
            string assemblyFileName = GetAssemblyPath(targetFramework, extension);

            var observable = ModuleDataHelpers.GetModulesObservable(assemblyFileName);

            observable.ToObservableChangeSet().ObserveOn(ImmediateScheduler.Instance).Bind(out var items);

            Assert.Single(items);
        }

        private static string GetAssemblyPath(string targetFramework, string extension)
        {
            // Strip the bin directory from the path.
            var pathRegex = new Regex(@"\\bin(\\x86|\\x64)?\\(Debug|Release)(\\[a-zA-Z0-9.]*)?$", RegexOptions.Compiled);
            var directory = pathRegex.Replace(Directory.GetCurrentDirectory(), string.Empty);

            // Get to the executable directory for the project.
            return Path.Combine(directory, @"..\CoverDotNet.Integration\bin\", Configuration + @"\", targetFramework + @"\", @"CoverDotNet.Integrations" + extension);
        }
    }
}
