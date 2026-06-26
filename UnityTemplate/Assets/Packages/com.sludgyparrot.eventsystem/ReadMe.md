# Parrot Code Event System

A lightweight event-driven framework providing an Event Bus, messaging utilities, and publish-subscribe patterns for building decoupled Unity applications.

## Features

* 📢 Global Event Bus
* 📬 Publish-subscribe messaging
* 🎯 Strongly typed events
* 🔗 Event channels
* ⚡ High-performance event dispatching
* 🧩 Decoupled system communication
* 🛠️ Event debugging utilities
* 🔄 Runtime event registration
* 🧹 Automatic listener cleanup
* 📦 Lightweight runtime implementation

## Installation

Install the package via the Unity Package Manager.

### Dependencies

* Parrot Code Native
* Parrot Code Extensions

These dependencies will be resolved automatically by the Unity Package Manager.

## Usage

Import the required namespace into your scripts:

```csharp
using ParrotCode.EventSystem;
```

Example:

```csharp
EventBus.Subscribe<PlayerSpawnedEvent>(OnPlayerSpawned);

EventBus.Publish(new PlayerSpawnedEvent(player));

EventBus.Unsubscribe<PlayerSpawnedEvent>(OnPlayerSpawned);
```

The package simplifies communication between independent systems without requiring direct references, making applications easier to maintain and extend.

## Planned Features

* Global Event Bus
* Scoped event buses
* Event channels
* Sticky events
* Event priorities
* Asynchronous events
* Event filtering
* Event recording and playback
* Debug visualizer
* Inspector integration

## Compatibility

* Unity 2022.3 LTS or newer

## License

Proprietary software.

Copyright (c) Sludgy Parrot (Pty) Ltd. All Rights Reserved.
