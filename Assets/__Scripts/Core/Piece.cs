using System;
using Tetris.Managers;
using Tetris.Extensions;
using UnityEngine;

namespace Tetris.Core
{
    public class Piece : MonoBehaviour
    {
        public LayerMask fallingPieceCollisionLayerMask;
    
        private float currentFallTime;
        private float timeCounter;

        Action finishedFalling;

        GameObject preview;
        [SerializeField] Material previewMat;

        private void Start()
        {
            timeCounter = Time.time;
            currentFallTime = GameManager.Instance.GetStandardFallTime();
            gameObject.SetSelfAndChildrenLayer(LayerMask.NameToLayer("FallingPiece"));
            InstantiatePreview();
        }

        void InstantiatePreview()
        {
            preview = Instantiate(gameObject);
            Destroy(preview.GetComponent<Piece>());        
            foreach(var col in preview.GetComponentsInChildren<Collider>())
            {
                Destroy(col);
            }
            foreach(var ren in preview.GetComponentsInChildren<Renderer>())
            {
                ren.material = previewMat;
            }
        }

        void PlacePreview()
        {
            if (preview == null) return;
            float closerDistToCollide = CalculateCollisionDistance();
            preview.transform.position = transform.position + Vector3.down * closerDistToCollide;
            preview.transform.eulerAngles = transform.eulerAngles;
        }

        void Update()
        {
            if(Time.time > timeCounter + currentFallTime)
            {
                Fall();
                timeCounter = Time.time;
            }

            PlacePreview();
        }

        internal void SetAcceleration(bool isAccelerating)
        {
            currentFallTime = isAccelerating ? GameManager.Instance.GetAcceleratedFallTime() : GameManager.Instance.GetStandardFallTime();
        }

        void Fall()
        {
            if (IsFutureChildrenPositionValid(0, -1))
                transform.position += Vector3.down;
            else
                FinishFalling();
        }

        public void SkipFall()
        {
            float closerDistToCollide = CalculateCollisionDistance();
            transform.position += Vector3.down * closerDistToCollide;

            FinishFalling();
        }

        private float CalculateCollisionDistance()
        {
            float[] raycastHitsY = new float[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                RaycastHit hit;
                if (Physics.Raycast(child.position, Vector3.down, out hit, Mathf.Infinity, fallingPieceCollisionLayerMask))
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

            closerDistToCollide -= 0.5f;
            return closerDistToCollide;
        }

        public void Rotate()
        {
            transform.eulerAngles += Vector3.forward * 90;
            if (!IsFutureChildrenPositionValid())
            {
                TryToFindValidRot();
            }
        }

        void TryToFindValidRot()
        {
            var validOffset = TryToFindValidPositionWithinDist(1);
            if (validOffset == Vector3.zero)
            {
                var validOffestWithDouble = TryToFindValidPositionWithinDist(2);
                if (validOffestWithDouble == Vector3.zero)
                {
                    //did not find available rot, undo
                    transform.eulerAngles -= Vector3.forward * 90;
                }
                else
                {
                    transform.position += validOffestWithDouble;
                }
            }
            else
            {
                transform.position += validOffset;
            }
        }

        Vector3 TryToFindValidPositionWithinDist(int dist)
        {
            bool isAvailableToTheLeft = IsFutureChildrenPositionValid(-dist, 0);
            bool isAvailableToTheRight = IsFutureChildrenPositionValid(dist, 0);

            if (isAvailableToTheLeft)
                return Vector3.left * dist;
            else if (isAvailableToTheRight)
                return Vector3.right * dist;
            else
                return Vector3.zero;
        }

        public void TryMoveInDirection(Vector3 dir)
        {
            if (IsFutureChildrenPositionValid(dir.x, dir.y))
            {
                transform.position += dir;
            } 
        }

        private bool IsFutureChildrenPositionValid(float xOffset = 0, float yOffset = 0)
        {
            Transform[] children = transform.GetChildren();
            foreach(var c in children)
            {
                bool isPositionOccupied = GridManager.Instance.CheckPosition(c.position.x + xOffset, c.position.y + yOffset);
                if (isPositionOccupied) return false;
            }
            return true;
        }

        private void FinishFalling()
        {
            gameObject.SetSelfAndChildrenLayer(LayerMask.NameToLayer("StationaryPiece"));
            finishedFalling.Invoke();
            enabled = false;
            LineBreakChecker.Instance.CheckFallenPiece(transform);
        }

        public void AddFinishedFallingListener(Action callback)
        {
            finishedFalling += callback;
        }

        public void OnDestroy()
        {
            Destroy(preview);
            preview = null;
        }

        public void OnDisable()
        {
            Destroy(preview);
            preview = null;
        }

        public void ReturnToGame()
        {
            if (preview == null)
            {
                currentFallTime = GameManager.Instance.GetStandardFallTime();
                InstantiatePreview();
            }
        }
    }
}
