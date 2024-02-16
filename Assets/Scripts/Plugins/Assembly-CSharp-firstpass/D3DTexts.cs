using System.Collections.Generic;

public class D3DTexts
{
	public enum MsgBoxState
	{
		ON_DESTROY_GEAR = 0,
		ON_LOOT_NOT_GET_ALL_QUIT = 1,
		ON_RESET_GAME = 2,
		ON_CHANGE_GEAR_IF_STORE_FULL = 3,
		ON_UNLOCK_SKILL_SLOTS = 4,
		ON_CANNOT_REMOVE_DUAL = 5,
		ON_UPGRADE_SKILL_IF_LEVEL_NOT_ENOUGH = 6,
		ON_CASH_NOT_ENOUGH_OPEN_IAP = 7,
		ON_CONFIRM_UPGRADE_SKILL = 8,
		ON_UNLOCK_STORE_PAGE = 9,
		ON_CONFIRM_HIRE_HERO = 10,
		ON_TEAM_EMPTY_TRY_QUIT = 11,
		ON_LOOT_GET_ALL_BUT_STORE_FULL_CAN_UNLOCK = 12,
		ON_LOOT_GET_ALL_BUT_STORE_FULL_MAX_PAGE = 13,
		ON_BUY_ITEM = 14,
		ON_SELL_ITEM = 15,
		ON_USE_CRYSTAL_EXCHANGE = 16,
		ON_CRYSTAL_EXCHANGE_NOT_ENOUGH = 17,
		ON_PURCHASE_SUCCESS = 18,
		ON_PURCHASE_FAILED = 19,
		ON_WAITING_PURCHASE = 20,
		ON_GET_IAP_ITEM_GAME_QUIT_EXCEPTION = 21,
		ON_CLICK_PURCHASED_IAP = 22,
		ON_IAP_GOLD_OVER_LIMIT = 23,
		ON_UNLOCK_NEW_HERO = 24,
		ON_QUIT_TO_TITLE = 25,
		ON_RESPAWNNOW = 26,
		NewHeroComing = 27,
		MiniShop_title = 28,
		MiniShop_Go = 29
	}

	private static D3DTexts instance;

	private List<List<string>> MsgBoxContents;

	private List<string> tBankNames;

	private List<List<string>> tBankContents;

	private List<string> _respawnTipContents;

	public static D3DTexts Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new D3DTexts();
			}
			return instance;
		}
	}

	public List<string> RespawnTipContents
	{
		get
		{
			return _respawnTipContents;
		}
		set
		{
			_respawnTipContents = value;
		}
	}

	public D3DTexts()
	{
		MsgBoxContents = new List<List<string>>();
		tBankNames = new List<string>();
		tBankContents = new List<List<string>>();
	}

	public void AddMsgBoxContent(List<string> content)
	{
		MsgBoxContents.Add(content);
	}

	public List<string> GetMsgBoxContent(MsgBoxState box_state)
	{
		return MsgBoxContents[(int)box_state];
	}

	public void AddTBankText(string name, List<string> content)
	{
		tBankNames.Add(name);
		tBankContents.Add(content);
	}

	public string GetTBankName(D3DGamer.IapMenu tBankType)
	{
		return tBankNames[(int)tBankType];
	}

	public List<string> GetTBankContent(D3DGamer.IapMenu tBankType)
	{
		return tBankContents[(int)tBankType];
	}
}
