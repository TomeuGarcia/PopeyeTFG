using TMPro;

namespace Popeye.Scripts.TextUtilities
{
    public static class TextContentUtilities
    {
        public static void SetContent(this TMP_Text textMesh, TextContent textContent)
        {
            textMesh.text = textContent.Content;
        }
    }
}

