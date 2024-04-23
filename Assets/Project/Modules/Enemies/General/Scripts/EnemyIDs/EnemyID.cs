using Popeye.IDSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    [CreateAssetMenu(fileName = "ID_EnemyName", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "EnemyID")]
    public class EnemyID : IDAsset
    {
        public string GetEnemyName()
        {
            string enemyName = this.name;
            enemyName = enemyName.Replace("_", string.Empty);
            enemyName = enemyName.Replace("ID", string.Empty);
                
            return enemyName;
        }
    }
}