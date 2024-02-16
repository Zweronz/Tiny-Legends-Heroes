using System.Collections.Generic;
using UnityEngine;

public class D3DDungeonPortalRemakeUI : D3DScrollManager
{
	public class FloorButton
	{
		public int floor_index;

		public D3DImageCell[] button_cells;

		public UIImage button_img;

		public List<UIImage> index_number;

		public bool TapButton(Vector2 position)
		{
			if (button_img.PtInRect(position))
			{
				return true;
			}
			return false;
		}
	}

	public delegate void DungeonPortal(int portal_level);

	private bool button_click;

	private DungeonPortal dungeonPortal;

	private Dictionary<string, D3DImageCell> teleport_cell_cfgs;

	public List<FloorButton> FloorButtons = new List<FloorButton>();

	private float map_scale = 1f;

	private int last_floor = -1;

	public D3DDungeonPortalRemakeUI(UIManager manager, UIHelper helper, Rect camera_view_port, DungeonPortal dungeonPortal)
		: base(manager, helper, camera_view_port)
	{
		D3DDungeon dungeon = D3DMain.Instance.exploring_dungeon.dungeon;
		AddTeleportCellCfgs();
		map_scale = camera_view_port.height / 320f;
		int num = 0;
		List<UIImage> list = new List<UIImage>();
		float num2 = 480 * D3DMain.Instance.HD_SIZE;
		float num3 = 320 * D3DMain.Instance.HD_SIZE;
		float num4 = ((D3DMain.Instance.HD_SIZE != 2) ? 0.5f : 1f);
		foreach (string item in dungeon.dungeon_map_jigsaw)
		{
			UIImage uIImage = new UIImage();
			D3DImageCell teleportCell = GetTeleportCell(item);
			Rect texture_rect = D3DMain.Instance.ConvertRectAutoHD(teleportCell.cell_rect);
			if (texture_rect.width > num2)
			{
				texture_rect = new Rect(texture_rect.x, texture_rect.y, num2 + 1f, texture_rect.height);
			}
			uIImage.SetTexture(GetTeleportCellMat(teleportCell.cell_texture), texture_rect, new Vector2(texture_rect.width, num3) * map_scale);
			uIImage.Rect = new Rect(num2 * map_scale * (float)num, 0f, texture_rect.width * map_scale, num3 * map_scale);
			list.Add(uIImage);
			num++;
		}
		list.Reverse();
		foreach (UIImage item2 in list)
		{
			manager.Add(item2);
		}
		scroll_limit = new Rect(0f, 0f, num2 * map_scale * (float)num, 0f);
		if (dungeon.dungeon_town != null)
		{
			FloorButton floorButton = new FloorButton
			{
				floor_index = 0,
				button_cells = new D3DImageCell[3]
			};
			floorButton.button_cells[0] = GetTeleportCell("dian-weijiesuo");
			floorButton.button_cells[1] = GetTeleportCell("dian1-weixuanzhong");
			floorButton.button_cells[2] = GetTeleportCell("dian1-xuanzhong");
			floorButton.button_img = new UIImage();
			Rect texture_rect = D3DMain.Instance.ConvertRectAutoHD(floorButton.button_cells[1].cell_rect);
			floorButton.button_img.Rect = new Rect(dungeon.dungeon_town.town_map_position.x * map_scale * num4, (num3 - dungeon.dungeon_town.town_map_position.y * num4) * map_scale, texture_rect.width * map_scale, texture_rect.height * map_scale);
			manager.Add(floorButton.button_img);
			FloorButtons.Add(floorButton);
		}
		foreach (D3DDungeonFloor dungeon_floor in dungeon.dungeon_floors)
		{
			FloorButton floorButton2 = new FloorButton
			{
				floor_index = dungeon_floor.floor_index,
				button_cells = new D3DImageCell[3]
			};
			float num5 = 0f;
			if (dungeon_floor.boss_level)
			{
				floorButton2.button_cells[0] = GetTeleportCell("boss-weijiesuo");
				floorButton2.button_cells[1] = GetTeleportCell("boss-weixuanzhong");
				floorButton2.button_cells[2] = GetTeleportCell("boss-xuanzhong");
				num5 = -8.5f * (float)D3DMain.Instance.HD_SIZE;
			}
			else
			{
				floorButton2.button_cells[0] = GetTeleportCell("dian-weijiesuo");
				floorButton2.button_cells[1] = GetTeleportCell("dian-weixuanzhong");
				floorButton2.button_cells[2] = GetTeleportCell("dian-xuanzhong");
				num5 = -10 * D3DMain.Instance.HD_SIZE;
			}
			floorButton2.button_img = new UIImage();
			Rect texture_rect = D3DMain.Instance.ConvertRectAutoHD(floorButton2.button_cells[0].cell_rect);
			floorButton2.button_img.Rect = new Rect(dungeon_floor.floor_map_position.x * map_scale * num4, (num3 - dungeon_floor.floor_map_position.y * num4) * map_scale, texture_rect.width * map_scale, texture_rect.height * map_scale);
			manager.Add(floorButton2.button_img);
			FloorButtons.Add(floorButton2);
			floorButton2.index_number = new List<UIImage>();
			string text = floorButton2.floor_index.ToString();
			float num6 = ((float)(text.Length * 15 * D3DMain.Instance.HD_SIZE) - 7.5f * (float)D3DMain.Instance.HD_SIZE * (float)(text.Length - 1)) * map_scale;
			Vector2 position = floorButton2.button_img.GetPosition();
			position = new Vector2(position.x - num6 * 0.5f, position.y);
			for (int i = 0; i < text.Length; i++)
			{
				D3DImageCell teleportCell = GetTeleportCell(text[i].ToString());
				UIImage uIImage2 = new UIImage();
				uIImage2.SetTexture(GetTeleportCellMat(teleportCell.cell_texture), D3DMain.Instance.ConvertRectAutoHD(teleportCell.cell_rect), new Vector2(15f, 20.5f) * D3DMain.Instance.HD_SIZE * map_scale);
				uIImage2.Rect = new Rect(position.x + 7.5f * (float)D3DMain.Instance.HD_SIZE * (float)i * map_scale, position.y + num5 * map_scale, (float)(15 * D3DMain.Instance.HD_SIZE) * map_scale, 20.5f * (float)D3DMain.Instance.HD_SIZE * map_scale);
				manager.Add(uIImage2);
				floorButton2.index_number.Add(uIImage2);
			}
		}
		this.dungeonPortal = dungeonPortal;
	}

