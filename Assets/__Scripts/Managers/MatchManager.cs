using UnityEngine;
using Tetris.Utils;

namespace Tetris.Managers
{
    public class MatchManager : Singleton<MatchManager>
    {
        [SerializeField] PieceFactory factory;
        [SerializeField] PlayerController player;
        [SerializeField] Transform BlocksParent;

        private void Start()
        {
            StartMatch();
        }

        public void SpawnNewPiece()
        {
            Piece newPiece = factory.Spawn().GetComponent<Piece>();
            newPiece.blocksParent = BlocksParent;
            player.SetNewPiece(newPiece);
        }

        public void StartMatch()
        {
            SpawnNewPiece();
        }
    }
}
