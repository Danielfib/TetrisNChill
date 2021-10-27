using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFactory : MonoBehaviour
{
    public GameObject[] pieces;

    public GameObject Spawn()
    {
        int randomIndex = Random.Range(0, pieces.Length);
        GameObject newPiece = Instantiate(pieces[randomIndex]);
        newPiece.transform.position = transform.position;
        return newPiece;
    }
}
