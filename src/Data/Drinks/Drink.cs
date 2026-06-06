using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class Drink : Resource {
    [Export] public string displayName { get; private set; }
    [Export] public Dictionary<Ingredient, int> recipe;
}
