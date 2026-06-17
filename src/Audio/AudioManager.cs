using FmodSharp;
using Godot;
using System;

public partial class AudioManager : Node {
    public static AudioManager I;

    public bool audioEnabled = false;

    public override void _Ready() {
        I = this;
    }

    public void PlayMusic(TimeOfDay timeOfDay) {
        if(!audioEnabled) return;

        FmodEvent e;
        switch(timeOfDay) {
            case TimeOfDay.MORNING:
                e = FmodServer.CreateEventInstance("event:/MUSIC/bartending_music_01");
                break;
            case TimeOfDay.AFTERNOON:
                e = FmodServer.CreateEventInstance("event:/MUSIC/bartending_music_02");
                break;
            case TimeOfDay.EVENING:
                e = FmodServer.CreateEventInstance("event:/MUSIC/bartending_music_01");
                break;
            default:
                e = FmodServer.CreateEventInstance("event:/MUSIC/bartending_music_01");
                break;
        }
        
        e.Start();
    }

    public void PlayAmbience() {
        if(!audioEnabled) return;

        FmodEvent e2 = FmodServer.CreateEventInstance("event:/AMBIENCE/beach_loop");
        e2.Start();
    }

    public void PlaySFX(SFX sfx) {
        if(!audioEnabled) return;

        FmodEvent e  = FmodServer.CreateEventInstance("event:/SFX/Bartending/" + sfx.ToString().ToLower());        
        e.Start();
    }

}

public enum SFX {
    POUR_LIQUID,
    ICE_DROP,
    SHAKER,
    STIR_COCKTAIL,
    BOTTLE_PUT_DOWN
}