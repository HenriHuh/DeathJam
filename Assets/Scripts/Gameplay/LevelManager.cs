using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleEvents;

/// <summary>
/// Handles player inputs and manages visual behaviours
/// </summary>
public class LevelManager : MonoBehaviour
{
    public GameObject diePrefab;
    public int gridSizeX;
    public int gridSizeY;
    public LayerMask dieLayer;

    public PuzzleController controller { get; private set; }

    private List<PuzzleEvent> eventStack = new List<PuzzleEvent>();


    private IEnumerator Start()
    {

        // Wait one frame since WebGL does not understand script exec order (awake, start, onenable neither)
        // GameManager should be active when LevelManager is instantiated anyway
        yield return new WaitForEndOfFrame();

        controller = new PuzzleController(gridSizeX, gridSizeY, CreateDice(4*4));
        controller.MatchDelegate += AddMatchRoutine;
        controller.RollDelegate += AddRollRoutine;
        controller.RollAll();
        StartCoroutine(MainRoutine());
    }

    private void Update()
    {
        if(controller != null)
        {
            for (int k = 0; k < controller.dice.Length; k++)
            {
                Debug.DrawLine(controller.dice[k].TransformPosition, controller.dice[k].TransformPosition + Vector3.up * 2, Tools.ColorRainbowLerp(controller.dice[k].CurrentSideIndex / 6f));

            }
        }
    }

    private IEnumerator MainRoutine()
    {
        while (true)
        {
            yield return WaitEventRoutines();
            yield return WaitForInput();
            yield return WaitEventRoutines();
            yield return WaitMatch();
        }
    }

    private IEnumerator WaitForInput()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, dieLayer))
            {
                controller.Roll(hitInfo.collider.GetComponent<PuzzleDieBehaviour>().PuzzleDie);
                break;
            }

            yield return null;
        }
    }

    private IEnumerator WaitMatch()
    {
        yield return null;

        eventStack.Add(new PuzzleEvent_Wait(1f));
        List<PuzzleDie> dice = controller.CheckAllMatches();
        while(dice.Count > 0)
        {
            controller.Roll(dice);
            eventStack.Add(new PuzzleEvent_Wait(dice.Count * 0.1f + 0.75f));
            dice = controller.CheckAllMatches();
        }
    }

    private IEnumerator WaitEventRoutines()
    {
        for (int i = 0; i < eventStack.Count; i++)
        {
            yield return eventStack[i].EventRoutine();
        }
        eventStack.Clear();
    }

    private void AddMatchRoutine(List<PuzzleDie> dice)
    {
        PuzzleEvent_DiceMatch e = new PuzzleEvent_DiceMatch(dice);
        eventStack.Add(e);
    }

    private void AddRollRoutine(PuzzleDie die)
    {
        PuzzleEvent_DieRoll e = new PuzzleEvent_DieRoll(die);
        eventStack.Add(e);
    }


    /// <summary>
    /// Should replace with some proper logic instead of just randomizing
    /// </summary>
    private PuzzleDie[] CreateDice(int count)
    {
        PuzzleDie[] dice = new PuzzleDie[count];
        for (int i = 0; i < count; i++)
        {
            GameObject dieObject = Instantiate(diePrefab);
            SO_PuzzleDie asset = GameManager.instance.diceAssets[Random.Range(0, GameManager.instance.diceAssets.Count)];
            dice[i] = new PuzzleDie(asset, dieObject);
            dieObject.GetComponent<PuzzleDieBehaviour>().Init(dice[i]);
        }
        return dice;
    }

}
