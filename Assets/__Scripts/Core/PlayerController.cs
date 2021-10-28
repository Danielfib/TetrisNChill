using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Piece fallingPiece;

    public void OnMoveLeft()
    {
        fallingPiece.MoveInDirection(Vector3.left);
    }

    public void OnMoveRight()
    {
        fallingPiece.MoveInDirection(Vector3.right);
    }

    public void OnRotate()
    {
        fallingPiece.Rotate();
    }

    public void OnAccelerate(InputValue value)
    {
        fallingPiece.SetAcceleration(value.isPressed);
    }
    public void OnSkipFall()
    {
        fallingPiece.SkipFall();
    }

    public void SetNewPiece(Piece p)
    {
        fallingPiece = p;
    }
}
