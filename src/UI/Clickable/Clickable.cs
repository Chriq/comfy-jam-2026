using Godot;
using System;

public partial class Clickable : Button {

    private CustomCursor cursor;

    public override void _Ready() {
        // cursor = CustomCursor.Instance;

        // MouseEntered += ShowMouseClickable;
        // MouseExited += ShowMouseArrow;
    }

    private void ShowMouseClickable() {
        cursor.ChangeCursorType(CursorType.POINTER);
    }

    private void ShowMouseArrow() {
        cursor.ChangeCursorType(CursorType.ARROW);
    }
}
