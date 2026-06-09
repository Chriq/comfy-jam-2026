using Godot;
using System;

[GlobalClass]
public partial class Character : Resource {
	[Export] public string displayName;
	[Export] public Texture2D texture;
	[Export] public Godot.Collections.Array<Drink> drinkList;
	public int reputation = 1;
}
