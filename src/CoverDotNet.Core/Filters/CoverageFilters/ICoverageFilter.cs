// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace CoverDotNet.Core.Filters.CoverageFilters
{
    /// <summary>
    /// A set of filters which will exclude assemblies and classes from coverage reports.
    /// </summary>
    public interface ICoverageFilter
    {
        /// <summary>
        /// Determines if we should do coverage testing of the specified assembly.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to test.</param>
        /// <returns>If we should do coverage testing or not.</returns>
        bool ShouldCoverAssembly(string assemblyName);

        /// <summary>
        /// Determines if we should do coverage testing of the specified class.
        /// </summary>
        /// <param name="className">The name of the class to test.</param>
        /// <returns>If we should do coverage testing or not.</returns>
        bool ShouldCoverClass(string className);
    }
}
