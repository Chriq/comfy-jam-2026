using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RecipeBook : Control {
    [Export] Button toggle;
    [Export] RecipePage p1;
    [Export] RecipePage p2;

    [Export] Button left;
    [Export] Button right;

    private int totalPages => (int)Mathf.Ceil(allEntries.Count / 2f) - 1;

    private List<Drink> allEntries = new();

    private int currentPages = 0;

    public override void _Ready() {
        InitList();
        toggle.Pressed += ToggleCompendiumUI;
        InitPages();

        left.Pressed += PageLeft;
        right.Pressed += PageRight;
    }

    private void PageLeft() {
        if (currentPages > 0) {
            currentPages--;
            InitPages();
        }
    }

    private void PageRight() {
        if (currentPages < totalPages) {
            currentPages++;
            InitPages();
        }
    }

    private void ToggleCompendiumUI() {
        Visible = !Visible;
    }

    public void InitPages() {
        int page = currentPages * 2;
        Drink drink1 = allEntries[page];
        p1.InitPage(drink1);

        Drink drink2 = null;
        if (page + 1 < allEntries.Count) {
            drink2 = allEntries[page + 1];
        }
        p2.InitPage(drink2);

        HandlePageButtons();
    }

    private void HandlePageButtons() {
        // page left
        if (currentPages <= 0) {
            left.Disabled = true;
            left.Modulate = new Color(1, 1, 1, 0);
        } else {
            left.Disabled = false;
            left.Modulate = new Color(1, 1, 1, 1);
        }

        // page right
        if (currentPages >= totalPages) {
            right.Disabled = true;
            right.Modulate = new Color(1, 1, 1, 0);
        } else {
            right.Disabled = false;
            right.Modulate = new Color(1, 1, 1, 1);
        }
    }

    private void InitList() {
        allEntries = NodeUtil.LoadResourcesFromFolder("res://Data/Drinks/").OfType<Drink>().ToList();
    }
}
