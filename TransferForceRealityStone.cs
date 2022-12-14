using System;
using Landfall.TABS;
using UnityEngine;

public class TransferForceRealityStone : MonoBehaviour
{
	private void Start()
	{
		RealityStoneSpawner component = GetComponent<RealityStoneSpawner>();
		component.spawnUnitAction = (Action<GameObject>)Delegate.Combine(component.spawnUnitAction, new Action<GameObject>(Transfer));
	}

	public void Transfer(GameObject unitRoot)
	{
		Unit component = unitRoot.GetComponent<Unit>();
		Unit component2 = base.transform.root.GetComponent<Unit>();
		for (int i = 0; i < component.data.allRigs.AllRigs.Length; i++)
		{
			component.data.allRigs.AllRigs[i].velocity = component2.data.mainRig.velocity;
		}
	}
}
