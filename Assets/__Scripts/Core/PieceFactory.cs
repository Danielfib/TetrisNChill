using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PieceFactory : MonoBehaviour
{
    public GameObject[] pieces;

    Dictionary<int, bool> piecesChosen = new Dictionary<int, bool>();

    public void Awake()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            piecesChosen.Add(i, false);
        }
    }

    public GameObject Spawn()
    {
        int chosenPiece = SelectAvalailablePiece();
        piecesChosen[chosenPiece] = true;

        GameObject newPiece = Instantiate(pieces[chosenPiece]);
        newPiece.transform.position = transform.position;
        return newPiece;
    }

    private int SelectAvalailablePiece()
    {
        var avalilableIndexes = piecesChosen.Where(p => !p.Value).ToList();
        if (avalilableIndexes.Count == 0)
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                piecesChosen[i] = false;
            }
            avalilableIndexes = piecesChosen.Where(p => !p.Value).ToList();
        }
        int randomIndex = Random.Range(0, avalilableIndexes.Count());
        return avalilableIndexes[randomIndex].Key;
    }
}
