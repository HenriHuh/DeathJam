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
    public LayerMask levelLayer;
    public Transform graveVirtualCameraTarget;
    public Cinemachine.CinemachineVirtualCamera camera_LevelSelect;
    public Cinemachine.CinemachineVirtualCamera camera_Level;
    public Cinemachine.CinemachineVirtualCamera camera_Menu;

    public PuzzleController controller { get; private set; }

    private List<PuzzleEvent> eventStack = new List<PuzzleEvent>();
    public Level level { get; private set; }
    private bool lost = false;
    private bool won = false;
    private int levelsCompleted;

    public static LevelManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple level manager instances.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private IEnumerator Start()
    {

        // Wait one frame since WebGL does not understand script exec order (awake, start, onenable neither)
        // GameManager should be active when LevelManager is instantiated anyway
        yield return new WaitForEndOfFrame();

        controller = new PuzzleController(gridSizeX, gridSizeY, CreateDice(4 * 4));
        controller.MatchDelegate += AddMatchRoutine;
        controller.RollDelegate += AddRollRoutine;
        controller.RollAll();
        SetCamera(camera_Menu);
    }


    public void StartGame()
    {
        SetCamera(camera_LevelSelect);
        StartCoroutine(MainRoutine());
    }

    private void SetCamera(Cinemachine.CinemachineVirtualCamera camera)
    {
        camera_LevelSelect.enabled = false;
        camera_Level.enabled = false;
        camera_Menu.enabled = false;

        camera.enabled = true;
    }

    private void Update()
    {
        if(controller != null && Application.isEditor)
        {
            for (int k = 0; k < controller.dice.Length; k++)
            {
                Debug.DrawLine(controller.dice[k].TransformPosition, controller.dice[k].TransformPosition + Vector3.up, Tools.ColorRainbowLerp(controller.dice[k].CurrentSideIndex / 6f));

            }
        }
    }

    private IEnumerator MainRoutine()
    {
        while (true)
        {
            SetCamera(camera_LevelSelect);
            yield return LevelSelectRoutine();
            SetCamera(camera_Level);
            GameManager.instance.board.SetActive(true);
            yield return LevelRoutine();
        }
    }

    private IEnumerator LevelSelectRoutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, levelLayer))
            {
                level = hitInfo.collider.GetComponentInChildren<Level>();
                if (!level.CheckRequirements())
                {
                    level.enabled = true;
                    graveVirtualCameraTarget.transform.position = level.transform.position;
                    level.UpdateUI();
                    controller.RollAll();

                    break;
                }
            }
            yield return null;
        }
    }

    private IEnumerator LevelRoutine()
    {
        lost = false;
        won = false;

        while (true)
        {
            yield return WaitEventRoutines();
            if (won || lost)
            {
                level.EndLevel();
                break;
            }
            yield return WaitForInput();
            yield return WaitEventRoutines();
            yield return WaitMatch();
            yield return WaitEventRoutines();
            if (won || lost)
            {
                level.EndLevel();
                break;
            }
        }
    }

    private IEnumerator WaitForInput()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, dieLayer))
            {
                controller.Roll(hitInfo.collider.GetComponent<PuzzleDieBehaviour>().PuzzleDie);
                lost = level.UseMove();
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
            won = level.CheckRequirements(true);
            if (won)
            {
                // Wait for cool animation and break;
                levelsCompleted++;
                yield return new WaitForSeconds(6.5f);
                if (levelsCompleted >= 6)
                {
                    GameManager.instance.victoryDisco.SetActive(true);
                    Object[] objects = FindObjectsOfType<GraveRiser>();
                    for (int j = 0; j < objects.Length; j++)
                    {
                        (objects[j] as GraveRiser).GetComponent<GraveRiser>().TriggerDance();
                    }
                }
                break;
            }
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
            GameObject dieObject = Instantiate(diePrefab, GameManager.instance.board.transform);
            SO_PuzzleDie asset = GameManager.instance.diceAssets[Random.Range(0, GameManager.instance.diceAssets.Count)];
            dice[i] = new PuzzleDie(asset, dieObject);
            dieObject.GetComponent<PuzzleDieBehaviour>().Init(dice[i]);
        }
        return dice;
    }

}
