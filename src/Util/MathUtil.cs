using System;
using Godot;

public static class MathUtil {
    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax, bool clamp = false) {
        float val = toMin + (toMax - toMin) * ((value - fromMin) / (fromMax - fromMin));
        return clamp ? Mathf.Clamp(val, Mathf.Min(toMin, toMax), Mathf.Max(toMin, toMax)) : val;
    }

    // 0 <= x <= 1
    public static float EaseInOut(float t) {
        return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
    }

    public static float VectorToAngle(Vector2 direction) {
        float r = Mathf.Acos(Vector2.Right.Dot(direction.Normalized()));
        float deg = Mathf.RadToDeg(r);
        if (direction.Y >= 0f) {
            return deg;
        } else {
            return -deg;
        }
    }

    public static double PointLineDist(Vector2 A, Vector2 B, Vector2 P) {
        Vector2 line = B - A;
        Vector2 dir = line.Normalized();

        Vector2 pointLine = P - A;

        double dist = Mathf.Abs(pointLine.X * dir.Y - pointLine.Y * dir.X);

        return dist;
    }

    public static bool CompareColor(Color c1, Color c2) {
        bool r = c1.R8 == c2.R8;
        bool g = c1.G8 == c2.G8;
        bool b = c1.B8 == c2.B8;
        bool a = c1.A8 == c2.A8;

        return r && g && b && a;
    }

    public static Color AddColors(Color c1, Color c2) {
        int r = Math.Clamp(c1.R8 + c2.R8, 0, 255);
        int g = Math.Clamp(c1.G8 + c2.G8, 0, 255);
        int b = Math.Clamp(c1.B8 + c2.B8, 0, 255);
        int a = Math.Clamp(c1.A8 + c2.A8, 0, 255);

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }

    public static Color ReflectColors(Color src, Color surface) {
        int r = src.R8 + surface.R8 - 255;
        int g = src.G8 + surface.G8 - 255;
        int b = src.B8 + surface.B8 - 255;
        int a = src.A8 + surface.A8 - 255;

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
}
