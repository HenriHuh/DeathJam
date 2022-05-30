using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public Requirement[] requirements;
    public int maxMoves;
    public TMPro.TextMeshPro text_Scores;
    public TMPro.TextMeshPro text_ScoresType;
    public TMPro.TextMeshPro text_Moves;
    public Transform board;


    private Requirement[] scores;
    private int movesLeft;

    public bool Finished => CheckRequirements();

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

    private void UpdateUI()
    {
        text_Scores.text = "";
        text_ScoresType.text = "";
        for (int i = 0; i < requirements.Length; i++)
        {
            text_Scores.text += scores[(int)requirements[i].type].count + "/" + requirements[i].count + "\n";
            text_ScoresType.text += requirements[i].type + "\n";
        }
        text_Moves.text = movesLeft.ToString() + " moves";
    }
    
    public bool CheckRequirements(bool playEffects = false)
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            if(scores[(int)requirements[i].type].count < requirements[i].count) {
                return false;
            }
        }

        if(playEffects)
        {
            GameManager.instance.board.SetActive(false);
            GetComponent<GraveRiser>().triggerGrave();
        }

        return true;

    }

}
