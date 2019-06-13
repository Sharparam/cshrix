# Contributing guidelines

Thank you for your interest in contributing to [cshrix][]!

This document describes guidelines you need to follow when developing code for
or otherwise contributing to this project. Please read through this document
in its entirety before contributing.

If you have any questions, do not hesitate to [contact me][sharparam] or open
an issue in this repository with your question.

You can also join [#cshrix:sharparam.com][room] on Matrix to discuss
potential changes.

## Code of Conduct

This repository is covered by a [Code of Conduct][coc].

If you notice anyone breaking the CoC or have questions about it, please
contact [@Sharparam][sharparam] or send an email to
[cshrix@sharparam.com](mailto:cshrix@sharparam.com).

## Reporting issues or requesting features

If you want to report an issue, bug, or request a new feature to be added,
please [open a new issue][new-issue] and describe your bug or feature request.

There are different issue templates defined depending on what you are reporting.

We kindly ask that you do not "fire and forget" issues. If you report an issue
then please remember to come back and respond to any questions or feedback
given by us!

## Code style

This project defines a code style compatible with both ReSharper and Rider,
please make use of them if you are in a supported environment. The project
also has an EditorConfig file supplied, defining some basic formatting rules.

Below follow some basic style guidelines but they may not be exhaustive due
to the immense number of rules defined in the settings. For a definitive
list of what style rules are required, consult the EditorConfig and solution
settings files.

### File header

Each file should begin with the following header text:

```
Copyright (c) 2019 by Adam Hellberg.

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.
```

Additionally, for C# source code files, wrap the notice inside a copyright
tag:

```csharp
// <copyright file="MyFile.cs">
//   Copyright (c) 2019 by Adam Hellberg.
//
//   This Source Code Form is subject to the terms of the Mozilla Public
//   License, v. 2.0. If a copy of the MPL was not distributed with this
//   file, You can obtain one at http://mozilla.org/MPL/2.0/.
// </copyright>
```

The ReSharper/Rider settings include a file template which inserts this text
automatically in new files created by them.

### Indentation

Indentation is done only with space, unless tabs are required by the file
format. The number of spaces to indent with is by default 4, but can be
different in certain file types, as shown below.

 * Indentation of C# source files is done with 4 spaces.
 * Indentation of XML files (including CSPROJ files) is done with 2 spaces.
 * Indentation of YAML and JSON files is done with 2 spaces.
 * For other files, consult the EditorConfig files.

### EOL whitespace

All end-of-line (EOL) whitespace is to be stripped from all files.

If a file format explicitly requires EOL whitespace for some functionality,
exceptions can be made and will be listed in the EditorConfig file.

### Naming rules

 * **Private non-readonly fields:** `_lowerCamelCase`
 * **Parameters:** `lowerCamelCase`
 * **Local variables:** `lowerCamelCase`
 * **Interfaces:** `IUpperCamelCase`
 * **Type parameters:** `TUpperCamelCase`
 * **Everything else:** `UpperCamelCase` (AKA `PascalCase`)

The above naming rules are defined in the ReSharper/Rider settings.

In addition, `async` methods must be namd with an `Async` suffix.
For example: `async Task DoSomethingAsync()`.

Names of type parameters should either be a singular `T`, or a `T` followed
by a PascalCased name, like `TError`.

### Braces

Brace layout is to put braces on the next line for all constructs that use
braces, for example:

```csharp
namespace Test
{
    public class Foo
    {
    }
}
```

For short inline stuff like arrays, braces on one line can be accepted,
for example:

```csharp
var mySimpleArray = new int[] { 1, 2, 3, 4, 5 };
```

### File layout

Follow the same file layout as mandated by StyleCop ([SA1201][] and [SA1203][]).

#### Using statements

`using` statements are placed as deep as possible, usually this means placing
them just inside the namespace definition.

They should also be alphabetically ordered and grouped based on the top-level
name, with the exception that the `System` namespace is always the first group.

Fully qualified using statements are not required when they can be shortened.

For example:

```csharp
namespace Test
{
    using System;
    using System.Net;

    using Cshrix;

    using Newtonsoft.Json;

    public class Foo
    {
    }
}
```

### Miscellaneous

 * Use `var` where possible.

[cshrix-bot]: https://github.com/Sharparam/cshrix
[room]: https://matrix.to/#/!FWdevnnPQnzpNpaUJf:sharparam.com?via=sharparam.com
[new-issue]: https://github.com/Sharparam/cshrix/issues/new
[coc]: CODE_OF_CONDUCT.md
[sharparam]: https://github.com/Sharparam

[SA1201]: http://stylecop.soyuz5.com/SA1201.html
[SA1203]: http://stylecop.soyuz5.com/SA1203.html
