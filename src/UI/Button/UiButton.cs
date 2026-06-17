using FmodSharp;
using Godot;
using System;

public partial class UiButton : Button {
    public void Click() {
        if(!AudioManager.I.audioEnabled) return;

        FmodEventEmitter2D sfxEvent = new(GetChild<Node2D>(0));
        sfxEvent.Play();
    }
}
