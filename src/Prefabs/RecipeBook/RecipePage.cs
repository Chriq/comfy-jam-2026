using Godot;
using System;

public partial class RecipePage : Control {
    RichTextLabel header;
    TextureRect img;
    RichTextLabel description;
    RichTextLabel recipe;

    public override void _Ready() {
        header = GetChild<RichTextLabel>(0);
        img = GetChild<TextureRect>(1);
        description = GetChild<RichTextLabel>(2);
        recipe = GetChild<RichTextLabel>(3);
    }

    public void InitPage(Drink drink) {
        if (drink != null) {
            header.Text = drink.displayName;
            description.Text = $"[i] {drink.description} [/i]";
            img.Texture = drink.texture;
            recipe.Text = GetRecipeString(drink);
        } else {
            header.Text = "";
            img.Texture = null;
            description.Text = "";
            recipe.Text = "";
        }
    }

    // TODO: refactor to make recipe amounts editable and start on random values. Let's cap at 8
    private string GetRecipeString(Drink drink) {
        string recipeStringBuilder = "Recipe:\n";

        foreach (Ingredient i in drink.recipe.Keys) {
            recipeStringBuilder += $"- {i.displayName} {drink.recipe[i]} {i.units}";
        }

        return recipeStringBuilder;
    }
}
