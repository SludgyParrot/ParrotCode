# Introduction

![Logo](images/Logo.png)

Welcome to **Parrot Code**, a modular framework designed to accelerate Unity application and game development through reusable systems, platform abstractions, and editor tooling.

Parrot Code provides a collection of independent modules that can be used together or individually depending on the needs of your project.

---

## Overview

The framework focuses on:

- Modular architecture
- Cross-platform development
- Reduced boilerplate code
- Editor productivity tools
- Reusable gameplay and application systems
- Consistent APIs across modules

Whether you are building a game, simulation, business application, VR experience, or WebGL application, Parrot Code aims to provide a solid foundation that scales with your project.

---

## Core Modules

### Input System

<xref:ParrotCode.InputSystem.InputManager>

Provides input abstraction and action-based input handling.

Features:

- Action schemes
- Input event binding
- Device-independent controls
- Runtime input management

---

### Event System

<xref:ParrotCode.EventSystem>

Provides a lightweight event-driven architecture for communication between systems.

Features:

- Decoupled messaging
- Event dispatching
- Subscriber management

---

### User Interface

<xref:ParrotCode.UserInterface>

Provides reusable UI components and workflows.

Features:

- UI management
- Navigation systems
- Reusable controls
- State-driven interfaces

---

### Audio

<xref:ParrotCode.Audio>

Tools and utilities for managing audio playback.

Features:

- Audio management
- Sound effects
- Music control
- Audio utilities

---

### AI

<xref:ParrotCode.AI>

Contains systems and utilities for implementing artificial intelligence behaviors.

---

### Multiplayer

<xref:ParrotCode.Multiplayer>

Networking and multiplayer-related abstractions.

---

### Platform Services

Provides platform-specific implementations through dedicated modules:

- ParrotCodeAndroid
- ParrotCodeiOS
- ParrotCodeWebGL
- ParrotCodeVR
- ParrotCodeAR

---

### Native Code

<xref:ParrotCode.Native.Common>

Provides native platform integration and interop functionality.

---

## Architecture

High-level module layout:

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
├── Native Code
│
├── Android
├── iOS
├── WebGL
├── VR
└── AR
```

Each module is designed to be loosely coupled and independently maintainable.

---

## Design Principles

### Modularity

Modules should be usable without requiring unrelated systems.

### Extensibility

Systems are designed to be extended through inheritance, composition, and custom implementations.

### Performance

Runtime allocations and unnecessary dependencies are minimized whenever practical.

### Maintainability

Consistent naming conventions and API design help reduce project complexity.

---

## Supported Platforms

| Platform | Support |
|----------|----------|
| Windows | ✓ |
| macOS | ✓ |
| Linux | ✓ |
| Android | ✓ |
| iOS | ✓ |
| WebGL | ✓ |
| VR | ✓ |
| AR | ✓ |

> Actual platform support depends on the modules used.

---

## Getting Started

If you are new to Parrot Code, start with:

1. [Installation](installation.md)
2. [Quick Start](quick-start.md)
3. [Input System](input-system.md)
4. [Event System](event-system.md)
5. [User Interface](user-interface.md)

---

## API Documentation

For complete API documentation, see:

- API Reference
- Module Documentation
- Code Examples

Many pages throughout this documentation use API cross-references such as:

```md
<xref:ParrotCode.InputSystem.InputManager>
```

which automatically link to generated API pages when using DocFX.

---

## License

Parrot Code is licensed under the terms specified in the project's LICENSE file.

---

## Support

For bug reports, feature requests, or questions:

- GitHub Issues
- Project Discussions
- Contact: licensing@sludgyparrot.com
