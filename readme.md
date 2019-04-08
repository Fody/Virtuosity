[![Chat on Gitter](https://img.shields.io/gitter/room/fody/fody.svg?style=flat&max-age=86400)](https://gitter.im/Fody/Fody)
[![NuGet Status](http://img.shields.io/nuget/v/Virtuosity.Fody.svg?style=flat&max-age=86400)](https://www.nuget.org/packages/Virtuosity.Fody/)


## This is an add-in for [Fody](https://github.com/Fody/Home/)

![Icon](https://raw.githubusercontent.com/Fody/Virtuosity/master/package_icon.png)

Change all members to `virtual` as part of your build.


## Usage

See also [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md).


### NuGet installation

Install the [Virtuosity.Fody NuGet package](https://nuget.org/packages/Virtuosity.Fody/) and update the [Fody NuGet package](https://nuget.org/packages/Fody/):

```powershell
PM> Install-Package Fody
PM> Install-Package Virtuosity.Fody
```

The `Install-Package Fody` is required since NuGet always defaults to the oldest, and most buggy, version of any dependency.


### Add to FodyWeavers.xml

Add `<Virtuosity/>` to [FodyWeavers.xml](https://github.com/Fody/Home/blob/master/pages/usage.md#add-fodyweaversxml)

```xml
<Weavers>
  <Virtuosity/>
</Weavers>
```


## What it actually does to your assembly


### Selects all members that meet the following criteria

 * from non `sealed` classes
 * non `static` members
 * non `abstract` members
 * non `private` members
 * non `virtual` members


### Change them to `virtual`


### For all (now `virtual`) members

 * change calls to those members to `virtual`
 * change `new` modifiers to `override` modifiers


# Configuration Options


## Exclude types with an Attribute

To exclude a specific class you can mark it with a `DoNotVirtualizeAttribute`.

Add the below class to your assembly. Namespace does not matter.

```
public class DoNotVirtualizeAttribute : Attribute
{
}
```

The class will look like this

```
[DoNotVirtualize]
public class ClassToSkip
{
    ...
}
```


## Include or exclude namespaces

These config options are access by modifying the `Virtuosity` node in FodyWeavers.xml


### ExcludeNamespaces

A list of namespaces to exclude.

Can not be defined with `IncludeNamespaces`.

Can take two forms. 

As an element with items delimited by a newline.

```xml
<Virtuosity>
    <ExcludeNamespaces>
        Foo
        Bar
    </ExcludeNamespaces>
</Virtuosity>
```

Or as a attribute with items delimited by a pipe `|`.

```xml
<Virtuosity ExcludeNamespaces='Foo|Bar'/>
```


## IncludeNamespaces

A list of namespaces to include.

Can not be defined with `ExcludeNamespaces`.

Can take two forms.

As an element with items delimited by a newline.

```xml
<Virtuosity>
    <IncludeNamespaces>
        Foo
        Bar
    </IncludeNamespaces>
</Virtuosity>
```

Or as a attribute with items delimited by a pipe `|`.

```xml
<Virtuosity IncludeNamespaces='Foo|Bar'/>
```


# Why is this useful

When using one of the following tools

 * [NHibernate](https://nhibernate.info/)
 * [Rhino Mocks](https://hibernatingrhinos.com/oss/rhino-mocks)
 * [Moq](https://github.com/moq/moq)
 * [Ninject](http://www.ninject.org/)
 * [NSubstitute](https://nsubstitute.github.io/)
 * [Entity Framework](https://docs.microsoft.com/en-us/ef/)

All these tools make use of [DynamicProxy](http://www.castleproject.org/projects/dynamicproxy/). DynamicProxy allows for runtime interception of members. The one caveat is that all intercepted members must be virtual. This means that that all the above tools, to some extent, require members to be virtual.

 * [EF: Lazy Loading](https://docs.microsoft.com/en-us/ef/ef6/querying/related-data#lazy-loading) "When using POCO entity types, lazy loading is achieved by creating instances of derived proxy types and then overriding virtual properties to add the loading hook"
 * [Must Everything Be Virtual With NHibernate?](http://davybrion.com/blog/2009/03/must-everything-be-virtual-with-nhibernate/)
 * [RhinoMocks Why methods need to be declared virtual](http://groups.google.com/group/RhinoMocks/browse_thread/thread/a2cb93f1ba8d4735/37d377ddb92cb729?lnk=gst&q=virtual)
 * [Moq My method needs to be virtual?](http://groups.google.com/group/moqdisc/browse_thread/thread/2e02e367d017f274)
 * ["NMock supports mocking of classes with virtual methods"](http://www.nmock.org/nmock1-documentation.html)
 * [Ninject Important Note:Your methods/properties to be intercepted must be virtual!](http://innovatian.com/2010/03/using-ninject-extensions-interception-part-1-the-basics/)
 * [NSubstitute:Some operations on non virtual members should throw an exception](http://groups.google.com/group/nsubstitute/browse_thread/thread/407cb0408ce97bfd)

When a member is not virtual these tools will not work and fail in sometimes very unhelpful ways. So rather than having to remember to use the virtual keyword Virtuosity means members will be virtual by default.


## Icon

<a href="http://thenounproject.com/noun/russian-doll/#icon-No1964" target="_blank">Russian Doll</a> designed by <a href="http://thenounproject.com/Simon Child" target="_blank">Simon Child</a> from The Noun Project