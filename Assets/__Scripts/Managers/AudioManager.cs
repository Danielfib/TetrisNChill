using UnityEngine;
using Tetris.Utils;

namespace Tetris.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}