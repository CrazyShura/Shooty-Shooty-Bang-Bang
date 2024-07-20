using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton. Manager class that handles gameplay as a whole.
/// </summary>
public class GameMaster : MonoBehaviour
{
	#region Fields
	static GameMaster instance;
	[SerializeField]
	List<Waypoint> waypoints;

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

	public Waypoint GiveNextWaypoint()
	{
		curretnWaypoint++;
		if (curretnWaypoint >= waypoints.Count)
		{
			Debug.Log("Player is at the final waypoint");
			return null;
		}
		return waypoints[curretnWaypoint];
	}

	public void Restart()
	{
		SceneManager.LoadScene(0);
	}
	#endregion
}
