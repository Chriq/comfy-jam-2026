using FmodSharp;
using Godot;
using System;

public partial class AudioManager : Node {

    public override void _Ready() {
        // PlayBackground();
    }

    public void PlayBackground() {
        FmodEvent e = FmodServer.CreateEventInstance("event:/MUSIC/bartending_music_01");
        e.Start();

        FmodEvent e2 = FmodServer.CreateEventInstance("event:/AMBIENCE/beach_loop");
        e2.Start();
    }

}
