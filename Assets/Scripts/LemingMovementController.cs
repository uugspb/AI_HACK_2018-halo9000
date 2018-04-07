using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;


public enum Killer
{
	None, Suicide, Player
}
public partial class LemingMovementController : MonoBehaviour
{
	
	public event Action<LemingMovementController, Killer> OnDead;

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
	public bool isGrounded = false;	
	public Collider2D HittedCollider;
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
		_rigidbody2D = GetComponent<Rigidbody2D>();
		CurrentState = new SpawnState(this);
		_audioSource = GetComponent<AudioSource>();
	}

	public void ManualFixedUpdate(LemmingMovementDirection input)
	{
		_groundHit = Physics2D.BoxCast(transform.position, _boxCollider2D.size * transform.lossyScale.x, 0, Vector2.down, _groundCollisionVectorLength,
			CollisitonMask.value);
		
		isGrounded = _groundHit.collider != null;
		HittedCollider = _groundHit.collider;

		motion = (input & LemmingMovementDirection.Right) > 0 ? 1 : (input & LemmingMovementDirection.Left) > 0 ? -1 : 0;
		jump = (input & LemmingMovementDirection.Jump) > 0; 
		
		CurrentState.Move(motion);
		if(jump)
			CurrentState.Jump();
		CurrentState.Update();


		motion = 0;
		jump = false;
	}

	bool IsGrounded()
	{
		return isGrounded;
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
			
	}
}
