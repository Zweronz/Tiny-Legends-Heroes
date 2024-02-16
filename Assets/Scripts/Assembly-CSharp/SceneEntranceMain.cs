using System.Collections;
using UnityEngine;

public class SceneEntranceMain : MonoBehaviour
{
	private void Start()
	{
		string[] array = new string[5] { "com.trinitigame.tinylegendsheroes.499centsv121", "com.trinitigame.tinylegendsheroes.1999cents2", "com.trinitigame.tinylegendsheroes.4999cents2", "com.trinitigame.tinylegendsheroes.9999centsv135", "com.trinitigame.tinylegendsheroes.299centsv135new" };
		D3DMain.Instance.AndroidPlatform = D3DMain.ANDROID_PLATFORM.GOOGLE_PLAY;
		GameObject gameObject = new GameObject();
		if (D3DMain.Instance.AndroidPlatform == D3DMain.ANDROID_PLATFORM.AMAZON)
		{
			gameObject.AddComponent<AmazonIAPManager>();
			ChartBoostAndroid.init("50f623e917ba47141000008d", "4175cb97fe0d34910cf6cb022df5b3c737958ae8");
			AmazonIAP.initiateItemDataRequest(array);
		}
		else
		{
			gameObject.AddComponent<GoogleIABManager>();
			ChartBoostAndroid.init("50ed32e316ba477c2b000000", "42b0c3d838e9e58e57b6edb2205f877407776af0");
			ChartBoostAndroid.onStart();
			GoogleIAB.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtlhFYlem3iKeacI1OYyxAYyCjLHN8Y7y5qKYYzifoxcuzzGBh/ZKdfKkNasrAi64AJ2qjmb416k49qg7sJYGVOLnaSz9W8kkyblpoc3YCk71M+CKQ0mXMWqf97i1icIngkZafpLZSwr+OFMWajspbnJ6N/0XEbNh00HoWYC2QpJUfsi9kg3zem7PUCAPMDop2ZNApiZU7gRfO4D9FxBO/DfHjVihBzLu3H5k9F+8Tpz/WZUL0etZbeib9+sddb7V2pRktte/o080OVwCoxf79S/isXEXUWUMc+D8ok4hcn+YuTkP4zWtAo6Xf8nTkiiUnEqsBwrav6ZiHDD8nU2a7wIDAQAB");
			GoogleIAB.queryInventory(array);
		}
		ChartBoostAndroid.cacheInterstitial(null);
		ChartBoostAndroid.showInterstitial(null);
		GameScreen.width = 960;
		GameScreen.height = 640;
		Application.targetFrameRate = 60;
		if (Utils.IsRetina())
		{
			D3DMain.Instance.HD_SIZE = 2;
		}
		else
		{
			D3DMain.Instance.HD_SIZE = 1;
		}
		D3DMain.Instance.CreateFontManagers();
		D3DMessageBox.InitMgbResources();
		D3DLootFromula.Instance.InitLootFormula();
		string text = D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0]));
		D3DMain.Instance.LoadD3DImplicitPopedomFromFile("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3Dclass_equipments", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DClassesBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DClassCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DEquipmentsBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DEquipmentCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DActiveSkillsBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DActiveSkillCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DPassiveSkillsBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DPassiveSkillCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DPuppetProfilesBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DPuppetProfileCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DPuppetAIBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DAIConfig", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DEnemyGroupBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DEnemyGroupsCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DDungeonBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DDungeonsCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DCustomLootBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DCustomLoot", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DTreasureBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DTreasure", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DDungeonNpcConfigBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DNpcCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DDungeonProgressBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DDungeonProgress", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DFormula("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3Dgongshi", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DDrawRule("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3Dfanpai", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DHelmConfig("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("helmconfig", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DPuppetTransforms("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DPuppetTransformCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DMagicPower("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3Dlvzhuangguize", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DTavern("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3Dguyongshangdian", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DTexts("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Text_UI", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DStory("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("story", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadTutorialHins("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("tutorialhints", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DDungeonModelPresetBatch("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DDungeonModelCfg", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DBattlePosition("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3Dstartingpoint", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.LoadD3DHowTo("Dungeons3D/" + text + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("D3DOthersCfg", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("HowTo", D3DGamer.Instance.Sk[0])));
		D3DMain.Instance.InitDungeonsSpawnersOnNewGame();
		D3DMain.Instance.D3DDungeonsSpawnInit();
		D3DGamer.Instance.ConvertSaveDoc();
		D3DGamer.Instance.LoadAllDataNew();
		D3DAudioManager.Instance.LoadD3DAudioCfg();
		D3DShopRuleEx.Instance.RestBattleCount = 0;
		D3DShopRuleEx.Instance.ResetOdds();
		D3DIapDiscount.Instance.CheckCurrentDiscountIsExpiredOnAppStart();
		D3DIapDiscount.Instance.CheckAvailableDiscount();
		GameObject gameObject2 = new GameObject("NeverDestroyedObj");
		gameObject2.AddComponent<NeverDestroyedScript>();
		gameObject2 = new GameObject("TapJoyObj");
		gameObject2.AddComponent<MyTapjoy>();
		gameObject2.AddComponent<GotTapPointsMono>();
		StartCoroutine(EnterGame());
	}

	private void Update()
	{
	}

	private IEnumerator EnterGame()
	{
		yield return 0;
		UIHDBoard.DEVICE = UIHDBoard.GetHDDeviceType();
		if (UIHDBoard.DEVICE != UIHDBoard.HD_DEVICE.OTHERS)
		{
			GameObject hd_board = Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIHDBoard") as GameObject;
			D3DMain.Instance.HD_BOARD_OBJ = (GameObject)Object.Instantiate(hd_board);
			Object.DontDestroyOnLoad(D3DMain.Instance.HD_BOARD_OBJ);
		}
		D3DMain.Instance.CurrentScene = 1;
		Application.LoadLevel(1);
		D3DGamer.Instance.UserCome(D3DGamer.EUserFisrtCome.InLogo);
	}
}
