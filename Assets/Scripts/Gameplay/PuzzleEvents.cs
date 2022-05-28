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

        public PuzzleEvent_DieRoll(PuzzleDie puzzleDie)
        {
            this.puzzleDie = puzzleDie;
        }

        public IEnumerator EventRoutine()
        {
            // Play and wait for die roll animation here
            puzzleDie.GameObject.SetActive(true);

            // Do Tumble
            puzzleDie.GameObject.transform.position = new Vector3(puzzleDie.Position.x, 1, puzzleDie.Position.y) - Vector3.one * 1.5f + Vector3.up * 1.5f;
            DiceTumble tumble = puzzleDie.GameObject.GetComponentInChildren<DiceTumble>();
            tumble.DiceRoll(puzzleDie.CurrentSideIndex);

            float t = 0;
            while (t < 0.1f)
            {
                t += Time.deltaTime;
                yield return null;
            }


            yield return null;
        }


    }

    public class PuzzleEvent_DiceMatch : PuzzleEvent
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

