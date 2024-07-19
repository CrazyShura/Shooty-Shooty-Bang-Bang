using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton. Manager class that handles gameplay as a whole.
/// </summary>
public class GameMaster : MonoBehaviour
{
	#region Fields
	static GameMaster instance;
	[SerializeField]
	List<Waypoint> waypoints;
	[SerializeField]
	PlayerController player;

	int curretnWaypoint = -1;
	#endregion

	#region Properties
	public static GameMaster Instance { get => instance; }
	#endregion

	#region Methods
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Trying to create more then one GameMaster");
			Destroy(gameObject);
			return;
		}
		instance = this;
	}

	void SendPlayerToNextWaypoint()
	{
		curretnWaypoint++;
		if (curretnWaypoint >= waypoints.Count)
		{
			Debug.Log("Player is at the final waypoint");
			return;
		}
		if(player.GoTo(waypoints[curretnWaypoint]))
		{
			
		}
		else
		{
			curretnWaypoint--;
		}
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0)) 
		{
			SendPlayerToNextWaypoint();
		}
	}
	#endregion
}
