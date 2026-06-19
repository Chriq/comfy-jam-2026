using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class DrinkBuilder : Node {
	[Export] public IngredientPanel ingredientPanel;
	[Export] public RichTextLabel currentOrderDisplay;

	[Signal] public delegate void DrinkServedEventHandler(Dictionary<Ingredient, int> drink);

	public Dictionary<Ingredient, int> currentDrink { get; private set; } = new();

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready() {
		ingredientPanel.IngredientSelected += UpdateCurrentDrink;
		ingredientPanel.DrinkServed += ServeDrink;
		currentOrderDisplay.Text = "Current Drink:";

		rng.Randomize();
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
		if(drinkServed.Count == 0) return "Why did you hand me an empty glass?";

		System.Collections.Generic.List<Ingredient> sortedIngredients = drinkOrdered.Keys.ToList();
		sortedIngredients.Sort((a,b) => {
			return a.ingredientType - b.ingredientType;
		});

		Ingredient missedIngredient = null;
		int difference = 0;

		foreach(Ingredient i in sortedIngredients) {
			if(drinkServed.ContainsKey(i)) {
				if(drinkServed[i] != drinkOrdered[i]) {
					missedIngredient = i;
					difference = drinkServed[i] - drinkOrdered[i];
					break;
				}
			} else {
				missedIngredient = i;
				difference = 0;
				break;
			}
		}

		return GetCritique(missedIngredient, difference);
	}

	private string GetCritique(Ingredient ingredient, int difference) {
		if(ingredient == null) return "I don't even know what's wrong with it. Do you know how to make the drink I ordered?";
		
		string joke = rng.Randf() < 0.5f ? "Did you even try?" : "";

		switch (ingredient.ingredientType) {
			case IngredientType.LIQUOR:
				if (difference > 0) {
					return "Whoa, this is way too strong! Less " + (ingredient != null ? ingredient.displayName : "liquor") + ", please.";
				}else if(difference < 0) {
					return "It's ok, it just tastes like a mocktail, though... Maybe add more "  + (ingredient != null ? ingredient.displayName : "liquor") + " next time.";
				} else {
					return "I think you used the wrong liquor. " + joke;
				}
			case IngredientType.JUICE:
				if (difference > 0) {
					return "Maybe tone down the" + (ingredient != null ? ingredient.displayName : "mixer") + " next time."; 
				}else if(difference < 0) {
					return "I didn't ask for a shot... Needs a bit more " + (ingredient != null ? ingredient.displayName : "mixer");
				} else {
					return "I think you used the wrong mixer. " + joke;
				}
			case IngredientType.GARNISH:
				if (difference > 0) {
					return "I didn't ask for a bouquet... Less " + (ingredient != null ? ingredient.displayName : "garnish");
				}else if(difference < 0) {
					return "So close! needs a bit more "  + (ingredient != null ? ingredient.displayName : "garnish") + " for that final touch!";
				} else {
					return "I think you used the wrong garnish. " + joke;
				}
			default:
				return "I don't even know what's wrong with it. Do you know how to make the drink I ordered?";
		}
	}

	public string GetDrinkFeedbackOld(Godot.Collections.Dictionary<Ingredient, int> drinkOrdered, Godot.Collections.Dictionary<Ingredient, int> drinkServed) {
		int critique = 3; //This holds the ingredientType that is currently the highest priority problem. 3 means no issue.
		Ingredient missedIngredient = null;
		int[] levels = [0,0,0]; //Used to see if the drink needs more or less of each ingredientType
		foreach( System.Collections.Generic.KeyValuePair<Ingredient, int> kvp in drinkOrdered) {
			levels[(int)kvp.Key.ingredientType] -= kvp.Value;
		}
		foreach( System.Collections.Generic.KeyValuePair<Ingredient, int> kvp in drinkServed){
			levels[(int)kvp.Key.ingredientType] += kvp.Value;
			if(!drinkOrdered.TryGetValue(kvp.Key, out int value) || value != kvp.Value) {
				if ((int)kvp.Key.ingredientType < critique) {
					critique = (int)kvp.Key.ingredientType;
					missedIngredient = kvp.Key;
				}
			}
		}

		string joke = rng.Randf() < 0.5f ? "Did you even try?" : "";

		switch (critique) {
			case (int)IngredientType.LIQUOR:
				if (levels[(int)IngredientType.LIQUOR] > 0) {
					return "Whoa, this is way too strong! Less " + (missedIngredient != null ? missedIngredient.displayName : "liquor") + ", please.";
				}else if(levels[(int)IngredientType.LIQUOR] < 0) {
					return "It's ok, it just tastes like a mocktail, though... Maybe add more "  + (missedIngredient != null ? missedIngredient.displayName : "liquor") + " next time.";
				} else {
					return "I think you used the wrong liquor. " + joke;
				}
			case (int)IngredientType.JUICE:
				if (levels[(int)IngredientType.JUICE] > 0) {
					return "Less Juice";
				}else if(levels[(int)IngredientType.JUICE] < 0) {
					return "I didn't ask for a shot... Needs a bit more " + (missedIngredient != null ? missedIngredient.displayName : "mixer");
				} else {
					return "I think you used the wrong mixer. " + joke;
				}
			case (int)IngredientType.GARNISH:
				if (levels[(int)IngredientType.GARNISH] > 0) {
					return "I didn't ask for a bouquet... Less " + (missedIngredient != null ? missedIngredient.displayName : "garnish");
				}else if(levels[(int)IngredientType.GARNISH] < 0) {
					return "So close! needs a bit more "  + (missedIngredient != null ? missedIngredient.displayName : "garnish") + " for that final touch!";
				} else {
					return "I think you used the wrong garnish. " + joke;
				}
			default: //Empty Glass
				return "Why did you hand me an empty glass?";
		}
	}

}
