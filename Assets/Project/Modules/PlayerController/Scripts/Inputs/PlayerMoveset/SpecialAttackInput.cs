namespace Popeye.Modules.PlayerController.Inputs
{
    public class SpecialAttackInput
    {
        private readonly InputAction _pressedAction;
        private readonly InputAction _heldPressedAction;
        private readonly InputAction _releasedAction;

        public delegate bool InputAction();
        
        public SpecialAttackInput(
            InputAction pressedAction,
            InputAction heldPressedAction,
            InputAction releasedAction)
        {
            _pressedAction = pressedAction;
            _heldPressedAction = heldPressedAction;
            _releasedAction = releasedAction;
        }
        
        public bool IsPressed()
        {
            return _pressedAction();
        }
        public bool IsBeingHeldPressed()
        {
            return _heldPressedAction();
        }
        public bool WasReleased()
        {
            return _releasedAction();
        }
    }
}