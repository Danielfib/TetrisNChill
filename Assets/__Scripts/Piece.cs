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

    private void Start()
    {
        timeCounter = Time.time;
        updateWait = new WaitForFixedUpdate();
        currentFallTime = GameManager.Instance.GetStandardFallTime();
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
            enabled = false;
            MatchManager.Instance.SpawnNewPiece();
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
}
