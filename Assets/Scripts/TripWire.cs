using UnityEngine;
using UnityEngine.Events;

public class TripWire : MonoBehaviour
{
	public float destroyTime;

	public UnityEvent OnTrippedEvent;

	public delegate void OnEvent(GameObject caller);
	public event OnEvent OnTripped;


	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			OnTrip(collider.gameObject);
		}
	}

	protected void OnTrip(GameObject playerObj)
	{
		OnTripped(gameObject);
		OnTrippedEvent.Invoke();

		Destroy(gameObject, destroyTime);

		//Remove all connections to delegates
		OnTripped = null;
		OnTrippedEvent = null;
	}
}
