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

	// TODO: Make feedback more specific
	// only return 1 critique at a time: priority = liquor, juice/mixer, garnish
	public string GetDrinkFeedback(Dictionary<Ingredient, int> drinkOrdered, Dictionary<Ingredient, int> drinkServed) {
		int critique = 3; //This holds the ingredientType that is currently the highest priority problem. 3 means no issue.
		foreach( System.Collections.Generic.KeyValuePair<Ingredient, int> kvp in drinkServed){
			if(!drinkOrdered.TryGetValue(kvp.Key, out int value) || value != kvp.Value) {
				if ((int)kvp.Key.ingredientType < critique) {
					critique = (int)kvp.Key.ingredientType;
				}
			}
		}
		switch (critique) {
			case (int)IngredientType.LIQUOR:
				return "Adjust Liquor";
			case (int)IngredientType.JUICE:
				return "Adjust Juice";
			case (int)IngredientType.GARNISH:
				return "Adjust Garnish";
			case 3:
				return "No Issues";
		}
		return "ERROR";
	}

}
