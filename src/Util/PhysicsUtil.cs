using Godot;
using System;
using System.Collections.Generic;

public partial class PhysicsUtil : Node2D {

    public static PhysicsUtil Instance;

    public override void _Ready() {
        Instance = this;
    }

    public List<PointHit> CollidePoint(Vector2 globalPosition) {
        PhysicsPointQueryParameters2D point = new() {
            Position = globalPosition,
            CollideWithAreas = true
        };

        List<PointHit> hits = new();
        Godot.Collections.Array<Godot.Collections.Dictionary> array = GetWorld2D().DirectSpaceState.IntersectPoint(point);
        if (array.Count > 0) {
            foreach (Godot.Collections.Dictionary d in array) {
                PointHit hit = NodeUtil.GDictToObject<PointHit>(d);
                hits.Add(hit);
            }
        }

        return hits;
    }

    public List<PointHit> CollideCircle(Vector2 globalPosition, float radius) {
        PhysicsShapeQueryParameters2D shape = new() {
            Transform = new Transform2D { Origin = globalPosition },
            Shape = new CircleShape2D() { Radius = radius },
            CollideWithAreas = true
        };

        List<PointHit> hits = new();
        Godot.Collections.Array<Godot.Collections.Dictionary> array = GetWorld2D().DirectSpaceState.IntersectShape(shape);
        if (array.Count > 0) {
            foreach (Godot.Collections.Dictionary d in array) {
                PointHit hit = NodeUtil.GDictToObject<PointHit>(d);
                hits.Add(hit);
            }
        }

        return hits;
    }

    public RaycastHit CollideRay(Vector2 from, Vector2 to) {
        PhysicsRayQueryParameters2D ray = new() {
            From = from,
            To = to,
            CollideWithAreas = true
        };

        Godot.Collections.Dictionary dict = GetWorld2D().DirectSpaceState.IntersectRay(ray);
        if (dict.Count > 0) {
            return NodeUtil.GDictToObject<RaycastHit>(dict);
        }

        return null;
    }

    public RaycastHit CollideRay(Vector2 origin, Vector2 dir, float dist) {
        PhysicsRayQueryParameters2D ray = new() {
            From = origin,
            To = origin + (dir * dist),
            CollideWithAreas = true
        };

        Godot.Collections.Dictionary dict = GetWorld2D().DirectSpaceState.IntersectRay(ray);
        if (dict.Count > 0) {
            return NodeUtil.GDictToObject<RaycastHit>(dict);
        }

        return null;
    }
}

public class RaycastHit {
    public Vector2 position { get; set; }
    public Vector2 normal { get; set; }
    public GodotObject collider { get; set; }
    public long collider_id { get; set; }
    public Rid rid { get; set; }
    public long shape { get; set; }
    public Variant metadata { get; set; }
}

public class PointHit {
    public GodotObject collider { get; set; }
    public long collider_id { get; set; }
    public Rid rid { get; set; }
    public long shape { get; set; }
}