// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CoverDotNet.Core.Filters.CoverageFilters;
using CoverDotNet.Core.Filters.ItemFilters;

namespace CoverDotNet.Core.Filters
{
    /// <summary>
    /// The main filter which contains all the filters for a the project.
    /// </summary>
    internal class Filter
    {
        private readonly ImmutableArray<IRegexItemFilter> _attributeFilters;
        private readonly ImmutableArray<IRegexItemFilter> _fileFilters;
        private readonly ImmutableArray<ICoverageFilter> _coverageFilters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="attributeFilters">A collection of attribute exclusion filters.</param>
        /// <param name="fileFilters">A collection of file exclusion filters.</param>
        /// <param name="coverageFilters">A collection of coverage filters.</param>
        public Filter(IEnumerable<IRegexItemFilter> attributeFilters, IEnumerable<IRegexItemFilter> fileFilters, IEnumerable<ICoverageFilter> coverageFilters)
        {
            _attributeFilters = attributeFilters.ToImmutableArray();
            _fileFilters = fileFilters.ToImmutableArray();
            _coverageFilters = coverageFilters.ToImmutableArray();
        }

        /// <summary>
        /// Gets a list of attribute exclusion filters.
        /// </summary>
        public IImmutableList<IRegexItemFilter> AttributeFilters => _attributeFilters;

        /// <summary>
        /// Gets a list of file exclusion filters.
        /// </summary>
        public IImmutableList<IRegexItemFilter> FileFilters => _fileFilters;

        /// <summary>
        /// Gets a list of coverage filters.
        /// </summary>
        public IImmutableList<ICoverageFilter> CoverageFilters => _coverageFilters;

        /// <summary>
        /// If we should cover the assembly.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <returns>If we should include the assembly in the coverage or not.</returns>
        public bool ShouldCoverAssembly(string assemblyName)
        {
            if (_coverageFilters.IsDefaultOrEmpty)
            {
                return false;
            }

            return _coverageFilters.All(x => x.ShouldCoverAssembly(assemblyName));
        }

        /// <summary>
        /// If we should cover the class.
        /// </summary>
        /// <param name="className">The name of the class.</param>
        /// <returns>If we should include the class in the assembly or not.</returns>
        public bool ShouldCoverClass(string className)
        {
            if (_coverageFilters.IsDefaultOrEmpty)
            {
                return false;
            }

            return _coverageFilters.All(x => x.ShouldCoverClass(className));
        }

        /// <summary>
        /// If we should exclude because of a attribute name.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>If we should exclude or not.</returns>
        public bool ShouldExcludeByAttribute(string attributeName)
        {
            return _attributeFilters.Any(x => x.IsMatch(attributeName));
        }

        /// <summary>
        /// If we should exclude because of a file name.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>If we should exclude or not.</returns>
        public bool ShouldExcludeByFileName(string fileName)
        {
            return _fileFilters.Any(x => x.IsMatch(fileName));
        }
    }
}
