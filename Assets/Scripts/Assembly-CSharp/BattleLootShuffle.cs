using System.Collections.Generic;
using UnityEngine;

public class BattleLootShuffle : MonoBehaviour
{
	public enum ShuffleState
	{
		LOOT_SHOW = 0,
		LOOT_CASED = 1,
		BOX_CLOSE = 2,
		SHUFFING = 3,
		SHUFF_END = 4
	}

	private ShuffleState shuff_state;

	private D3DBattleResultUI BattleResultUI;

	private List<D3DLootTreasure> LootTreasures;

	private float last_shuff_real_time;

	private float current_shuff_real_time;

	private float delta_shuff_real_time;

	private float shuff_time;

	private float loot_gear_y = 60f;

	private int shuff_count = 5;

	private int[] random_shuff_index = new int[2] { -1, -1 };

	private void Start()
	{
		last_shuff_real_time = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		current_shuff_real_time = Time.realtimeSinceStartup;
		delta_shuff_real_time = current_shuff_real_time - last_shuff_real_time;
		last_shuff_real_time = current_shuff_real_time;
		switch (shuff_state)
		{
		case ShuffleState.LOOT_SHOW:
			shuff_time += delta_shuff_real_time;
			if (shuff_time > 1.5f)
			{
				shuff_state = ShuffleState.LOOT_CASED;
			}
			break;
		case ShuffleState.LOOT_CASED:
			loot_gear_y -= 60f * delta_shuff_real_time;
			foreach (D3DLootTreasure lootTreasure in LootTreasures)
			{
				lootTreasure.SetGearUILocalPosition(new Vector2(14.5f, loot_gear_y));
			}
			if (!(loot_gear_y < 25f))
			{
				break;
			}
			shuff_state = ShuffleState.BOX_CLOSE;
			{
				foreach (D3DLootTreasure lootTreasure2 in LootTreasures)
				{
					lootTreasure2.SetGearVisible(false);
					lootTreasure2.TreasureAnimation.Play();
				}
				break;
			}
		case ShuffleState.BOX_CLOSE:
		{
			bool flag2 = true;
			foreach (D3DLootTreasure lootTreasure3 in LootTreasures)
			{
				if (lootTreasure3.TreasureAnimation.IsPlaying())
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				RandomShuffTreasure();
			}
			break;
		}
		case ShuffleState.SHUFFING:
		{
			bool flag = true;
			int[] array = random_shuff_index;
			foreach (int index in array)
			{
				Rect rect = LootTreasures[index].TreasureAnimation.AnimationControl.Rect;
				if (!(Vector2.Dot(LootTreasures[index].ShuffDirection, LootTreasures[index].ShuffTarget - new Vector2(rect.x, rect.y)) <= 0f))
				{
					flag = false;
					LootTreasures[index].TreasureAnimation.AnimationControl.Rect = new Rect(rect.x + LootTreasures[index].ShuffVelocity.x * delta_shuff_real_time, rect.y + LootTreasures[index].ShuffVelocity.y * delta_shuff_real_time, rect.width, rect.height);
				}
			}
			if (!flag)
			{
				break;
			}
			int[] array2 = random_shuff_index;
			foreach (int index2 in array2)
			{
				LootTreasures[index2].SetPosition(new Vector2(LootTreasures[index2].TreasureButton.Rect.x, LootTreasures[index2].TreasureButton.Rect.y) * (1f / (float)D3DMain.Instance.HD_SIZE));
			}
			shuff_count--;
			if (shuff_count == 0)
			{
				foreach (D3DLootTreasure lootTreasure4 in LootTreasures)
				{
					lootTreasure4.TreasureButton.Enable = true;
					lootTreasure4.TreasureAnimation.SetAnimationReverse();
					lootTreasure4.SetGearUILocalPosition(new Vector2(14.5f, 25f));
					lootTreasure4.TreasureAnimation.AnimationEndCallBack = lootTreasure4.OnTreaureOpen;
				}
				shuff_state = ShuffleState.SHUFF_END;
			}
			else
			{
				RandomShuffTreasure();
			}
			break;
		}
		case ShuffleState.SHUFF_END:
			BattleResultUI.EnableLootUI();
			base.enabled = false;
			break;
		}
	}

	public void Init(List<D3DLootTreasure> LootTreasures, D3DBattleResultUI result_ui)
	{
		this.LootTreasures = LootTreasures;
		shuff_state = ShuffleState.SHUFF_END;
		List<int> list = new List<int>();
		for (int i = 0; i < D3DMain.Instance.LootEquipments.Count; i++)
		{
			list.Add(i);
		}
		foreach (D3DLootTreasure lootTreasure in this.LootTreasures)
		{
			int index = Random.Range(0, list.Count);
			lootTreasure.UpdateLootUIGear(D3DMain.Instance.LootEquipments[list[index]]);
			lootTreasure.OpenTreaureByNewRule();
			list.RemoveAt(index);
		}
		BattleResultUI = result_ui;
	}

	public void SkipShuff()
	{
		foreach (D3DLootTreasure lootTreasure in LootTreasures)
		{
			lootTreasure.SetPosition(new Vector2(lootTreasure.TreasureButton.Rect.x, lootTreasure.TreasureButton.Rect.y) * (1f / (float)D3DMain.Instance.HD_SIZE));
			lootTreasure.TreasureButton.Enable = true;
			lootTreasure.TreasureAnimation.Stop();
			lootTreasure.TreasureAnimation.SetAnimationReverse();
			lootTreasure.SetGearUILocalPosition(new Vector2(14.5f, 25f));
			lootTreasure.SetGearVisible(false);
			lootTreasure.TreasureAnimation.AnimationEndCallBack = lootTreasure.OnTreaureOpen;
		}
		BattleResultUI.EnableLootUI();
		if (!D3DGamer.Instance.TutorialState[1])
		{
			((GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UITutorial"))).GetComponent<UITutorial>().Init(D3DHowTo.TutorialType.FIRST_LOOT);
		}
		base.enabled = false;
	}

	private void RandomShuffTreasure()
	{
		while (true)
		{
			int num = Random.Range(0, LootTreasures.Count);
			if (LootTreasures.Count < 3)
			{
				if (num != random_shuff_index[0])
				{
					random_shuff_index[0] = num;
					break;
				}
			}
			else if (num != random_shuff_index[0] && num != random_shuff_index[1])
			{
				random_shuff_index[0] = num;
				break;
			}
		}
		do
		{
			random_shuff_index[1] = Random.Range(0, LootTreasures.Count);
		}
		while (random_shuff_index[0] == random_shuff_index[1]);
		shuff_state = ShuffleState.SHUFFING;
		LootTreasures[random_shuff_index[0]].SetShuffTarget(new Vector2(LootTreasures[random_shuff_index[1]].TreasureButton.Rect.x, LootTreasures[random_shuff_index[1]].TreasureButton.Rect.y));
		LootTreasures[random_shuff_index[1]].SetShuffTarget(new Vector2(LootTreasures[random_shuff_index[0]].TreasureButton.Rect.x, LootTreasures[random_shuff_index[0]].TreasureButton.Rect.y));
	}
}
