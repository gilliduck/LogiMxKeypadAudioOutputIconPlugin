// ReSharper disable once CheckNamespace
namespace Loupedeck.AudioOutputPlugin;

using NAudio.CoreAudioApi;

internal sealed class WindowsAudioOutputMonitor : IDisposable
{
    private readonly MMDeviceNotificationClient _notificationClient;
    private readonly MMDeviceEnumerator _notificationEnumerator;

    private volatile String _currentOutputName;
    private volatile Boolean _disposed;

    public WindowsAudioOutputMonitor()
    {
        // Read the current output immediately so the keypad has the
        // correct value as soon as the action is loaded.
        this._currentOutputName = ReadCurrentOutputName();

        // This enumerator is kept alive for the lifetime of the monitor
        // because the notification client depends on it.
        this._notificationEnumerator = new MMDeviceEnumerator();

        // Do not depend on the Logi Plugin Service having a
        // SynchronizationContext. Notifications are received directly
        // from Windows, and we move the actual endpoint query onto the
        // thread pool below.
        this._notificationClient =
            this._notificationEnumerator.CreateNotificationClient(
                false);

        this._notificationClient.DefaultDeviceChanged +=
            this.OnDefaultDeviceChanged;
    }

    public String CurrentOutputName => this._currentOutputName;

    public void Dispose()
    {
        if (this._disposed)
        {
            return;
        }

        this._disposed = true;

        this._notificationClient.DefaultDeviceChanged -=
            this.OnDefaultDeviceChanged;

        this._notificationClient.Dispose();
        this._notificationEnumerator.Dispose();
    }

    public event Action<String> OutputChanged;

    private void OnDefaultDeviceChanged(
        Object sender,
        DefaultDeviceChangedEventArgs e)
    {
        if (this._disposed)
        {
            return;
        }

        // NAudio warns against doing additional audio-stack work directly
        // inside the Windows audio notification callback.
        //
        // Queue the read instead so this callback returns immediately.
        ThreadPool.QueueUserWorkItem(_ => this.RefreshCurrentOutput());
    }

    private void RefreshCurrentOutput()
    {
        if (this._disposed)
        {
            return;
        }

        try
        {
            var outputName = ReadCurrentOutputName();

            if (this._disposed)
            {
                return;
            }

            if (String.Equals(
                    this._currentOutputName,
                    outputName,
                    StringComparison.Ordinal))
            {
                return;
            }

            this._currentOutputName = outputName;

            this.OutputChanged?.Invoke(outputName);
        }
        catch (Exception ex)
        {
            PluginLog.Warning(
                ex,
                "Unexpected error while reading the current audio output device.");
        }
    }

    private static String ReadCurrentOutputName()
    {
        try
        {
            using var enumerator = new MMDeviceEnumerator();

            if (!enumerator.TryGetDefaultAudioEndpoint(
                    DataFlow.Render,
                    Role.Multimedia,
                    out var device))
            {
                return "No audio output";
            }

            using (device)
            {
                return GetDisplayName(device.FriendlyName);
            }
        }
        catch (CoreAudioException)
        {
            // Windows may temporarily have no usable default endpoint,
            // for example while unplugging one device and switching to
            // another.
            return "No audio output";
        }
    }

    private static String GetDisplayName(String friendlyName)
    {
        if (String.IsNullOrWhiteSpace(friendlyName))
        {
            return "Unknown output";
        }

        var parenthesisIndex = friendlyName.IndexOf(
            " (",
            StringComparison.Ordinal);

        return parenthesisIndex > 0
            ? friendlyName[..parenthesisIndex].Trim()
            : friendlyName.Trim();
    }
}