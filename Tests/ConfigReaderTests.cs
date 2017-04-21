using System.Xml.Linq;
using NUnit.Framework;

[TestFixture]
public class ConfigReaderTests
{

    [Test]
    public void ExcludeNamespacesNode()
    {
        var xElement = XElement.Parse(@"
<Virtuosity>
    <ExcludeNamespaces>
Foo
Bar
Foo.Bar
    </ExcludeNamespaces>
</Virtuosity>");
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.ExcludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.ExcludeNamespaces[1]);
        Assert.AreEqual("Foo.Bar", moduleWeaver.ExcludeNamespaces[2]);
    }

    [Test]
    public void ExcludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(@"
<Virtuosity ExcludeNamespaces='Foo|Bar'/>");
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.ExcludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.ExcludeNamespaces[1]);
    }

    [Test]
    public void ExcludeNamespacesCombined()
    {
        var xElement = XElement.Parse(@"
<Virtuosity  ExcludeNamespaces='Foo'>
    <ExcludeNamespaces>
Bar
    </ExcludeNamespaces>
</Virtuosity>");
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.ExcludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.ExcludeNamespaces[1]);
    }

    [Test]
    public void IncludeNamespacesNode()
    {
        var xElement = XElement.Parse(@"
<Virtuosity>
    <IncludeNamespaces>
Foo
Bar
Foo.Bar
    </IncludeNamespaces>
</Virtuosity>");
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.IncludeNamespaces[1]);
        Assert.AreEqual("Foo.Bar", moduleWeaver.IncludeNamespaces[2]);
    }

    [Test]
    public void IncludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(@"
<Virtuosity IncludeNamespaces='Foo|Bar'/>");
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.IncludeNamespaces[1]);
    }

    [Test]
    public void IncludeAndExcludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(@"
<Virtuosity IncludeNamespaces='Bar' ExcludeNamespaces='Foo'/>");
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        var exception = Assert.Throws<WeavingException>(() => moduleWeaver.ReadConfig());
        Assert.AreEqual("Either configure IncludeNamespaces OR ExcludeNamespaces, not both.", exception.Message);
    }

    [Test]
    public void IncludeNamespacesCombined()
    {
        var xElement = XElement.Parse(@"
<Virtuosity  IncludeNamespaces='Foo'>
    <IncludeNamespaces>
Bar
    </IncludeNamespaces>
</Virtuosity>");
        var moduleWeaver = new ModuleWeaver
        {
            Config = xElement
        };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.IncludeNamespaces[1]);
    }

}