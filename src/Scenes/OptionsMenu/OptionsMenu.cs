using Godot;
using System;

public partial class OptionsMenu : Control {
    [Export] HSlider musicVolume;
    [Export] HSlider sfxVolume;
    [Export] Button reset;

    public override void _Ready() {
        musicVolume.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(1));
        sfxVolume.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(2));

        musicVolume.ValueChanged += (value) => { OnVolumeChanged((float)value, 1); };
        sfxVolume.ValueChanged += (value) => { OnVolumeChanged((float)value, 2); };

        reset.Pressed += ResetDefaults;
    }

    private void ResetDefaults() {
        musicVolume.Value = 1f;
        sfxVolume.Value = 1f;
    }

    public void OnVolumeChanged(float value, int busIdx) {
        float v = Mathf.LinearToDb(value);
        AudioServer.SetBusVolumeDb(busIdx, v);
    }
}
