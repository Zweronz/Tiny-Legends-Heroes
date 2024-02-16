using UnityEngine;

public class GuideBasic : MonoBehaviour
{
	protected UIManager manager;

	protected UIImage DialogBoard;

	protected UIImage Mascot;

	protected Material[] MascotStateMat;

	protected Rect[] MascotStateTextureRect;

	protected UIImage DialogNext;

	protected UIImage ButtonHighLight;

	protected UIImage HitFinger;

	protected UIClickButton DialogButton;

	protected UIText DialogText;

	protected float Rad;

	protected int finger_state;

	protected Vector2[] finger_move_points;

	protected int move_index;

	private float last_real_time;

	private float current_real_time;

	private float delta_real_time;

	private void Start()
	{
		last_real_time = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		current_real_time = Time.realtimeSinceStartup;
		delta_real_time = current_real_time - last_real_time;
		last_real_time = current_real_time;
		Rad += delta_real_time * 10f;
		DialogNext.SetPosition(DialogNext.GetPosition() + Vector2.up * Mathf.Cos(Rad) * 0.3f);
		ButtonHighLight.SetAlpha(Mathf.Abs(Mathf.Sin(Rad)));
		if (finger_state == 0)
		{
			HitFinger.SetScale(1f + Mathf.Sin(Rad) * 0.1f);
		}
		else if (finger_state != 1)
		{
		}
	}

	public void InitGuide(UIHelper helper, UIManager manager)
	{
		this.manager = manager;
		D3DImageCell imageCell = helper.GetImageCell("anniu1-1");
		ButtonHighLight = new UIImage();
		ButtonHighLight.Enable = false;
		ButtonHighLight.SetTexture(helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		ButtonHighLight.Rect = D3DMain.Instance.ConvertRectAutoHD(70.5f, 0f, 409.5f, 62f);
		this.manager.Add(ButtonHighLight);
		imageCell = helper.GetImageCell("hand");
		HitFinger = new UIImage();
		HitFinger.Enable = false;
		HitFinger.SetTexture(helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		HitFinger.Rect = D3DMain.Instance.ConvertRectAutoHD(70.5f, 0f, 409.5f, 62f);
		this.manager.Add(HitFinger);
		imageCell = helper.GetImageCell("ui_monolayer");
		DialogButton = new UIClickButton();
		DialogButton.SetTexture(UIButtonBase.State.Normal, helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(480f, 320f) * D3DMain.Instance.HD_SIZE);
		DialogButton.SetTexture(UIButtonBase.State.Pressed, helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(480f, 320f) * D3DMain.Instance.HD_SIZE);
		DialogButton.SetTexture(UIButtonBase.State.Disabled, helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect), new Vector2(480f, 320f) * D3DMain.Instance.HD_SIZE);
		DialogButton.SetColor(UIButtonBase.State.Normal, new Color(0f, 0f, 0f, 0.6f));
		DialogButton.SetColor(UIButtonBase.State.Pressed, new Color(0f, 0f, 0f, 0.6f));
		DialogButton.SetColor(UIButtonBase.State.Disabled, new Color(0f, 0f, 0f, 0.6f));
		DialogButton.Rect = D3DMain.Instance.ConvertRectAutoHD(0f, 0f, 480f, 320f);
		this.manager.Add(DialogButton);
		imageCell = helper.GetImageCell("npc04");
		DialogBoard = new UIImage();
		DialogBoard.Enable = false;
		DialogBoard.SetTexture(helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		DialogBoard.Rect = D3DMain.Instance.ConvertRectAutoHD(70.5f, 0f, 409.5f, 62f);
		this.manager.Add(DialogBoard);
		DialogText = new UIText();
		DialogText.Set(helper.LoadFontAutoHD(D3DMain.Instance.GameFont1.FontName, 7), "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvccccccccccccccccccccccccc", D3DMain.Instance.CommonFontColor);
		DialogText.AutoLine = true;
		DialogText.Enable = false;
		DialogText.LineSpacing = D3DMain.Instance.GameFont1.GetLineSpacing(7 * D3DMain.Instance.HD_SIZE);
		DialogText.CharacterSpacing = D3DMain.Instance.GameFont1.GetCharSpacing(7 * D3DMain.Instance.HD_SIZE);
		DialogText.Rect = D3DMain.Instance.ConvertRectAutoHD(165f, 5f, 285f, 50f);
		this.manager.Add(DialogText);
		MascotStateMat = new Material[3]
		{
			helper.LoadUIMaterialAutoHD(helper.GetImageCell("npc01").cell_texture),
			helper.LoadUIMaterialAutoHD(helper.GetImageCell("npc02").cell_texture),
			helper.LoadUIMaterialAutoHD(helper.GetImageCell("npc03").cell_texture)
		};
		MascotStateTextureRect = new Rect[3]
		{
			D3DMain.Instance.ConvertRectAutoHD(helper.GetImageCell("npc01").cell_rect),
			D3DMain.Instance.ConvertRectAutoHD(helper.GetImageCell("npc02").cell_rect),
			D3DMain.Instance.ConvertRectAutoHD(helper.GetImageCell("npc03").cell_rect)
		};
		Mascot = new UIImage();
		Mascot.Enable = false;
		this.manager.Add(Mascot);
		DialogNext = new UIImage();
		imageCell = helper.GetImageCell("tishijiantou");
		DialogNext.SetTexture(helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		DialogNext.Rect = D3DMain.Instance.ConvertRectAutoHD(438f, 2f, 40f, 30f);
		DialogNext.Enable = false;
		this.manager.Add(DialogNext);
		SetMascotDialogVisible(false);
		HighLightButton(D3DMain.Instance.ConvertRectAutoHD(65f, 10f, 84f, 37f));
		SetHintFingerPoint(new Vector2(30f, 30f) * D3DMain.Instance.HD_SIZE);
	}

	public void SetMascotDialog(int mascot_state)
	{
		Mascot.SetTexture(MascotStateMat[mascot_state], MascotStateTextureRect[mascot_state], new Vector2(MascotStateTextureRect[mascot_state].width, MascotStateTextureRect[mascot_state].height));
		Mascot.Rect = new Rect(0f, 0f, MascotStateTextureRect[mascot_state].width, MascotStateTextureRect[mascot_state].height);
	}

	public void SetMascotDialogVisible(bool visible)
	{
		DialogBoard.Visible = visible;
		DialogNext.Visible = visible;
		Mascot.Visible = visible;
		DialogButton.Visible = visible;
		DialogText.Visible = visible;
		DialogButton.Enable = visible;
	}

	public void HighLightButton(Rect high_light_rect)
	{
		ButtonHighLight.Rect = high_light_rect;
		ButtonHighLight.SetTextureSize(new Vector2(high_light_rect.width, high_light_rect.height));
	}

	public void SetButtonHighLightVisible(bool visible)
	{
		ButtonHighLight.Visible = visible;
	}

	public void SetHintFingerPoint(Vector2 point)
	{
		finger_state = 0;
		HitFinger.SetScale(1f);
		HitFinger.SetPosition(point);
	}

	public void SetHintFingerMovePath(Vector2[] points)
	{
		finger_move_points = points;
		finger_state = 1;
		HitFinger.SetScale(1f);
		HitFinger.SetPosition(finger_move_points[0]);
		move_index = 0;
	}

	public void SetHintFingerVisible(bool visible)
	{
		HitFinger.Visible = visible;
	}
}
