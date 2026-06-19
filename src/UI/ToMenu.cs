using Godot;
using System;

public partial class ToMenu : Control {
	public void ToMainMenu() {
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
	}
	public void QuitGame() {
        GetTree().Quit();
    }
}
