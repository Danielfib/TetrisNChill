using UnityEngine;
using UnityEngine.InputSystem;

namespace Tetris.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PieceFactory factory;
        [SerializeField] Transform heldPieceHolder;

        Piece fallingPiece, heldPiece;

        Vector3 currentMoveDir;
        float lastMoveTime, moveInterval = 0.16f;
        bool isMoving, isAbleToHold = true;

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
            isAbleToHold = true;
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

        void HoldPiece(Piece piece)
        {
            piece.transform.parent = heldPieceHolder;
            piece.transform.localPosition = Vector3.zero;
            piece.enabled = false;
        }

        void SpawnHeldPiece()
        {
            heldPiece.transform.position = factory.transform.position;
            heldPiece.enabled = true;
            heldPiece.ReturnToGame();
        }

        #region InputAction Messages
        public void OnMoveLeft(InputValue value)
        {
            if (value.isPressed)
                StartMoving(Vector3.left);
            else
                StopMoving();
        }

        public void OnMoveRight(InputValue value)
        {
            if (value.isPressed)
                StartMoving(Vector3.right);
            else
                StopMoving();
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

        public void OnHoldPiece()
        {
            if (!isAbleToHold) return;

            isAbleToHold = false;
            if(heldPiece == null)
            {
                heldPiece = fallingPiece;
                HoldPiece(heldPiece);
                SpawnNewPiece();
            } else
            {
                SpawnHeldPiece();
                HoldPiece(fallingPiece);
                Piece aux = fallingPiece;
                fallingPiece = heldPiece;
                heldPiece = aux;
            }
        }
        #endregion
    }
}
