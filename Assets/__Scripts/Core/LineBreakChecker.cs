using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tetris.Utils;
using Tetris.Extensions;
using System;

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

        ReleaseBlocks(piece);
        var heightsToCheck = GetChildrenDifferentHeights(piece);
        var linesDestroyed = DestroyFullLines(heightsToCheck);

        yield return new WaitForFixedUpdate();

        LowerBlocksAboveDestroyedLines(linesDestroyed);
        Destroy(gameObject);
    }

    private void ReleaseBlocks(Transform piece)
    {
        Transform[] children = piece.GetChildren();
        foreach (var c in children)
        {
            c.parent = blocksParent;
            float y = (float)Math.Round(c.position.y, 2);
            c.position = new Vector3(c.position.x, y, c.position.z);
        }
    }

    private List<float> GetChildrenDifferentHeights(Transform piece)
    {
        Transform[] children = piece.GetChildren();
        List<float> differentHeights = new List<float>();
        foreach (var c in children)
        {
            if (!differentHeights.Contains(c.position.y)) differentHeights.Add(c.position.y);
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
            foreach (var l in lines) LowerBlocksAbove(l);
        }
    }

    public bool CheckLine(float lineY)
    {
        var childrenInLine = transform.GetComponentsInChildren<Transform>().Where(t => Distance(t.position.y, lineY) < TOLERANCE);
        //print("children in: " + lineY + "-----" + childrenInLine.Count());
        if(childrenInLine.Count() >= 10)
        {
            foreach (var child in childrenInLine) Destroy(child.gameObject);

            return true;
        }
        return false;
    }

    public void LowerBlocksAbove(float y)
    {
        var childrenAboveLine = transform.GetComponentsInChildren<Transform>().Where(t => t.position.y > y + TOLERANCE);
        foreach (var child in childrenAboveLine) child.position -= Vector3.up;
    }

    float Distance(float a, float b)
    {
        return Mathf.Abs(Mathf.Abs(a) - Mathf.Abs(b));
    }
}
