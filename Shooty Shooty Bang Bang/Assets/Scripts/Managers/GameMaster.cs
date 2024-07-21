using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
	Slider progeressBar;

	int curretnWaypoint = -1;
	bool paused = false;
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

		EventManager.WaypointWasCleared.AddListener(UpdateProgress);
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

	//NOTE usually I use another class to handle anything UI related but I dont see the neccecety here
	public void Restart()
	{
		SceneManager.LoadScene(0);
	}
	public void Pause()
	{
		if (paused)
		{
			Time.timeScale = 1f;
			paused = false;
		}
		else
		{
			Time.timeScale = 0f;
			paused = true;
		}
	}
	void UpdateProgress(Waypoint unused)
	{
		progeressBar.value = ((float)curretnWaypoint + 1f) / (float)waypoints.Count;
	}
	#endregion
}
