using Godot;
using System;

public partial class RecipePage : Control {
    RichTextLabel header;
    TextureRect img;
    RichTextLabel description;
    Control recipe;

    PackedScene listItemPrefab;

    public override void _Ready() {
        header = GetChild<RichTextLabel>(0);
        img = GetChild<TextureRect>(1);
        description = GetChild<RichTextLabel>(2);
        recipe = GetChild<Control>(3);

        listItemPrefab = GD.Load<PackedScene>("res://Prefabs/RecipeBook/RecipeListItem.tscn");
    }

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

    // TODO: refactor to make recipe amounts editable and start on random values. Let's cap at 8
    private string GetRecipeString(Drink drink) {
        string recipeStringBuilder = "Recipe:\n";

        foreach (Ingredient i in drink.recipe.Keys) {
            recipeStringBuilder += $"- {i.displayName} {drink.recipe[i]} {i.units}\n";
        }

        return recipeStringBuilder;
    }

    private void InitRecipeItems(Drink drink) {
        foreach (Ingredient i in drink.recipe.Keys) {
            Node item = listItemPrefab.Instantiate();
            SpinBox s = item.GetChild<SpinBox>(0);
            s.Suffix = i.units;
            // TODO: make +/- 1 difference from correct version
            s.Value = GD.RandRange(1, 8);

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
