// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection.PortableExecutable;
using Mono.Cecil;

namespace CoverDotNet.Core.Helpers
{
    /// <summary>
    /// A assembly definition loader helper.
    /// Helps with getting assembly definition information from mono cecil.
    /// </summary>
    internal static class AssemblyDefinitionHelper
    {
        /// <summary>
        /// Loads a assembly definition.
        /// Checks to make sure the file exists and it is a valid assembly.
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly.</param>
        /// <returns>The assembly definition for the file path.</returns>
        public static AssemblyDefinition LoadAssembly(string assemblyPath)
        {
            AssemblyDefinition result;

            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException($"Missing Assembly: {assemblyPath}");
            }

            using (Stream s = File.OpenRead(assemblyPath))
            {
                result = AssemblyDefinition.ReadAssembly(s);
            }

            if (result == null)
            {
                throw new NullReferenceException($"Failed to read assembly {assemblyPath}");
            }

            return result;
        }

        /// <summary>
        /// Determines if a assembly has a PDB file with the application, or if the
        /// application has the debug information embedded.
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly to scan.</param>
        /// <returns>If the assembly has debugging information.</returns>
        public static bool HasPdbOrIsEmbedded(string assemblyPath)
        {
            using (var moduleStream = File.OpenRead(assemblyPath))
            {
                using (var peReader = new PEReader(moduleStream))
                {
                    foreach (var entry in peReader.ReadDebugDirectory())
                    {
                        if (entry.Type == DebugDirectoryEntryType.CodeView)
                        {
                            var codeViewData = peReader.ReadCodeViewDebugDirectoryData(entry);
                            if (codeViewData.Path == $"{Path.GetFileNameWithoutExtension(assemblyPath)}.pdb")
                            {
                                // PDB is embedded
                                return true;
                            }

                            return File.Exists(codeViewData.Path);
                        }
                    }

                    return false;
                }
            }
        }
    }
}
