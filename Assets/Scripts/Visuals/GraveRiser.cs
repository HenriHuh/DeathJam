using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveRiser : MonoBehaviour
{
	// This script triggers skeleboi to rise and plays grave particles
	[SerializeField] ParticleSystem graveParticles;
	[SerializeField] Animator skeleboiAnim;
	[SerializeField] AudioSource[] graveAudio;

	public void TriggerGrave()
	{
		graveParticles.Play();
		skeleboiAnim.SetTrigger("Rise");
		foreach (AudioSource audioSource in graveAudio)
		{
			audioSource.Play();
		}
	}
}
