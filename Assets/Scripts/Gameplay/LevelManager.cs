using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player inputs and manages visual behaviours
/// </summary>
public class LevelManager : MonoBehaviour
{
    public GameObject diePrefab;
    public int gridSizeX;
    public int gridSizeY;

    public PuzzleController controller { get; private set; }

    private List<PuzzleEvent> eventStack = new List<PuzzleEvent>();

    /// <summary>
    /// Gameplay logic will initiate a new puzzle event which are seperately processed.
    /// </summary>
    private interface PuzzleEvent
    {
        public IEnumerator EventRoutine();
    }

    private class PuzzleEvent_DieRoll : PuzzleEvent
    {
        private PuzzleDie puzzleDie;

        public PuzzleEvent_DieRoll(PuzzleDie puzzleDie)
        {
            this.puzzleDie = puzzleDie;
        }

        public IEnumerator EventRoutine()
        {
            // Play and wait for die roll animation here

            // PLACEHOLDER COLOR SWAP !!!
            puzzleDie.GameObject.transform.position = new Vector3(puzzleDie.Position.x, 1, puzzleDie.Position.y);
            puzzleDie.GameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, (float)puzzleDie.CurrentSide.types[0] / (float)Enums.ElementalType.COUNT);


            yield return null;
        }


    }

    private class PuzzleEvent_DiceMatch : PuzzleEvent
    {
        private List<PuzzleDie> puzzleDice;
        public PuzzleEvent_DiceMatch(List<PuzzleDie> puzzleDice)
        {
            this.puzzleDice = puzzleDice;
        }

        public IEnumerator EventRoutine()
        {
            // Play match animations here

            // PLACEHOLDER SCALE ANIMATION !!!
            puzzleDice.ForEach((n) => n.GameObject.transform.localScale *= 2);
            yield return new WaitForSeconds(0.5f);
            puzzleDice.ForEach((n) => n.GameObject.transform.localScale /= 2);


            yield return null;
        }
    }


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
        // Just placeholder random roll. Insert proper input handling.
        while (!Input.GetKeyDown(KeyCode.Mouse0))
        {
            yield return null;
        }
        controller.Roll(controller.dice[Random.Range(0, controller.dice.Length)]);
    }

    private IEnumerator WaitMatch()
    {
        // Just placeholder random roll. Insert proper input handling.
        yield return null;
        while (!Input.GetKeyDown(KeyCode.Mouse0))
        {
            yield return null;
        }
        List<PuzzleDie> dice = controller.CheckAllMatches();
        if(dice.Count > 0)
        {
            eventStack.Add(new PuzzleEvent_DiceMatch(dice));
        }
        controller.Roll(dice);
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
        }
        return dice;
    }

}
