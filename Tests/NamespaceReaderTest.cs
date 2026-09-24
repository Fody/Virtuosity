using System.Linq;

public class NamespaceReaderTest
{
    [Test]
    public async Task GetLines()
    {
        var namespaces = ModuleWeaver.GetLines(
            [
                "Namespace1",
                "Namespace2"
            ])
            .ToList();
        await Assert.That(namespaces[0].Line).IsEqualTo("Namespace1");
        await Assert.That(namespaces[1].Line).IsEqualTo("Namespace2");
    }

    [Test]
    public async Task BuildLineMatcherSimple()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("Namespace1");
        await Assert.That(lineMatcher.Line).IsEqualTo("Namespace1");
        await Assert.That(lineMatcher.StarStart).IsFalse();
        await Assert.That(lineMatcher.StarEnd).IsFalse();
    }

    [Test]
    public async Task BuildLineMatcherStarStart()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("*Namespace1");
        await Assert.That(lineMatcher.Line).IsEqualTo("Namespace1");
        await Assert.That(lineMatcher.StarStart).IsTrue();
        await Assert.That(lineMatcher.StarEnd).IsFalse();
    }

    [Test]
    public async Task BuildLineMatcherStarEnd()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("Namespace1*");
        await Assert.That(lineMatcher.Line).IsEqualTo("Namespace1");
        await Assert.That(lineMatcher.StarStart).IsFalse();
        await Assert.That(lineMatcher.StarEnd).IsTrue();
    }

    [Test]
    public async Task BuildLineMatcherStarStartEnd()
    {
        var lineMatcher = ModuleWeaver.BuildLineMatcher("*Namespace1*");
        await Assert.That(lineMatcher.Line).IsEqualTo("Namespace1");
        await Assert.That(lineMatcher.StarStart).IsTrue();
        await Assert.That(lineMatcher.StarEnd).IsTrue();
    }
}