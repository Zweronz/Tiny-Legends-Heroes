using UnityEngine;

public class Tween
{
	public enum TweenType
	{
		Linear = 0,
		Cubic = 1,
		Back = 2,
		Bounce = 3
	}

	public static Vector2 CalcCur(Vector2 start, Vector2 end, float fRatio, TweenType type)
	{
		Vector2 result = default(Vector2);
		switch (type)
		{
		case TweenType.Linear:
			return calcLinear(start, end, fRatio);
		case TweenType.Cubic:
			return calcCubicEaseOut(start, end, fRatio);
		case TweenType.Back:
			return clacBackEaseOut(start, end, fRatio);
		default:
			Debug.LogWarning("Unknown tween type can be processed correctly! forced postion to ZERO!");
			return result;
		}
	}

	private static float calcLinear(float start, float end, float fRatio)
	{
		return start + (end - start) * fRatio;
	}

	private static Vector2 calcLinear(Vector2 start, Vector2 end, float fRatio)
	{
		return start + (end - start) * fRatio;
	}

	private static float calcCubicEaseOut(float start, float end, float fRatio)
	{
		float num = fRatio - 1f;
		float num2 = end - start;
		return num2 * (num * num * num + 1f) + start;
	}

	private static Vector2 calcCubicEaseOut(Vector2 start, Vector2 end, float fRatio)
	{
		float num = fRatio - 1f;
		Vector2 vector = end - start;
		return vector * (num * num * num + 1f) + start;
	}

	private static float clacBackEaseOut(float start, float end, float fRatio)
	{
		float num = 1.70158f;
		float num2 = fRatio - 1f;
		float num3 = end - start;
		return num3 * (num2 * num2 * ((num + 1f) * num2 + num) + 1f) + start;
	}

	private static Vector2 clacBackEaseOut(Vector2 start, Vector2 end, float fRatio)
	{
		float num = 1.70158f;
		float num2 = fRatio - 1f;
		Vector2 vector = end - start;
		return vector * (num2 * num2 * ((num + 1f) * num2 + num) + 1f) + start;
	}
}
