using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class Character : Resource {
	[Export] public string displayName;
	[Export] public Texture2D texture;

	[Export] public Dictionary<TimeOfDay, CharacterTimeInfo> info = new();
}
