// ReSharper disable CheckNamespace

namespace Loupedeck.AudioOutputPlugin;

using JetBrains.Annotations;

[UsedImplicitly]
public sealed class CurrentAudioOutputCommand : PluginDynamicCommand
{
    private WindowsAudioOutputMonitor? _monitor;
    private volatile String _outputName = "Loading...";

    public CurrentAudioOutputCommand()
        : base(
            "Current Audio Output",
            "Displays the current default Windows audio output device.",
            "Audio")
    {
    }

    protected override Boolean OnLoad()
    {
        try
        {
            this._monitor = new WindowsAudioOutputMonitor();
            this._outputName = this._monitor.CurrentOutputName;

            this._monitor.OutputChanged += this.OnOutputChanged;

            this.ActionImageChanged(null);

            PluginLog.Info(
                $"Audio output monitor started. Current output: {this._outputName}");
        }
        catch (Exception ex)
        {
            this._outputName = "Audio unavailable";

            PluginLog.Error(
                ex,
                "Unable to start the Windows audio output monitor.");
        }

        // We still load the action if audio initialization failed so that
        // the keypad can display the error state rather than silently
        // removing the action.
        return true;
    }

    protected override Boolean OnUnload()
    {
        if (this._monitor is not null)
        {
            this._monitor.OutputChanged -= this.OnOutputChanged;
            this._monitor.Dispose();
            this._monitor = null;
        }

        return true;
    }

    private void OnOutputChanged(String outputName)
    {
        this._outputName = outputName;

        PluginLog.Info(
            $"Audio output changed to: {outputName}");

        // Tell Logi Plugin Service that any visible buttons using this
        // action need to be rendered again.
        this.ActionImageChanged(null);
    }

    protected override BitmapImage GetCommandImage(
        String actionParameter,
        PluginImageSize imageSize)
    {
        using var bitmapBuilder = new BitmapBuilder(imageSize);

        var displayName = WrapDisplayName(this._outputName);

        bitmapBuilder.DrawText(displayName);

        return bitmapBuilder.ToImage();
    }

    private static String WrapDisplayName(String text)
    {
        const Int32 wrapThreshold = 12;

        if (String.IsNullOrWhiteSpace(text) ||
            text.Length <= wrapThreshold ||
            !text.Contains(' '))
        {
            return text;
        }

        var midpoint = text.Length / 2;

        var bestSpaceIndex = -1;
        var bestDistance = Int32.MaxValue;

        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] != ' ')
            {
                continue;
            }

            var distance = Math.Abs(i - midpoint);

            if (distance < bestDistance)
            {
                bestSpaceIndex = i;
                bestDistance = distance;
            }
        }

        if (bestSpaceIndex <= 0 ||
            bestSpaceIndex >= text.Length - 1)
        {
            return text;
        }

        return String.Concat(
            text[..bestSpaceIndex].Trim(),
            Environment.NewLine,
            text[(bestSpaceIndex + 1)..].Trim());
    }
}