// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace CoverDotNet.Core.Filters.ItemFilters
{
    /// <summary>
    /// A item filter which uses a regular expression.
    /// </summary>
    internal interface IRegexItemFilter : IItemFilter
    {
        /// <summary>
        /// Gets the filter used for the regular expression.
        /// </summary>
        string Filter { get; }
    }
}
