// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace CoverDotNet.Core.Filters.CoverageFilters
{
    /// <summary>
    /// A coverage filter that uses a Regular Expression.
    /// </summary>
    internal interface IRegexCoverageFilter : ICoverageFilter
    {
        /// <summary>
        /// Gets the filter for the assemblies.
        /// </summary>
        string AssemblyFilter { get; }

        /// <summary>
        /// Gets the filter uses for classes.
        /// </summary>
        string ClassFilter { get; }

        /// <summary>
        /// Gets a value indicating whether this filter is inclusive.
        /// </summary>
        bool IsInclusive { get; }
    }
}
