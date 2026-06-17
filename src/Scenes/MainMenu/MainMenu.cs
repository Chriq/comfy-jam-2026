using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control {
    [Export] private PackedScene toScene;
    [Export] private Control creditsContainer;
    [Export] private Control howContainer;
    [Export] Control cutsceneContainer;
    [Export] DialogManager dm;

    public override void _Ready() {
        AudioManager.I.PlayMusic(TimeOfDay.MORNING);
        AudioManager.I.PlayAmbience();
    }


    public void StartGame() {
        PlayButtonSFX();
        DoIntroCutscene();
    }

    public void ToggleCredits() {
        PlayButtonSFX();
        creditsContainer.Visible = !creditsContainer.Visible;
        howContainer.Hide();
    }

    public void ToggleControls() {
        PlayButtonSFX();
        howContainer.Visible = !howContainer.Visible;
        creditsContainer.Hide();
    }

    public void QuitGame() {
        PlayButtonSFX();
        GetTree().Quit();
    }

    private void PlayButtonSFX() {
    }

    private async Task DoIntroCutscene() {
        cutsceneContainer.MouseFilter = MouseFilterEnum.Stop;
        creditsContainer.Hide();
        howContainer.Hide();
        FadeController fade = new();
        AddChild(fade);

        await fade.FadeIn(cutsceneContainer, 2f);
        await dm.DisplayText("Hey, your first shift at the bar starts tomorrow morning!\n\nHere's a recipe book you can use for reference. The last guy wrote it though, so it may not be super reliable...\n\nOh well, you'll figure it out. Just update the book as you go along. I hope you're better than the last guy!\n\n\n\n\n\n[i](Click anywhere to continue)[/i]");
        await ToSignal(dm, "Finished");

        GetTree().ChangeSceneToPacked(toScene);
    }
}