	private void AddTeleportCellCfgs()
	{
		teleport_cell_cfgs = new Dictionary<string, D3DImageCell>();
		string text = "Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("TeleportMapCell", D3DGamer.Instance.Sk[0]));
		Object[] array = Resources.LoadAll(text, typeof(TextAsset));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			D3DMain.Instance.LoadD3DImageCell(ref teleport_cell_cfgs, text + "/" + @object.name);
		}
	}

	private D3DImageCell GetTeleportCell(string cell_name)
	{
		return teleport_cell_cfgs[cell_name];
	}

	private Material GetTeleportCellMat(string cell_texture)
	{
		cell_texture += ((D3DMain.Instance.HD_SIZE != 2) ? string.Empty : "_hd");
		return (Material)Resources.Load("Dungeons3D/Images/Teleport/" + cell_texture + "_M");
	}

	public void UpdateMap(int last_floor)
	{
		this.last_floor = last_floor;
		foreach (FloorButton floorButton in FloorButtons)
		{
			int num = 1;
			if (floorButton.floor_index > D3DMain.Instance.exploring_dungeon.dungeon.explored_level)
			{
				num = 0;
			}
			else if (floorButton.floor_index == D3DMain.Instance.exploring_dungeon.current_floor)
			{
				num = 2;
			}
			else if (floorButton.floor_index == last_floor)
			{
				num = 2;
			}
			Rect texture_rect = D3DMain.Instance.ConvertRectAutoHD(floorButton.button_cells[num].cell_rect);
			floorButton.button_img.SetTexture(GetTeleportCellMat(floorButton.button_cells[num].cell_texture), texture_rect, new Vector2(texture_rect.width, texture_rect.height) * map_scale);
		}
	}

	public void JumpScrollToCurrentFloor()
	{
		ResetScroll();
		Scroll(Vector2.right * (0f - FloorButtons[D3DMain.Instance.exploring_dungeon.current_floor].button_img.GetPosition().x + 480f * map_scale), true);
	}

	public void UIEvent(int move_command, Vector2 move_position, Vector2 move_delta)
	{
		switch (move_command)
		{
		case 0:
			button_click = true;
			StopInertia();
			break;
		case 1:
			button_click = false;
			break;
		case 2:
			Scroll(new Vector2(move_delta.x, 0f));
			break;
		case 4:
			if (button_click)
			{
				Vector3 localPosition = ui_manager.GetManagerCamera().transform.localPosition;
				{
					foreach (FloorButton floorButton in FloorButtons)
					{
						if (floorButton.TapButton(move_position + new Vector2(localPosition.x - camera_reset_position.x, localPosition.y - camera_reset_position.y)))
						{
							if (floorButton.floor_index != D3DMain.Instance.exploring_dungeon.current_floor && floorButton.floor_index <= D3DMain.Instance.exploring_dungeon.dungeon.explored_level)
							{
								dungeonPortal(floorButton.floor_index);
							}
							break;
						}
					}
					break;
				}
			}
			ScrollInertia(new Vector2(move_delta.x, 0f));
			break;
		}
	}
}
