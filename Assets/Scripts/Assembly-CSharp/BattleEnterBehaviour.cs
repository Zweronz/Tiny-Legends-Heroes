using UnityEngine;

public class BattleEnterBehaviour : MonoBehaviour
{
	private UIImage battle_enter_img;

	private Vector2 img_default_size;

	private Vector2 img_animation_rate;

	private int animation_phase = 2;

	private UIDungeon ui_dungeon;

	private float last_animation_real_time;

	private float current_animation_real_time;

	private float delta_animation_real_time;

	private void Start()
	{
		last_animation_real_time = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		current_animation_real_time = Time.realtimeSinceStartup;
		delta_animation_real_time = current_animation_real_time - last_animation_real_time;
		last_animation_real_time = current_animation_real_time;
		switch (animation_phase)
		{
		case 0:
			battle_enter_img.SetTextureSize(battle_enter_img.GetTextureSize() + img_animation_rate * delta_animation_real_time);
			if (battle_enter_img.GetTextureSize().x / img_default_size.x > 1.25f)
			{
				animation_phase = 1;
			}
			break;
		case 1:
			battle_enter_img.SetTextureSize(battle_enter_img.GetTextureSize() - img_animation_rate * delta_animation_real_time);
			if (battle_enter_img.GetTextureSize().x <= img_default_size.x)
			{
				animation_phase = 2;
				ui_dungeon.EnableUIFade(UIFade.FadeState.FADE_OUT, new Color(44f / 51f, 44f / 51f, 44f / 51f), ui_dungeon.EnterArena, false);
			}
			break;
		case 2:
			battle_enter_img.SetTextureSize(battle_enter_img.GetTextureSize() + img_animation_rate * delta_animation_real_time);
			break;
		}
	}

	public void Init(UIImage battle_img, UIDungeon ui_dungeon)
	{
		battle_enter_img = battle_img;
		img_default_size = battle_enter_img.GetTextureSize();
		img_animation_rate = new Vector2(2000f, img_default_size.y / img_default_size.x * 2000f);
		this.ui_dungeon = ui_dungeon;
		ui_dungeon.EnableUIFade(UIFade.FadeState.FADE_OUT, new Color(44f / 51f, 44f / 51f, 44f / 51f), ui_dungeon.EnterArena, false);
	}
}
