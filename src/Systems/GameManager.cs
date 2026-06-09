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

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready() {
		rng.Randomize();
		drinkBuilder.DrinkServed += ServeDrink;
		StartRound();
	}

	private async Task StartRound() {
		characterSpawner.SetNewCustomer();
		await dm.DisplayText($"Hey, can I get a {characterSpawner.currentOrder.displayName}?");
	}

	public async void ServeDrink(Dictionary<Ingredient, int> drinkServed) {
		bool correct = ValidateDrink(drinkServed);
		if (correct) {
			await dm.DisplayText($"Thanks for the {characterSpawner.currentOrder.displayName}");
		} else {
			await dm.DisplayText("This isn't right...");
		}

		EndRound();
	}

	private async Task EndRound() {
		characterSpawner.Clear();
		await ToSignal(GetTree().CreateTimer(rng.RandfRange(1f, 3f)), "timeout");
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
}
