using Godot;
using System;

public partial class CustomCursor : Node {
	public static CustomCursor Instance;

	[Export] private Texture2D cursorArrowTexture = null;
	private Vector2I cursorArrowSize = (Vector2I) Vector2.Zero;

	[Export] private Texture2D cursorPointerTexture = null;
	private Vector2I cursorPointerSize = (Vector2I) Vector2.Zero;

	private Vector2I baseResolution = (Vector2I) Vector2.Zero;

	private CursorData currentCursor;

	public override void _Ready()
	{
		Instance = this;
		cursorArrowSize = (Vector2I) cursorArrowTexture.GetSize();
		cursorPointerSize = (Vector2I) cursorPointerTexture.GetSize();
		baseResolution = (Vector2I) GetViewport().GetVisibleRect().Size;

		currentCursor = new(cursorArrowTexture, cursorArrowSize);
		UpdateCursor();
		GetTree().Root.SizeChanged += UpdateCursor;
		
	}

	private void UpdateCursor() {
		Vector2I windowSize = DisplayServer.WindowGetSize();
		float scale = Mathf.Min(windowSize.X / baseResolution.X, windowSize.Y / baseResolution.Y);

		ImageTexture texture = new();
		Image image = currentCursor.texture.GetImage();
		image.Resize((int) (currentCursor.cursorSize.X * scale), (int) (currentCursor.cursorSize.Y * scale), Image.Interpolation.Nearest);

		texture.SetImage(image);
		Input.SetCustomMouseCursor(texture, Input.CursorShape.Arrow, currentCursor.hotspot * scale);
	}

	public void ChangeCursorType(CursorType cursorType) {
		if(cursorType == CursorType.ARROW) {
			currentCursor.texture = cursorArrowTexture;
			currentCursor.cursorSize = cursorArrowSize;
			currentCursor.hotspot = Vector2.Zero;
		} else {
			currentCursor.texture = cursorPointerTexture;
			currentCursor.cursorSize = cursorPointerSize;
			currentCursor.hotspot = new Vector2(3, 0);
		}

		UpdateCursor();
	}
}

public struct CursorData {
	public Texture2D texture;
	public Vector2I cursorSize;
	public Vector2 hotspot;
	
	public CursorData(Texture2D texture, Vector2I size) {
		this.texture = texture;
		this.cursorSize = size;
		this.hotspot = Vector2.Zero;
	}

	public CursorData(Texture2D texture, Vector2I size, Vector2 hotspot) {
		this.texture = texture;
		this.cursorSize = size;
		this.hotspot = hotspot;
	}
}

public enum CursorType {
	ARROW,
	POINTER
}