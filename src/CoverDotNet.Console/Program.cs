// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using McMaster.Extensions.CommandLineUtils;

namespace CoverDotNet.Console
{
    /// <summary>
    /// The class that hosts the main execution point of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Gets a value indicating whether we should show more logs.
        /// </summary>
        [Option(Description = "Show more logs", ShortName = "v")]
        public bool Verbose { get; }

        /// <summary>
        /// Gets the location of the output file.
        /// </summary>
        [Required]
        [Option(ShortName = "o", Description = "The path where to output the coverage results.")]
        public string Output { get; }

        /// <summary>
        /// Gets a value indicating whether we should show unvisited methods and classes.
        /// </summary>
        [Option(ShortName = "u", Description = "Shows a list of unvisited methods or classes after the coverage.")]
        public bool ShowUnvisited { get; }

        /// <summary>
        /// Gets a list of directories to exclude assemblies from.
        /// </summary>
        [Option(CommandOptionType.MultipleValue, ShortName = "", Description = "List of directories to exclude assemblies from in the coverage results.")]
        public List<string> ExcludeDirs { get; }

        /// <summary>
        /// Gets a list of files to exclude.
        /// </summary>
        [Option(CommandOptionType.MultipleValue, ShortName = "", Description = "List of files to exclude from the coverage results. Wildcards are supported.")]
        public List<string> ExcludeByFile { get; }

        /// <summary>
        /// Gets the target directory where we run the process from.
        /// </summary>
        [Option(ShortName = "d", Description = "A path to the target directory.")]
        public string TargetDir { get; }

        /// <summary>
        /// Gets the arguments to pass to the target application.
        /// </summary>
        [Option(ShortName = "a", Description = "A set of arguments to pass to the target application.")]
        public string TargetArgs { get; }

        /// <summary>
        /// Gets the path to the target application.
        /// </summary>
        [Option(ShortName = "t", Description = "The path to the target application.")]
        public string Target { get; }

        /// <summary>
        /// The main execution point of the application.
        /// </summary>
        /// <param name="args">The arguments being passed in.</param>
        /// <returns>The error code for the application.</returns>
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private void OnExecute()
        {
        }
    }
}
