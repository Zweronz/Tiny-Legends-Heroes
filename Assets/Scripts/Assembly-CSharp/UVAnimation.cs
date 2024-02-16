using UnityEngine;

public class UVAnimation : MonoBehaviour
{
	private bool sleep = true;

	private bool loop;

	private bool autoHide = true;

	private Vector2 offsetSpeed;

	private Material objMat;

	private Vector2 matOffset;

	private void Awake()
	{
		objMat = base.gameObject.GetComponent<Renderer>().material;
		sleep = true;
		matOffset = Vector2.zero;
		offsetSpeed = Vector2.zero;
	}

	private void Update()
	{
		if (sleep || null == objMat)
		{
			return;
		}
		matOffset.x += offsetSpeed.x * Time.deltaTime;
		if (!loop && matOffset.x >= 1f)
		{
			matOffset.x = 1f;
			offsetSpeed.x = 0f;
		}
		matOffset.y += offsetSpeed.y * Time.deltaTime;
		if (!loop && matOffset.y >= 1f)
		{
			matOffset.y = 1f;
			offsetSpeed.y = 0f;
		}
		objMat.SetTextureOffset("_MainTex", matOffset);
		if (object.Equals(offsetSpeed, Vector2.zero))
		{
			sleep = true;
			if (autoHide)
			{
				base.gameObject.SetActiveRecursively(false);
			}
		}
	}

	public void StartUVAnimation(Vector2 speed, bool loop, bool autoHide, bool reset)
	{
		if (!(null == objMat))
		{
			sleep = false;
			this.loop = loop;
			this.autoHide = autoHide;
			offsetSpeed.x = speed.x;
			offsetSpeed.y = speed.y;
			if (reset)
			{
				matOffset = Vector2.zero;
				objMat.SetTextureOffset("_MainTex", Vector2.zero);
			}
			base.gameObject.SetActiveRecursively(true);
		}
	}
}
