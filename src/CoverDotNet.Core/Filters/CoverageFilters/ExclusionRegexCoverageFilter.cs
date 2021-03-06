﻿// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using CoverDotNet.Core.Filters.ItemFilters;

namespace CoverDotNet.Core.Filters.CoverageFilters
{
    /// <summary>
    /// A regex coverage filter which will match if the values don't match.
    /// </summary>
    internal class ExclusionRegexCoverageFilter : IRegexCoverageFilter
    {
        private readonly RegexItemFilter _assemblyFilter;
        private readonly RegexItemFilter _classFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExclusionRegexCoverageFilter"/> class.
        /// </summary>
        /// <param name="assemblyFilter">The text of our assembly filter regular expression.</param>
        /// <param name="classFilter">The text of our class filter regular expression.</param>
        public ExclusionRegexCoverageFilter(string assemblyFilter, string classFilter)
        {
            _assemblyFilter = new RegexItemFilter(assemblyFilter, true);
            _classFilter = new RegexItemFilter(classFilter, true);
        }

        /// <inheritdoc />
        public string AssemblyFilter => _assemblyFilter.Filter;

        /// <inheritdoc />
        public string ClassFilter => _classFilter.Filter;

        /// <inheritdoc />
        public bool IsInclusive => false;

        /// <inheritdoc />
        public bool ShouldCoverAssembly(string assemblyName)
        {
            return !_assemblyFilter.IsMatch(assemblyName);
        }

        /// <inheritdoc />
        public bool ShouldCoverClass(string className)
        {
            return !_classFilter.IsMatch(className);
        }
    }
}
