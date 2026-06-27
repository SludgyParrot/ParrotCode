# Parrot Code Helpers

A collection of reusable helper classes, builders, utility APIs, and development helpers that simplify common programming tasks in Unity applications.

## Features

* 🛠️ File and directory helpers
* 📄 JSON and serialization helpers
* 📅 Date and time utilities
* 🎲 Randomization helpers
* 🔢 Math and geometry helpers
* 🧮 Collection helpers
* 📦 Asset loading utilities
* 🧩 Reflection helpers
* 🧵 Threading and task helpers
* 🎯 Validation utilities
* ⚡ Common Unity helper methods
* 🏗️ Builder pattern utilities

## Installation

Install the package via the Unity Package Manager.

### Dependencies

* Parrot Code Native
* Parrot Code Extensions

These dependencies will be resolved automatically by the Unity Package Manager.

## Usage

Import the required namespace:

```csharp
using ParrotCode.Helpers;
```

Example:

```csharp
if(FileHelper.Exists(path))
{
    var json = FileHelper.ReadAllText(path);
}
```

The package provides reusable helper classes that encapsulate common operations, reducing duplicated code across Unity projects.

## Planned Features

* File helpers
* Serialization helpers
* Reflection helpers
* Random utilities
* Math helpers
* Color utilities
* String formatting helpers
* Asset helpers
* Coroutine helpers
* Task helpers
* Validation helpers
* Builder utilities
* Editor helper classes
* Runtime helper classes

## Compatibility

* Unity 2022.3 LTS or newer

## License

Proprietary software.

Copyright (c) Sludgy Parrot (Pty) Ltd. All Rights Reserved.
