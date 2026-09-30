# Changelog

All notable changes to the **AudioOutput Loupedeck Plugin** will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

---

## [1.0.0] - 2026-03-30

### Added
- **`WindowsAudioOutputMonitor`**: Real-time Windows default playback endpoint tracking via NAudio CoreAudio (WASAPI) and `MMDeviceEnumerator`.
- **`CurrentAudioOutputCommand`**: Dynamic plugin command (`PluginDynamicCommand`) displaying active output name on Loupedeck LCD buttons.
- **Asynchronous Endpoint Resolution**: Worker thread pooling (`ThreadPool.QueueUserWorkItem`) to prevent blocking audio notification callbacks.
- **Intelligent Text Wrapping**: Multi-line midpoint label splitting algorithm designed for touch key dimensions.
- **Fallback Status Handling**: Robust state display handling for missing endpoints, uninitialized states, and transient device disconnects.
- **Build and Deployment Automation**: MSBuild targets for link file creation (`.link`) and automatic plugin hot-reloading (`loupedeck:plugin/AudioOutput/reload`).
- **Hardware Optimization**: Target support for the `LoupedeckCtFamily` (Loupedeck CT, Live, Live S, Razer Stream Controller, Razer Stream Controller X).
- **Metadata Configuration**: Loupedeck package configuration metadata (`LoupedeckPackage.yaml`) with MIT licensing.
