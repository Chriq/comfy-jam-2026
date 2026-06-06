using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class IngredientPanel : Control {
    [Export] PackedScene ingredientSelectPrefab;

    [Export] private GridContainer liquorsTab;
    [Export] private GridContainer juicesTab;
    [Export] private GridContainer garnishesTab;

    [Export] private Button serveButton;

    [Signal] public delegate void IngredientSelectedEventHandler(Ingredient ingredient, int amount);
    [Signal] public delegate void DrinkServedEventHandler();

    public override void _Ready() {
        InitPanelDisplay();
        serveButton.Pressed += ServeDrink;
    }

    private void InitPanelDisplay() {
        List<Ingredient> ingredients = GetAllIngredients();

        foreach (Ingredient liquor in ingredients.Where(ing => ing.ingredientType == IngredientType.LIQUOR)) {
            IngredientSelect select = ingredientSelectPrefab.Instantiate<IngredientSelect>();
            select.nameLabel.Text = liquor.displayName;
            select.ingredientDisplay.Texture = liquor.texture;
            select.amountInput.Suffix = liquor.units;

            select.amountInput.ValueChanged += (double amt) => SelectionChanged(liquor, (int)amt);

            liquorsTab.AddChild(select);
        }

        // TODO: repeat structure for other tabs
    }

    public void SelectionChanged(Ingredient ingredient, int amount) {
        EmitSignal(SignalName.IngredientSelected, ingredient, amount);
    }

    public void ServeDrink() {
        EmitSignal(SignalName.DrinkServed);
        Clear();
    }

    public void Clear() {
        // TODO: clear out all amout input values
    }

    public List<Ingredient> GetAllIngredients() {
        return NodeUtil.LoadResourcesFromFolder("res://Data/Ingredients/").OfType<Ingredient>().ToList();
    }
}
