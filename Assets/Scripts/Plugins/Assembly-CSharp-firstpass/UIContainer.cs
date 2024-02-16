public interface UIContainer
{
	void DrawSprite(TUISprite sprite);

	void SendEvent(UIControl control, int command, float wparam, float lparam);
}
