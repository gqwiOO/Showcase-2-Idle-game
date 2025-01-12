using Core.Mechanics.CameraMovement.RTSCameraMovement.Data;
using Core.Mechanics.CameraMovement.RTSCameraMovement.Handler;
using UnityEngine;

namespace Core.Mechanics.CameraMovement.RTSCameraMovement
{
    [RequireComponent(typeof(Camera))]
    public abstract class BaseCameraMovementInput: MonoBehaviour
    {
        [SerializeField] protected CameraInputData _cameraInputData;

        protected ICameraMovementHandler _mCameraMovementHandler;

        protected virtual void Awake()
        {
            _mCameraMovementHandler = CreateInputHandlerInstance();
        }

        protected abstract ICameraMovementHandler CreateInputHandlerInstance();
    }
}