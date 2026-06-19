using FmodSharp;
using Godot;
using System;

public partial class AudioManager : Node {
    public static AudioManager I;

    public bool audioEnabled = true;

    private FmodEvent musicEvent;
    private FmodEvent beachAmbiance;

    private FmodEvent crowdAmbiance;

    public override void _Ready() {
        I = this;
    }

    public void PlayMainMenu() {
        if(!audioEnabled) return;

        if(musicEvent != null) musicEvent.Stop();
        if(beachAmbiance != null) beachAmbiance.Stop();
        if(crowdAmbiance != null) crowdAmbiance.Stop();

        musicEvent = FmodServer.CreateEventInstance("event:/MUSIC/main_menu");
        musicEvent.Start();
    }

    public void PlayStinger() {
        if(!audioEnabled) return;
        FmodEvent e = FmodServer.CreateEventInstance("event:/MUSIC/success_stinger");
        e.Start();
    }

    public void PlayBartendingMusic(TimeOfDay timeOfDay) {
        if(!audioEnabled) return;

        if(musicEvent != null) musicEvent.Stop();

        switch(timeOfDay) {
            case TimeOfDay.MORNING:
                musicEvent = FmodServer.CreateEventInstance("event:/MUSIC/bartending_music_01");
                break;
            case TimeOfDay.AFTERNOON:
                musicEvent = FmodServer.CreateEventInstance("event:/MUSIC/bartending_music_02");
                break;
            case TimeOfDay.EVENING:
                musicEvent = FmodServer.CreateEventInstance("event:/MUSIC/bartending_music_03");
                break;
            default:
                musicEvent = FmodServer.CreateEventInstance("event:/MUSIC/main_menu");
                break;
        }
        
        musicEvent.Start();
    }

    public void PlayAmbience(TimeOfDay timeOfDay) {
        if(!audioEnabled) return;

        beachAmbiance = FmodServer.CreateEventInstance("event:/AMBIENCE/beach_loop");

        switch(timeOfDay) {
            case TimeOfDay.MORNING:
                crowdAmbiance = FmodServer.CreateEventInstance("event:/AMBIENCE/crowd_loop_morning_eve");
                break;
            case TimeOfDay.AFTERNOON:
                crowdAmbiance = FmodServer.CreateEventInstance("event:/AMBIENCE/crowd_loop_afternn");
                break;
            case TimeOfDay.EVENING:
                crowdAmbiance = FmodServer.CreateEventInstance("event:/AMBIENCE/crowd_loop_morning_eve");
                break;
            default:
                crowdAmbiance = FmodServer.CreateEventInstance("event:/AMBIENCE/crowd_loop_morning_eve");
                break;
        }
        
        beachAmbiance.Start();
        crowdAmbiance.Start();
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
    SHAKER_SHAKE,
    STIR_COCKTAIL,
    GLASS_CLINK,
    DRINK_SERVE
}