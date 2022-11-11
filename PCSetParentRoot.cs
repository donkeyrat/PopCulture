using UnityEngine;

namespace PopCulture
{
    public class PCSetParentRoot : MonoBehaviour
    {
        public void Start()
        {
            if (setParentOnStart)
            {
                Go();
            }
        }

        public void Go()
        {
            transform.parent = transform.root;
            transform.SetParent(transform.root);
        }

        public bool setParentOnStart = true;
    }
}
