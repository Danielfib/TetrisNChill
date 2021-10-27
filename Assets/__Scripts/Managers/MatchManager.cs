using UnityEngine;
using Tetris.Utils;

namespace Tetris.Managers
{
    public class MatchManager : Singleton<MatchManager>
    {
        [SerializeField] PieceFactory factory;
        [SerializeField] PlayerController player;

        private void Start()
        {
            StartMatch();
        }

        public void SpawnNewPiece()
        {
            GameObject newPiece = factory.Spawn();
            player.SetNewPiece(newPiece.GetComponent<Piece>());
        }

        public void StartMatch()
        {
            SpawnNewPiece();
        }
    }
}
