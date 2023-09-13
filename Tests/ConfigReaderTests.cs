using System.Xml.Linq;
using Fody;
using Xunit;

public class ConfigReaderTests
{
    [Fact]
    public void ExcludeNamespacesNode()
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
        Assert.Equal("Foo", moduleWeaver.ExcludeNamespaces[0]);
        Assert.Equal("Bar", moduleWeaver.ExcludeNamespaces[1]);
        Assert.Equal("Foo.Bar", moduleWeaver.ExcludeNamespaces[2]);
    }

    [Fact]
    public void ExcludeNamespacesAttribute()
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
        Assert.Equal("Foo", moduleWeaver.ExcludeNamespaces[0]);
        Assert.Equal("Bar", moduleWeaver.ExcludeNamespaces[1]);
    }

    [Fact]
    public void ExcludeNamespacesCombined()
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
        Assert.Equal("Foo", moduleWeaver.ExcludeNamespaces[0]);
        Assert.Equal("Bar", moduleWeaver.ExcludeNamespaces[1]);
    }

    [Fact]
    public void IncludeNamespacesNode()
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
        Assert.Equal("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.Equal("Bar", moduleWeaver.IncludeNamespaces[1]);
        Assert.Equal("Foo.Bar", moduleWeaver.IncludeNamespaces[2]);
    }

    [Fact]
    public void IncludeNamespacesAttribute()
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
        Assert.Equal("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.Equal("Bar", moduleWeaver.IncludeNamespaces[1]);
    }

    [Fact]
    public void IncludeAndExcludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(
            """

            <Virtuosity IncludeNamespaces='Bar' ExcludeNamespaces='Foo'/>
            """);
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        var exception = Assert.Throws<WeavingException>(() => moduleWeaver.ReadConfig());
        Assert.Equal("Either configure IncludeNamespaces OR ExcludeNamespaces, not both.", exception.Message);
    }

    [Fact]
    public void IncludeNamespacesCombined()
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
        Assert.Equal("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.Equal("Bar", moduleWeaver.IncludeNamespaces[1]);
    }
}