using Godot;
using System;
using Godot.Collections;

// TODO: refactor into Dictionary of TimeOfDay to roundf info, new struct for greeting, response, and drink info
[GlobalClass]
public partial class Character : Resource {
	[Export] public string displayName;
	[Export] public Texture2D texture;

	[Export] public Dictionary<TimeOfDay, CharacterTimeInfo> info = new();

	// [Export] public Godot.Collections.Array<Drink> drinkList;

	// [Export] public string[] morningGreeting;
	// [Export] public string[] morningResponse;
	// [Export] public string[] afternoonGreeting;
	// [Export] public string[] afternoonResponse;
	// [Export] public string[] eveningGreeting;
	// [Export] public string[] eveningResponse;

	// public int reputation = 0;
	// public void increaseReputation() {
	// 	if(reputation < drinkList.Count-1) {
	// 		reputation++;
	// 	}
	// }
}
