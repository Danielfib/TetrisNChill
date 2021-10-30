using Tetris.Utils;
using UnityEngine;

namespace Tetris.Managers
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] Transform map;
        [SerializeField] LayerMask obstacleLayer;

        public bool CheckPosition(float x, float y)
        {
            bool isPosOccupied = false;
            Vector3 checkPosition = map.position + new Vector3(x, y + 0.5f, 0);
            Vector3 rayOrigin = checkPosition + Vector3.back * 10;
            if(Physics.Raycast(rayOrigin, checkPosition - rayOrigin, Mathf.Infinity, obstacleLayer))
            {
                isPosOccupied = true;
            }
            return isPosOccupied;
        }           
    }
}
