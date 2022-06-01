using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

	public Requirement[] requirements;
	public int maxMoves;
	public TMPro.TextMeshPro text_Scores;
	//public TMPro.TextMeshPro text_ScoresType;
	public TMPro.TextMeshPro text_Moves;
	public UnityEngine.UI.Image[] image_Requirements;
	public Transform board;

	private Requirement[] scores;
	private int movesLeft;

	private bool levelFinished;

	[System.Serializable]
	public struct Requirement
	{
		public Enums.ElementalType type;
		public int count;
		public Requirement(Enums.ElementalType type, int count)
		{
			this.type = type;
			this.count = count;
		}
	}

	private void Start()
	{
		UpdateUI();
		enabled = false;
	}

	private void OnEnable()
	{
		scores = new Requirement[]
		{
			new Requirement(Enums.ElementalType.None, 0),
			new Requirement(Enums.ElementalType.Heart, 0),
			new Requirement(Enums.ElementalType.Soul, 0),
			new Requirement(Enums.ElementalType.Mind, 0),
			new Requirement(Enums.ElementalType.Weird, 0),
		};

		movesLeft = maxMoves;
	}

	/// <summary>
	/// Returns true if lost
	/// </summary>
	public bool UseMove()
	{
		movesLeft--;
		text_Moves.text = movesLeft.ToString() + " moves";
		return movesLeft <= 0;
	}

	public void StartLevel()
	{
		enabled = true;
	}

	public void EndLevel()
	{
		enabled = false;
	}

	public void AddType(Enums.ElementalType type)
	{
		scores[(int)type].count++;
		UpdateUI();
	}

	public void UpdateUI()
	{
		text_Scores.text = "";
		for (int i = 0; i < requirements.Length; i++)
		{
			text_Scores.text += scores[(int)requirements[i].type].count + "/" + requirements[i].count + "\n";
			image_Requirements[i].gameObject.SetActive(true);

			switch (requirements[i].type)
            {
                case Enums.ElementalType.None:
					image_Requirements[i].gameObject.SetActive(false);
					break;
				case Enums.ElementalType.Heart:
					image_Requirements[i].sprite = GameManager.instance.sprite_ElementHeart;
                    break;
				case Enums.ElementalType.Soul:
					image_Requirements[i].sprite = GameManager.instance.sprite_ElementSoul;
                    break;
				case Enums.ElementalType.Mind:
					image_Requirements[i].sprite = GameManager.instance.sprite_ElementMind;
                    break;
				case Enums.ElementalType.Weird:
					image_Requirements[i].sprite = GameManager.instance.sprite_ElementWeird;
                    break;
				case Enums.ElementalType.COUNT:
                    break;
                default:
                    break;
            }
		}
        for (int i = requirements.Length; i < 4; i++)
        {
			image_Requirements[i].gameObject.SetActive(false);
		}
		text_Moves.text = movesLeft.ToString() + " moves";
	}

	public bool CheckRequirements(bool playEffects = false)
	{
		if (levelFinished) return true;

		for (int i = 0; i < requirements.Length; i++)
		{
			if (scores[(int)requirements[i].type].count < requirements[i].count)
			{
				return false;
			}
		}

		if (playEffects && !levelFinished)
		{
			levelFinished = true;
			GameManager.instance.board.SetActive(false);
			GetComponent<GraveRiser>().TriggerGrave();
		}

		return true;

	}

}
