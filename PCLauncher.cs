using BepInEx;
using UnityEngine;

namespace PopCulture
{
	[BepInPlugin("teamgrad.popculture", "PopCulture", "1.1.1")]
	public class PCLauncher : BaseUnityPlugin
	{
		public PCLauncher()
		{
			PCBinder.UnitGlad();
		}
	}
}
