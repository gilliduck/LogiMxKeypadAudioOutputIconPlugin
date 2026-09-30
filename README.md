# AudioOutput Loupedeck Plugin

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Target Framework](https://img.shields.io/badge/.NET-10.0--windows-purple.svg)](https://dotnet.microsoft.com/)
[![Loupedeck Version](https://img.shields.io/badge/Loupedeck%20Software-6.0%2B-green.svg)](https://loupedeck.com/)

A lightweight, real-time default Windows audio output device monitor and display plugin for Loupedeck and Razer Stream Controller consoles running the Logitech / Loupedeck Plugin Service.

---

## Overview

**AudioOutput** provides a dynamic key action for your Loupedeck workspace that continuously displays the name of the currently active default Windows multimedia playback device. 

When your audio endpoint changes (e.g., plugging in headphones, switching to speakers, or connecting a Bluetooth headset), the button label updates immediately without polling lag, user intervention, or manual refreshes.

### Key Features
- **Real-Time Endpoint Tracking**: Utilizes Windows CoreAudio (WASAPI) notifications via `NAudio.Wasapi` to receive instant system events on default audio device switches.
- **Asynchronous, Non-Blocking Execution**: Device endpoint resolutions are offloaded to background thread pool workers, preventing any UI stutter or latency on the console.
- **Intelligent Text Wrapping**: Formats device names cleanly across multiple lines to ensure high legibility on LCD keys.
- **Graceful Error States**: Renders descriptive fallback status messages (`Loading...`, `No audio output`, `Audio unavailable`) rather than silently breaking.
- **Instant Developer Reload**: Configured with automated build targets that regenerate the Loupedeck plugin link and trigger live reloads via the `loupedeck:plugin/AudioOutput/reload` URI scheme.

---

## Supported Hardware

The plugin is optimized for the **`LoupedeckCtFamily`** hardware ecosystem:
- Loupedeck CT
- Loupedeck Live
- Loupedeck Live S
- Razer Stream Controller
- Razer Stream Controller X

---

## Prerequisites

Ensure your development or host system meets the following requirements:
- **Operating System**: Windows 10 or Windows 11 (CoreAudio endpoint monitoring requires Windows OS).
- **Loupedeck / Logi Software**: Loupedeck Software **6.0+** (Logi Plugin Service) installed at `C:\Program Files\Logi\LogiPluginService\`.
- **.NET SDK**: [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (supporting `net10.0-windows`).

---

## Installation & Local Development

### 1. Clone Repository
```powershell
git clone https://github.com/myusername/AudioOutputPlugin.git
cd AudioOutputPlugin
```

### 2. Build and Deploy Link
Run a Debug or Release build using the .NET CLI:
```powershell
dotnet build src/AudioOutputPlugin.csproj -c Debug
```

#### What Happens During Build:
1. Compiles the plugin binaries into `bin/Debug/bin/`.
2. Automatically copies package metadata and icon assets from `src/package/` to `bin/Debug/`.
3. Creates/updates the plugin link file at `%LocalAppData%\Logi\LogiPluginService\Plugins\AudioOutputPlugin.link`.
4. Sends a reload command (`loupedeck:plugin/AudioOutput/reload`) to the running Loupedeck Plugin Service to refresh your active plugin instance instantly.

### 3. Debugging in IDEs
- **JetBrains Rider**: Open `AudioOutputPlugin.sln`. Attach to the `LogiPluginService.exe` process (or configure a .NET Executable Run Configuration pointing to LogiPluginService) to hit breakpoints in `CurrentAudioOutputCommand` or `WindowsAudioOutputMonitor`.
- **Visual Studio 2022 / 2026**: Open `AudioOutputPlugin.sln`, set build configuration to `Debug`, build, and attach to `LogiPluginService.exe`.

---

## Architecture & How It Works

```
AudioOutputPlugin/
├── src/
│   ├── Audio/
│   │   ├── CurrentAudioOutputCommand.cs   # PluginDynamicCommand rendering the active device label
│   │   └── WindowsAudioOutputMonitor.cs   # CoreAudio / MMDeviceEnumerator event listener
│   ├── Helpers/                           # Logging and resource helper utilities
│   ├── package/                           # Icons and LoupedeckPackage.yaml metadata
│   ├── AudioOutputPlugin.cs               # Plugin lifecycle root entry point
│   ├── AudioOutputApplication.cs          # Client application definition
│   └── AudioOutputPlugin.csproj           # Build targets and link automation
└── AudioOutputPlugin.sln                  # Solution definition
```

1. **`WindowsAudioOutputMonitor`**: Instantiates `MMDeviceEnumerator` and listens for `DefaultDeviceChanged` events using `IMMNotificationClient`. On change, it delegates endpoint resolution to a worker thread via `ThreadPool.QueueUserWorkItem` to prevent blocking the OS audio callback.
2. **`CurrentAudioOutputCommand`**: Inherits from `PluginDynamicCommand`, registers to the output monitor, and invokes `ActionImageChanged(null)` to trigger redraws on all visible action tiles when audio configuration changes.
3. **`WrapDisplayName`**: Measures string midpoint and applies balanced word-wrapping across LCD touch buttons.

---

## Troubleshooting

- **Plugin does not appear in Loupedeck software**:
  - Verify that `%LocalAppData%\Logi\LogiPluginService\Plugins\AudioOutputPlugin.link` exists and points to the full path of your `bin\Debug\` directory.
  - Restart the Logi Plugin Service via Windows Services or task manager.
- **Button shows "Audio unavailable" or "No audio output"**:
  - Check Windows Sound Settings to ensure at least one active default playback device is connected and enabled.
  - Inspect the Loupedeck plugin logs in `%LocalAppData%\Logi\LogiPluginService\Logs\` for exception stack traces.
- **Compilation error missing `PluginApi.dll`**:
  - Confirm Loupedeck Software is installed in `C:\Program Files\Logi\LogiPluginService\`. If installed elsewhere, override `<PluginApiDir>` in `src/AudioOutputPlugin.csproj`.

---

## License

This project is licensed under the [MIT License](LICENSE).
