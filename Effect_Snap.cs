using UnityEngine;

namespace PopCulture
{
    public class Effect_Snap : UnitEffectBase
    {
        public override void DoEffect()
        {
        }

        public override void Ping()
        {
        }

        public void Remove()
        {
            if (mat)
            {
                transform.root.GetComponentInChildren<UnitColorHandler>().SetMaterial(mat);
            }
        }


        public Material mat;
    }
}
