using UnityEngine;

public class BattleWinBehaviour : MonoBehaviour
{
	private enum WinBehaviourCase
	{
		CHECK_IN_POSITION = 0,
		ROTATE_TO_SCREEN = 1,
		STAT_EXP = 2,
		POP_WIN = 3
	}

	private WinBehaviourCase win_case;

	private SceneArena scene_arena;

	private int battle_exp_bonus;

	private void Start()
	{
		win_case = WinBehaviourCase.CHECK_IN_POSITION;
	}

	private void Update()
	{
		switch (win_case)
		{
		case WinBehaviourCase.CHECK_IN_POSITION:
			foreach (PuppetArena playerWinner in scene_arena.playerWinnerList)
			{
				if (!playerWinner.IsIdle())
				{
					return;
				}
			}
			win_case = WinBehaviourCase.ROTATE_TO_SCREEN;
			{
				foreach (PuppetArena playerWinner2 in scene_arena.playerWinnerList)
				{
					playerWinner2.RotateToScreen();
				}
				break;
			}
		case WinBehaviourCase.ROTATE_TO_SCREEN:
			foreach (PuppetArena playerWinner3 in scene_arena.playerWinnerList)
			{
				if (playerWinner3.DoingRotation)
				{
					return;
				}
			}
			if (!D3DGamer.Instance.TutorialState[0])
			{
				foreach (PuppetArena playerWinner4 in scene_arena.playerWinnerList)
				{
					playerWinner4.WinnerBehaviour();
				}
				win_case = WinBehaviourCase.POP_WIN;
				Invoke("ShowWinnerUI", 3f);
				break;
			}
			win_case = WinBehaviourCase.STAT_EXP;
			{
				foreach (PuppetArena playerWinner5 in scene_arena.playerWinnerList)
				{
					playerWinner5.InitExpComponents(battle_exp_bonus);
					playerWinner5.WinnerBehaviour();
				}
				break;
			}
		case WinBehaviourCase.STAT_EXP:
		{
			bool flag = false;
			foreach (PuppetArena playerWinner6 in scene_arena.playerWinnerList)
			{
				if (playerWinner6.UpdateExpComponents())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				win_case = WinBehaviourCase.POP_WIN;
				Invoke("ShowWinnerUI", 3f);
				D3DGamer.Instance.SaveAllData();
			}
			break;
		}
		}
	}

	public void Init(SceneArena scene_arena)
	{
		this.scene_arena = scene_arena;
		foreach (PuppetArena playerWinner in scene_arena.playerWinnerList)
		{
			playerWinner.TargetPuppet = null;
			playerWinner.ClearHatredPuppet();
		}
		if (D3DGamer.Instance.TutorialState[0])
		{
			battle_exp_bonus = Mathf.RoundToInt((float)scene_arena.battle_gained_exp * D3DFormulas.GetExpPunitive(D3DGamer.Instance.BattleTeamMaxLevel - D3DMain.Instance.exploring_dungeon.player_battle_group_data.group_level));
			battle_exp_bonus = Mathf.Max(Mathf.RoundToInt((float)battle_exp_bonus / (float)scene_arena.playerWinnerList.Count), 1);
		}
	}

	private void ShowWinnerUI()
	{
		scene_arena.ui_arena.PopBattleResultUI(true);
	}
}
