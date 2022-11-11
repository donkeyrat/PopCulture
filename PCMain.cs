using Landfall.TABS;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection;
using Landfall.TABS.UnitEditor;
using Landfall.TABS.Workshop;

namespace PopCulture {
    public class PCMain {

        public PCMain() {

            //AssetBundle.LoadFromMemory(Properties.Resources.judgementhall);

            var db = LandfallUnitDatabase.GetDatabase();

            List<Faction> factions = (List<Faction>)typeof(LandfallUnitDatabase).GetField("Factions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
            foreach (var fac in popculture.LoadAllAssets<Faction>()) {

                var theNew = new List<UnitBlueprint>(fac.Units);
                var veryNewUnits = (
                    from UnitBlueprint unit
                    in fac.Units
                    orderby unit.GetUnitCost()
                    select unit).ToList();
                fac.Units = veryNewUnits.ToArray();
                foreach (var vFac in factions) {

                    if (fac.Entity.Name == vFac.Entity.Name + "_NEW") {

                        var vFacUnits = new List<UnitBlueprint>(vFac.Units);
                        vFacUnits.AddRange(fac.Units);
                        var newUnits = (
                            from UnitBlueprint unit
                            in vFacUnits
                            orderby unit.GetUnitCost()
                            select unit).ToList();
                        vFac.Units = newUnits.ToArray();
                        Object.DestroyImmediate(fac);
                    }
                }
            }

            foreach (var sb in popculture.LoadAllAssets<SoundBank>()) {

                if (sb.name.Contains("Sound")) {

                    var vsb = ServiceLocator.GetService<SoundPlayer>().soundBank;
                    var cat = vsb.Categories.ToList();
                    cat.AddRange(sb.Categories);
                    vsb.Categories = cat.ToArray();
                }
                if (sb.name.Contains("Music")) {

                    var vsb = ServiceLocator.GetService<MusicHandler>().bank;
                    var cat = vsb.Categories.ToList();
                    cat.AddRange(sb.Categories);
                    foreach (var categ in sb.Categories) {
                        foreach (var sound in categ.soundEffects) {
                            var song = new SongInstance();
                            song.clip = sound.clipTypes[0].clips[0];
                            song.soundEffectInstance = sound;
                            song.songRef = categ.categoryName + "/" + sound.soundRef;
                            ServiceLocator.GetService<MusicHandler>().m_songs.Add(song.songRef, song);
                        }
                    }
                }
            }

            foreach (var fac in popculture.LoadAllAssets<Faction>()) {

                db.FactionList.AddItem(fac);
                db.AddFactionWithID(fac);
            }

            foreach (var unit in popculture.LoadAllAssets<UnitBlueprint>()) {

                if (!db.UnitList.Contains(unit)) {

                    db.UnitList.AddItem(unit);
                    db.AddUnitWithID(unit);
                }
            }

            foreach (var map in popculture.LoadAllAssets<MapAsset>()) {

                db.MapList.AddItem(map);
                List<MapAsset> maps = (List<MapAsset>)typeof(LandfallUnitDatabase).GetField("Maps", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                maps.Add(map);
                typeof(LandfallUnitDatabase).GetField("Maps", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, maps);
            }

            foreach (var mat in popculture.LoadAllAssets<Material>()) {

                if (mat.shader.name == "Standard") { mat.shader = Shader.Find("Standard"); }
            }

            foreach (var objecting in popculture.LoadAllAssets<GameObject>()) {
                if (objecting != null)  {
                    if (objecting.GetComponent<Unit>()) {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("UnitBases", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("UnitBases", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                    else if (objecting.GetComponent<WeaponItem>()) {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("Weapons", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("Weapons", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                    else if (objecting.GetComponent<ProjectileEntity>()) {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("Projectiles", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("Projectiles", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                    else if (objecting.GetComponent<SpecialAbility>()) {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("CombatMoves", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("CombatMoves", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                    else if (objecting.GetComponent<PropItem>()) {
                        List<GameObject> stuff = (List<GameObject>)typeof(LandfallUnitDatabase).GetField("CharacterProps", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                        stuff.Add(objecting);
                        typeof(LandfallUnitDatabase).GetField("CharacterProps", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, stuff);
                    }
                }
            }

            new GameObject() {
                name = "Bulldog",
                hideFlags = HideFlags.HideAndDontSave
            }.AddComponent<PCMapManager>();

            ServiceLocator.GetService<CustomContentLoaderModIO>().QuickRefresh(WorkshopContentType.Unit, null);
        }

        public static AssetBundle popculture;// = AssetBundle.LoadFromMemory(Properties.Resources.popculture);

        public static Material wet;
    }
}
