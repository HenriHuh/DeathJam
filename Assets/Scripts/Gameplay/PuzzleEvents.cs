using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleEvents
{
    /// <summary>
    /// Visual events which are triggered by gameplay.
    /// </summary>
    public interface PuzzleEvent
    {
        public IEnumerator EventRoutine();
    }

    public class PuzzleEvent_DieRoll : PuzzleEvent
    {
        private PuzzleDie puzzleDie;
        private int dieSideIndex;

        public PuzzleEvent_DieRoll(PuzzleDie puzzleDie)
        {
            this.puzzleDie = puzzleDie;
            dieSideIndex = puzzleDie.CurrentSideIndex;
        }

        public IEnumerator EventRoutine()
        {
            // Play and wait for die roll animation here
            puzzleDie.GameObject.SetActive(true);

            // Do Tumble
            puzzleDie.GameObject.transform.position = GameManager.instance.transform.position +
                new Vector3(puzzleDie.Position.x * GameManager.instance.diceScaleFactor, 0, puzzleDie.Position.y * GameManager.instance.diceScaleFactor)
                - Vector3.one * GameManager.instance.diceScaleFactor * 1.5f + Vector3.up * GameManager.instance.diceScaleFactor * 1.5f;
            DiceTumble tumble = puzzleDie.GameObject.GetComponentInChildren<DiceTumble>();
            tumble.DiceRoll(dieSideIndex + 1);

            yield return new WaitForSeconds(0.05f);
        }


    }

    public class PuzzleEvent_DiceMatch : PuzzleEvent
    {
        private List<(PuzzleDie, int)> puzzleDice;
        public PuzzleEvent_DiceMatch(List<PuzzleDie> puzzleDice)
        {
            this.puzzleDice = new List<(PuzzleDie, int)>();
            for (int i = 0; i < puzzleDice.Count; i++)
            {
                this.puzzleDice.Add((puzzleDice[i], puzzleDice[i].CurrentSideIndex));
            }
        }

        public IEnumerator EventRoutine()
        {
            // Play match animations here

            // PLACEHOLDER SCALE ANIMATION !!!
            for (int i = 0; i < puzzleDice.Count; i++)
            {
                switch (puzzleDice[i].Item2)
                {
                    case 0:
                        LevelManager.instance.level.AddType(Enums.ElementalType.Mind);
                        break;
                    case 1:
                        LevelManager.instance.level.AddType(Enums.ElementalType.Heart);
                        break;
                    case 2:
                        LevelManager.instance.level.AddType(Enums.ElementalType.Mind);
                        LevelManager.instance.level.AddType(Enums.ElementalType.Heart);
                        break;
                    case 3:
                        LevelManager.instance.level.AddType(Enums.ElementalType.Soul);
                        break;
                    case 4:
                        LevelManager.instance.level.AddType(Enums.ElementalType.Soul);
                        LevelManager.instance.level.AddType(Enums.ElementalType.Weird);
                        break;
                    case 5:
                        LevelManager.instance.level.AddType(Enums.ElementalType.Weird);
                        break;
                    default:
                        Debug.LogError("Invalid index: " + puzzleDice[i].Item2);
                        break;
                }
            }
            puzzleDice.ForEach((n) => n.Item1.GameObject.GetComponentInChildren<DiceTumble>().DiceScore(n.Item2));
            yield return new WaitForSeconds(1f);


            yield return null;
        }
    }

    public class PuzzleEvent_Wait : PuzzleEvent
    {
        float time;
        public PuzzleEvent_Wait(float time)
        {
            this.time = time;
        }

        public IEnumerator EventRoutine()
        {
            yield return new WaitForSeconds(time);
        }
    }

}

