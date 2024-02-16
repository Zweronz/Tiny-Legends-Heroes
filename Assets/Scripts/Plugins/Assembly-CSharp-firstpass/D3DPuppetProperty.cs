public class D3DPuppetProperty
{
	public float hp;

	public float hp_max;

	public float mp;

	public float mp_max;

	public float strength;

	public float agility;

	public float spirit;

	public float stamina;

	public float intelligence;

	public float[] puppet_physical_dmg;

	public float[] puppet_magical_dmg;

	public float[] main_weapon_physical_dmg;

	public float[] sub_weapon_physical_dmg;

	public float[] main_weapon_magical_dmg;

	public float[] sub_weapon_magical_dmg;

	public float dual_adjust = 1f;

	public float armor;

	public float magic_armor;

	public float attack_power;

	public float magic_power;

	public float hp_recover;

	public float mp_recover;

	public float attack_interval_coe;

	public float attack_interval;

	public float move_speed;

	public float move_speed_coe;

	public float dodge_chance;

	public float critical_chance;

	public float dmg_extra = 1f;

	public float dmg_reduce;

	public float fixed_dmg_extra;

	public float fixed_dmg_reduce;

	public float exp_up;

	public float exp_percent = 1f;

	public float stun_resist;

	public float fear_resist;

	public float trady_resist;

	public float stakme_resist;

	public float DamageExtra
	{
		get
		{
			if (dmg_extra < 0f)
			{
				return 0f;
			}
			return dmg_extra;
		}
	}

	public float DamageReduce
	{
		get
		{
			float num = 1f - dmg_reduce;
			if (num < 0f)
			{
				num = 0f;
			}
			return num;
		}
	}

	public bool Dodge
	{
		get
		{
			return D3DMain.Instance.Lottery(dodge_chance);
		}
	}

	public float MoveSpdScale
	{
		get
		{
			if (move_speed_coe == 0f)
			{
				return 0f;
			}
			float num = move_speed / move_speed_coe;
			if (num < 0f)
			{
				num = 0f;
			}
			return num;
		}
	}
}
