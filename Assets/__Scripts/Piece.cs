using System;
using System.Collections;
using System.Collections.Generic;
using Tetris.Managers;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private float currentFallTime;

    private float timeCounter;

    int currentCollisions;

    WaitForFixedUpdate updateWait;

    public LayerMask fallingPieceCollisionLayerMask;

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
        StartCoroutine(FallCoroutine());
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
        StartCoroutine(RotateBackCoroutine());
    }

    public void MoveInDirection(Vector3 dir)
    {
        transform.position += dir;
        StartCoroutine(MoveBackCoroutine(dir));
    }

    IEnumerator RotateBackCoroutine()
    {
        yield return updateWait;
        if (!IsPositionValid())
        {
            transform.eulerAngles -= Vector3.forward * 90;
        }
    }

    IEnumerator MoveBackCoroutine(Vector3 dir)
    {
        yield return updateWait;
        if (!IsPositionValid())
        {
            transform.position -= dir;
        }
    }

    IEnumerator FallCoroutine()
    {
        yield return updateWait;
        if (!IsPositionValid())
        {
            transform.position -= Vector3.down;
            FinishFalling();
        }
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
        enabled = false;
        ChangeSelfAndChildrenLayer(LayerMask.NameToLayer("StationaryPiece"));
        MatchManager.Instance.SpawnNewPiece();
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
}
