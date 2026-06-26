# Parrot Code UI

A comprehensive UI framework for Unity providing reusable controls, theming, layout utilities, animation helpers, virtual input components, and TextMeshPro integration.

## Features

* 🎨 Theme management system
* 🔘 Custom buttons and interactive controls
* 📝 TextMeshPro-based text components
* 🖼️ Advanced image display components
* ⌨️ Input fields and validation
* 📱 Cross-platform virtual keyboard support
* 🧭 UI navigation utilities
* 📐 Responsive layout helpers
* ✨ UI animation and transition helpers
* 📊 Progress bars and sliders
* 📋 Lists and scroll view utilities
* 🪟 Modal dialogs and popups
* 🔔 Toasts and notifications
* ⚡ Runtime UI utilities

## Installation

Install the package via the Unity Package Manager.

### Dependencies

* Parrot Code Native
* Parrot Code Extensions
* Unity TextMeshPro
* Unity Input System

These dependencies will be resolved automatically by the Unity Package Manager.

## Usage

Import the required namespace:

```csharp
using ParrotCode.UI;
```

Example:

```csharp
PrimaryButton button = GetComponent<PrimaryButton>();

button.Clicked += OnButtonClicked;

button.SetTheme(AppTheme.Dark);
```

The package provides reusable UI components designed to reduce repetitive development while maintaining a consistent appearance and behavior across Unity projects.

## Planned Features

* Theme manager
* Button framework
* Text components
* Image components
* Input fields
* Dropdown controls
* Toggle controls
* Tabs
* Accordions
* Progress indicators
* Loading spinners
* Dialog framework
* Notification system
* Responsive layouts
* UI animations
* Localization-ready components
* Accessibility support
* UI Toolkit interoperability

## Compatibility

* Unity 2022.3 LTS or newer
* Unity UI (uGUI)
* Unity Input System
* TextMeshPro

## License

Proprietary software.

Copyright (c) Sludgy Parrot (Pty) Ltd. All Rights Reserved.
