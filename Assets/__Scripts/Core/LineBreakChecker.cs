using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tetris.Utils;
using Tetris.Extensions;
using System;
using Tetris.Managers;

public class LineBreakChecker : Singleton<LineBreakChecker>
{
    [SerializeField] Transform blocksParent;

    private const float TOLERANCE = 0.1f;

    public void CheckFallenPiece(Transform piece)
    {
        StartCoroutine(CheckFallenPieceCoroutine(piece));
    }

    private IEnumerator CheckFallenPieceCoroutine(Transform piece)
    {
        yield return new WaitForFixedUpdate();

        var heightsToCheck = GetChildrenDifferentHeights(piece);
        ReleaseBlocks(piece);
        var linesDestroyed = DestroyFullLines(heightsToCheck);

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

        AudioManager.Instance.PieceFell(linesDestroyed.Count);

        return linesDestroyed;
    }

    private void LowerBlocksAboveDestroyedLines(List<float> lines)
    {
        if (lines.Count > 0)
        {
            lines.Sort();
            lines.Reverse();
            foreach (var l in lines) LowerBlocksAbove(l);
        }
    }

    public bool CheckLine(float lineY)
    {
        var childrenInLine = transform.GetChildren().Where(t => t.position.y.Difference(lineY) < TOLERANCE);
        //print("children in: " + lineY + "-----" + childrenInLine.Count());
        if (childrenInLine.Count() >= 10)
        {
            foreach (var child in childrenInLine) Destroy(child.gameObject);

            MatchHUDManager.Instance.Score();
            return true;
        }
        return false;
    }

    public void LowerBlocksAbove(float y)
    {
        var childrenAboveLine = transform.GetComponentsInChildren<Transform>().Where(t => t.position.y > y + TOLERANCE);
        foreach (var child in childrenAboveLine) child.position -= Vector3.up;
    }
}