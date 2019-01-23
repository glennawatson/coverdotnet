// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoverDotNet.Core.Model;
using Polly;

namespace CoverDotNet.Core.Helpers
{
    /// <summary>
    /// Assists in getting data about a <see cref="ModuleData"/>.
    /// </summary>
    internal static class ModuleDataHelpers
    {
        private static readonly int MaxProcessingCount = Environment.ProcessorCount - 1;

        /// <summary>
        /// Gets a observable which will populated with <see cref="ModuleData"/>.
        /// It will take a starting assembly, then will load all subsequent modules included from there.
        /// </summary>
        /// <param name="startAssemblyPath">The starting assembly path.</param>
        /// <returns>The observable of <see cref="ModuleData"/>.</returns>
        public static IObservable<ModuleData> GetModulesObservable(string startAssemblyPath)
        {
            var seenAssemblies = new ConcurrentDictionary<string, int>();
            var processingAssemblies = new ConcurrentStack<string>();
            processingAssemblies.Push(startAssemblyPath);

            using (var bulkhead = Policy.BulkheadAsync(MaxProcessingCount))
            {
                return Observable.Create<ModuleData>(async (obs, token) =>
                {
                    while (!seenAssemblies.IsEmpty)
                    {
                        string[] items = new string[MaxProcessingCount];
                        var itemsPopped = processingAssemblies.TryPopRange(items);

                        if (itemsPopped == 0)
                        {
                            continue;
                        }

                        List<Task<ModuleData>> tasks = new List<Task<ModuleData>>();
                        foreach (var assemblyPath in items.Take(itemsPopped))
                        {
                            var numberTimesSeen = seenAssemblies.AddOrUpdate(assemblyPath, 1, (_, currentCount) => currentCount + 1);

                            if (numberTimesSeen != 1)
                            {
                                continue;
                            }

                            tasks.Add(bulkhead.ExecuteAsync((context) => GetModuleDataAsync(assemblyPath, context), token));
                        }

                        var results = await Task.WhenAll(tasks).ConfigureAwait(true);

                        foreach (var result in results)
                        {
                            processingAssemblies.PushRange(result.AssemblyDefinition.Modules.Select(x => x.FileName).ToArray());
                            obs.OnNext(result);
                        }
                    }

                    obs.OnCompleted();
                });
            }
        }

        private static Task<ModuleData> GetModuleDataAsync(string assemblyPath, CancellationToken token)
        {
            return Task.Run(
                () =>
                {
                    var moduleData = new ModuleData();
                    moduleData.AssemblyDefinition = AssemblyDefinitionHelper.LoadAssembly(assemblyPath);
                    moduleData.Hash = GetSHA1HashOfFile(assemblyPath);
                    moduleData.ModuleName = Path.GetFileName(assemblyPath);
                    moduleData.ModulePath = assemblyPath;
                    moduleData.ModuleTime = File.GetLastWriteTimeUtc(assemblyPath);
                    return moduleData;
                },
                token);
        }

        [SuppressMessage("Security", "CA5350: SHA1 is not secure", Justification = "Not used for security")]
        private static string GetSHA1HashOfFile(string sPath)
        {
            using (var sr = new StreamReader(sPath))
            {
                using (var prov = new SHA1CryptoServiceProvider())
                {
                    return BitConverter.ToString(prov.ComputeHash(sr.BaseStream));
                }
            }
        }
    }
}
