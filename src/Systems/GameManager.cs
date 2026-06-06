using Godot;
using Godot.Collections;
using System;
using System.Linq;

// TODO: sequencing of game events; random delay, spawn character + order, serve drink, get feedback, new customer
public partial class GameManager : Node {
    [Export] DrinkBuilder drinkBuilder;
    [Export] CharacterSpawner characterSpawner;

    public bool ValidateDrink() {
        Dictionary<Ingredient, int> drinkServed = drinkBuilder.currentDrink;
        Dictionary<Ingredient, int> drinkOrdered = characterSpawner.currentOrder.recipe;

        return drinkServed.Count == drinkOrdered.Count &&
            drinkServed.All(
                kvp =>
                drinkOrdered.TryGetValue(kvp.Key, out int value) &&
                value == kvp.Value
            );
    }
}
