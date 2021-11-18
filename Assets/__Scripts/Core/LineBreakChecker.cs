using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tetris.Utils;
using Tetris.Extensions;

namespace Tetris.Managers
{
    public class LineBreakChecker : Singleton<LineBreakChecker>
    {
        [SerializeField] Transform blocksParent;

        private const float TOLERANCE = 0.1f, MAX_BLOCKS_PER_ROW = 10;

        public void CheckFallenPiece(Transform piece)
        {
            StartCoroutine(HandleFallenPieceCoroutine(piece));
        }

        private IEnumerator HandleFallenPieceCoroutine(Transform piece)
        {
            yield return new WaitForFixedUpdate();

            var heightsToCheck = GetChildrenDifferentHeights(piece);
            ReleaseBlocks(piece);
            var linesDestroyed = DestroyFullLines(heightsToCheck);
            AudioManager.Instance.PieceFell(linesDestroyed.Count);

            yield return new WaitForFixedUpdate();

            LowerBlocksAboveDestroyedLines(linesDestroyed);
            Destroy(piece.gameObject);
        }

        private void ReleaseBlocks(Transform piece)
        {
            Transform[] children = piece.GetChildren();
            foreach (var c in children)
            {
                c.parent = blocksParent;
            }
        }

        private List<float> GetChildrenDifferentHeights(Transform piece)
        {
            Transform[] children = piece.GetChildren();
            List<float> differentHeights = new List<float>();
            foreach (var c in children)
            {
                float roundedLineY = c.position.y.Round(0.5f);
                if (!differentHeights.Contains(roundedLineY)) differentHeights.Add(roundedLineY);
            }
            return differentHeights;
        }

        private List<float> DestroyFullLines(List<float> lines)
        {
            List<float> linesDestroyed = new List<float>();
            foreach (var y in lines)
            {
                bool didDestroyLine = CheckLine(y);
                if (didDestroyLine) linesDestroyed.Add(y);
            }

            return linesDestroyed;
        }

        private void LowerBlocksAboveDestroyedLines(List<float> lines)
        {
            if (lines.Count > 0)
            {
                lines.Sort();
                lines.Reverse();
                foreach (var line in lines) LowerBlocksAbove(line);
            }
        }

        private bool CheckLine(float lineY)
        {
            var blocksInLine = transform.GetChildren().Where(t => t.position.y.Difference(lineY) < TOLERANCE);
            if (blocksInLine.Count() >= MAX_BLOCKS_PER_ROW)
            {
                DestroyBlocksInLine(blocksInLine);

                MatchHUDManager.Instance.Score();
                return true;
            }
            return false;
        }

        private void DestroyBlocksInLine(IEnumerable<Transform> blocksInLine)
        {
            foreach (var block in blocksInLine) Destroy(block.gameObject);
        }

        private void LowerBlocksAbove(float lineY)
        {
            var childrenAboveLine = transform.GetComponentsInChildren<Transform>().Where(t => t.position.y > lineY + TOLERANCE);
            foreach (var child in childrenAboveLine) child.position += Vector3.down;
        }
    }
}