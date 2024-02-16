using UnityEngine;

public class D3DPlaneGeometry
{
	public static bool SegmentIntersectCircle(Vector2 segment_pt1, Vector2 segment_pt2, Vector2 circle_center, float circle_radius)
	{
		if (Vector2.Distance(segment_pt1, circle_center) <= circle_radius || Vector2.Distance(segment_pt2, circle_center) <= circle_radius)
		{
			return true;
		}
		float num = (segment_pt2.x - segment_pt1.x) * (segment_pt2.x - segment_pt1.x) + (segment_pt2.y - segment_pt1.y) * (segment_pt2.y - segment_pt1.y);
		float num2 = ((segment_pt2.x - segment_pt1.x) * (segment_pt1.x - circle_center.x) + (segment_pt2.y - segment_pt1.y) * (segment_pt1.y - circle_center.y)) * 2f;
		float num3 = circle_center.x * circle_center.x + circle_center.y * circle_center.y + segment_pt1.x * segment_pt1.x + segment_pt1.y * segment_pt1.y - (circle_center.x * segment_pt1.x + circle_center.y * segment_pt1.y) * 2f - circle_radius * circle_radius;
		float num4 = Mathf.Sqrt(num2 * num2 - 4f * num * num3);
		float num5 = (0f - num2 + num4) / (2f * num);
		float num6 = (0f - num2 - num4) / (2f * num);
		if (num5 > 0f && num5 < 1f && num6 > 0f && num6 < 1f)
		{
			return true;
		}
		if (num5 * num6 < 0f)
		{
			return true;
		}
		return false;
	}

	public static bool CircleIntersectCircle(Vector2 circle1_center, float circle1_radius, Vector2 circle2_center, float circle2_radius)
	{
		if (Vector2.Distance(circle1_center, circle2_center) <= circle1_radius + circle2_radius)
		{
			return true;
		}
		return false;
	}

	public static bool CircleIntersectQuads(Vector2 rect_center, float rect_rad, Vector2 rect_size, Vector2 circle_center, float circle_radius)
	{
		float num = circle_radius * 2f;
		Vector2 vector = new Vector2(rect_center.x + (rect_size.x + num) * Mathf.Cos(rect_rad) * 0.5f - (rect_size.y + num) * Mathf.Sin(rect_rad) * 0.5f, rect_center.y - (rect_size.x + num) * Mathf.Sin(rect_rad) * 0.5f - (rect_size.y + num) * Mathf.Cos(rect_rad) * 0.5f);
		Vector2 vector2 = new Vector2(rect_center.x - (rect_size.x + num) * Mathf.Cos(rect_rad) * 0.5f - (rect_size.y + num) * Mathf.Sin(rect_rad) * 0.5f, rect_center.y + (rect_size.x + num) * Mathf.Sin(rect_rad) * 0.5f - (rect_size.y + num) * Mathf.Cos(rect_rad) * 0.5f);
		Vector2 vector3 = new Vector2(rect_center.x + (rect_size.x + num) * Mathf.Cos(rect_rad) * 0.5f + (rect_size.y + num) * Mathf.Sin(rect_rad) * 0.5f, rect_center.y - (rect_size.x + num) * Mathf.Sin(rect_rad) * 0.5f + (rect_size.y + num) * Mathf.Cos(rect_rad) * 0.5f);
		Vector2 vector4 = new Vector2(rect_center.x - (rect_size.x + num) * Mathf.Cos(rect_rad) * 0.5f + (rect_size.y + num) * Mathf.Sin(rect_rad) * 0.5f, rect_center.y + (rect_size.x + num) * Mathf.Sin(rect_rad) * 0.5f + (rect_size.y + num) * Mathf.Cos(rect_rad) * 0.5f);
		float num2 = Vector2CrossMul(vector2 - vector, circle_center - vector);
		float num3 = Vector2CrossMul(vector4 - vector2, circle_center - vector2);
		if (num2 * num3 < 0f)
		{
			return false;
		}
		num3 = Vector2CrossMul(vector3 - vector4, circle_center - vector4);
		if (num2 * num3 < 0f)
		{
			return false;
		}
		num3 = Vector2CrossMul(vector - vector3, circle_center - vector3);
		if (num2 * num3 < 0f)
		{
			return false;
		}
		return true;
	}

	public static bool PointInpolygon(Vector2[] vertex_list, Vector2 point)
	{
		if (vertex_list.Length < 3)
		{
			return false;
		}
		for (int i = 0; i < vertex_list.Length; i++)
		{
			int num = i;
			int num2 = ((i != 0) ? (i - 1) : (vertex_list.Length - 1));
			if (Vector2CrossMul(vertex_list[num] - vertex_list[num2], point - vertex_list[num]) < 0f)
			{
				return false;
			}
		}
		return true;
	}

	public static float Vector2CrossMul(Vector2 v1, Vector2 v2)
	{
		return v1.x * v2.y - v2.x * v1.y;
	}

	public static Vector3 GetPointInSphere(Vector3 center_point, Quaternion direction, float radius)
	{
		Vector3 vector = direction * Vector3.forward;
		return center_point + vector * radius;
	}

	public static bool PtInRect(Rect rect, Vector2 point)
	{
		return point.x >= rect.xMin && point.x < rect.xMax && point.y >= rect.yMin && point.y < rect.yMax;
	}

	public static bool CircleIntersectRect(Vector2 circle_center, float circle_radius, Rect rect)
	{
		float num = Mathf.Abs(circle_center.x - rect.center.x);
		float num2 = Mathf.Abs(circle_center.y - rect.center.y);
		return num <= circle_radius + rect.width * 0.5f && num2 <= circle_radius + rect.height * 0.5f;
	}

	public static float DistanceLine(Vector2 line_point1, Vector2 line_point2, Vector2 point)
	{
		Vector2 vector = line_point2 - line_point1;
		Vector2 rhs = point - line_point1;
		float num = Vector2.Dot(vector, rhs);
		if (num < 0f)
		{
			return Vector2.Distance(point, line_point1);
		}
		float num2 = Vector2.Dot(vector, vector);
		if (num > num2)
		{
			return Vector2.Distance(point, line_point2);
		}
		num /= num2;
		Vector2 b = line_point1 + num * vector;
		return Vector2.Distance(point, b);
	}

	public static Vector2 GetPointOnLine(Vector2 line_point1, Vector2 line_point2, float scale)
	{
		return new Vector2(line_point1.x + (line_point2.x - line_point1.x) * scale, line_point1.y + (line_point2.y - line_point1.y) * scale);
	}

	public static Vector2 GetRandomPointOnLine(Vector2 line_point1, Vector2 line_point2)
	{
		float num = Random.Range(line_point1.x, line_point2.x);
		float num2 = (num - line_point1.x) / (line_point2.x - num);
		float y = (line_point1.y + line_point2.y * num2) / (1f + num2);
		return new Vector2(num, y);
	}
}
