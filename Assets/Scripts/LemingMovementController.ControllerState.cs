using UnityEngine;

public partial class LemingMovementController
{
    private class ControllerState
    {
        protected readonly LemingMovementController _controller;
        protected readonly Rigidbody2D _controllerRigidbody2D;
		

        public ControllerState(LemingMovementController controller)
        {
            _controller = controller;
            _controllerRigidbody2D = _controller._rigidbody2D;
        }

        public virtual void Update()
        {
        }

        public virtual void Move(int direction)
        {
			
        }

        public virtual void Jump()
        {
        }

        public virtual void Die()
        {
            _controller.CurrentState = new DeathState(_controller);
        }
		
		
        public virtual void Start()
        {}
		
        public virtual void End()
        {}

    }
}