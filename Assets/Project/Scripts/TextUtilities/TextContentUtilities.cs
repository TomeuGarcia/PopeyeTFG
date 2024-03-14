using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Popeye.Scripts.TextUtilities
{
    public static class TextContentUtilities
    {
        public static void SetContent(this TextMeshPro textMesh, TextContent textContent)
        {
            textMesh.text = textContent.Content;
        }
    }
}

