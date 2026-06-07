using Godot;
using Godot.Collections;
using System;

public partial class DrinkBuilder : Node {
	[Export] public IngredientPanel ingredientPanel;

	public Dictionary<Ingredient, int> currentDrink { get; private set; } = new();

	public override void _Ready() {
		ingredientPanel.IngredientSelected += UpdateCurrentDrink;
		ingredientPanel.DrinkServed += ServeDrink;
	}

	private void UpdateCurrentDrink(Ingredient ingredient, int amount) {
		currentDrink[ingredient] = amount;
		GD.Print(ingredient.displayName + " " + amount);
	}

	private void ServeDrink() {
		GD.Print("Serving: ");
		foreach (Ingredient i in currentDrink.Keys) {
			GD.Print($"{i.displayName} {currentDrink[i]}");
		}

		currentDrink.Clear();
	}

}
