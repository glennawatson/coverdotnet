// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace CoverDotNet.Core.Filters.ItemFilters
{
    /// <summary>
    /// A filter to exclude modules from the the coverage.
    /// </summary>
    internal interface IItemFilter
    {
        /// <summary>
        /// Determines if the name should be included.
        /// </summary>
        /// <param name="name">The name of the item we are comparing.</param>
        /// <returns>If we should include the item.</returns>
        bool IsMatch(string name);
    }
}
