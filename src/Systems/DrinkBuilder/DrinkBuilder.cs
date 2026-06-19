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

	public string GetDrinkFeedback(Dictionary<Ingredient, int> drinkOrdered, Dictionary<Ingredient, int> drinkServed) {
		int critique = 3; //This holds the ingredientType that is currently the highest priority problem. 3 means no issue.
		int[] levels = [0,0,0]; //Used to see if the drink needs more or less of each ingredientType
		foreach( System.Collections.Generic.KeyValuePair<Ingredient, int> kvp in drinkOrdered) {
			levels[(int)kvp.Key.ingredientType] -= kvp.Value;
		}
		foreach( System.Collections.Generic.KeyValuePair<Ingredient, int> kvp in drinkServed){
			levels[(int)kvp.Key.ingredientType] += kvp.Value;
			if(!drinkOrdered.TryGetValue(kvp.Key, out int value) || value != kvp.Value) {
				if ((int)kvp.Key.ingredientType < critique) {
					critique = (int)kvp.Key.ingredientType;
				}
			}
		}
		switch (critique) {
			case (int)IngredientType.LIQUOR:
				if (levels[(int)IngredientType.LIQUOR] > 0) {
					return "Less Liquor";
				}else if(levels[(int)IngredientType.LIQUOR] < 0) {
					return "More Liquor";
				} else {
					return "Wrong type of liquor";
				}
			case (int)IngredientType.JUICE:
				if (levels[(int)IngredientType.JUICE] > 0) {
					return "Less Juice";
				}else if(levels[(int)IngredientType.JUICE] < 0) {
					return "More Juice";
				} else {
					return "Wrong type of juice";
				}
			case (int)IngredientType.GARNISH:
				if (levels[(int)IngredientType.GARNISH] > 0) {
					return "Less Garnish";
				}else if(levels[(int)IngredientType.GARNISH] < 0) {
					return "More Garnish";
				} else {
					return "Wrong type of garnish";
				}
			case 3: //Empty Glass
				return "Why did you hand me an empty glass?";
		}
		
		return "ERROR";
	}

}
