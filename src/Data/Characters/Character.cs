using Godot;
using System;

[GlobalClass]
public partial class Character : Resource {
    [Export] public string displayName { get; private set; }
    [Export] public Texture2D texture;
}
