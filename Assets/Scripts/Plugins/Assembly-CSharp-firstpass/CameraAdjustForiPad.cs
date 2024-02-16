using UnityEngine;

public class CameraAdjustForiPad : MonoBehaviour
{
	public bool bFullScreen;

	private void Awake()
	{
		if (!bFullScreen)
		{
			Camera camera = base.GetComponent<Camera>();
			float width = ((!(camera.pixelRect.width > (float)Utils.StandardScreenSize[0])) ? camera.pixelRect.width : ((float)Utils.StandardScreenSize[0]));
			float height = ((!(camera.pixelRect.height > (float)Utils.StandardScreenSize[1])) ? camera.pixelRect.height : ((float)Utils.StandardScreenSize[1]));
			if (GameScreen.width >= Utils.StandardScreenSize[0] && GameScreen.height >= Utils.StandardScreenSize[1])
			{
				camera.pixelRect = new Rect((float)(GameScreen.width - Utils.StandardScreenSize[0]) * 0.5f, (float)(GameScreen.height - Utils.StandardScreenSize[1]) * 0.5f, width, height);
			}
		}
	}
}
