using UnityEngine;
using System;
using Landfall.TABS;

namespace PopCulture
{
    public class Kill : MonoBehaviour
    {
        public void KillYourself()
        {
            transform.root.GetComponent<Unit>().data.healthHandler.Die();
        }
    }
}
