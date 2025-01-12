using System;
using UnityEngine;

namespace Core.Mechanics.CameraMovement.RTSCameraMovement.Data
{
    [Serializable]
    public class CameraInputData
    {
        public Transform Pivot;
        public float Smoothness;
        public float MouseSensitivity;
        public float KeyboardSensitivity;
    }
}