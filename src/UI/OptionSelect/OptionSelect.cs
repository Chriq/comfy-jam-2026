using Godot;
using System;

public partial class OptionSelect : ItemList {
	[Export] float maxWidth = 150f;

    public void Populate(string[] options, bool clearPrevious = true) {
		if(clearPrevious) ClearOptions();
		float maxX = 0;
		float totalY = 0;
		for(int i = 0; i < options.Length; i++) {
			AddItem(options[i]);
			Font f = GetThemeDefaultFont();
			Vector2 size = f.GetStringSize(options[i]);
			if(size.X > maxX) maxX = size.X;
			totalY += size.Y;

			SetItemTooltipEnabled(i, false);
		}

		//float x = Mathf.Clamp(maxX, 60f, maxWidth);
		CustomMinimumSize = new Vector2(maxX, totalY);
		FixedColumnWidth = (int) maxX;
	}

	public void ClearOptions() {
		int count = ItemCount;
		for(int i = count-1; i >= 0; i--) {
			RemoveItem(i);
		}
	}
}
