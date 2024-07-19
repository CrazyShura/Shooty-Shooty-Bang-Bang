using UnityEngine;

[RequireComponent(typeof(BulletBag))]
public class Gun : MonoBehaviour
{
	#region Fields
	[SerializeField]
	float rateOfFire = .1f;
	[SerializeField]
	Transform barrelEnd;

	BulletBag bulletBag;
	float fireTimer = 0;
	#endregion

	#region Properties
	#endregion

	#region Methods
	private void Awake()
	{
		bulletBag = GetComponent<BulletBag>();
	}
	public void PewPew(Vector3 atWho)
	{
		if(fireTimer <= 0)
		{
			fireTimer = rateOfFire;
			bulletBag.GiveMeBullet().Go(barrelEnd.position, atWho);
		}
	}
	private void Update()
	{
		if (fireTimer > 0)
		{
			fireTimer -= Time.deltaTime;
		}
	}
	#endregion
}
