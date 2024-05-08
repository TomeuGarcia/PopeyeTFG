using UnityEngine;
using UnityEngine.UI;

namespace Project.Modules.GameMenus.GameCredits
{
    public class CreditsImage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        public RectTransform Transform => _image.rectTransform;


        public void Init(Sprite content, ImageSettings settings)
        {
            _image.sprite = content;

            _image.SetNativeSize();
            _image.rectTransform.localScale = Vector3.one * settings.Scale;
        }

    }
}