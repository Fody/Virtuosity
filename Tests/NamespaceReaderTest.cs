using System.Linq;
using Xunit;

public class NamespaceReaderTest
{
    [Fact]
    public void GetLines()
    {
        var namespaces = ModuleWeaver.GetLines(
                new()
                {
                    "Namespace1",
                    "Namespace2"
                })
            .ToList();
        Assert.Equal("Namespace1", namespaces[0].Line);
        Assert.Equal("Namespace2", namespaces[1].Line);
    }

    [Fact]
    public void BuildLineMatcherSimple()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("Namespace1");
        Assert.Equal("Namespace1", lineMatcher.Line);
        Assert.False(lineMatcher.StarStart);
        Assert.False(lineMatcher.StarEnd);
    }

    [Fact]
    public void BuildLineMatcherStarStart()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("*Namespace1");
        Assert.Equal("Namespace1", lineMatcher.Line);
        Assert.True(lineMatcher.StarStart);
        Assert.False(lineMatcher.StarEnd);
    }

    [Fact]
    public void BuildLineMatcherStarEnd()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("Namespace1*");
        Assert.Equal("Namespace1", lineMatcher.Line);
        Assert.False(lineMatcher.StarStart);
        Assert.True(lineMatcher.StarEnd);
    }

    [Fact]
    public void BuildLineMatcherStarStartEnd()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("*Namespace1*");
        Assert.Equal("Namespace1", lineMatcher.Line);
        Assert.True(lineMatcher.StarStart);
        Assert.True(lineMatcher.StarEnd);
    }
}