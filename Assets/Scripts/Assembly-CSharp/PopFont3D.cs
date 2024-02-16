using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class PopFont3D : MonoBehaviour
{
	private Mesh _mesh;

	private Material _mat;

	private Color font_color = Color.white;

	private int _texWidth;

	private int _texHeight;

	private int _cellWidth;

	private int _cellHeight;

	private int _cellxOffset;

	private int _cellyOffset;

	public ArrayList _widths = new ArrayList();

	private float _space;

	private float _scale = 0.02f;

	private BoardType _type;

	private float raiseUpSpeed = 3.5f;

	private float raiseUpTime;

	private float RaiseUpTime = 0.6f;

	private float default_scale;

	private float critical_rate;

	private int critical_state;

	private float jumpUpSpeed = 6f;

	private float jumpUpTime;

	private float JumpUpTime = 1f;

	private float jumpUpGravity = 12f;

	private float jumpXSpeed;

	private void Start()
	{
		jumpXSpeed = (float)Random.Range(-5, 5) * 0.2f;
	}

	private void SetMesh()
	{
		MeshFilter meshFilter = base.gameObject.GetComponent("MeshFilter") as MeshFilter;
		meshFilter.mesh = new Mesh();
		_mesh = meshFilter.mesh;
	}

	public void SetFont(string fontName, int fontSize, Color font_color)
	{
		SetMesh();
		_mat = Resources.Load("Dungeons3D/Fonts/" + fontName + fontSize + "_M") as Material;
		_mat = new Material(_mat);
		if (_mat == null)
		{
			return;
		}
		MeshRenderer meshRenderer = base.gameObject.GetComponent("MeshRenderer") as MeshRenderer;
		meshRenderer.material = _mat;
		this.font_color = font_color;
		TextAsset textAsset = Resources.Load("Dungeons3D/Fonts/" + fontName + fontSize + "_cfg") as TextAsset;
		if (textAsset != null && textAsset.text != null)
		{
			string[] array = textAsset.text.Split('\n');
			string[] array2 = array[0].Split(' ');
			_texWidth = int.Parse(array2[0]);
			_texHeight = int.Parse(array2[1]);
			_cellWidth = int.Parse(array2[2]);
			_cellHeight = int.Parse(array2[3]);
			_cellxOffset = int.Parse(array2[4]);
			_cellyOffset = int.Parse(array2[5]);
			string[] array3 = array[1].Split(' ');
			for (int i = 0; i < array3.Length; i++)
			{
				_widths.Add(float.Parse(array3[i]));
			}
		}
	}

	public void SetString(string text, float characterSpace, BoardType curType, float scale)
	{
		_space = characterSpace;
		_type = curType;
		Vector2[] array = new Vector2[text.Length];
		Vector2[] array2 = new Vector2[text.Length];
		Vector2[] array3 = new Vector2[text.Length];
		Vector2[] array4 = new Vector2[text.Length];
		float num = 0f;
		int num2 = _texWidth / _cellWidth;
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			int num3 = c - 32;
			int num4 = num3 % num2;
			int num5 = num3 / num2;
			float num6 = num4 * _cellWidth;
			float num7 = num5 * _cellHeight;
			float num8 = (float)_widths[num3] + (float)(_cellxOffset * 2);
			float num9 = 0f - num - num8 * 0.5f;
			float num10 = 1f / (float)_texWidth;
			float num11 = 1f / (float)_texHeight;
			float num12 = (float)_texHeight / (float)_texWidth;
			array[i] = new Vector2(num9 * _scale, 0f);
			array2[i] = new Vector2(num8 * _scale, (float)_cellHeight * num12 * _scale);
			array3[i] = new Vector2(num6 * num10, 1f - num7 * num11);
			array4[i] = new Vector2(num8 * num10, (float)_cellHeight * num11);
			num += num8 + _space;
		}
		num -= _space;
		addMesh(array, array2, array3, array4, num * 0.5f * _scale);
		if (_type == BoardType.HitNumberCritical || _type == BoardType.NewSkill)
		{
			default_scale = scale;
			critical_rate = (default_scale * 3f - scale) / 0.1f;
			critical_state = 1;
			base.transform.position += Vector3.up * 1.5f;
		}
		base.transform.localScale = new Vector3(scale, scale, 1f);
	}

	private void addMesh(Vector2[] pos, Vector2[] size, Vector2[] uvp, Vector2[] uvs, float halfWidth)
	{
		int num = pos.Length;
		Vector3[] array = new Vector3[num * 4];
		Vector3[] array2 = new Vector3[num * 4];
		Color[] array3 = new Color[num * 4];
		Vector2[] array4 = new Vector2[num * 4];
		int[] array5 = new int[num * 6];
		for (int i = 0; i < num; i++)
		{
			array[i * 4] = new Vector3(pos[i].x + size[i].x * 0.5f + halfWidth, pos[i].y + size[i].y * 0.5f, 0f);
			array[i * 4 + 1] = new Vector3(pos[i].x + size[i].x * 0.5f + halfWidth, pos[i].y - size[i].y * 0.5f, 0f);
			array[i * 4 + 2] = new Vector3(pos[i].x - size[i].x * 0.5f + halfWidth, pos[i].y + size[i].y * 0.5f, 0f);
			array[i * 4 + 3] = new Vector3(pos[i].x - size[i].x * 0.5f + halfWidth, pos[i].y - size[i].y * 0.5f, 0f);
			array4[i * 4] = new Vector2(uvp[i].x, uvp[i].y);
			array4[i * 4 + 1] = new Vector2(uvp[i].x, uvp[i].y - uvs[i].y);
			array4[i * 4 + 2] = new Vector2(uvp[i].x + uvs[i].x, uvp[i].y);
			array4[i * 4 + 3] = new Vector2(uvp[i].x + uvs[i].x, uvp[i].y - uvs[i].y);
			array3[i * 4] = font_color;
			array3[i * 4 + 1] = font_color;
			array3[i * 4 + 2] = font_color;
			array3[i * 4 + 3] = font_color;
			array2[i * 4] = new Vector3(0f, 0f, 1f);
			array2[i * 4 + 1] = new Vector3(0f, 0f, 1f);
			array2[i * 4 + 2] = new Vector3(0f, 0f, 1f);
			array2[i * 4 + 3] = new Vector3(0f, 0f, 1f);
			array5[i * 6] = i * 4;
			array5[i * 6 + 1] = i * 4 + 1;
			array5[i * 6 + 2] = i * 4 + 2;
			array5[i * 6 + 3] = i * 4 + 2;
			array5[i * 6 + 4] = i * 4 + 1;
			array5[i * 6 + 5] = i * 4 + 3;
		}
		_mesh.Clear();
		_mesh.vertices = array;
		_mesh.uv = array4;
		_mesh.colors = array3;
		_mesh.normals = array2;
		_mesh.triangles = array5;
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(360f - Camera.main.transform.rotation.eulerAngles.x, 0f, 0f);
		switch (_type)
		{
		case BoardType.HitNumberRaise:
			raiseUpTime += Time.deltaTime;
			if (raiseUpTime > RaiseUpTime)
			{
				_mesh.Clear();
				Object.DestroyObject(base.gameObject);
			}
			else
			{
				Vector3 position2 = base.transform.position;
				position2.y += raiseUpSpeed * Time.deltaTime;
				base.transform.position = position2;
			}
			break;
		case BoardType.HitNumberCritical:
			if (critical_state != 0)
			{
				float x = base.transform.localScale.x;
				if (critical_state == 1)
				{
					x += Time.deltaTime * critical_rate;
					if (x >= default_scale * 2f)
					{
						x = default_scale * 2f;
						critical_state = -1;
					}
				}
				else
				{
					x -= Time.deltaTime * critical_rate;
					if (x <= default_scale)
					{
						x = default_scale;
						critical_state = 0;
					}
				}
				base.transform.localScale = new Vector3(x, x, 1f);
			}
			raiseUpTime += Time.deltaTime;
			if (raiseUpTime > RaiseUpTime)
			{
				_mesh.Clear();
				Object.DestroyObject(base.gameObject);
			}
			break;
		case BoardType.NewSkill:
		{
			raiseUpTime += Time.deltaTime;
			if (!(raiseUpTime > 0.8f) || critical_state == 0)
			{
				break;
			}
			float x2 = base.transform.localScale.x;
			if (critical_state == 1)
			{
				x2 += Time.deltaTime * critical_rate * 0.25f;
				if (x2 >= default_scale * 2f)
				{
					x2 = default_scale * 2f;
					critical_state = -1;
				}
			}
			else
			{
				x2 -= Time.deltaTime * critical_rate;
				if (x2 <= default_scale)
				{
					x2 = default_scale;
					critical_state = 0;
				}
			}
			base.transform.localScale = new Vector3(x2, x2, 1f);
			break;
		}
		case BoardType.HitNumberJump:
		{
			jumpUpTime += Time.deltaTime;
			if (jumpUpTime > JumpUpTime)
			{
				_mesh.Clear();
				Object.DestroyObject(base.gameObject);
				break;
			}
			Vector3 position = base.transform.position;
			position.y += jumpUpSpeed * Time.deltaTime;
			position.x += jumpXSpeed * Time.deltaTime;
			base.transform.position = position;
			jumpUpSpeed -= jumpUpGravity * Time.deltaTime;
			break;
		}
		}
	}

	public void SetRaiseParameter(float speed, float time)
	{
		raiseUpSpeed = speed;
		RaiseUpTime = time;
	}
}
