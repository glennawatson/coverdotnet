// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Mono.Cecil;

namespace CoverDotNet.Core.Model
{
    /// <summary>
    /// Contains information about a module we are tracking.
    /// </summary>
    [DataContract]
    public class ModuleData
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        [DataMember]
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the path to the module.
        /// </summary>
        [DataMember]
        public string ModulePath { get; set; }

        /// <summary>
        /// Gets or sets the hash of the module.
        /// </summary>
        [DataMember]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the last time accessed.
        /// </summary>
        [DataMember]
        public DateTime ModuleTime { get; set; }

        /// <summary>
        /// Gets or sets a internal assembly definition.
        /// </summary>
        internal AssemblyDefinition AssemblyDefinition { get; set; }
    }
}
