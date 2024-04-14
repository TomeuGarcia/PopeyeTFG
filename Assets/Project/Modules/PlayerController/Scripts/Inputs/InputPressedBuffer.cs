using Popeye.Timers;
using UnityEngine.InputSystem;

namespace Popeye.Modules.PlayerController.Inputs
{
    public class InputPressedBuffer : IInputBuffer
    {
        private readonly InputAction _inputAction;
        private readonly Timer _bufferTimer;
        
        public InputPressedBuffer(InputAction inputAction, float duration)
        {
            _inputAction = inputAction;
            _bufferTimer = new Timer(duration, duration);            
        }

        public void Update(float deltaTime)
        {
            if (_inputAction.WasPressedThisFrame())
            {
                _bufferTimer.Clear();
            }
            
            _bufferTimer.Update(deltaTime);
        }

        public bool WasPressed()
        {
            return !_bufferTimer.HasFinished();
        }
        
    }
}