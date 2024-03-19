using UnityEngine;

namespace Popeye.Modules.GameMenus.Generic
{
    public abstract class AMenuController : MonoBehaviour
    {
        [Header("GROUP")]
        [SerializeField] private GameObject _groupHolder;
        
        [Header("CLOSE")]
        [SerializeField] private SmartButtonAndConfig _backButtonAndConfig;


        public void Init(SmartButton.SmartButtonEvent closeMenuCallback)
        {
            _backButtonAndConfig.SmartButton.Init(_backButtonAndConfig.Config, closeMenuCallback);
            
            DoInit();
        }

        protected abstract void DoInit();

        public void Show()
        {
            _groupHolder.SetActive(true);
        }
        
        public void Hide()
        {
            _groupHolder.SetActive(false);
        }

    }
}