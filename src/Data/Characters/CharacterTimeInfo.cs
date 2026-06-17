using Godot;
using System;

[GlobalClass]
public partial class CharacterTimeInfo: Resource {
	[Export] public string[] greeting;
	[Export] public string[]  response;
	[Export] public Drink drink;
}