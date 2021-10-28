using System;
using System.Collections;
using System.Collections.Generic;
using Tetris.Managers;
using UnityEngine;
using System.Linq;


public class Piece : MonoBehaviour
{
    private float currentFallTime;

    private float timeCounter;

    int currentCollisions;

    WaitForFixedUpdate updateWait;

    public LayerMask fallingPieceCollisionLayerMask;

    [HideInInspector]
    public Transform blocksParent;

    private void Start()
    {
        timeCounter = Time.time;
        updateWait = new WaitForFixedUpdate();
        currentFallTime = GameManager.Instance.GetStandardFallTime();
        ChangeSelfAndChildrenLayer(LayerMask.NameToLayer("FallingPiece"));
    }

    void Update()
    {
        if(Time.time > timeCounter + currentFallTime)
        {
            Fall();
            timeCounter = Time.time;
        }
    }

    internal void SetAcceleration(bool isAccelerating)
    {
        currentFallTime = isAccelerating ? GameManager.Instance.GetAcceleratedFallTime() : GameManager.Instance.GetStandardFallTime();
    }

    void Fall()
    {
        transform.position += Vector3.down;
        StartCoroutine(WaitForPhysicsCoroutine(() =>
        {
            if (!IsPositionValid())
            {
                transform.position -= Vector3.down;
                FinishFalling();
            }
        }));
    }

    public void SkipFall()
    {
        //TODO: refatorar para ser um for só

        float[] raycastHitsY = new float[transform.childCount];
        for(var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            RaycastHit hit;
            if(Physics.Raycast(child.position, Vector3.down, out hit, Mathf.Infinity, fallingPieceCollisionLayerMask))
            {
                raycastHitsY[i] = hit.point.y;
            }
        }

        //vê qual é a menor distância em que um dos filhos irá colidir com algo
        float closerDistToCollide = Mathf.Infinity;
        for (var i = 0; i < transform.childCount; i++)
        {
            float distToCollide = transform.GetChild(i).position.y - raycastHitsY[i];
            if (distToCollide < closerDistToCollide)
            {
                closerDistToCollide = distToCollide;
            }
        }

        closerDistToCollide -= 0.75f;
        transform.position += Vector3.down * closerDistToCollide;

        FinishFalling();
    }

    public void Rotate()
    {
        transform.eulerAngles += Vector3.forward * 90;
        StartCoroutine(WaitForPhysicsCoroutine(() =>
        {
            if (!IsPositionValid()) transform.eulerAngles -= Vector3.forward * 90;
        }));
    }

    public void MoveInDirection(Vector3 dir)
    {
        transform.position += dir;
        StartCoroutine(WaitForPhysicsCoroutine(() =>
        {
            if (!IsPositionValid()) transform.position -= dir;
        }));
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentCollisions++;
    }

    private void OnCollisionExit(Collision collision)
    {
        currentCollisions = Mathf.Max(0, currentCollisions - 1);
    }

    private bool IsPositionValid()
    {
        return currentCollisions == 0;
    }

    private void FinishFalling()
    {
        ChangeSelfAndChildrenLayer(LayerMask.NameToLayer("StationaryPiece"));
        MatchManager.Instance.SpawnNewPiece();

        StartCoroutine(CheckLineBreak());
    }

    IEnumerator CheckLineBreak()
    {
        yield return updateWait;
        List<float> differentHeights = new List<float>();
        Transform[] children = GetChildren(transform);
        foreach (var c in children)
        {
            c.parent = blocksParent;
            float y = (float)Math.Round(c.position.y, 2);
            c.position = new Vector3(c.position.x, y, c.position.z);
        }

        foreach (var c in children)
        {
            if (!differentHeights.Contains(c.position.y)) differentHeights.Add(c.position.y);
        }


        List<float> linesDestroyed = new List<float>();
        foreach (var y in differentHeights)
        {
            bool didDestroyLine = LineBreakChecker.Instance.CheckLine(y);
            if (didDestroyLine) linesDestroyed.Add(y);
        }

        yield return updateWait;
        if (linesDestroyed.Count > 0)
        {
            linesDestroyed.Sort();
            linesDestroyed.Reverse();
            foreach (var l in linesDestroyed)
            {
                LineBreakChecker.Instance.LowerBlocksAbove(l);
            }
        }

        Destroy(gameObject);
    }

    private void ChangeSelfAndChildrenLayer(int layer)
    {
        gameObject.layer = layer;
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.layer = layer;
        }
    }

    private IEnumerator WaitForPhysicsCoroutine(Action doThat)
    {
        yield return updateWait;
        doThat.Invoke();
    }

    private Transform[] GetChildren(Transform p)
    {
        return GetComponentsInChildren<Transform>().Where(t => t != p).ToArray();
    }
}
