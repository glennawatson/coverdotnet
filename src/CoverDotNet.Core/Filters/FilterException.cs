// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace CoverDotNet.Core.Filters
{
    /// <summary>
    /// A exception that has occurred when there is a issue with a filter.
    /// </summary>
    public class FilterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterException"/> class.
        /// </summary>
        public FilterException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterException"/> class.
        /// </summary>
        /// <param name="message">The message about the exception.</param>
        public FilterException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterException"/> class.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public FilterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
