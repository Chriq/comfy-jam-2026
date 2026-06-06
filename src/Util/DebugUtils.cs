using Godot;
using System;

public static class DebugUtils {
    public static Line2D DrawDebugLine(Vector2 from, Vector2 to) {
        Line2D line = new();
        line.AddPoint(from);
        line.AddPoint(to);
        line.Width = 2;

        return line;
    }

    public static Line2D DrawDebugLine(Vector2 from, Vector2 to, Color color) {
        Line2D line = new();
        line.AddPoint(from);
        line.AddPoint(to);
        line.Width = 2;
        line.DefaultColor = color;

        return line;
    }
}
