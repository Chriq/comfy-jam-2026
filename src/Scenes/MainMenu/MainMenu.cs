using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control {
	[Export] private PackedScene toScene;
	[Export] private Control creditsContainer;
	[Export] private Control howContainer;
	[Export] Control cutsceneContainer;
	[Export] DialogManager dm;

	[Export] Button test;

	public override void _Ready() {
		AudioManager.I.PlayMainMenu();
	}

	public void StartGame() {
		DoIntroCutscene();
	}

	public void ToggleCredits() {
		creditsContainer.Visible = !creditsContainer.Visible;
		howContainer.Hide();
	}

	public void ToggleControls() {
		howContainer.Visible = !howContainer.Visible;
		creditsContainer.Hide();
	}

	public void QuitGame() {
		GetTree().Quit();
	}

	private async Task DoIntroCutscene() {
		cutsceneContainer.MouseFilter = MouseFilterEnum.Stop;
		creditsContainer.Hide();
		howContainer.Hide();
		FadeController fade = new();
		AddChild(fade);

		await fade.FadeIn(cutsceneContainer, 2f);
		await dm.DisplayText("Hey, I guess you're the new beachside bartender at Castaway's. Your first shift starts tomorrow morning!\n\nHere's a recipe book you can use for reference. The last guy wrote it though, so it may not be super reliable...\n\nOh well, you'll figure it out. Just update the book as you figure stuff out. I hope you're better than the last guy!\n\n\n\n\n\n[i](Click anywhere to continue)[/i]");
		await ToSignal(dm, "Finished");

		GetTree().ChangeSceneToPacked(toScene);
	}
}
