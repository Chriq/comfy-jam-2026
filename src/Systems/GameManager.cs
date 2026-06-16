using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class GameManager : Node {
	[Export] DrinkBuilder drinkBuilder;
	[Export] CharacterSpawner characterSpawner;

	[Export] DialogManager dm;
	[Export] public Control panelContainer;
	[Export] Node2D drinksContainer;
	[Export] TextureRect background;

	private TimeOfDay timeOfDay = TimeOfDay.MORNING;

	private RandomNumberGenerator rng = new RandomNumberGenerator();
	private FadeController fade = new();

	public override async void _Ready() {
		panelContainer.Hide();
		rng.Randomize();
		drinkBuilder.DrinkServed += ServeDrink;
		AddChild(fade);
		SetBackground();
 
		await ToSignal(GetTree().CreateTimer(1f), "timeout");

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
			if (characterSpawner.isUniqueCharacter) {
				switch (timeOfDay) {
					case TimeOfDay.MORNING:
						await dm.DisplayText(characterSpawner.currentCustomer.morningResponse);
						break;
					case TimeOfDay.AFTERNOON:
						await dm.DisplayText(characterSpawner.currentCustomer.afternoonResponse);
						break;
					case TimeOfDay.EVENING:
						await dm.DisplayText(characterSpawner.currentCustomer.eveningResponse);
						break;
				}
			} else {
				await dm.DisplayText("Looks great!");
			}

			// await ToSignal(dm, "Finished");
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
			// await ToSignal(dm, "Finished");

			string feedback = drinkBuilder.GetDrinkFeedback(characterSpawner.currentOrder.recipe, drinkServed);
			await dm.DisplayText(feedback);
			await ToSignal(dm, "Finished");
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

	private async Task SetBackground() {
		TextureRect oldBackground = background.Duplicate() as TextureRect;
		background.AddChild(oldBackground);

		string backgroundPath = "res://Scenes/background_" + timeOfDay.ToString().ToLower() + ".png";
		background.Texture = GD.Load<Texture2D>(backgroundPath);

		await fade.FadeOut(oldBackground);
		oldBackground.QueueFree();
	}
}

public enum TimeOfDay {
	MORNING,
	AFTERNOON,
	EVENING
}
