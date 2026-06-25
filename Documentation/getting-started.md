![Logo](../Resources/images/Logo.png)
# Parrot Code

A modular framework for building scalable, maintainable and cross-platform Unity applications.

![Unity](https://img.shields.io/badge/Unity-2021.3+-black)
![License](https://img.shields.io/badge/License-Commercial-purple)
![Status](https://img.shields.io/badge/Status-Active-blue)

# Getting Started

Welcome to **Parrot Code**.

This guide will help you install the framework and get your project ready for development.

---

# Prerequisites

Before using Parrot Code, ensure you have:

* Unity 2021.3 LTS or newer
* Basic knowledge of Unity and C#
* A project configured for your target platform

---

# Installation

## Available Packages

Browse the Parrot Code package collection on npm:

* Parrot Code Native
* Parrot Code Extensions
* Parrot Code Shared Editor Utilities
* Parrot Code Runtime Platform Builder

Additional packages and documentation will be added over time.

## Install via Unity Package Manager

1. Open the Unity Editor.
2. Navigate to **Edit → Project Settings → Package Manager**.
3. Add the Parrot Code scoped registry:

```json
{
  "scopedRegistries": [
    {
      "name": "Parrot Code",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.sludgyparrot"
      ]
    }
  ]
}
```

4. Open **Window → Package Manager**.
5. Select **My Registries** from the package source dropdown.
6. Install the desired Parrot Code package.

Once installed, Unity will automatically resolve and import all required package dependencies.

---


# Verify Installation

After importing the package, verify that the following assemblies are available:

```text
ParrotCode
ParrotCode.Editor
```

Additional platform-specific assemblies may also be available depending on the installed modules:

```text
ParrotCode.Android
ParrotCode.iOS
ParrotCode.WebGL
ParrotCode.VR
ParrotCode.AR
```

---

# Configure the Project

Many Parrot Code modules include project configuration utilities.

Open:

```text
Tools
└── Parrot Code
```

and apply the recommended project settings for your target platform.

> Some modules may require additional setup depending on the platform being used.

---

# Your First Script

Create a new MonoBehaviour:

```csharp
using UnityEngine;

public class ExampleBehaviour : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Parrot Code is ready.");
    }
}
```

Attach the component to a GameObject and enter Play Mode.

If the project runs successfully, the framework has been installed correctly.

---

# Understanding the Framework

Parrot Code is divided into independent modules.

## Core Systems

These modules provide common functionality used throughout applications.

* Input System
* Event System
* User Interface
* Audio
* AI
* Multiplayer

## Platform Modules

These modules provide platform-specific functionality.

* Android
* iOS
* WebGL
* VR
* AR

## Tooling

Editor tooling is available to simplify common development workflows.

* Build configuration
* Platform configuration
* Project validation
* Development utilities

---

# Recommended Learning Path

If you are new to Parrot Code, read the following guides in order:

1. Introduction
2. Architecture Overview
3. Event System
4. Input System
5. User Interface
6. Platform Configuration
7. API Reference

---

# Example Project Structure

A typical project may look like:

```text
Assets
│
├── Scripts
├── Art
├── Audio
├── Prefabs
├── Scenes
│
└── Settings
```

Parrot Code does not enforce a specific project structure, but maintaining a consistent organization is recommended.

---

# Documentation

Additional documentation can be found throughout the Documentation section.

Topics include:

* Architecture
* Input System
* Event System
* User Interface
* Audio
* Multiplayer
* Platform Support
* API Reference

---

# Next Steps

Continue with:

* Architecture Overview
* Event System
* Input System

These guides explain the core concepts used throughout the framework.
