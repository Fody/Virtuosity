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
    </ExcludeNamespaces>
</Virtuosity>");
        var moduleWeaver = new ModuleWeaver { Config = xElement };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.ExcludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.ExcludeNamespaces[1]);
    }

    [Test]
    public void ExcludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(@"
<Virtuosity ExcludeNamespaces='Foo|Bar'/>");
        var moduleWeaver = new ModuleWeaver { Config = xElement };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.ExcludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.ExcludeNamespaces[1]);
    }

    [Test]
    public void ExcludeNamespacesConbined()
    {
        var xElement = XElement.Parse(@"
<Virtuosity  ExcludeNamespaces='Foo'>
    <ExcludeNamespaces>
Bar
    </ExcludeNamespaces>
</Virtuosity>");
        var moduleWeaver = new ModuleWeaver { Config = xElement };
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
    </IncludeNamespaces>
</Virtuosity>");
        var moduleWeaver = new ModuleWeaver { Config = xElement };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.IncludeNamespaces[1]);
    }

    [Test]
    public void IncludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(@"
<Virtuosity IncludeNamespaces='Foo|Bar'/>");
        var moduleWeaver = new ModuleWeaver { Config = xElement };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.IncludeNamespaces[1]);
    }

    [Test]
    [ExpectedException(ExpectedMessage = "Either configure IncludeNamespaces OR ExcludeNamespaces, not both.")]
    public void IncludeAndExcludeNamespacesAttribute()
    {
        var xElement = XElement.Parse(@"
<Virtuosity IncludeNamespaces='Bar' ExcludeNamespaces='Foo'/>");
        var moduleWeaver = new ModuleWeaver { Config = xElement };
        moduleWeaver.ReadConfig();
    }

    [Test]
    public void IncludeNamespacesConbined()
    {
        var xElement = XElement.Parse(@"
<Virtuosity  IncludeNamespaces='Foo'>
    <IncludeNamespaces>
Bar
    </IncludeNamespaces>
</Virtuosity>");
        var moduleWeaver = new ModuleWeaver { Config = xElement };
        moduleWeaver.ReadConfig();
        Assert.AreEqual("Foo", moduleWeaver.IncludeNamespaces[0]);
        Assert.AreEqual("Bar", moduleWeaver.IncludeNamespaces[1]);
    }

}