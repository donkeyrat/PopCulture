using UnityEngine;
using System.Collections;

namespace PopCulture
{
    public class PCMaterialEvent : MonoBehaviour
    {
        public void ChangeMaterial()
        {
            t = 0f;
            changing = true;
        }

        void Update()
        {
            if (changing && t < lerpTime && Time.timeScale != 0f)
            {
                t += Time.deltaTime;
                rend.materials[index].Lerp(rend.materials[index], newMat, t / lerpTime);
            }
            else if (t > lerpTime) { changing = false; }
            else if (!changing) { t = 0f; }
        }

        private bool changing;

        private float t;

        public Renderer rend;

        public int index;

        public Material newMat;

        public float lerpTime = 0.6f;
    }
}
