using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;

// TODO: sequencing of game events; random delay, spawn character + order, serve drink, get feedback, new customer
public partial class GameManager : Node {
	[Export] DrinkBuilder drinkBuilder;
	[Export] CharacterSpawner characterSpawner;

	[Export] DialogManager dm;
	[Export] public Control panelContainer;
	[Export] Node2D drinksContainer;
	[Export] TextureRect background;

	private TimeOfDay timeOfDay = TimeOfDay.MORNING;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready() {
		panelContainer.Hide();
		rng.Randomize();
		drinkBuilder.DrinkServed += ServeDrink;
		SetBackground();

		StartRound();
	}

	private async Task StartRound() {
		characterSpawner.SetNewCustomer();
		await dm.DisplayText($"Hey! I'm {characterSpawner.currentCustomer.displayName}");
		await ToSignal(dm, "Finished");
		await dm.DisplayText($"Can I get a {characterSpawner.currentOrder.displayName}?");
		await ToSignal(dm, "Finished");
		panelContainer.Show();
	}

	public async void ServeDrink(Dictionary<Ingredient, int> drinkServed) {
		panelContainer.Hide();

		Sprite2D shaker = drinksContainer.GetChild<Sprite2D>(0);
		Sprite2D drink = drinksContainer.GetChild<Sprite2D>(1);
		drink.Texture = characterSpawner.currentOrder.texture;

		shaker.Show();
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		shaker.Hide();
		drink.Show();

		bool correct = ValidateDrink(drinkServed);
		if (correct) {
			characterSpawner.CustomerSatisfied();
			await dm.DisplayText("Looks great!");
			await ToSignal(dm, "Finished");
			drink.Hide();

			await dm.DisplayText($"Thanks for the {characterSpawner.currentOrder.displayName}");
			if (characterSpawner.isUniqueCharacter) {
				characterSpawner.currentCustomer.increaseReputation();
			}
		} else {
			await dm.DisplayText("Looks...interesting...");
			await ToSignal(dm, "Finished");
			drink.Hide();

			await dm.DisplayText("This isn't right...");
		}

		await ToSignal(dm, "Finished");
		EndRound();
	}

	private async Task EndRound() {
		characterSpawner.Clear();
		await ToSignal(GetTree().CreateTimer(rng.RandfRange(1f, 3f)), "timeout");

		bool allServed = characterSpawner.AllCharactersServed();
		if (allServed) {
			AdvanceTimeOfDay();
			await ToSignal(GetTree().CreateTimer(rng.RandfRange(1f, 3f)), "timeout");
		}

		StartRound();
	}

	public bool ValidateDrink(Dictionary<Ingredient, int> drinkServed) {
		Dictionary<Ingredient, int> drinkOrdered = characterSpawner.currentOrder.recipe;

		return drinkServed.Count == drinkOrdered.Count &&
			drinkServed.All(
				kvp =>
				drinkOrdered.TryGetValue(kvp.Key, out int value) &&
				value == kvp.Value
			);
	}

	private void AdvanceTimeOfDay() {
		timeOfDay++;
		characterSpawner.ResetCharacterMap();

		if (timeOfDay > Enum.GetValues<TimeOfDay>().Max()) {
			GetTree().ChangeSceneToFile("res://Scenes/WinGame.tscn");
		} else {
			SetBackground();
		}
	}

	private void SetBackground() {
		string backgroundPath = "res://Scenes/background_" + timeOfDay.ToString().ToLower() + ".png";
		background.Texture = GD.Load<Texture2D>(backgroundPath);
	}
}

public enum TimeOfDay {
	MORNING,
	AFTERNOON,
	EVENING
}
