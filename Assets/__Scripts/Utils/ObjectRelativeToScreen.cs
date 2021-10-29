using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris.Utils
{
    [ExecuteInEditMode]
    public class ObjectRelativeToScreen : MonoBehaviour
    {
        [Range(0, 1)]
        public float xMargin = 0.1f, yMargin = 0.1f;
        public HorizontalAlignmentEnum horizontalAlignment = HorizontalAlignmentEnum.LEFT;
        public VerticalAlignmentEnum verticalAlignment = VerticalAlignmentEnum.TOP;

        void Start()
        {
            Position();
        }

        private void Position()
        {
            var camera = Camera.main;
            float x = CalculateHorizontalPosition(horizontalAlignment);
            float y = CalculateVerticalPosition(verticalAlignment);
            transform.position = camera.ScreenToWorldPoint(new Vector3(x, y, 5));
        }

        private float CalculateHorizontalPosition(HorizontalAlignmentEnum hEnum)
        {
            float screenHPos = 0;
            float absX = Screen.width * xMargin;
            switch (hEnum)
            {
                case HorizontalAlignmentEnum.LEFT:
                    screenHPos = absX;
                    break;
                case HorizontalAlignmentEnum.CENTER:
                    screenHPos = Screen.width / 2 + absX;
                    break;
                case HorizontalAlignmentEnum.RIGHT:
                    screenHPos = Screen.width - absX;
                    break;
            }
            return screenHPos;
        }

        private float CalculateVerticalPosition(VerticalAlignmentEnum vEnum)
        {
            float screenVPos = 0;
            float absY = Screen.height * yMargin;
            switch (vEnum)
            {
                case VerticalAlignmentEnum.TOP:
                    screenVPos = Screen.height - absY;
                    break;
                case VerticalAlignmentEnum.CENTER:
                    screenVPos = Screen.height / 2 - absY;
                    break;
                case VerticalAlignmentEnum.BOTTOM:
                    screenVPos = absY;
                    break;
            }
            return screenVPos;
        }

        public enum HorizontalAlignmentEnum
        {
            LEFT,
            CENTER,
            RIGHT
        }

        public enum VerticalAlignmentEnum
        {
            TOP,
            CENTER,
            BOTTOM
        }

#if UNITY_EDITOR
        private void Update()
        {
            Position();
        }
#endif
    }
}
