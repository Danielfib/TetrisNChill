using UnityEngine;
using Tetris.Utils;

namespace Tetris.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] AudioSource soundtrackSrc, sfxSource;
        [SerializeField] AudioClip soundTrack;
        [SerializeField] AudioClip[] chordsProg;
        [SerializeField] AudioClip pieceFinishedFalling, transitionWhoosh;

        int currentChord;

        public void PlayChordProg()
        {
            AudioClip chord = chordsProg[currentChord];
            sfxSource.PlayOneShot(chord);
            currentChord++;
            if (currentChord > chordsProg.Length - 1) currentChord = 0;
        }

        public void PlayFinishedFalling()
        {
            sfxSource.PlayOneShot(pieceFinishedFalling, 0.8f);
        }

        public void PlayTransitionWhoosh()
        {
            sfxSource.PlayOneShot(transitionWhoosh, 1.2f);
        }

        public void ResetChordProg()
        {
            currentChord = 0;
        }

        public void PieceFell(int destroyedHowManyLines)
        {
            if (destroyedHowManyLines > 0)
            {
                PlayChordProg();
            }
            else
            {
                ResetChordProg();
                PlayFinishedFalling();
            }
        }

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
            soundtrackSrc.clip = soundTrack;
            soundtrackSrc.Play();
        }
    }
}