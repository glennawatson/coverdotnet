// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CoverDotNet.Core.Filters.ItemFilters;

namespace CoverDotNet.Core.Filters.CoverageFilters
{
    /// <summary>
    /// A builder class for generating filters.
    /// </summary>
    internal class FilterBuilder
    {
        private static readonly Regex wildcardAssemblyMatch = new Regex(@"^(?<type>([+-]))(\[(?<assembly>(.+))\])(?<class>(.+))$", RegexOptions.Compiled);
        private static readonly Regex regexAssemblyMatch = new Regex(@"^(?<type>([+-]))(\[\((?<assembly>(.+))\)\])(\((?<class>(.+))\))$", RegexOptions.Compiled);

        private readonly ConcurrentBag<ICoverageFilter> _coverageFilters = new ConcurrentBag<ICoverageFilter>();
        private readonly ConcurrentBag<IRegexItemFilter> _attributeExclusions = new ConcurrentBag<IRegexItemFilter>();
        private readonly ConcurrentBag<IRegexItemFilter> _fileExclusions = new ConcurrentBag<IRegexItemFilter>();

        /// <summary>
        /// Gets a set of default exclusions for the platform.
        /// </summary>
        /// <returns>A list of the filters.</returns>
        public FilterBuilder AddDefaultFilters()
        {
            AddWildcardCoverageFilter("-[mscorlib]*");
            AddWildcardCoverageFilter("-[mscorlib.*]*");
            AddWildcardCoverageFilter("-[System]*");
            AddWildcardCoverageFilter("-[System.*]*");
            AddWildcardCoverageFilter("-[Microsoft.VisualBasic]*");

            return this;
        }

        /// <summary>
        /// Generates a filter based on a specified filter.
        /// </summary>
        /// <param name="filterText">The filter text to process.</param>
        /// <returns>The current builder.</returns>
        /// <remarks>
        /// Filter is of the format (+ or -)&lt;processFilter&gt;[assemblyFilter]classFilter, wildcards are allowed.
        /// </remarks>
        public FilterBuilder AddWildcardCoverageFilter(string filterText)
        {
            var (isInclusion, assemblyFilter, classFilter) = GetAssemblyClassName(filterText, false);

            assemblyFilter = ValidateAndEscape(assemblyFilter, @"\[]", "assembly");
            classFilter = ValidateAndEscape(classFilter, @"\[]", "class/type");

            ICoverageFilter coverageFilter = isInclusion ? (ICoverageFilter)new InclusionRegexCoverageFilter(assemblyFilter, classFilter) : new ExclusionRegexCoverageFilter(assemblyFilter, classFilter);
            _coverageFilters.Add(coverageFilter);

            return this;
        }

        /// <summary>
        /// Generates a filter based on a specified filter.
        /// </summary>
        /// <param name="filterText">The filter text to process.</param>
        /// <returns>The current builder.</returns>
        /// <remarks>
        /// Filter is of the format (+ or -)&lt;processFilter&gt;[assemblyFilter]classFilter, wildcards are allowed.
        /// </remarks>
        public FilterBuilder AddRegexCoverageFilter(string filterText)
        {
            var (isInclusion, assemblyFilter, classFilter) = GetAssemblyClassName(filterText, false);

            ICoverageFilter coverageFilter = isInclusion ? (ICoverageFilter)new InclusionRegexCoverageFilter(assemblyFilter, classFilter) : new ExclusionRegexCoverageFilter(assemblyFilter, classFilter);
            _coverageFilters.Add(coverageFilter);

            return this;
        }

        /// <summary>
        /// Generates a filter which will exclude a item based on a specified attribute.
        /// </summary>
        /// <param name="filterText">The text to exclude.</param>
        /// <returns>The current builder.</returns>
        public FilterBuilder AddWilcardAttributeExclusionFilter(string filterText)
        {
            var filter = new RegexItemFilter(ValidateAndEscape(filterText, "[]", "attribute"), true);

            _attributeExclusions.Add(filter);
            return this;
        }

        /// <summary>
        /// Generates a filter which will exclude a item based on a specified attribute.
        /// </summary>
        /// <param name="filterText">The text to exclude.</param>
        /// <returns>The current builder.</returns>
        public FilterBuilder AddRegexAttributeExclusionFilter(string filterText)
        {
            var filter = new RegexItemFilter(filterText, true);

            _attributeExclusions.Add(filter);
            return this;
        }

                /// <summary>
        /// Generates a filter which will exclude a item based on a specified attribute.
        /// </summary>
        /// <param name="filterText">The text to exclude.</param>
        /// <returns>The current builder.</returns>
        public FilterBuilder AddWilcardFileExclusionFilter(string filterText)
        {
            var filter = new RegexItemFilter(ValidateAndEscape(filterText, "[]", "file"), true);

            _fileExclusions.Add(filter);
            return this;
        }

        /// <summary>
        /// Generates a filter which will exclude a item based on a specified attribute.
        /// </summary>
        /// <param name="filterText">The text to exclude.</param>
        /// <returns>The current builder.</returns>
        public FilterBuilder AddRegexFileExclusionFilter(string filterText)
        {
            var filter = new RegexItemFilter(filterText, true);

            _fileExclusions.Add(filter);
            return this;
        }

        /// <summary>
        /// Produce the filter based on what we have built.
        /// </summary>
        /// <returns>The built filter.</returns>
        public Filter Build()
        {
            return new Filter(_attributeExclusions, _fileExclusions, _coverageFilters);
        }

        private static string ValidateAndEscape(string match, string notAllowed, string filterType)
        {
            if (match.IndexOfAny(notAllowed.ToCharArray()) >= 0)
            {
                throw new FilterException($"Unable to process the filter '{match}' for the {filterType}. Please check your syntax against the usage guide and try again.");
            }

            return match.Replace(@"\", @"\\").Replace(".", @"\.").Replace("*", ".*");
        }

        private static (bool isInclusion, string assemblyFilter, string classFilter) GetAssemblyClassName(string assemblyClassFilter, bool useRegEx)
        {
            var regEx = useRegEx ? regexAssemblyMatch : wildcardAssemblyMatch;

            var match = regEx.Match(assemblyClassFilter);
            if (match.Success)
            {
                var filterType = ParseIsInclusion(match.Groups["type"].Value);
                var assemblyFilter = match.Groups["assembly"].Value;
                var classFilter = match.Groups["class"].Value;

                if (string.IsNullOrWhiteSpace(assemblyFilter))
                {
                    throw new FilterException($"Unable to process the filter '{match}'. Please check your syntax against the usage guide and try again.");
                }

                return (filterType, assemblyFilter, classFilter);
            }

            throw new FilterException($"Unable to process the filter '{match}'. Please check your syntax against the usage guide and try again.");
        }

        /// <summary>
        /// Parse the filter into the specified type.
        /// </summary>
        /// <param name="type">The string value we want to convert.</param>
        /// <returns>The type of inclusion.</returns>
        private static bool ParseIsInclusion(string type)
        {
            switch (type)
            {
                case "+":
                    return true;
                case "-":
                    return false;
                default:
                    throw new ArgumentException("Unknown Filter Type: " + type);
            }
        }
    }
}
