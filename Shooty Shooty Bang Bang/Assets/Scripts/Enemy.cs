using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Transform healthbar;
	[SerializeField]
	GameObject healthIcon;
	[SerializeField, Min(1)]
	int health = 1;

	Waypoint host;
	#endregion

	#region Properties
	#endregion

	#region Methods
	private void Awake()
	{
		host = transform.GetComponentInParent<Waypoint>();
		if (host == null)
		{
			Debug.LogError("Enemy has no waypoint assigned (Its not a child of any waypoint). This is not allowed");
			throw new ArgumentNullException();
		}

		for (int _i = 0; _i < health; _i++)
		{
			Instantiate(healthIcon, healthbar);
		}
	}
	public void Ouch()
	{
		health--;
		Destroy(healthbar.GetChild(0).gameObject);
		if (health <= 0)
		{
			host.RemoveEnemy(this);
			Destroy(gameObject);
		}
	}
	#endregion
}
