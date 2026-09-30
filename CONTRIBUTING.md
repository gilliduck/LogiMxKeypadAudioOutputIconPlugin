# Contributing to AudioOutput Loupedeck Plugin

Thank you for your interest in improving the AudioOutput plugin! Clear and well-structured contributions maximize software reliability, reduce maintainer overhead, and deliver direct utility to all users of Loupedeck and Razer Stream Controller hardware.

---

## Code of Conduct

All contributors and maintainers are expected to adhere to our [Code of Conduct](CODE_OF_CONDUCT.md). Please read it to ensure a respectful and collaborative development environment.

---

## Development Setup & Prerequisites

### Required Environment
- **Operating System**: Windows 10 or Windows 11 (64-bit).
- **.NET SDK**: [.NET 10.0 Windows SDK](https://dotnet.microsoft.com/download) or newer.
- **Loupedeck Software**: Loupedeck Software **6.0+** (Logi Plugin Service).
  - Default installation path: `C:\Program Files\Logi\LogiPluginService\` (supplies `PluginApi.dll`).

### Getting Started
1. **Fork and Clone**:
   ```powershell
   git clone https://github.com/<your-username>/AudioOutputPlugin.git
   cd AudioOutputPlugin
   ```
2. **Build the Solution**:
   ```powershell
   dotnet build src/AudioOutputPlugin.csproj -c Debug
   ```
   *Note: Building in Debug mode automatically generates the `.link` file in `%LocalAppData%\Logi\LogiPluginService\Plugins\` and triggers a live reload in Logi Plugin Service.*

---

## Codebase Conventions & Standards

To ensure code quality and consistency across the repository, please adhere to the following standards:

### 1. C# and .NET Standards
- **Target Framework**: `net10.0-windows` with C# 13 language features.
- **Nullable Reference Types**: Strictly enabled (`<Nullable>enable</Nullable>`). Avoid null-forgiving operators (`!`) unless justified with invariant guarantees.
- **File-Scoped Namespaces**: Always use file-scoped namespace declarations:
  ```csharp
  namespace Loupedeck.AudioOutputPlugin;
  ```
- **Explicit Framework Type Names**: Follow the existing codebase convention of using explicit type keywords/classes (e.g., `String`, `Boolean`, `Int32`, `Object`, `Exception`).
- **Member Qualification**: Use `this.` for accessing instance fields, properties, and methods.
- **Annotations**: Use `JetBrains.Annotations` attributes (such as `[UsedImplicitly]`) for plugin entry points and dynamic commands loaded via reflection.

### 2. Code Quality & Build Checks
- **Warnings as Errors**: The project enforces `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` under the `Debug` configuration. Ensure your code compiles with zero compiler warnings.
- **EditorConfig**: Adhere to the formatting rules defined in `src/.editorconfig`.

### 3. Thread Safety & Audio Architecture
- Never execute long-running operations or COM endpoint queries synchronously within OS callbacks (such as `IMMNotificationClient` notifications).
- Offload endpoint queries and state transitions to worker threads (`ThreadPool.QueueUserWorkItem`).
- Ensure thread-safe access to cached device strings via `volatile` or memory synchronization.

---

## Workflow & Git Practices

### Branching Strategy
- Create focused feature or bugfix branches based on `main`:
  - `feature/<short-description>`: For new capabilities or architectural enhancements.
  - `fix/<short-description>`: For bug fixes and regression resolutions.
  - `docs/<short-description>`: For documentation additions or updates.

### Commit Messages
Write clear, imperative commit messages:
```
feat: add fallback device name resolution when multimedia role is empty
fix: handle CoreAudioException during Bluetooth disconnect
docs: update troubleshooting guide with log file locations
```

---

## Submitting a Pull Request

1. **Verify Builds**: Run `dotnet build src/AudioOutputPlugin.csproj -c Debug` and verify zero errors/warnings.
2. **Hardware/Runtime Testing**: Test your changes against a physical or simulated Loupedeck device running Logi Plugin Service. Confirm that endpoint switches update LCD button states without UI lag.
3. **Open Pull Request**:
   - Provide a clear summary of the changes and motivation.
   - Reference any related GitHub issue numbers (`Fixes #123`).
   - Fill out the provided [Pull Request Template](.github/PULL_REQUEST_TEMPLATE.md).
