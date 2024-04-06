using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Project.Modules.Enemies.General
{
    [ExecuteInEditMode]
    public class WorldEnemyPlacerUpdater : MonoBehaviour
    {
        [SerializeField] private WorldEnemyPlacer _worldEnemyPlacer;


        private void Awake()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            
            Destroy(this);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!EditorApplication.isPlaying)
            {
                _worldEnemyPlacer.UpdateView();
            }
        }
#endif
        
        
    }
}