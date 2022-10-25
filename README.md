# Big Book of Data Types

[![Build status](https://ci.appveyor.com/api/projects/status/435koyg8vsdq3ii6?svg=true)](https://ci.appveyor.com/project/JaCraig/bigbookofdatatypes)

Big Book of Data Types is a set of classes and extension methods to help with data. This includes classes for caching, data comparison, data conversion, data mapping, string formatting, as well as various data types that are missing from .Net.

## Setting Up the Library

Big Book of Data Types relies on [Canister](https://github.com/JaCraig/Canister) in order to hook itself up. In order for this to work, you must do the following at startup:

    services.AddCanisterModules();
					
The RegisterBigBookOfDataTypes function is an extension method that registers it with the IoC container. When this is done, the library is ready to use.

## Basic usage

Most of the library is simply data types that can be used fairly easily. These include:

1. Bag
2. BinaryTree
3. DateSpan
4. Fraction
5. ListMapping
6. Matrix
7. ObservableList
8. PriorityQueue
9. RingBuffer
10. Set
11. Table
12. TagDictionary
13. TaskQueue
14. Vector3

Similarly the extension methods for various types can be found by adding:

    using BigBook;
	
To your list of usings. From there a number of extension methods should be available for arrays, IEnumerable, string, ConcurrentBag, ConcurrentDictionary, DateTime, Exception, ICollection, IComparable, IDictionary, MatchCollection, Process, Stream, TimeSpan, etc. There are a couple hundred extension methods and as such you should just take a look at them to see what they do. The portion of the code that might be of some interest and yet not completely intuitive is the Dynamo class.

Dynamo is a true dynamic type for .Net. ExpandoObject is generally great for basic work that requires a dynamic, however it is not easy to convert to other data types. For instance you can't do this:

    dynamic MyDynamicValue=new ExpandoObject();
	SomeClass FinalObject=MyDynamicValue;
	
Dynamo, on the other hand, has no issues with this:

    dynamic MyDynamicValue=new Dynamo();
	SomeClass FinalObject=MyDynamicValue;
	
The class handles conversion to and from class types, can convert properties from one type to another, and comes with a set of built in functionality. The class implements INotifyPropertyChanged, has a built in change log, and is thread safe. It can also be added as a base class for other classes to add this functionality automatically.

## Installation

The library is available via Nuget with the package name "BigBook". To install it run the following command in the Package Manager Console:

Install-Package BigBook

## Build Process

In order to build the library you will require the following:

1. Visual Studio 2019

Other than that, just clone the project and you should be able to load the solution and build without too much effort.
