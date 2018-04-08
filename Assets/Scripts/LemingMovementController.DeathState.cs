public partial class LemingMovementController
{
    private class DeathState : ControllerState
    {
        public DeathState(LemingMovementController controller) : base(controller)
        {
        }

        public override void Start()
        {
            _controller._movementDirection = 0;
            _controller._verticalSpeed = 0;
            _controller.motion = 0;

            var onDead = _controller.OnDead;
            if (onDead != null)
            {
                var killer = _controller.Killer;
                _controller.Killer = Killer.None;
                onDead(_controller, killer);
            }

        }
    }
}