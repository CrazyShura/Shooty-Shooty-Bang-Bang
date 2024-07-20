using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Gun))]
public class PlayerController : MonoBehaviour
{
	#region Fields
	[SerializeField]
	LayerMask ShootingMask;
	[SerializeField]
	Animator animator;

	NavMeshAgent navAgent;
	PlayerState currentState = PlayerState.Idle;
	Gun funToy;
	Camera cam;
	#endregion

	#region Properties
	#endregion

	#region Methods
	private void Awake()
	{
		navAgent = GetComponent<NavMeshAgent>();
		funToy = GetComponent<Gun>();

		cam = Camera.main;
		
		EventManager.WaypointWasReached.AddListener(OnWaypointReach);
		EventManager.WaypointWasCleared.AddListener(OnWaypointClear);
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			switch (currentState)
			{
				case PlayerState.Idle:
					{
						GoTo(GameMaster.Instance.GiveNextWaypoint());
						currentState = PlayerState.Moving;
						break;
					}
				case PlayerState.Moving:
					{
						// no action needed
						break;
					}
				case PlayerState.InCombat:
					{
						Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
						RaycastHit _hit;
						if (Physics.Raycast(_ray, out _hit, 20f, ShootingMask))
						{
							funToy.PewPew(_hit.point);
						}
						else
						{
							funToy.PewPew(_ray.GetPoint(20f));
						}
						break;
					}
			}
		}
		float _speed = navAgent.velocity.magnitude / navAgent.desiredVelocity.magnitude;
		animator.SetFloat("_speed", _speed);
	}
	/// <summary>
	/// Tells player to move to a waypoint. Player wont accept another waypoint if its in motion or in combat.
	/// </summary>
	/// <param name="to">Waypoint to move to</param>
	/// <returns>If player is not in motion and a valid waypoint was given returnes true, otherwise returns false.</returns>
	public bool GoTo(Waypoint waypoint)
	{
		if (waypoint == null)
		{
			Debug.LogError("Waypoint is null");
			throw new ArgumentNullException();
		}
		NavMeshPath _path = new NavMeshPath();
		if (!navAgent.CalculatePath(waypoint.Postition, _path))
		{
			Debug.LogWarning("Couldnt make a path to a provided waypoint");
			return false;
		}
		if (currentState != PlayerState.Idle)
		{
			Debug.Log("Trying to set new destination to a player while its in motion or in combat, this is not allowed");
			return false;
		}
		navAgent.path = _path;
		return true;
	}
	void OnWaypointReach(Waypoint unused)
	{
		currentState = PlayerState.InCombat;
	}
	void OnWaypointClear(Waypoint unused)
	{
		currentState = PlayerState.Idle;
		Waypoint _nextWaypoint = GameMaster.Instance.GiveNextWaypoint();
		if (_nextWaypoint != null)
		{
			GoTo(_nextWaypoint);
			currentState = PlayerState.Moving;
		}
		else
		{
			Debug.Log("Journey Ended");
			GameMaster.Instance.Restart();
		}
	}
	#endregion
}
