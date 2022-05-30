using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTumble : MonoBehaviour
{
	// This controls the dice animator and triggers effects
	[SerializeField] ParticleSystem scoreParticleRed, scoreParticleBlue, scoreParticleGreen, scoreParticleYellow, scoreParticleMixed;
	Animator anim;
	public Animator Anim
	{
		get
		{
			if (anim == null) anim = GetComponent<Animator>();
			return anim;
		}
	}

	[Header("Change this value and press enter to roll")] //Only for editor testing
	[SerializeField][Delayed] int testDiceResult; //Only for editor testing
	int lastTestDiceResult = 0; //Only for editor testing
	public void DiceRoll(int diceResult)
	{
		Anim.SetTrigger("Toss");
		Anim.SetInteger("DiceResult", diceResult);
		//Do other effects?

	}
	public void DiceScore(int diceResult)
	{
		Anim.SetTrigger("Jolt");
		switch (diceResult)
		{
			case 0:  //earth fire earthfire water waterwind wind
				scoreParticleGreen.Play();
				break;
			case 1:
				scoreParticleRed.Play();
				break;
			case 2:
				scoreParticleMixed.Play();
				break;
			case 3:
				scoreParticleBlue.Play();
				break;
			case 4:
				scoreParticleMixed.Play();
				break;
			case 5:
				scoreParticleYellow.Play();
				break;
		}

	}
}

