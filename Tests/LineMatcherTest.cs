using Xunit;

public class LineMatcherTest
{
    [Fact]
    public void Simple()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Namespace1"
                              };
        Assert.True(lineMatcher.Match("Namespace1"));
        Assert.False(lineMatcher.Match("Namespace2"));
    }

    [Fact]
    public void StarStart()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Diagnostics",
                                  StarStart = true
                              };
        Assert.True(lineMatcher.Match("System.Diagnostics"));
        Assert.True(lineMatcher.Match("Diagnostics"));
        Assert.False(lineMatcher.Match("NUnit.Framework"));
    }

    [Fact]
    public void StarEnd()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "System",
                                  StarEnd = true
                              };
        Assert.True(lineMatcher.Match("System.Diagnostics"));
        Assert.True(lineMatcher.Match("System"));
        Assert.False(lineMatcher.Match("NUnit.Framework"));
    }

    [Fact]
    public void StarStartEnd()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Diag",
                                  StarStart = true,
                                  StarEnd = true
                              };
        Assert.True(lineMatcher.Match("System.Diagnostics"));
        Assert.True(lineMatcher.Match("System.Diag"));
        Assert.True(lineMatcher.Match("Diagnostics"));
        Assert.False(lineMatcher.Match("NUnit.Framework"));
    }
}