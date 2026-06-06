using GDExtensionBindgen;
using Godot;
using System;

public partial class NewScript : Node
{
    private FmodEventEmitter2D _fmod;

    [Export]
    public Node2D FmodNode
    {
        get => _fmod;
        set => _fmod = value != null
            ? (FmodEventEmitter2D)(Variant)value
            : null;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_up"))
        {
            GD.Print("Play");
            _fmod.Play();
        }
    }

}
