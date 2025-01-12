using Core.Mechanics.CameraMovement.RTSCameraMovement.Data;
using UnityEngine;

namespace Core.Mechanics.CameraMovement.RTSCameraMovement.Handler
{
    public abstract class BaseCameraMovementHandler : ICameraMovementHandler
    {
        protected readonly CameraInputData _cameraInputData;

        public void Move(Vector3 delta)
        {
            var convertedDelta = new Vector3(-delta.x, 0, -delta.y);
            var targetPosition = GetTargetPosition(convertedDelta);
            
            _cameraInputData.Pivot.position = Vector3.Lerp(_cameraInputData.Pivot.position,targetPosition,Time.deltaTime / _cameraInputData.Smoothness) ;
        }

        protected abstract Vector3 GetTargetPosition(Vector3 delta);

        public BaseCameraMovementHandler(CameraInputData cameraInputData)
        {
            _cameraInputData = cameraInputData;
        }
    }
}