using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class IngredientPanel : Control {
    [Export] PackedScene ingredientSelectPrefab;

    [Export] private GridContainer liquorsTab;
    [Export] private GridContainer juicesTab;
    [Export] private GridContainer garnishesTab;

    public override void _Ready() {
        InitPanelDisplay();
    }

    private void InitPanelDisplay() {
        List<Ingredient> ingredients = GetAllIngredients();

        foreach (Ingredient liquor in ingredients.Where(ing => ing.ingredientType == IngredientType.LIQUOR)) {
            IngredientSelect select = ingredientSelectPrefab.Instantiate<IngredientSelect>();
            select.nameLabel.Text = liquor.displayName;
            select.ingredientDisplay.Texture = liquor.texture;
            select.amountInput.Suffix = liquor.units;

            liquorsTab.AddChild(select);
        }

        // TODO: repeat structure for other tabs
    }

    public List<Ingredient> GetAllIngredients() {
        return NodeUtil.LoadResourcesFromFolder("res://Data/Ingredients/").OfType<Ingredient>().ToList();
    }
}
