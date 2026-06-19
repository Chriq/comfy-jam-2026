using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class Drink : Resource {
	[Export] public string displayName { get; private set; }
	[Export] public string description { get; private set; }
	[Export] public Texture2D texture;
    [Export] public Texture2D emptyGlassTexture;
	[Export] public Dictionary<Ingredient, int> recipe;
	[Export] public TimeOfDay drinkTime;
    
}
