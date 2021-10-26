using UnityEngine;
using Tetris.Utils;

namespace Tetris.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}
