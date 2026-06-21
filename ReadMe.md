![Logo](./Resources/images/Logo.png)
# Parrot Code

<p align="center">
  <strong>A modular framework for building scalable, maintainable, and cross-platform Unity applications.</strong>
</p>

<p align="center">
  Reusable systems • Platform abstractions • Editor tooling • Productivity focused
</p>

---

## Overview

Parrot Code is a collection of modular systems and tools designed to accelerate Unity development while promoting clean architecture, maintainability, and code reuse.

The framework provides independent modules that can be used together or individually, allowing developers to adopt only the functionality they need.

Whether you are building games, business applications, simulations, XR experiences, or WebGL applications, Parrot Code provides a solid foundation for development.

---

## Features

### Modular Architecture

Use individual modules without unnecessary dependencies.

### Cross-Platform Support

Build applications for:

* Windows
* macOS
* Linux
* Android
* iOS
* WebGL
* VR
* AR

### Productivity Focused

Reduce boilerplate code through reusable systems and editor tooling.

### Extensible

Designed to support custom implementations and project-specific workflows.

### Well Documented

Comprehensive API documentation and usage guides.

---

## Framework Modules

### Core

* Input System
* Event System
* User Interface
* Audio
* AI
* Multiplayer
* Extensions
* Helpers

### Platform Integrations

* Android
* iOS
* WebGL
* VR
* AR

### Tooling

* Editor Extensions
* Build Configuration Tools
* Platform Configuration Tools

### Native Integrations

* Native Code Utilities
* Platform Interop

---

## Architecture

```text
Parrot Code
│
├── AI
├── Audio
├── Event System
├── Input System
├── Multiplayer
├── User Interface
├── Extensions
├── Helpers
│
├── Android
├── iOS
├── WebGL
├── VR
├── AR
│
├── Native Code
└── Editor Tooling
```

---

## Installation

### Unity Package Manager

Add the package to your project:

```json
{
  "dependencies": {
    "com.sludgyparrot.parrotcode": "1.0.0"
  }
}
```

### Local Package

Clone or download the repository and add it through the Unity Package Manager.

---

## Quick Start

Example initialization:

```csharp
public class GameBootstrap : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Parrot Code Initialized");
    }
}
```

Refer to the documentation for module-specific setup instructions.

---

## Documentation

Documentation can be found in:

```text
Documentation~/
```

Or online:

```text
https://sludgyparrot.github.io/ParrotCode
```

### Recommended Reading

* Introduction
* Installation
* Input System
* Event System
* User Interface
* Platform Configuration
* API Reference

---

## Compatibility

| Unity Version   | Support     |
| --------------- | ----------- |
| 2021.3 LTS      | Supported   |
| 2022 LTS        | Supported   |
| 2023 LTS        | Supported   |
| Future Versions | Best Effort |

---

## Design Goals

Parrot Code is built around several key principles:

* Modularity
* Maintainability
* Performance
* Extensibility
* Platform Independence
* Developer Productivity

---

## Contributing

Contributions, bug reports, and feature suggestions are welcome.

Please submit issues and pull requests through the repository.

---

## Support

For licensing inquiries:

[licensing@sludgyparrot.com](mailto:licensing@sludgyparrot.com)

For bug reports and feature requests:

GitHub Issues

---

## License

Copyright (c) 2026 Sludgy Parrot (Pty) Ltd.

All Rights Reserved.

Parrot Code is proprietary commercial software developed and maintained by Sludgy Parrot (Pty) Ltd.

Unauthorized copying, modification, distribution, reverse engineering, decompilation, disclosure, or use of this software is prohibited unless expressly authorized.
