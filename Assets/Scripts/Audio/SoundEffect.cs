using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
	// Gives extra options to trigger audio with AudioSources at specific times or ways
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip[] audioClips;
	[SerializeField] AudioClip[] randomAudioClips;
	[SerializeField] float minPitch = 0.8f, maxPitch = 1.2f;
	[SerializeField] float minRandomPitch = 0.8f, maxRandomPitch = 1.2f;

	static bool firstSFX;
	static bool secondSFX;
	static bool thirdSFX;

	void Start()
	{

	}

    private void Update()
    {
		firstSFX = false;
		secondSFX = false;
		thirdSFX = false;
	}

	// Update is called once per frame
	public void PlayRandomSFX()
	{
		if (firstSFX) return;
		firstSFX = true;

		audioSource.clip = randomAudioClips[Random.Range(0, randomAudioClips.Length)];
		audioSource.pitch = Random.Range(minRandomPitch, maxRandomPitch);
		audioSource.Play();
	}
	public void PlaySFX(int i)
	{
		if (secondSFX) return;
		secondSFX = true;

		audioSource.clip = audioClips[i];
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.Play();
	}

	public void PlayPitchlessSFX(int i)
	{
		if (thirdSFX) return;
		thirdSFX = true;

		audioSource.clip = audioClips[i];
		audioSource.pitch = 1f;
		audioSource.Play();
	}

}
