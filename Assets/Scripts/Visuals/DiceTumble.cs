using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTumble : MonoBehaviour
{
	// This controls the dice animator and triggers effects
	Animator anim;

	[Header("Change this value and press enter to roll")] //Only for editor testing
	[SerializeField][Delayed] int testDiceResult; //Only for editor testing
	int lastTestDiceResult = 0; //Only for editor testing
	void Start()
	{
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		TestDiceRoll(); //Only for editor testing
	}

	void TestDiceRoll() //Only for editor testing
	{
		if (lastTestDiceResult != testDiceResult && testDiceResult > 0)
		{
			Debug.Log("Test Rolling");
			DiceRoll(testDiceResult);
			lastTestDiceResult = testDiceResult;
		}
	}

	public void DiceRoll(int diceResult)
	{
		anim.SetTrigger("Toss");
		anim.SetInteger("DiceResult", diceResult);
		//Do other effects?
	}
}

