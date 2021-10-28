using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tetris.Utils;

public class LineBreakChecker : Singleton<LineBreakChecker>
{
    private const float TOLERANCE = 0.1f;

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
