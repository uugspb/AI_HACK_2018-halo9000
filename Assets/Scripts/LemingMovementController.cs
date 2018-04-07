using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum EInput
{
	None = 0, 
	MoveRigth = 1 << 0, 
	MoveLeft = 1 << 1, 
	Jump = 1 << 2
}


public class LemingMovementController : MonoBehaviour
{
	private ControllerState _currentState;
	public LayerMask CollisitonMask;
	
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


	}
	
	private class IdleState : ControllerState
	{
		public IdleState(LemingMovementController controller) : base(controller)
		{
		}

		public override void Move(int direction)
		{
			if (direction != 0)
			{
				_controller.CurrentStateS = new MoveState(_controller);
				
			}
			_controller._movementDirection = direction;
		}
		
		public override void Jump()
		{
			_controller._verticalSpeed = _controller.JumpForce;
			_controller.CurrentStateS = new JumpState(_controller);
		}

		public override void Update()
		{
			if (!_controller.isGrounded)
			{
				_controller.CurrentStateS = new JumpState(_controller);
			}
		}
	}
	
	private class MoveState : ControllerState
	{
		public MoveState(LemingMovementController controller) : base(controller)
		{
		}

		public override void Move(int direction)
		{
			if (direction == 0)
			{
				_controller.CurrentStateS = new IdleState(_controller);
			}
			_controller._movementDirection = direction;				
		}

		public override void Jump()
		{
			_controller._verticalSpeed = _controller.JumpForce;
			_controller.CurrentStateS = new JumpState(_controller);
		}

		public override void Update()
		{
			_controllerRigidbody2D.MovePosition(_controllerRigidbody2D.position +
			                                    new Vector2(_controller._movementDirection * _controller.Speed,
				                                    0)); 
			if (!_controller.isGrounded)
			{
				_controller.CurrentStateS = new JumpState(_controller);
			}
		}
		
		
	}
	
	private class JumpState : ControllerState
	{
		public JumpState(LemingMovementController controller) : base(controller)
		{
		}
		
		public override void Update()
		{			
			_controller._verticalSpeed += Physics2D.gravity.y * Time.fixedDeltaTime;
			_controllerRigidbody2D.MovePosition(
				_controllerRigidbody2D.position + new Vector2(_controller._movementDirection * _controller.AirControllSpeed,
				                                    _controller._verticalSpeed * Time.fixedDeltaTime));

			if (_controller.IsGrounded())
			{
				_controller.CurrentStateS = new IdleState(_controller);
			}
		}

		public override void Move(int direction)
		{
			if(direction != 0)
				_controller._movementDirection = direction;				
		}
	}
	
//	private class FallDown : ControllerState
//	{
//		public FallDown(LemingMovementController controller) : base(controller)
//		{
//		}
//		
//		public override void Update()
//		{			
//			_controller._verticalSpeed += Physics2D.gravity.y * Time.fixedDeltaTime;
//			_controllerRigidbody2D.MovePosition(
//				_controllerRigidbody2D.position + new Vector2(_controller._movementDirection * _controller.Speed,
//					_controller._verticalSpeed * Time.fixedDeltaTime));
//
//			if (_controller.IsGrounded())
//			{
//				_controller.CurrentStateS = new IdleState(_controller);
//			}
//		}
//	}

	public float Speed;
	public float AirControllSpeed;
	
	public float JumpForce;
	public int motion;
	private Rigidbody2D _rigidbody2D;
	public int _movementDirection;
	public float _verticalSpeed;
	public float _groundCollisionVectorLength = 0.1f;

	public string CurrentState;
	private bool jump;
	public bool isGrounded = false;	
	public Collider2D HittedCollider;
	public RaycastHit2D _groundHit;


	private ControllerState CurrentStateS
	{
		get { return _currentState; }
		set
		{
			Debug.LogFormat("{0}->{1}", _currentState, value);
			_currentState = value;
			CurrentState = _currentState.GetType().Name;
		}
	}

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		CurrentStateS = new IdleState(this);
	}

	public void ManualFixedUpdate(int input)
	{
		_groundHit = Physics2D.Raycast(transform.position, Vector2.down, _groundCollisionVectorLength, CollisitonMask.value);
		isGrounded = _groundHit.collider != null;
		HittedCollider = _groundHit.collider;

		motion = (input & (int) EInput.MoveRigth) > 0 ? 1 : (input & (int) EInput.MoveLeft) > 0 ? -1 : 0;
		jump = (input & (int) EInput.Jump) > 0; 
		
		CurrentStateS.Move(motion);
		if(jump)
			CurrentStateS.Jump();
		CurrentStateS.Update();


		motion = 0;
		jump = false;
	}

	bool IsGrounded()
	{
		return isGrounded;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(0,Vector2.down.y * _groundCollisionVectorLength, 0));
	}
}
