using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Gun))]
public class PlayerController : MonoBehaviour
{
	#region Fields
	NavMeshAgent navAgent;
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
	}
	private void Update()
	{
		if (!navAgent.hasPath) //TODO suld make rudementory FSM for this instead
		{
			if (Input.GetMouseButtonDown(1))
			{
				Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit _hit;
				if (Physics.Raycast(_ray, out _hit, 20f))
				{
					funToy.PewPew(_hit.point);
				}
				else
				{
					funToy.PewPew(_ray.GetPoint(20f));
				}
			}
		}
	}
	/// <summary>
	/// Tells player to move to a waypoint. Player wont accept another waypoint if its in motion.
	/// </summary>
	/// <param name="to">Waypoint to move to</param>
	/// <returns>If player is not in motion and a valid waypoint was given returnes true, otherwise returns false.</returns>
	public bool GoTo(Waypoint waypoint)
	{
		NavMeshPath _path = new NavMeshPath();
		if (!navAgent.CalculatePath(waypoint.Postition, _path))
		{
			Debug.LogWarning("Couldnt make a path to a provided waypoint");
			return false;
		}
		if (navAgent.hasPath)
		{
			Debug.Log("Trying to set new destination to a player while its in motion, this is not allowed");
			return false;
		}
		navAgent.path = _path;
		return true;
	}
	#endregion
}
