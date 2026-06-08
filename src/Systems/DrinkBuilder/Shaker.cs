using Godot;
using System;

public partial class Shaker : Sprite2D {
    [Export] public float shakeFrequency = 15f;
    [Export] public float shakeAmount = 50f;
    [Export] public float rotateFrequency = 5f;
    [Export] public float rotateAmount = 0.2f;

    private Vector2 startPosition;
    private float startRotation;
    private float timer;

    public override void _Ready() {
        startPosition = Position;
        startRotation = Rotation;
    }

    public override void _Process(double delta) {
        if (!Visible) timer = 0f;

        timer += (float)delta;

        float x = Mathf.Sin(timer * shakeFrequency) * shakeAmount / 4f;
        float y = Mathf.Sin(timer * shakeFrequency) * shakeAmount;

        float r = Mathf.Sin(timer * rotateFrequency) * rotateAmount;

        Position = startPosition + new Vector2(x, y);
        Rotation = startRotation + r;
    }
}
