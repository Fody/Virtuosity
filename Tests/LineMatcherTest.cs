
public class LineMatcherTest
{
    [Test]
    public async Task Simple()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Namespace1"
                              };
        await Assert.That(lineMatcher.Match("Namespace1")).IsTrue();
        await Assert.That(lineMatcher.Match("Namespace2")).IsFalse();
    }

    [Test]
    public async Task StarStart()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Diagnostics",
                                  StarStart = true
                              };
        await Assert.That(lineMatcher.Match("System.Diagnostics")).IsTrue();
        await Assert.That(lineMatcher.Match("Diagnostics")).IsTrue();
        await Assert.That(lineMatcher.Match("NUnit.Framework")).IsFalse();
    }

    [Test]
    public async Task StarEnd()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "System",
                                  StarEnd = true
                              };
        await Assert.That(lineMatcher.Match("System.Diagnostics")).IsTrue();
        await Assert.That(lineMatcher.Match("System")).IsTrue();
        await Assert.That(lineMatcher.Match("NUnit.Framework")).IsFalse();
    }

    [Test]
    public async Task StarStartEnd()
    {
        var lineMatcher = new LineMatcher
                              {
                                  Line = "Diag",
                                  StarStart = true,
                                  StarEnd = true
                              };
        await Assert.That(lineMatcher.Match("System.Diagnostics")).IsTrue();
        await Assert.That(lineMatcher.Match("System.Diag")).IsTrue();
        await Assert.That(lineMatcher.Match("Diagnostics")).IsTrue();
        await Assert.That(lineMatcher.Match("NUnit.Framework")).IsFalse();
    }
}