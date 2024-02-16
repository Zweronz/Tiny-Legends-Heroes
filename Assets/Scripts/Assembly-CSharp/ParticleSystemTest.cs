using UnityEngine;

public class ParticleSystemTest : MonoBehaviour
{
	private ParticleSystem[] particle_systems;

	private void Awake()
	{
		particle_systems = GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = particle_systems;
		foreach (ParticleSystem particleSystem in array)
		{
			particleSystem.loop = false;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		ParticleSystem[] array = particle_systems;
		foreach (ParticleSystem particleSystem in array)
		{
		}
	}
}
