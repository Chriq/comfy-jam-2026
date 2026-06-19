using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RecipeBook : Control {
	[Export] Button toggle;

	[Export] Control p1Container;
	[Export] Control p2Container;

	[Export] RecipePage p1;
	[Export] RecipePage p2;

	[Export] Button left;
	[Export] Button right;

	[Export] PackedScene recipePagePrefab;

	private int totalPages => (int)Mathf.Ceil(allEntries.Count / 2f) - 1;

	private List<Drink> allEntries = new();

	private int currentPages = 0;

	private Dictionary<Drink, RecipePage> pages = new();

	public override void _Ready() {
		InitList();
		toggle.Pressed += ToggleCompendiumUI;

		foreach(Drink d in allEntries) {
			RecipePage p = recipePagePrefab.Instantiate<RecipePage>();
			p.InitPage(d);

			pages[d] = p;
		}

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

		foreach(Node child in p1Container.GetChildren()) p1Container.RemoveChild(child);
		p1Container.AddChild(pages[drink1]);
		p1 = pages[drink1];

		Drink drink2 = null;
		foreach(Node child in p2Container.GetChildren()) p2Container.RemoveChild(child);

		if (page + 1 < allEntries.Count) {
			drink2 = allEntries[page + 1];
			p2Container.AddChild(pages[drink2]);
			p2 = pages[drink2];
		}

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
