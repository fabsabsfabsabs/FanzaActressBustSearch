using FanzaActressSearch.ViewModel.SearchText;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActressGetterTest
{
    [TestClass]
    public class SearchAnalyzeTest
    {
        [TestMethod]
        public void SearchAnalyze()
        {
            Assert.AreEqual(SearchTextExtention.Analysis("99"), new SearchTextResult(SearchTextResultType.Bust, "99", 99, "", ComparisonOperator.Equal));
            Assert.AreEqual(SearchTextExtention.Analysis("99+"), new SearchTextResult(SearchTextResultType.Bust, "99+", 99, "", ComparisonOperator.Plus));
            Assert.AreEqual(SearchTextExtention.Analysis("99"), new SearchTextResult(SearchTextResultType.Bust, "99", 99, "", ComparisonOperator.Equal));

            Assert.AreEqual(SearchTextExtention.Analysis("b99"), new SearchTextResult(SearchTextResultType.Bust, "99", 99, "", ComparisonOperator.Equal));
            Assert.AreEqual(SearchTextExtention.Analysis("B99+"), new SearchTextResult(SearchTextResultType.Bust, "99+", 99, "", ComparisonOperator.Plus));
            Assert.AreEqual(SearchTextExtention.Analysis("‚a99"), new SearchTextResult(SearchTextResultType.Bust, "99", 99, "", ComparisonOperator.Equal));

            Assert.AreEqual(SearchTextExtention.Analysis("C"), new SearchTextResult(SearchTextResultType.Cup, "C", 0, "C", ComparisonOperator.Equal));
            Assert.AreEqual(SearchTextExtention.Analysis("D+"), new SearchTextResult(SearchTextResultType.Cup, "D+", 0, "D", ComparisonOperator.Plus));

            Assert.AreEqual(SearchTextExtention.Analysis("‚ "), new SearchTextResult(SearchTextResultType.Text, "‚ ", 0, "", ComparisonOperator.Equal));
            Assert.AreEqual(SearchTextExtention.Analysis(""), new SearchTextResult(SearchTextResultType.Bust, "100+", 100, "", ComparisonOperator.Plus));
        }
    }
}
