using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public static class NodeUtil {
    public static T[] GetChildrenOfType<T>(Node n) {

        List<T> children = n.GetChildren()
            .Where(child => child is T)
            .Cast<T>()
            .ToList();

        foreach (Node child in n.GetChildren()) {
            T[] grandChildren = GetChildrenOfType<T>(child);
            children.AddRange(grandChildren);
        }

        return children.ToArray();
    }

    // https://stackoverflow.com/questions/4943817/mapping-object-to-dictionary-and-vice-versa
    public static T GDictToObject<T>(Dictionary source)
        where T : class, new() {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source) {
            someObjectType
                     .GetProperty((string)item.Key)
                     .SetValue(someObject, item.Value.Obj, null);
        }

        return someObject;
    }

    public static T LoadJSONToObject<T>(string path) {
        if (FileAccess.FileExists(path)) {
            FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            Json.ParseString(json);
            return JsonSerializer.Deserialize<T>(json);
        }

        return default;
    }

    public static List<Resource> LoadResourcesFromFolder(string folderPath) {
        var resources = new List<Resource>();
        using var dir = DirAccess.Open(folderPath);

        if (dir != null) {
            foreach (string fileName in dir.GetFiles()) {
                string filePath = folderPath.PathJoin(fileName);

                if (filePath.EndsWith(".remap") || filePath.EndsWith(".import")) {
                    filePath = filePath.Replace(".remap", "").Replace(".import", "");
                }

                if (filePath.EndsWith(".tres") || filePath.EndsWith(".res")) {
                    var resource = GD.Load<Resource>(filePath);
                    if (resource != null && !resources.Contains(resource)) {
                        resources.Add(resource);
                    }
                }

            }
        } else {
            GD.PrintErr($"Failed to open directory: {folderPath}");
        }

        return resources;
    }

    public static List<AudioStream> LoadStreamsFromFolder(string folderPath) {
        var resources = new List<AudioStream>();
        using var dir = DirAccess.Open(folderPath);

        if (dir != null) {
            foreach (string fileName in dir.GetFiles()) {
                string filePath = folderPath.PathJoin(fileName);

                if (filePath.EndsWith(".remap") || filePath.EndsWith(".import")) {
                    filePath = filePath.Replace(".remap", "").Replace(".import", "");
                }

                if (filePath.EndsWith(".mp3") || filePath.EndsWith(".wav")) {
                    var resource = GD.Load<AudioStream>(filePath);
                    if (resource != null && !resources.Contains(resource)) {
                        resources.Add(resource);
                    }
                }

            }
        } else {
            GD.PrintErr($"Failed to open directory: {folderPath}");
        }

        return resources;
    }
}