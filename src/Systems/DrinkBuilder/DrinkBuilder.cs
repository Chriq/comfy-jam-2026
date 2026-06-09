using Godot;
using Godot.Collections;
using System;

public partial class DrinkBuilder : Node {
	[Export] public IngredientPanel ingredientPanel;
	[Export] public RichTextLabel currentOrderDisplay;

	[Signal] public delegate void DrinkServedEventHandler(Dictionary<Ingredient, int> drink);

	public Dictionary<Ingredient, int> currentDrink { get; private set; } = new();

	public override void _Ready() {
		ingredientPanel.IngredientSelected += UpdateCurrentDrink;
		ingredientPanel.DrinkServed += ServeDrink;
		currentOrderDisplay.Text = "Current Drink:";
	}

	private void UpdateCurrentDrink(Ingredient ingredient, int amount) {
		if (amount > 0) {
			currentDrink[ingredient] = amount;
		} else {
			currentDrink.Remove(ingredient);
		}

		UpdateCurrentDrinkUI();
	}

	private void UpdateCurrentDrinkUI() {
		string orderDisplay = "Current Drink:\n";
		foreach (Ingredient i in currentDrink.Keys) {
			orderDisplay += $"x {currentDrink[i]}{i.units} {i.displayName}\n";
		}

		currentOrderDisplay.Text = orderDisplay;
	}

	private void ServeDrink() {
		EmitSignal(SignalName.DrinkServed, new Dictionary<Ingredient, int>(currentDrink));
		GD.Print("Serving: ");
		foreach (Ingredient i in currentDrink.Keys) {
			GD.Print($"{i.displayName} {currentDrink[i]}");
		}

		currentDrink.Clear();
	}

}
