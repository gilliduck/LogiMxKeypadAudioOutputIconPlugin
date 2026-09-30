# Support & Troubleshooting Guide

Welcome to the **AudioOutput Loupedeck Plugin** support center. This document offers quick solutions to common issues and outlines how to report defects or request features.

---

## Frequently Encountered Issues

### 1. The plugin does not appear in Loupedeck / Logi software
- **Check the Plugin Link File**: Verify that `%LocalAppData%\Logi\LogiPluginService\Plugins\AudioOutputPlugin.link` exists and contains the correct absolute path to your plugin's build output folder (e.g. `C:\dev\AudioOutputPlugin\bin\Debug\`).
- **Restart Loupedeck / Logi Plugin Service**:
  - Right-click the Loupedeck icon in the Windows taskbar system tray and choose **Restart**, or
  - Open Windows Task Manager, end the `LogiPluginService.exe` process, and relaunch Loupedeck.
- **Check Software Version**: Ensure you are running Loupedeck Software version **6.0 or later**.

### 2. The LCD key shows "Audio unavailable" or "No audio output"
- **"No audio output"**: Indicates that Windows currently reports no active default multimedia playback device (e.g., all audio devices are disabled or unplugged). Check Windows Sound Settings.
- **"Audio unavailable"**: An unhandled COM or WASAPI exception occurred during initialization. Check the log files (see below) to see if an endpoint lock or audio service interruption occurred.

### 3. Locating Log Files
When diagnosing issues or preparing a bug report, review the plugin logs:
- **Loupedeck / Logi Service Logs**: Located in `%LocalAppData%\Logi\LogiPluginService\Logs\`
- Search log files for lines prefixed with `[AudioOutput]` or containing exceptions logged by `PluginLog`.

---

## How to Get Help

- **Bug Reports**: If you encounter unexpected behavior or errors, please submit a [Bug Report](https://github.com/myusername/AudioOutputPlugin/issues/new?template=bug_report.md).
- **Feature Requests**: If you have an idea for extending hardware support, actions, or visual customizations, open a [Feature Request](https://github.com/myusername/AudioOutputPlugin/issues/new?template=feature_request.md).
- **Discussions**: For general questions, setup inquiries, or configuration tips, participate in GitHub Discussions.
