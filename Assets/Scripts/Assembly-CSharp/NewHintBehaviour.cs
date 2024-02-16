using System;
using System.Collections.Generic;
using UnityEngine;

public class NewHintBehaviour : MonoBehaviour
{
	public List<UIImage> HintImages = new List<UIImage>();

	private float last_real_time;

	private float current_real_time;

	private float delta_real_time;

	private float flash_deg;

	private void Start()
	{
	}

	private void Update()
	{
		current_real_time = Time.realtimeSinceStartup;
		delta_real_time = current_real_time - last_real_time;
		last_real_time = current_real_time;
		foreach (UIImage hintImage in HintImages)
		{
			hintImage.SetAlpha(Mathf.Abs(Mathf.Cos(flash_deg * ((float)Math.PI / 180f))) * 0.5f + 0.5f);
		}
		flash_deg += 120f * delta_real_time;
	}

	public void RemoveHintImage(UIImage image)
	{
		if (HintImages.Contains(image))
		{
			HintImages.Remove(image);
		}
	}

	public void AddHintImage(UIImage image)
	{
		if (!HintImages.Contains(image))
		{
			HintImages.Add(image);
		}
	}

	public void AddHintImage(List<UIImage> images)
	{
		if (images == null)
		{
			return;
		}
		foreach (UIImage image in images)
		{
			if (!HintImages.Contains(image))
			{
				HintImages.Add(image);
			}
		}
	}
}
