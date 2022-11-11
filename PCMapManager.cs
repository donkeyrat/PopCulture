using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using Landfall.TABS;

namespace PopCulture
{
    public class PCMapManager : MonoBehaviour
    {
        public PCMapManager()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        public void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name.Contains("PC_"))
            {
                GameObject astar = null;
                GameObject map = null;
                foreach (var obj in scene.GetRootGameObjects())
                {
                    if (obj.name == "AStar_Lvl1_Grid")
                    {
                        astar = obj;
                    }
                    if (obj.GetComponent<MapSettings>())
                    {
                        map = obj;
                    }
                }
                if (astar != null && map != null)
                {
                    var path = astar.GetComponentInChildren<AstarPath>(true);
                    astar.SetActive(true);
                    if (path.data.graphs.Length > 0) { path.data.RemoveGraph(path.data.graphs[0]); }
                    path.data.AddGraph(typeof(RecastGraph));
                    path.data.recastGraph.minRegionSize = 0.1f;
                    path.data.recastGraph.characterRadius = 0.3f;
                    path.data.recastGraph.cellSize = 0.2f;
                    path.data.recastGraph.forcedBoundsSize = new Vector3(map.GetComponent<MapSettings>().m_mapRadius * 2f, map.GetComponent<MapSettings>().m_mapRadius * map.GetComponent<MapSettings>().mapRadiusYMultiplier * 2f, map.GetComponent<MapSettings>().m_mapRadius * 2f);
                    path.data.recastGraph.rasterizeMeshes = false;
                    path.data.recastGraph.rasterizeColliders = true;
                    path.Scan();
                    /*
                    path.data.GetNodes(delegate (GraphNode node)
                    {
                        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        gameObject.transform.position = (Vector3)node.position;
                        gameObject.GetComponent<Renderer>().material.color = Color.green;
                        gameObject.GetComponent<Collider>().enabled = false;
                        gameObject.transform.localScale *= 0.5f;
                    });
                    */
                }
            }
        }
    }
}
