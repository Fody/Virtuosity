using NUnit.Framework;

[TestFixture]
public class LineMatcherTest
{
    [Test]
    public void Simple()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Namespace1"
                              };
        Assert.IsTrue(lineMatcher.Match("Namespace1"));
        Assert.IsFalse(lineMatcher.Match("Namespace2"));
    }

    [Test]
    public void StarStart()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Diagnostics",
                                  StarStart = true
                              };
        Assert.IsTrue(lineMatcher.Match("System.Diagnostics"));
        Assert.IsTrue(lineMatcher.Match("Diagnostics"));
        Assert.IsFalse(lineMatcher.Match("NUnit.Framework"));
    }

    [Test]
    public void StarEnd()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "System",
                                  StarEnd = true
                              };
        Assert.IsTrue(lineMatcher.Match("System.Diagnostics"));
        Assert.IsTrue(lineMatcher.Match("System"));
        Assert.IsFalse(lineMatcher.Match("NUnit.Framework"));
    }
    
    [Test]
    public void StarStartEnd()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Diag",
                                  StarStart = true,
                                  StarEnd = true
                              };
        Assert.IsTrue(lineMatcher.Match("System.Diagnostics"));
        Assert.IsTrue(lineMatcher.Match("System.Diag"));
        Assert.IsTrue(lineMatcher.Match("Diagnostics"));
        Assert.IsFalse(lineMatcher.Match("NUnit.Framework"));
    }
}