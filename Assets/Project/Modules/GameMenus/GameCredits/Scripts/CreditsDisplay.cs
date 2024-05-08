using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.GameMenus.GameCredits
{
    [System.Serializable]
    public class CreditsBlock
    {
        [SerializeField] private string _title = "Team";
        [SerializeField] private float _extraHeightGap = 0f;
        [SerializeField] private CreditsBlockElement[] _elements;
        
        public string Title => _title;
        public float ExtraHeightGap => _extraHeightGap;
        public CreditsBlockElement[] Elements => _elements;
    }
    
    
    [System.Serializable]
    public class CreditsBlockElement
    {
        [SerializeField] private string _title = "Programming";
        [SerializeField] private float _extraHeightGap = 0f;
        [SerializeField] private string[] _items;
        
        public string Title => _title;
        public float ExtraHeightGap => _extraHeightGap;
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
    
    
    [System.Serializable]
    public class ImageSettings
    {
        [SerializeField, Min(0)] private float _scale;
        [SerializeField, Min(0)] private float _heightGap;

        public float Scale => _scale;
        public float HeightGap => _heightGap;
    }
    
}