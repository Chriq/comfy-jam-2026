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
        characterSpawner.SetNewCustomer(timeOfDay);

        if (characterSpawner.isUniqueCharacter) {
            string[] greeting = characterSpawner.currentCustomer.info[timeOfDay].greeting;

            // greeting will leave off with space for drink name
            greeting[greeting.Count() - 1] += $" {characterSpawner.currentOrder.displayName}?";
            await dm.DisplayText(greeting);
        } else {
            await dm.DisplayText($"Hey! I'm {characterSpawner.currentCustomer.displayName}");
            await ToSignal(dm, "Finished");
            await dm.DisplayText($"Can I get a {characterSpawner.currentOrder.displayName}?");
            await ToSignal(dm, "Finished");
        }
        panelContainer.Show();
    }

	public async void ServeDrink(Dictionary<Ingredient, int> drinkServed) {
		panelContainer.Hide();

		Sprite2D shaker = drinksContainer.GetChild<Sprite2D>(0);
		Sprite2D drink = drinksContainer.GetChild<Sprite2D>(1);
		drink.Texture = characterSpawner.currentOrder.texture;

        AudioManager.I.PlaySFX(SFX.SHAKER);
        shaker.Show();
        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        shaker.Hide();

        AudioManager.I.PlaySFX(SFX.BOTTLE_PUT_DOWN);
        drink.Show();

        bool correct = ValidateDrink(drinkServed);
        if (correct) {
            characterSpawner.CustomerSatisfied();
            if (characterSpawner.isUniqueCharacter) {
                await dm.DisplayText(characterSpawner.currentCustomer.info[timeOfDay].response);
            } else {
                await dm.DisplayText("Looks great!");
            }

            drink.Hide();

            await dm.DisplayText($"Thanks for the {characterSpawner.currentOrder.displayName}");
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
