# Parrot Code Input System

A comprehensive input framework for Unity providing cross-platform input abstraction, virtual keyboards, navigation systems, themed UI controls, and seamless integration with the Parrot Code Event System.

## Features

* ⌨️ Cross-platform virtual keyboard
* 🎮 Unified input abstraction
* 🖱️ Mouse, keyboard, touch, and gamepad support
* 📱 Mobile-friendly input utilities
* 🧭 UI navigation framework
* 🔘 Themed buttons and interactive controls
* 🎨 UI state management (Normal, Hover, Pressed, Disabled, Selected)
* 📢 Global input events via the Event Bus
* 📝 Input validation utilities
* ⚡ Runtime input device detection

## Installation

Install the package via the Unity Package Manager.

### Dependencies

* Parrot Code Native
* Parrot Code Extensions
* Parrot Code UI
* Parrot Code Event System

These dependencies will be resolved automatically by the Unity Package Manager.

## Usage

Import the required namespace:

```csharp
using ParrotCode.InputSystem;
```

Example:

```csharp
InputManager.InputSubmitted += OnInputSubmitted;

private void OnInputSubmitted(InputSubmittedEvent e)
{
    Debug.Log($"Input received: {e.Value}");
}
```

The package provides reusable components for building consistent user input experiences across desktop, mobile, console, and XR platforms.

## Planned Features

* Cross-platform virtual keyboard
* Input abstraction layer
* Keyboard and mouse helpers
* Touch gesture recognition
* Gamepad navigation
* UI navigation framework
* Focus management
* Themed buttons
* Text input controls
* Input validation
* Rebindable controls
* Input recording and playback
* Input simulation for testing
* Accessibility helpers
* Event Bus integration

## Compatibility

* Unity 2022.3 LTS or newer
* Unity Input System
* Unity UI Toolkit
* Unity uGUI

## License

Proprietary software.

Copyright (c) Sludgy Parrot (Pty) Ltd. All Rights Reserved.
