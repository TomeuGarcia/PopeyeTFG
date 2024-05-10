using System;
using UnityEngine;

namespace Project.General.Scripts.CursorUtilities
{
    public class FocusCursorOnStart : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}