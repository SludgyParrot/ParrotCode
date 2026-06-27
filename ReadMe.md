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

### Install via Unity Package Manager

1. Open the Unity Editor.
2. Navigate to **Edit → Project Settings → Package Manager**.
3. Add the Sludgy Parrot scoped registry:

```json
{
  "scopedRegistries": [
    {
      "name": "Sludgy Parrot",
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

## Quick Start

Example initialization:

```csharp
public class GameBootstrap : BaseMonoBehaviour
{
    protected override void Init()
    {
        Log("Parrot Code Initialized", LogVerbosity.Debug, LogChannel.Events);
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

| Unity Version | Support |
| -------------- | -------- |
| 2022.3 LTS     | ✅ Supported |
| 2023.2 LTS     | ✅ Supported |
| Unity 6 LTS    | ✅ Supported |
| Future Releases | 🟡 Best Effort |

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
