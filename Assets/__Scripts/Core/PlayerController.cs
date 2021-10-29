using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PieceFactory factory;

    Piece fallingPiece;

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

    #region InputAction Messages
    public void OnMoveLeft()
    {
        fallingPiece?.MoveInDirection(Vector3.left);
    }

    public void OnMoveRight()
    {
        fallingPiece?.MoveInDirection(Vector3.right);
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
