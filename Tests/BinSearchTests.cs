using Core.Utils;
using NUnit.Framework;
using System.Collections;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }
        public class MyFactoryClass
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(
                        new string[] { "a", "aa", "aab", "ab", "abc", "abce" },
                        "abc")
                    .Returns(4);
                    yield return new TestCaseData(
                        new string[] { "a", "aa", "aab", "ab",  "abce" },
                        "abc")
                    .Returns(4);
                    yield return new TestCaseData(
                        new string[] { "a", "aa", "aab", "ab", "abd", "abde" },
                        "abc")
                    .Returns(4);
                    yield return new TestCaseData(
                        new string[] { "ab", "abca", "abcd", "abcde", "abce" },
                        "abc")
                    .Returns(1);
                    yield return new TestCaseData(
                        new string[] {  "abca", "abcd", "abcde", "abce" },
                        "abc")
                    .Returns(0);
                    yield return new TestCaseData(
                        new string[] { "acd", "ace", "ae", "cba" },
                        "abc")
                    .Returns(0);
                    yield return new TestCaseData(
                       new string[] { "ab", "abc", "abcde", "abce" },
                       "abc")
                   .Returns(1);
                    yield return new TestCaseData(
                       new string[] {  "abc", "abcde", "abce" },
                       "abc")
                   .Returns(0);
                    yield return new TestCaseData(
                       new string[] { "a", "abc", "abcd" },
                       "abc")
                   .Returns(1);
                    yield return new TestCaseData(
                       new string[] { "abc" },
                       "abc")
                   .Returns(0);
                    yield return new TestCaseData(
                       new string[] {"a", "ab","abe","abeg" },
                       "abc")
                   .Returns(2);
                    yield return new TestCaseData(
                       new string[] { "abe" },
                       "abc")
                   .Returns(0);
                    yield return new TestCaseData(
                       new string[] {  },
                       "abc")
                   .Returns(-1);
                }
            }
        }
        [Test, TestCaseSource(typeof(MyFactoryClass), "TestCases")]
        public int BinSearch_ShouldFindFirstGreatOrEqual(string[] source, string prefix)
        {
            return BinSearch(source, prefix);
        }

        public int BinSearch(string[] source, string prefix)
        {
            return Search.IndexOfFirstGreatOrEqual(source, x => x, prefix);
        }
    }
}