// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Text;
using Polly;

namespace CoverDotNet.Core.Helpers
{
    /// <summary>
    /// Helper methods with handling file moving.
    /// </summary>
    internal static class FileMoverHelper
    {
        /// <summary>
        /// Temporarily backup a file to a location. When the returned <see cref="IDisposable"/> is disposed it will restore the file.
        /// This will handle performing retry operations if it can't immediately copy the file.
        /// </summary>
        /// <param name="filePath">The path to the file to backup.</param>
        /// <returns>A disposable which will restore the file when disposed.</returns>
        public static IDisposable TemporaryBackupFile(string filePath)
        {
            var temporayPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(filePath) + "_" + Guid.NewGuid() + ".dll");

            File.Copy(filePath, temporayPath, true);
            return Disposable.Create(() =>
            {
                var policy = Policy
                  .Handle<Exception>()
                  .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(0.5, retryAttempt)));

                policy.Execute(() =>
                {
                    File.Copy(temporayPath, filePath, true);
                    File.Delete(temporayPath);
                });
            });
        }
    }
}
