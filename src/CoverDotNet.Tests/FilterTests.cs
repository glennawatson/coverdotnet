using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoverDotNet.Core.Filters;
using CoverDotNet.Core.Filters.CoverageFilters;
using Xunit;

namespace CoverDotNet.Tests
{
    /// <summary>
    /// Tests associated with filters.
    /// </summary>
    public class FilterTests
    {
        /// <summary>
        /// Tests to make sure that invalid assembly class pairs are correctly firing exceptions.
        /// </summary>
        /// <param name="assemblyClassPair">The pair to test.</param>
        [Theory]
        [InlineData("Garbage")]
        [InlineData("+[]")]
        [InlineData("-[ ]")]
        [InlineData("[ ")]
        [InlineData(" ]")]
        [InlineData("+[]]")]
        [InlineData("-[][")]
        [InlineData(@"-[\]")]
        [InlineData(@"+[X]\")]
        [InlineData("-[X]]")]
        [InlineData("+[X][")]
        [InlineData("-<[*]*")]
        [InlineData("+>[*]*")]
        [InlineData("+<>[*]*")]
        [InlineData("-[*]")]
        [InlineData("-[]*")]
        [InlineData("-<*>[*]")]
        [InlineData("-<*>[]*")]
        [InlineData("-[\u00a0]*")]
        public void AddFilterThrowsExceptionWhenInvalidAssemblyClassPair(string assemblyClassPair)
        {
            FilterBuilder filterBuilder = new FilterBuilder();
            Assert.Throws<FilterException>(() => filterBuilder.AddWildcardCoverageFilter(assemblyClassPair));
        }

        /// <summary>
        /// Tests to make sure that valid assembly class pairs are treated properly.
        /// </summary>
        /// <param name="filterExpression">The expression we are evaluating.</param>
        /// <param name="assemblyResult">The assembly portion we are evaluating.</param>
        /// <param name="classResult">The class result we are evaluating.</param>
        /// <param name="isInclusion">If this filter type is for inclusion.</param>
        [Theory]
        [InlineData("+[My App]Namespace", "My App", "Namespace", true)]
        [InlineData("-[System.*]Console", @"System\..*", "Console", false)]
        [InlineData("+[System]Console.*", "System", @"Console\..*", true)]
        [InlineData("-[System.*]Console.*", @"System\..*", @"Console\..*", false)]
        public void AddFilterAddsValidAssemblyClassPairWorks(string filterExpression, string assemblyResult, string classResult, bool isInclusion)
        {
            FilterBuilder filterBuilder = new FilterBuilder();

            filterBuilder.AddWildcardCoverageFilter(filterExpression);

            var filter = filterBuilder.Build();

            var numberInclusionFilters = filter.CoverageFilters.OfType<IRegexCoverageFilter>().Where(x => x.IsInclusive).Count();
            var numberExclusionFilters = filter.CoverageFilters.OfType<IRegexCoverageFilter>().Where(x => !x.IsInclusive).Count();

            Assert.Equal(1, isInclusion ? numberInclusionFilters : numberExclusionFilters);

            var coverageFilter = filter.CoverageFilters.OfType<IRegexCoverageFilter>().First();

            Assert.Equal(assemblyResult, coverageFilter.AssemblyFilter);
            Assert.Equal(classResult, coverageFilter.ClassFilter);
        }

        /// <summary>
        /// Tests to makes sure different combinations of assembly pairs match the expected result.
        /// </summary>
        /// <param name="filters">The filters to test.</param>
        /// <param name="assembly">The assembly to match.</param>
        /// <param name="expectedResult">If we expect a result or not.</param>
        [Theory]
        [InlineData(new string[0], "System.Debug", false)]
        [InlineData(new string[] { "-[System.*]R*" }, "System.Debug", true)]
        [InlineData(new string[] { "-[System.*]*" }, "System.Debug", false)]
        [InlineData(new string[] { "+[System.*]*" }, "System.Debug", true)]
        [InlineData(new string[] { "-[mscorlib]*", "-[System.*]*", "+[*]*" }, "mscorlib", false)]
        [InlineData(new string[] { "-[System.*]*", "+[*]*" }, "mscorlib", true)]
        [InlineData(new string[] { "+[XYZ]*" }, "XYZ", true)]
        [InlineData(new string[] { "+[XYZA]*" }, "XYZ", false)]

        public void AddFilterAssemblyNameWorks(string[] filters, string assembly, bool expectedResult)
        {
            FilterBuilder filterBuilder = new FilterBuilder();

            foreach (var filterExpression in filters)
            {
                filterBuilder.AddWildcardCoverageFilter(filterExpression);
            }

            var filter = filterBuilder.Build();

            var shouldUseAssembly = filter.ShouldCoverAssembly(assembly);

            Assert.Equal(expectedResult, shouldUseAssembly);
        }
    }
}
