using Godot;
using Godot.Collections;
using System;
using System.Linq;

// TODO: decide of Character structure and instantiation, how to store their dialog, what their special requests can look like
public partial class CharacterSpawner : Node {
	[Export] float specialCharacterProbability = 0.4f;
	[Export] TextureRect characterRect;
	[Export] Texture2D genericCharacterTexture;

	public Character currentCustomer { get; private set; }
	public Drink currentOrder { get; private set; }
	public bool isUniqueCharacter = false;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	private Dictionary<Character, bool> characterServedMap = new();

	public override void _Ready() {
		ResetCharacterMap();
		rng.Randomize();
	}

	public void SetNewCustomer() {
		 float val = rng.Randf();
		//float val = 0.3f;
		if (val < specialCharacterProbability) {
			// get special character
			isUniqueCharacter = true;
			Array<Character> availableCharacters = [.. characterServedMap.Where(entry => !entry.Value).Select(entry => entry.Key)];
			if (availableCharacters.Count > 0) {
				currentCustomer = availableCharacters.PickRandom();
				currentOrder = currentCustomer.drinkList[Math.Clamp(currentCustomer.reputation, 0, currentCustomer.drinkList.Count)]; //TODO If only 1 drink per person per time, can run this off time of day
			}
		} else {
			// use silhouette texture and generate random name
			currentCustomer = new() {
				displayName = GenerateRandomName(),
				texture = genericCharacterTexture
			};

			currentOrder = new Array<Drink>(NodeUtil.LoadResourcesFromFolder("res://Data/Drinks/").OfType<Drink>().ToArray()).PickRandom();
		}

		characterRect.Texture = currentCustomer.texture;
	}

	public void CustomerSatisfied() {
		// TODO: play success stinger
		characterServedMap[currentCustomer] = true;
	}

	public bool AllCharactersServed() {
		foreach (bool served in characterServedMap.Values) {
			if (!served) return false;
		}

		return true;
	}

	public void ResetCharacterMap() {
		foreach (Character c in NodeUtil.LoadResourcesFromFolder("res://Data/Characters").OfType<Character>()) {
			characterServedMap[c] = false;
		}
	}

	public void Clear() {
		characterRect.Texture = null;
		currentCustomer = null;
		currentOrder = null;
		isUniqueCharacter = false;
	}

	public string GenerateRandomName() {
		return $"{firstNames.PickRandom()} {lastNames.PickRandom()}";
	}

	public void ResetCharacterQueue() {

	}

	private static readonly Array<string> firstNames = new() { "Michael", "Christopher", "Matthew", "Joshua", "Daniel", "Jennifer", "Jessica", "Ashley", "Sarah", "Amanda" };
	private static readonly Array<string> lastNames = new() { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
}
