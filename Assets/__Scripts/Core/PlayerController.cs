using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PieceFactory factory;

    Piece fallingPiece;

    Vector3 currentMoveDir;
    float lastMoveTime, moveInterval = 0.16f;
    bool isMoving;

    public void StartPlaying()
    {
        SpawnNewPiece();
    }

    public void SpawnNewPiece()
    {
        Piece newPiece = factory.Spawn().GetComponent<Piece>();
        newPiece.AddFinishedFallingListener(PieceFinishedFalling);
        fallingPiece = newPiece;
    }

    public void PieceFinishedFalling()
    {
        SpawnNewPiece();
    }

    public void StopPlaying()
    {
        if(fallingPiece != null)
        {
            Destroy(fallingPiece.gameObject);
            fallingPiece = null;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            if(Time.time > lastMoveTime + moveInterval)
            {
                fallingPiece?.TryMoveInDirection(currentMoveDir);
                lastMoveTime = Time.time;
            }
        }
    }

    void StartMoving(Vector3 dir)
    {
        fallingPiece?.TryMoveInDirection(dir);
        lastMoveTime = Time.time;
        isMoving = true;
        currentMoveDir = dir;
    }

    void StopMoving()
    {
        isMoving = false;
    }

    #region InputAction Messages
    public void OnMoveLeft(InputValue value)
    {
        if (value.isPressed)
        {
            StartMoving(Vector3.left);
        } else
        {
            StopMoving();
        }
    }

    public void OnMoveRight(InputValue value)
    {
        if (value.isPressed)
        {
            StartMoving(Vector3.right);
        }
        else
        {
            StopMoving();
        }
    }

    public void OnRotate()
    {
        fallingPiece?.Rotate();
    }

    public void OnAccelerate(InputValue value)
    {
        fallingPiece?.SetAcceleration(value.isPressed);
    }

    public void OnSkipFall()
    {
        fallingPiece?.SkipFall();
    }
    #endregion
}
