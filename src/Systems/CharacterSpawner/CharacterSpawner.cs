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

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready() {
        rng.Randomize();
    }

    public void SetNewCustomer() {
        float val = rng.Randf();
        if (val < specialCharacterProbability) {
            // get special character
            NodeUtil.LoadResourcesFromFolder("res://Data/Characters/").OfType<Character>().ToList();
        } else {
            // use silhouette texture and generate random name
            string customerName = GenerateRandomName();
            characterRect.Texture = genericCharacterTexture;
        }
    }

    public void Clear() {
        characterRect.Texture = null;
        currentCustomer = null;
        currentOrder = null;
    }

    public string GenerateRandomName() {
        return $"{firstNames.PickRandom()} {lastNames.PickRandom()}";
    }

    private static readonly Array<string> firstNames = new() { "Michael", "Christopher", "Matthew", "Joshua", "Daniel", "Jennifer", "Jessica", "Ashley", "Sarah", "Amanda" };
    private static readonly Array<string> lastNames = new() { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
}

