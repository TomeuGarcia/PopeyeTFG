using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    [System.Serializable]
    public class CreditsBlock
    {
        [SerializeField] private string _title = "Team";
        [SerializeField] private float _heightGap = 5f;
        [SerializeField] private CreditsBlockElement[] _elements;
        
        public string Title => _title;
        public float HeightGap => _heightGap;
        public CreditsBlockElement[] Elements => _elements;
    }
    
    
    [System.Serializable]
    public class CreditsBlockElement
    {
        [SerializeField] private string _title = "Programming";
        [SerializeField] private float _heightGap = 5f;
        [SerializeField] private string[] _items;
        
        public string Title => _title;
        public float HeightGap => _heightGap;
        public string[] Items => _items;
    }




    [System.Serializable]
    public class TextSettings
    {
        [SerializeField] private Color _color;
        [SerializeField] private TMPro.TMP_FontAsset _font;
        [SerializeField, Min(1)] private int _fontSize;
        [SerializeField, Min(1)] private float _heightGap;
        
        public Color Color => _color;
        public TMPro.TMP_FontAsset Font => _font;
        public int FontSize => _fontSize;
        public float HeightGap => _heightGap;
    }
    
}