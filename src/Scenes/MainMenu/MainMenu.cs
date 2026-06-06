using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control {
    [Export] private PackedScene toScene;
    [Export] private Control creditsContainer;
    [Export] private Control howContainer;
    [Export] Control cutsceneContainer;

    public override void _Ready() {
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
        GetTree().ChangeSceneToPacked(toScene);
    }
}
