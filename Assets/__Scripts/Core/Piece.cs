using System;
using System.Collections;
using Tetris.Managers;
using Tetris.Extensions;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public LayerMask fallingPieceCollisionLayerMask;
    
    private float currentFallTime;
    private float timeCounter;

    int currentCollisions;
    WaitForFixedUpdate updateWait;

    private void Start()
    {
        timeCounter = Time.time;
        updateWait = new WaitForFixedUpdate();
        currentFallTime = GameManager.Instance.GetStandardFallTime();
        gameObject.SetSelfAndChildrenLayer(LayerMask.NameToLayer("FallingPiece"));
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
        gameObject.SetSelfAndChildrenLayer(LayerMask.NameToLayer("StationaryPiece"));
        MatchManager.Instance.SpawnNewPiece();
        LineBreakChecker.Instance.CheckFallenPiece(transform);
    }

    private IEnumerator WaitForPhysicsCoroutine(Action doThat)
    {
        yield return updateWait;
        doThat.Invoke();
    }
}
