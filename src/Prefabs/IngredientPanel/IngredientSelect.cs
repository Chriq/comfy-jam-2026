using Godot;
using System;

public partial class IngredientSelect : Control {
    [Export] public RichTextLabel nameLabel;
    [Export] public TextureRect ingredientDisplay;
    [Export] public SpinBox amountInput;
}
