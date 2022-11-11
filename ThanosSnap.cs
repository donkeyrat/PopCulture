using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Landfall.TABS;
using Landfall.TABS.AI.Systems;
using Unity.Entities;

namespace PopCulture {
	public class ThanosSnap : MonoBehaviour {
	
	    void Start() {
	
	        team = transform.root.GetComponent<Unit>().Team;
	        teamSustem = World.Active.GetOrCreateManager<TeamSystem>();
	    }
	
	    public void AddEffectToUnits() {
			ClearHitList();
			StartCoroutine(DoEffect(teamSustem.GetTeamUnits(team == Team.Red ? Team.Blue : Team.Red)));
	    }
	
		public IEnumerator DoEffect(List<Unit> enemyUnits) {
			if (addEffectType == SelectionType.Percentage && enemyUnits.Count / specific > 0) {
				for (int i = 0; i < enemyUnits.Count / specific; i++) {
					if (!hitList.Contains(enemyUnits[i])) {
						AddEffectt(transform.root.GetComponent<Unit>(), enemyUnits[i].data);
						hitList.Add(enemyUnits[i]);
						yield return new WaitForSeconds(delay);
					}
				}
			}
			else if (addEffectType == SelectionType.Specific) {
				for (int i = 0; i < specific + 1; i++) {
					if (enemyUnits.Count <= 0) { yield break; }
					var chosenUnit = enemyUnits[Random.Range(0, enemyUnits.Count - 1)];
					if (!hitList.Contains(chosenUnit)) {
						AddEffectt(transform.root.GetComponent<Unit>(), chosenUnit.data);
						hitList.Add(chosenUnit);
						yield return new WaitForSeconds(delay);
					}
				}
			}
		}
	
		public void ClearHitList() {
			hitList.Clear();
		}
	
		public void AddEffectt(Unit attacker, DataHandler targetData)
		{
			if (!(attacker == null) && (!targetData || !targetData.Dead) && (bool)targetData)
			{
				effectToAdd.GetType();
				UnitEffectBase unitEffectBase = UnitEffectBase.AddEffectToTarget(targetData.unit.transform.gameObject, effectToAdd);
				if (unitEffectBase == null)
				{
					GameObject obj = Object.Instantiate(effectToAdd.gameObject, targetData.unit.transform.root);
					obj.transform.position = targetData.unit.transform.position;
					obj.transform.rotation = Quaternion.LookRotation(targetData.mainRig.position - attacker.data.mainRig.position);
					unitEffectBase = obj.GetComponent<UnitEffectBase>();
					TeamHolder.AddTeamHolder(obj, base.transform.root.gameObject);
					unitEffectBase.DoEffect();
				}
				else if (!onlyOnce)
				{
					unitEffectBase.transform.rotation = Quaternion.LookRotation(targetData.mainRig.position - attacker.data.mainRig.position);
					unitEffectBase.Ping();
				}
				this.AddedEffectE?.Invoke(attacker, targetData);
			}
		}
	
		public enum SelectionType {
			Percentage,
			Specific
		}
	
		private Team team;
	
	    private TeamSystem teamSustem;
	
		private List<Unit> hitList = new List<Unit>();
	
		public SelectionType addEffectType;
	
		public int specific;
	
		public delegate void AddedEffectEventHandlerr(Unit attacker, DataHandler targetData);
	
		public UnitEffectBase effectToAdd;
	
		public float delay;
	
		public event AddedEffectEventHandlerr AddedEffectE;
	
		public bool onlyOnce;
	}
}