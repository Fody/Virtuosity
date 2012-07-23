## This is an add-in for [Fody](https://github.com/SimonCropp/Fody/) 

Change all members to `virtual` as part of your build.

[Introduction to Fody](http://github.com/SimonCropp/Fody/wiki/SampleUsage)

## Nuget package http://nuget.org/packages/Virtuosity.Fody 

What it actually does to your assembly

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

[Why Is This Useful](https://github.com/SimonCropp/Virtuosity/wiki/WhyIsThisUseful)

