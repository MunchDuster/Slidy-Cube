using UnityEngine;
using UnityEngine.Events;

public class EnableTrip : TripWire
{
	public UnityEvent OnTripped;
	protected override void OnTrip(GameObject playerObj)
	{
		OnTripped.Invoke();
	}
}
