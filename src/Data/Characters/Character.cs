using Godot;
using System;

[GlobalClass]
public partial class Character : Resource {
	[Export] public string displayName;
	[Export] public Texture2D texture;
	[Export] public Godot.Collections.Array<Drink> drinkList;
	[Export] public string[] morningResponse;
	[Export] public string[] afternoonResponse;
	[Export] public string[] eveningResponse;
	public int reputation = 1;
	public void increaseReputation() {
		if(reputation < drinkList.Count-1) {
			reputation++;
		}
	}
}
