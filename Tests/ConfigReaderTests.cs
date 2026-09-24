using System.Xml.Linq;
using Fody;

public class ConfigReaderTests
{
    [Test]
    public async Task ExcludeNamespacesNode()
    {
        var xElement = XElement.Parse(
            """

            <Virtuosity>
                <ExcludeNamespaces>
            Foo
            Bar
            Foo.Bar
                </ExcludeNamespaces>
            </Virtuosity>
            """);
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        await Assert.That(moduleWeaver.ExcludeNamespaces[0]).IsEqualTo("Foo");
        await Assert.That(moduleWeaver.ExcludeNamespaces[1]).IsEqualTo("Bar");
        await Assert.That(moduleWeaver.ExcludeNamespaces[2]).IsEqualTo("Foo.Bar");
    }

    [Test]
    public async Task ExcludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(
            """

            <Virtuosity ExcludeNamespaces='Foo|Bar'/>
            """);
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        await Assert.That(moduleWeaver.ExcludeNamespaces[0]).IsEqualTo("Foo");
        await Assert.That(moduleWeaver.ExcludeNamespaces[1]).IsEqualTo("Bar");
    }

    [Test]
    public async Task ExcludeNamespacesCombined()
    {
        var xElement = XElement.Parse(
            """

            <Virtuosity  ExcludeNamespaces='Foo'>
                <ExcludeNamespaces>
            Bar
                </ExcludeNamespaces>
            </Virtuosity>
            """);
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        await Assert.That(moduleWeaver.ExcludeNamespaces[0]).IsEqualTo("Foo");
        await Assert.That(moduleWeaver.ExcludeNamespaces[1]).IsEqualTo("Bar");
    }

    [Test]
    public async Task IncludeNamespacesNode()
    {
        var xElement = XElement.Parse(
            """

            <Virtuosity>
                <IncludeNamespaces>
            Foo
            Bar
            Foo.Bar
                </IncludeNamespaces>
            </Virtuosity>
            """);
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        await Assert.That(moduleWeaver.IncludeNamespaces[0]).IsEqualTo("Foo");
        await Assert.That(moduleWeaver.IncludeNamespaces[1]).IsEqualTo("Bar");
        await Assert.That(moduleWeaver.IncludeNamespaces[2]).IsEqualTo("Foo.Bar");
    }

    [Test]
    public async Task IncludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(
            """

            <Virtuosity IncludeNamespaces='Foo|Bar'/>
            """);
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        await Assert.That(moduleWeaver.IncludeNamespaces[0]).IsEqualTo("Foo");
        await Assert.That(moduleWeaver.IncludeNamespaces[1]).IsEqualTo("Bar");
    }

    [Test]
    public async Task IncludeAndExcludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(
            """

            <Virtuosity IncludeNamespaces='Bar' ExcludeNamespaces='Foo'/>
            """);
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        var exception = await Assert.That(() => moduleWeaver.ReadConfig()).Throws<WeavingException>();
        await Assert.That(exception!.Message).IsEqualTo("Either configure IncludeNamespaces OR ExcludeNamespaces, not both.");
    }

    [Test]
    public async Task IncludeNamespacesCombined()
    {
        var xElement = XElement.Parse(
            """

            <Virtuosity  IncludeNamespaces='Foo'>
                <IncludeNamespaces>
            Bar
                </IncludeNamespaces>
            </Virtuosity>
            """);
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        await Assert.That(moduleWeaver.IncludeNamespaces[0]).IsEqualTo("Foo");
        await Assert.That(moduleWeaver.IncludeNamespaces[1]).IsEqualTo("Bar");
    }
}