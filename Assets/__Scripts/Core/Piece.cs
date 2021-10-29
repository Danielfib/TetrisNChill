using System;
using System.Collections;
using Tetris.Managers;
using Tetris.Extensions;
using UnityEngine;
using System.Linq;

public class Piece : MonoBehaviour
{
    public LayerMask fallingPieceCollisionLayerMask;
    
    private float currentFallTime;
    private float timeCounter;

    int currentCollisions;
    WaitForFixedUpdate updateWait;
    Action finishedFalling;

    GameObject preview;
    [SerializeField] Material previewMat;

    private void Start()
    {
        timeCounter = Time.time;
        updateWait = new WaitForFixedUpdate();
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

        closerDistToCollide -= 0.75f;
        return closerDistToCollide;
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
        finishedFalling.Invoke();
        enabled = false;
        LineBreakChecker.Instance.CheckFallenPiece(transform);
    }

    public void AddFinishedFallingListener(Action callback)
    {
        finishedFalling += callback;
    }

    private IEnumerator WaitForPhysicsCoroutine(Action doThat)
    {
        yield return updateWait;
        doThat.Invoke();
    }

    public void OnDestroy()
    {
        Destroy(preview);
    }
}
