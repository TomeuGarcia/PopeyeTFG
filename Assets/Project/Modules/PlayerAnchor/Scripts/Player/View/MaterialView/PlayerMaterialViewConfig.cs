using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [System.Serializable]
    public class PlayerMaterialViewConfig
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _damagedColor;
        [SerializeField] private Color _healColor;
        public Color NormalColor => _normalColor;
        public Color DamagedColor => _damagedColor;
        public Color HealColor => _healColor;
        

        [SerializeField] private string _baseColorProperty;
        [SerializeField] private string _isTiredProperty;
        public int BaseColorPropertyID { get; private set; }
        public int IsTiredPropertyID { get; private set; }
        

        [SerializeField] private PlayerMaterialView.FlickData _takeDamageFlick;
        [SerializeField] private PlayerMaterialView.FlickData _deathFlick;
        [SerializeField] private PlayerMaterialView.FlickData _healFlick;
        public PlayerMaterialView.FlickData TakeDamageFlick => _takeDamageFlick;
        public PlayerMaterialView.FlickData DeathFlick => _deathFlick;
        public PlayerMaterialView.FlickData HealFlick => _healFlick;


        public void OnValidate()
        {
            BaseColorPropertyID = Shader.PropertyToID(_baseColorProperty);
            IsTiredPropertyID = Shader.PropertyToID(_isTiredProperty);
        }
    }
}