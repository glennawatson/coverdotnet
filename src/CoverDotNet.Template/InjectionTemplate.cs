// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace CoverDotNet.Template
{
    /// <summary>
    /// This is a template of what gets injected for each class inside the classes we are targetting.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class InjectionTemplate
    {
        private static ConcurrentDictionary<int, int> hitDictionary = new ConcurrentDictionary<int, int>();

        public static void RecordHit(int hitLocationIndex)
        {

        }
    }
}
