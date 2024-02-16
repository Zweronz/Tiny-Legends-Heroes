using System.Collections.Generic;
using UnityEngine;

public class D3DLootTreasure : D3DComplexSlotUI
{
	private UIImage grade_light_mask;

	private D3DUIImageAnimation treasure_animation;

	private UIClickButton treasure_button;

	private D3DCurrencyText treasure_cost;

	private D3DCurrencyText gold_bag;

	private int gold_bag_value;

	private D3DBattleResultUI result_ui;

	private bool grade_mask_use;

	private Vector2 shuff_target;

	private Vector2 shuff_velocity;

	private Vector2 shuff_direction;

	public D3DUIImageAnimation TreasureAnimation
	{
		get
		{
			return treasure_animation;
		}
	}

	public UIClickButton TreasureButton
	{
		get
		{
			return treasure_button;
		}
	}

	public Vector2 ShuffTarget
	{
		get
		{
			return shuff_target;
		}
	}

	public Vector2 ShuffVelocity
	{
		get
		{
			return shuff_velocity;
		}
	}

	public Vector2 ShuffDirection
	{
		get
		{
			return shuff_direction;
		}
	}

	public D3DLootTreasure(UIManager manager, UIHelper helper)
		: base(manager, helper)
	{
	}

	public void CreateControl(Vector2 position)
	{
		ui_position = position;
		grade_mask_use = false;
		treasure_animation = new D3DUIImageAnimation(ui_manager, ui_helper, new Rect(ui_position.x, ui_position.y, 70f, 68f));
		List<D3DImageCell> list = new List<D3DImageCell>();
		D3DImageCell imageCell;
		for (int i = 1; i < 7; i++)
		{
			imageCell = ui_helper.GetImageCell("box0" + i);
			list.Add(imageCell);
		}
		treasure_animation.Cells = list;
		treasure_animation.Rate = 18f;
		treasure_animation.Loop = false;
		slot_hover_mask = new UIImage();
		imageCell = ui_helper.GetImageCell("ring");
		slot_hover_mask.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		slot_hover_mask.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x - 20.5f, ui_position.y + 18.5f, 111.5f, 82.5f));
		slot_hover_mask.Enable = false;
		slot_hover_mask.Visible = false;
		ui_manager.Add(slot_hover_mask);
		grade_light_mask = new UIImage();
		imageCell = ui_helper.GetImageCell("ring01");
		grade_light_mask.SetTexture(ui_helper.LoadUIMaterialAutoHD(imageCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(imageCell.cell_rect));
		grade_light_mask.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x - 20.5f, ui_position.y + 18.5f, 111.5f, 82.5f));
		grade_light_mask.Enable = false;
		grade_light_mask.Visible = false;
		grade_light_mask.SetAlpha(0.7f);
		ui_manager.Add(grade_light_mask);
		slot_icon = new UIImage();
		slot_icon.Enable = false;
		slot_icon.Visible = false;
		slot_icon.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + 14.5f, ui_position.y + 25f, 41f, 41f));
		ui_manager.Add(slot_icon);
		treasure_button = new UIClickButton();
		treasure_button.Rect = treasure_animation.AnimationControl.Rect;
		treasure_button.Enable = false;
		ui_manager.Add(treasure_button);
		treasure_cost = new D3DCurrencyText(ui_manager, ui_helper);
		int[] drawCost = D3DBattleRule.Instance.GetDrawCost(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id, D3DMain.Instance.exploring_dungeon.current_floor);
		if (drawCost[0] == 0)
		{
			treasure_cost.EnableGold = false;
		}
		if (drawCost[1] == 0)
		{
			treasure_cost.EnableCrystal = false;
		}
		treasure_cost.SetCurrency(drawCost[0], drawCost[1]);
		treasure_cost.SetColor(Color.red);
		treasure_cost.SetPosition(new Vector2(((ui_position.x + 35f) * (float)D3DMain.Instance.HD_SIZE - treasure_cost.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), ui_position.y - 18f));
		treasure_cost.Visible(false);
		gold_bag = null;
	}

	public void UpdateLootUIGear(D3DEquipment gear)
	{
		treasure_cost.Visible(false);
		D3DImageCell iconCell = ui_helper.GetIconCell(gear.use_icon);
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		slot_icon.SetTextureSize(new Vector2(slot_icon.Rect.width, slot_icon.Rect.height));
		if (gear.equipment_grade >= D3DEquipment.EquipmentGrade.SUPERIOR)
		{
			slot_hover_mask.SetColor(D3DMain.Instance.GetEquipmentGradeColor(gear.equipment_grade));
			slot_hover_mask.SetAlpha(0.7f);
			grade_mask_use = true;
		}
		else
		{
			slot_hover_mask.Visible = false;
			grade_light_mask.Visible = false;
			grade_mask_use = false;
		}
	}

	public void UpdateGoldBag(D3DEquipment.EquipmentGrade grade)
	{
		treasure_cost.Visible(false);
		D3DImageCell iconCell;
		switch (grade)
		{
		case D3DEquipment.EquipmentGrade.NORMAL:
			iconCell = ui_helper.GetIconCell("gold01");
			break;
		case D3DEquipment.EquipmentGrade.SUPERIOR:
			iconCell = ui_helper.GetIconCell("gold02");
			break;
		default:
			iconCell = ui_helper.GetIconCell("gold03");
			break;
		}
		grade_mask_use = false;
		slot_hover_mask.Visible = false;
		grade_light_mask.Visible = false;
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		slot_icon.SetTextureSize(new Vector2(slot_icon.Rect.width, slot_icon.Rect.height));
	}

	public void UpdateGoldBag(D3DEquipment.EquipmentGrade grade, int gold, D3DBattleResultUI result_ui)
	{
		treasure_cost.Visible(false);
		D3DImageCell iconCell;
		switch (grade)
		{
		case D3DEquipment.EquipmentGrade.NORMAL:
			iconCell = ui_helper.GetIconCell("gold01");
			break;
		case D3DEquipment.EquipmentGrade.SUPERIOR:
			iconCell = ui_helper.GetIconCell("gold02");
			break;
		default:
			iconCell = ui_helper.GetIconCell("gold03");
			break;
		}
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		slot_icon.SetTextureSize(new Vector2(slot_icon.Rect.width, slot_icon.Rect.height));
		grade_mask_use = false;
		slot_hover_mask.Visible = false;
		grade_light_mask.Visible = false;
		gold_bag_value = gold;
		gold_bag = new D3DCurrencyText(ui_manager, ui_helper);
		gold_bag.EnableCrystal = false;
		gold_bag.SetGold(gold_bag_value);
		gold_bag.SetPosition(new Vector2(((ui_position.x + 35f) * (float)D3DMain.Instance.HD_SIZE - treasure_cost.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), ui_position.y + 62f));
		gold_bag.Visible(false);
		this.result_ui = result_ui;
	}

	public void UpdateCrystal()
	{
		treasure_cost.Visible(false);
		D3DImageCell iconCell = ui_helper.GetIconCell("shuijing");
		grade_mask_use = false;
		slot_hover_mask.Visible = false;
		grade_light_mask.Visible = false;
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		slot_icon.SetTextureSize(new Vector2(15f, 24f) * D3DMain.Instance.HD_SIZE);
	}

	public void UpdateCrystal(D3DBattleResultUI result_ui)
	{
		treasure_cost.Visible(false);
		D3DImageCell iconCell = ui_helper.GetIconCell("shuijing");
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		slot_icon.SetTextureSize(new Vector2(15f, 24f) * D3DMain.Instance.HD_SIZE);
		grade_mask_use = false;
		slot_hover_mask.Visible = false;
		grade_light_mask.Visible = false;
		gold_bag_value = 2;
		gold_bag = new D3DCurrencyText(ui_manager, ui_helper);
		gold_bag.EnableGold = false;
		gold_bag.SetCrystal(2);
		gold_bag.SetPosition(new Vector2(((ui_position.x + 35f) * (float)D3DMain.Instance.HD_SIZE - treasure_cost.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), ui_position.y + 62f));
		gold_bag.Visible(false);
		this.result_ui = result_ui;
	}

	public void SetPosition(Vector2 position)
	{
		ui_position = position;
		treasure_animation.AnimationControl.Rect = new Rect(ui_position.x * (float)D3DMain.Instance.HD_SIZE, ui_position.y * (float)D3DMain.Instance.HD_SIZE, treasure_animation.AnimationControl.Rect.width, treasure_animation.AnimationControl.Rect.height);
		treasure_button.Rect = treasure_animation.AnimationControl.Rect;
		treasure_cost.SetPosition(new Vector2(((ui_position.x + 35f) * (float)D3DMain.Instance.HD_SIZE - treasure_cost.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), ui_position.y - 18f));
		if (gold_bag != null)
		{
			gold_bag.SetPosition(new Vector2(((ui_position.x + 35f) * (float)D3DMain.Instance.HD_SIZE - gold_bag.GetUIWidth() * 0.5f) * (1f / (float)D3DMain.Instance.HD_SIZE), ui_position.y + 62f));
		}
	}

	public void SetGearUILocalPosition(Vector2 local_position)
	{
		slot_hover_mask.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + local_position.x - 35f, ui_position.y + local_position.y - 6.5f, 111.5f, 82.5f));
		grade_light_mask.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + local_position.x - 35f, ui_position.y + local_position.y - 6.5f, 111.5f, 82.5f));
		slot_icon.Rect = D3DMain.Instance.ConvertRectAutoHD(new Rect(ui_position.x + local_position.x, ui_position.y + local_position.y, 41f, 41f));
	}

	public void UpdateTreasureCost()
	{
		treasure_cost.Visible(true);
		int[] drawCost = D3DBattleRule.Instance.GetDrawCost(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id, D3DMain.Instance.exploring_dungeon.current_floor);
		treasure_cost.SetColor(Color.red);
		treasure_cost.EnableGold = 0 != drawCost[0];
		treasure_cost.EnableCrystal = 0 != drawCost[1];
		if (int.Parse(D3DGamer.Instance.CurrencyText) < drawCost[0])
		{
			treasure_cost.SetGoldColor(Color.gray);
		}
		if (int.Parse(D3DGamer.Instance.CrystalText) < drawCost[1])
		{
			treasure_cost.SetCrystalColor(Color.gray);
		}
	}

	public void OnTreaureOpen()
	{
		slot_icon.Visible = true;
		if (!grade_mask_use)
		{
			slot_hover_mask.Visible = false;
			grade_light_mask.Visible = false;
		}
		else
		{
			slot_hover_mask.Visible = true;
			grade_light_mask.Visible = true;
		}
		if (gold_bag != null)
		{
			gold_bag.Visible(true);
			if (gold_bag.EnableCrystal)
			{
				D3DGamer.Instance.UpdateCrystal(2);
			}
			else
			{
				D3DGamer.Instance.UpdateCurrency(gold_bag_value);
			}
			result_ui.UpdateCurrency();
			D3DGamer.Instance.SaveAllData();
		}
		treasure_cost.Visible(false);
	}

	public void OpenTreaureByNewRule()
	{
		slot_icon.Visible = true;
		if (!grade_mask_use)
		{
			slot_hover_mask.Visible = false;
			grade_light_mask.Visible = false;
		}
		else
		{
			slot_hover_mask.Visible = true;
			grade_light_mask.Visible = true;
		}
		treasure_cost.Visible(false);
		treasure_animation.AnimationControl.Visible = false;
		treasure_button.Enable = false;
	}

	private void UpdateGearSlot(D3DEquipment gear)
	{
		D3DImageCell iconCell = ui_helper.GetIconCell(gear.use_icon);
		slot_icon.SetTexture(ui_helper.LoadUIMaterialAutoHD(iconCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(iconCell.cell_rect));
		slot_icon.SetTextureSize(new Vector2(slot_icon.Rect.width, slot_icon.Rect.height));
		slot_icon.Visible = true;
		if (gear.equipment_grade >= D3DEquipment.EquipmentGrade.SUPERIOR)
		{
			slot_hover_mask.SetColor(D3DMain.Instance.GetEquipmentGradeColor(gear.equipment_grade));
			slot_hover_mask.SetAlpha(0.7f);
			slot_hover_mask.Visible = true;
			grade_light_mask.Visible = true;
		}
		else
		{
			slot_hover_mask.Visible = false;
			grade_light_mask.Visible = false;
		}
		treasure_button.Enable = false;
	}

	public void SetGearVisible(bool visible)
	{
		if (!grade_mask_use)
		{
			slot_hover_mask.Visible = false;
			grade_light_mask.Visible = false;
		}
		else
		{
			slot_hover_mask.Visible = visible;
			grade_light_mask.Visible = visible;
		}
		slot_icon.Visible = visible;
	}

	public void SetShuffTarget(Vector2 target)
	{
		shuff_target = target;
		Vector2 vector = new Vector2(TreasureAnimation.AnimationControl.Rect.x, TreasureAnimation.AnimationControl.Rect.y);
		shuff_velocity = new Vector2((shuff_target.x - vector.x) / 0.1f, (shuff_target.y - vector.y) / 0.1f);
		shuff_direction = shuff_target - vector;
	}
}
