using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineChecker : MonoBehaviour
{
    List<GameObject> collidingObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        collidingObjects.Add(other.gameObject);
        if(collidingObjects.Count == 10)
        {
            DestroyLine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidingObjects.Remove(other.gameObject);
    }

    private void DestroyLine()
    {
        foreach (var go in collidingObjects) Destroy(go);
        //TODO: descer objetos
        // talves seja melhor checar por altura, em vez de ter vários colliders
        // e aí esse mesmo cara desceria todos os blocos acima das linhas quebradas
        //
    }
}
