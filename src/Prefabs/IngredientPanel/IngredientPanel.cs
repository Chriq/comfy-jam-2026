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

            select.amountInput.ValueChanged += (double amt) => SelectionChanged(liquor, (int)amt); //ASK Why is this a double? //ANSWER 

            liquorsTab.AddChild(select);
        }

        foreach (Ingredient juice in ingredients.Where(ing => ing.ingredientType == IngredientType.JUICE)) {
            IngredientSelect select = ingredientSelectPrefab.Instantiate<IngredientSelect>();
            select.nameLabel.Text = juice.displayName;
            select.ingredientDisplay.Texture = juice.texture;
            select.amountInput.Suffix = juice.units;

            select.amountInput.ValueChanged += (double amt) => SelectionChanged(juice, (int)amt);

            juicesTab.AddChild(select);
        }

        foreach (Ingredient garnish in ingredients.Where(ing => ing.ingredientType == IngredientType.GARNISH)) {
            IngredientSelect select = ingredientSelectPrefab.Instantiate<IngredientSelect>();
            select.nameLabel.Text = garnish.displayName;
            select.ingredientDisplay.Texture = garnish.texture;
            select.amountInput.Suffix = garnish.units;

            select.amountInput.ValueChanged += (double amt) => SelectionChanged(garnish, (int)amt);

            garnishesTab.AddChild(select);
        }
    }

    public void SelectionChanged(Ingredient ingredient, int amount) {
        EmitSignal(SignalName.IngredientSelected, ingredient, amount);
    }

    public void ServeDrink() {
        EmitSignal(SignalName.DrinkServed);
        Clear();
    }

    public void Clear() {
        for (int i = 0; i < liquorsTab.GetChildCount(); i++) { //iterate through each tab and set their children's values to 0
            IngredientSelect select = liquorsTab.GetChild<IngredientSelect>(i);
            select.amountInput.Value = 0;
        }
        for (int i = 0; i < juicesTab.GetChildCount(); i++) {
            IngredientSelect select = juicesTab.GetChild<IngredientSelect>(i);
            select.amountInput.Value = 0;
        }
        for (int i = 0; i < garnishesTab.GetChildCount(); i++) {
            IngredientSelect select = garnishesTab.GetChild<IngredientSelect>(i);
            select.amountInput.Value = 0;
        }
    }

    public List<Ingredient> GetAllIngredients() {
        return NodeUtil.LoadResourcesFromFolder("res://Data/Ingredients/").OfType<Ingredient>().ToList();
    }
}
