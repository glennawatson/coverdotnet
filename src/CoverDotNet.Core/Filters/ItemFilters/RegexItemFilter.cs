// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CoverDotNet.Core.Filters.ItemFilters
{
    /// <summary>
    /// A filter that achieves its result by using Regex matching.
    /// </summary>
    internal class RegexItemFilter : IRegexItemFilter
    {
        private readonly Lazy<Regex> _regularExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexItemFilter"/> class.
        /// </summary>
        /// <param name="filterExpression">The regex to match against.</param>
        /// <param name="shouldWrapFilterExpression">If we should wrap the regular expression to match a entire string.</param>
        public RegexItemFilter(string filterExpression, bool shouldWrapFilterExpression)
        {
            Filter = filterExpression;
            _regularExpression = new Lazy<Regex>(() => new Regex(shouldWrapFilterExpression ? WrapWithAnchors(filterExpression) : filterExpression));
        }

        /// <inheritdoc />
        public string Filter { get; }

        /// <inheritdoc />
        public bool IsMatch(string name)
        {
            return _regularExpression.Value.IsMatch(name);
        }

        private static string WrapWithAnchors(string filter)
        {
            return '^' + filter + '$';
        }
    }
}
