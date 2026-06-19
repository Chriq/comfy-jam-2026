using Godot;
using System;

public partial class Shaker : Sprite2D {
    [Export] public float shakeDuration = 3f;
    [Export] public float shakeFrequency = 15f;
    [Export] public float shakeAmount = 50f;
    [Export] public float rotateFrequency = 5f;
    [Export] public float rotateAmount = 0.2f;

    [Export] Sprite2D cap;

    [Signal] public delegate void DoneShakingEventHandler();

    private Vector2 startPosition;
    private float startRotation;

    private Vector2 capStartPosition;
    private float capStartRotation;

    private float timer;
    private bool isShaking = false;

    public override void _Ready() {
        startPosition = Position;
        startRotation = Rotation;

        capStartPosition = cap.Position;
        capStartRotation = cap.Rotation;
    }

    public async void StartShaking() {
        Show();
        Tween tween = GetTree().CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Spring);

        tween.TweenProperty(cap, "position", Vector2.Zero, 0.5f);
        tween.Parallel().TweenProperty(cap, "rotation", 0f, 0.5f);

        tween.TweenCallback(Callable.From(Shake));
    }

    private async void Shake() {
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        AudioManager.I.PlaySFX(SFX.SHAKER_SHAKE);
        isShaking = true;
    }

    public override void _Process(double delta) {
        if(isShaking) {
            timer += (float)delta;

            float x = Mathf.Sin(timer * shakeFrequency) * shakeAmount / 4f;
            float y = Mathf.Sin(timer * shakeFrequency) * shakeAmount;

            float r = Mathf.Sin(timer * rotateFrequency) * rotateAmount;

            Position = startPosition + new Vector2(x, y);
            Rotation = startRotation + r;

            if(timer > shakeDuration) {
                EndShaking();
            }
        }        
    }

    private void EndShaking() {
        isShaking = false;
        Hide();

        Position = startPosition;
        Rotation = startRotation;

        EmitSignal(SignalName.DoneShaking);
    }
}
