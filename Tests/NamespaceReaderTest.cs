using System.Linq;
using NUnit.Framework;

[TestFixture]
public class NamespaceReaderTest
{
    [Test]
    public void GetLines()
    {
        var namespaces = InclusionChecker.GetLines(" Namespace1 \r\n Namespace2 ").ToList();
        Assert.AreEqual("Namespace1", namespaces[0].Line);
        Assert.AreEqual("Namespace2", namespaces[1].Line);
    }


    [Test]
    public void BuildLineMatcherSimple()
    {
        var lineMatcher = InclusionChecker.BuildLineMatcher("Namespace1");
        Assert.AreEqual("Namespace1", lineMatcher.Line);
        Assert.IsFalse(lineMatcher.StarStart);
        Assert.IsFalse(lineMatcher.StarEnd);
    }

    [Test]
    public void BuildLineMatcherStarStart()
    {
        var lineMatcher = InclusionChecker.BuildLineMatcher("*Namespace1");
        Assert.AreEqual("Namespace1", lineMatcher.Line);
        Assert.IsTrue(lineMatcher.StarStart);
        Assert.IsFalse(lineMatcher.StarEnd);
    }

    [Test]
    public void BuildLineMatcherStarEnd()
    {
        var lineMatcher = InclusionChecker.BuildLineMatcher("Namespace1*");
        Assert.AreEqual("Namespace1", lineMatcher.Line);
        Assert.IsFalse(lineMatcher.StarStart);
        Assert.IsTrue(lineMatcher.StarEnd);
    }

    [Test]
    public void BuildLineMatcherStarStartEnd()
    {
        var lineMatcher = InclusionChecker.BuildLineMatcher("*Namespace1*");
        Assert.AreEqual("Namespace1", lineMatcher.Line);
        Assert.IsTrue(lineMatcher.StarStart);
        Assert.IsTrue(lineMatcher.StarEnd);
    }
}