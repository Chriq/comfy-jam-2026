using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RecipePage : Control {
    [Export] RichTextLabel header;
    [Export] TextureRect img;
    [Export] RichTextLabel description;
    [Export] Control recipe;

    [Export] PackedScene listItemPrefab;

    public void InitPage(Drink drink) {
        if (drink != null) {
            header.Text = drink.displayName;
            description.Text = $"[i] {drink.description} [/i]";
            img.Texture = drink.texture;
            // recipe.Text = GetRecipeString(drink);
            InitRecipeItems(drink);
        } else {
            header.Text = "";
            img.Texture = null;
            description.Text = "";
            foreach (Node n in recipe.GetChildren()) recipe.RemoveChild(n);
        }
    }

    private string GetRecipeString(Drink drink) {
        string recipeStringBuilder = "Recipe:\n";

        foreach (Ingredient i in drink.recipe.Keys) {
            recipeStringBuilder += $"- {i.displayName} {drink.recipe[i]} {i.units}\n";
        }

        return recipeStringBuilder;
    }

    private void InitRecipeItems(Drink drink) {
        foreach(Node n in recipe.GetChildren()) {
            recipe.RemoveChild(n);
        }
        
        List<Ingredient> sortedIngredients = drink.recipe.Keys.ToList();
        sortedIngredients.Sort((a,b) => {
            return a.ingredientType - b.ingredientType;
        });

        foreach (Ingredient i in sortedIngredients) {
            Node item = listItemPrefab.Instantiate();
            SpinBox s = item.GetChild<SpinBox>(0);
            s.Suffix = i.units;
            int val = Mathf.Clamp(drink.recipe[i] + GD.RandRange(-1, 1), 1, (int) s.MaxValue);
            // int val = drink.recipe[i];
            s.Value = val;

            RichTextLabel l = item.GetChild<RichTextLabel>(1);
            l.Text = i.displayName;

            item.RemoveChild(s);
            recipe.AddChild(s);

            item.RemoveChild(l);
            recipe.AddChild(l);

            item.QueueFree();
        }
    }
}
