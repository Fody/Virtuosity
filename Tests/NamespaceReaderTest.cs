using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class NamespaceReaderTest
{
    [Test]
    public void GetLines()
    {
        var namespaces = ModuleWeaver.GetLines(new List<string> { "Namespace1","Namespace2" }).ToList();
        Assert.AreEqual("Namespace1", namespaces[0].Line);
        Assert.AreEqual("Namespace2", namespaces[1].Line);
    }


    [Test]
    public void BuildLineMatcherSimple()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("Namespace1");
        Assert.AreEqual("Namespace1", lineMatcher.Line);
        Assert.IsFalse(lineMatcher.StarStart);
        Assert.IsFalse(lineMatcher.StarEnd);
    }

    [Test]
    public void BuildLineMatcherStarStart()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("*Namespace1");
        Assert.AreEqual("Namespace1", lineMatcher.Line);
        Assert.IsTrue(lineMatcher.StarStart);
        Assert.IsFalse(lineMatcher.StarEnd);
    }

    [Test]
    public void BuildLineMatcherStarEnd()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("Namespace1*");
        Assert.AreEqual("Namespace1", lineMatcher.Line);
        Assert.IsFalse(lineMatcher.StarStart);
        Assert.IsTrue(lineMatcher.StarEnd);
    }

    [Test]
    public void BuildLineMatcherStarStartEnd()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("*Namespace1*");
        Assert.AreEqual("Namespace1", lineMatcher.Line);
        Assert.IsTrue(lineMatcher.StarStart);
        Assert.IsTrue(lineMatcher.StarEnd);
    }
}