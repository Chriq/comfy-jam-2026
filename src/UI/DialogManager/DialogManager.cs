using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class DialogManager : Control {

    public static DialogManager Instance;

    [Export] private RichTextLabel dialog;
    [Export] private OptionSelect optionSelect;
    [Export] private TextureRect portrait;

    [Signal]
    public delegate void FinishedEventHandler();

    public int increment = 1;

    [Signal]
    public delegate void OptionSelectedEventHandler(int optionIndex);

    public override void _Ready() {
        Instance = this;
    }


    public float characterSpeed = 0.032f;

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (@event.IsPressed() && dialog.VisibleCharacters == -1) {
            Hide();
            EmitSignal(SignalName.Finished);

        }
    }

    public async Task DisplayText(string text, bool showPrompt = true, DialogOptions dialogOptions = null) {
        if (dialogOptions != null && dialogOptions.portrait != null) {
            portrait.Texture = dialogOptions.portrait;
        } else {
            portrait.Texture = null;
        }

        Show();
        dialog.VisibleCharacters = 0;
        dialog.Text = text;

        for (int i = 0; i < text.Length; i += increment) {
            dialog.VisibleCharacters = i;
            await ToSignal(GetTree().CreateTimer(characterSpeed), "timeout");
        }

        dialog.VisibleCharacters = -1;
    }

    public async Task DisplayText(string[] lines, bool showPrompt = true, DialogOptions dialogOptions = null) {
        foreach (string line in lines) {
            await DisplayText(line);
            await ToSignal(this, "Finished");
        }
    }

    public void DisplayTextInstantaneous(string text, bool showPrompt = true, DialogOptions dialogOptions = null) {
        if (dialogOptions != null && dialogOptions.portrait != null) {
            portrait.Texture = dialogOptions.portrait;
        } else {
            portrait.Texture = null;
        }

        Show();
        dialog.Text = text;
        dialog.VisibleCharacters = -1;
    }

    public void EnableOptionSelect(string[] options) {
        optionSelect.Populate(options);
        optionSelect.Show();

        optionSelect.ItemSelected += OnOptionSelected;
    }

    public void DisableOptionSelect() {
        optionSelect.Hide();
        optionSelect.ItemSelected -= OnOptionSelected;
    }

    public void OnOptionSelected(long optionIndex) {
        optionSelect.Hide();
        optionSelect.ItemSelected -= OnOptionSelected;
        EmitSignal(SignalName.OptionSelected, optionIndex);
    }

    public string GetText() {
        return dialog.Text;
    }

    private void Close() {
        Hide();
        Finished -= Close;
    }
}

public class DialogOptions {
    public Color color;
    public bool showPrompt;
    public float textSpeed;
    public Texture2D portrait;
}

public partial class LineFinishedSignal : Node {
    [Signal]
    public delegate void LineFinishedEventHandler();

    public void EmitLineFinishedSignal() {
        EmitSignal(SignalName.LineFinished);
    }
}