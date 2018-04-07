using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using Debug = System.Diagnostics.Debug;


public enum Killer
{
	None, Suicide, Player
}
public partial class LemingMovementController : MonoBehaviour
{
    public Animator LemingAnimator;
	
	public event Action<LemingMovementController, Killer> OnDead;
	public event Action<LemingMovementController> OnExit;

	private void CallOnDead()
	{
		if (OnDead != null)
		{
			var killer = Killer;
			Killer = Killer.None;
			OnDead(this, killer);
		}
	}
	
	
	private ControllerState _currentState;
	public LayerMask CollisitonMask;


	public float Speed;
	public float AirControllSpeed;
	
	public float JumpForce;
	public int motion;
	private Rigidbody2D _rigidbody2D;
	[SerializeField] private BoxCollider2D _boxCollider2D;
	public int _movementDirection;
	public float _verticalSpeed;
	public float _groundCollisionVectorLength = 0.1f;

	public string CurrentStateName;
	private bool jump;
	public CollisionSide _collisionSide;	
	[CanBeNull] public Collider2D HittedCollider;
	public RaycastHit2D _groundHit;
	private AudioSource _audioSource;
	private Killer Killer;


	private ControllerState CurrentState
	{
		get { return _currentState; }
		set
		{
			
			if (_currentState != null)
			{
				_currentState.End();	
			}
			
//			Debug.LogFormat("{0}->{1}", _currentState, value);
			_currentState = value;

			if(_currentState != null)
				_currentState.Start();
			
			
			if (_currentState is DeathState)
			{
				CallOnDead();
			}

			CurrentStateName = _currentState.GetType().Name;
		}
	}

	private void Awake()
	{
        LemingAnimator = GetComponent<Animator>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		CurrentState = new SpawnState(this);
		_audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Die(Killer.Player);			
		}

	}

	public void ManualFixedUpdate(LemmingMovementDirection input)
	{
		if (this == null)
		{
			return;
		}
		
		_groundHit = Physics2D.BoxCast(transform.position, _boxCollider2D.size * transform.lossyScale.x, 0, Vector2.down, _groundCollisionVectorLength,
			CollisitonMask.value);

		HittedCollider = _groundHit.collider;
		if (HittedCollider != null)
		{
			Debug.Assert(HittedCollider.transform != null, "HittedCollider.transform != null");
			var colliderPosition = _groundHit.point;
			_collisionSide = colliderPosition.y <= transform.position.y ? CollisionSide.Ground : CollisionSide.Ceiling;
		}
		else
		{
			_collisionSide = CollisionSide.None;
		}

		motion = (input & LemmingMovementDirection.Right) > 0 ? 1 : (input & LemmingMovementDirection.Left) > 0 ? -1 : 0;
		jump = (input & LemmingMovementDirection.Jump) > 0; 
		
		CurrentState.Move(motion);
		if(jump)
			CurrentState.Jump();
		CurrentState.Update();


		motion = 0;
		jump = false;
	}

	public enum CollisionSide
	{
		None,
		Ground,
		Ceiling,
		Left,
		Right
	}

	bool IsGrounded()
	{
		return _collisionSide == CollisionSide.Ground;
	}

	bool IsCeiled()
	{
		return _collisionSide == CollisionSide.Ceiling;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position + new Vector3(_boxCollider2D.offset.x, _boxCollider2D.offset.y, 0)
			, _boxCollider2D.size * transform.lossyScale.x);
	}

	public void Die(Killer killer)
	{
		_currentState.Die(killer);
		if (!_audioSource.isPlaying)
			_audioSource.Play();
	}

	public void Respawn(Vector3 position)
	{
		transform.position = position;
		_currentState = new SpawnState(this);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Spike")
		{
			Die(Killer.Suicide);
		}
		else if (other.gameObject.tag == "Exit")
		{
			OnOnExit();
			Destroy(gameObject);
		}
	}

	protected virtual void OnOnExit()
	{
		Debug.Log("--------");
		var handler = OnExit;
		if (handler != null) 
			handler(this);
	}
}
