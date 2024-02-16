using UnityEngine;

public class PuppetSummonLife : MonoBehaviour
{
	private PuppetArena summoner;

	private PuppetArena summon_puppet;

	private float summon_life;

	private float summon_life_delta;

	private void Awake()
	{
		summon_puppet = GetComponent<PuppetArena>();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (summon_life > 0f)
		{
			summon_life_delta += Time.deltaTime;
		}
		if ((summon_life > 0f && summon_life_delta >= summon_life) || null == summoner || summoner.IsDead() || null == summon_puppet || summon_puppet.IsDead())
		{
			summon_puppet.Variable(TriggerVariable.VariableType.HP_DAMAGE, summon_puppet.profile_instance.puppet_property.hp_max, false);
			Object.Destroy(this);
		}
	}

	public void SummonLife(PuppetArena summoner, float time)
	{
		this.summoner = summoner;
		summon_life = time;
	}
}
