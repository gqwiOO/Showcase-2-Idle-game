using Core.Mechanics.CameraMovement.RTSCameraMovement.Handler;
using Core.Scripts.Services.UpdateService;
using UnityEngine;
using Zenject;

namespace Core.Mechanics.CameraMovement.RTSCameraMovement
{
    public class MouseKeyboardCameraMovementInput : BaseCameraMovementInput, IUpdatable
    {
        private IUpdateService _updateService;
        
        private bool _isDragging = false;

        public UpdateType UpdateType => UpdateType.Update;

        [Inject]
        private void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
        }
        
        protected override void Awake()
        {
            base.Awake();
            _updateService.Add(this);
        }
        
        public void Tick(float tickTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                return;
            }

            if (_isDragging)
                _mCameraMovementHandler.Move(Input.mousePositionDelta * _cameraInputData.MouseSensitivity);
        }
        protected override ICameraMovementHandler CreateInputHandlerInstance()
        {
            return new CameraMovementHandler(_cameraInputData);
        }

    }
}