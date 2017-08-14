using System;
using UnityEngine;

// By @Bullrich
namespace Test
{
    public class ChangeResolution : MonoBehaviour
    {
        [SerializeField] public resolution[] resolutions;

        public int resolutionIndex;
        public bool applyResolution;

        [Serializable]
        public struct resolution
        {
            public int height, width;
        }

        private void Update()
        {
            if (!applyResolution) return;
            updateResolution(resolutionIndex);
            applyResolution = false;
        }

        private void updateResolution(int index)
        {
            if (index > resolutions.Length - 1)
                return;

            Screen.SetResolution(resolutions[index].width, resolutions[index].height, true);
            print(string.Format("Changed resolution to {0}x{1}", resolutions[index].height, resolutions[index].width));
        }
    }
}