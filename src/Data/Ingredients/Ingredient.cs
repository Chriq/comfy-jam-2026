using Godot;
using System;

[GlobalClass]
public partial class Ingredient : Resource {
    [Export] public string displayName { get; private set; }
    [Export] public Texture2D texture { get; private set; }
    [Export] public string units { get; private set; } = "oz.";
    [Export] public IngredientType ingredientType { get; private set; }
}

public enum IngredientType {
    LIQUOR,
    JUICE,
    GARNISH
}