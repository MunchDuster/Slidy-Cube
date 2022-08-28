using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayTimeBackwards : MonoBehaviour
{
	public float timetoToPlayBack = 2f;
	public UnityEvent OnPlayedBack;



	private delegate void OnEvent();
	private OnEvent OnUpdate;

	private TimeRecorder.TimeRecord[] timeRecords;
	private int index = 0;


	// Start is called before the first frame update
	private void Start()
	{
		OnUpdate += RecordTime;
	}

	// FixedUpdate is called once per physics loop
	private void FixedUpdate()
	{
		if (OnUpdate != null) OnUpdate();
	}

	//Records current time
	private void RecordTime()
	{
		TimeRecorder.RecordAllTimes();
	}

	//Plays a time record
	private float timePlayedback = 0;
	private void PlayRecord()
	{
		//Exit check
		if (index >= timeRecords.Length || timePlayedback >= timetoToPlayBack)
		{
			StopPlayingRecords();
			return;
		}
		//Get timeRecord to play records from
		TimeRecorder.TimeRecord timeRecord = timeRecords[index++];

		//Update time played back
		timePlayedback += Time.fixedDeltaTime;

		//Play each record in timeRecord
		foreach (Record record in timeRecord.records)
		{
			if (record.GetType() == typeof(WorldRecord))
			{
				PlayWorldRecord(record as WorldRecord);
			}
		}
	}

	//Plays a world record
	private void PlayWorldRecord(WorldRecord record)
	{
		Debug.Log("Record is null: " + record == null);

		if (record.transform == null) return;
		record.transform.position = record.position;
		record.transform.rotation = record.rotation;
	}

	//Stop playing records
	private void StopPlayingRecords()
	{
		Debug.Log("Stopped playing records");

		//Remove listener from onUpdate
		OnUpdate -= PlayRecord;

		//Call UnityEvent
		if (OnPlayedBack != null) OnPlayedBack.Invoke();

		//Re-enable rigdbodies
		EnablePreviouslyDisabledRigidbodies();

		//Reset index
		index = 0;

		//Reset time played back
		timePlayedback = 0;

		//Clear Records
		//TimeRecorder.records.Clear();

		//Add normal listener back
		OnUpdate += RecordTime;
	}

	//Called by other class
	public void PlayRecordingSoon(float time)
	{
		StartCoroutine(StartPlayRecording(time));
	}
	private System.Collections.IEnumerator StartPlayRecording(float time)
	{
		yield return new WaitForSeconds(time);
		PlayRecording();
	}


	private void PlayRecording()
	{
		Debug.Log("Started PlayRecording");
		//Stop recording
		OnUpdate -= RecordTime;

		//Disable all rigidbodies
		DisableAllRigidbodies();

		// Init records array
		timeRecords = TimeRecorder.records.ToArray();

		//Start playing
		OnUpdate += PlayRecord;
	}

	private List<Rigidbody> disabledRigidbodies = new List<Rigidbody>();
	private void DisableAllRigidbodies()
	{
		foreach (Rigidbody rigidbody in Object.FindObjectsOfType<Rigidbody>(true))
		{
			if (rigidbody != null && rigidbody.isKinematic == false)
			{
				disabledRigidbodies.Add(rigidbody);
				rigidbody.isKinematic = true;
			}
		}
	}
	private void EnablePreviouslyDisabledRigidbodies()
	{
		foreach (Rigidbody rigidbody in disabledRigidbodies)
		{
			if (rigidbody != null)
			{
				rigidbody.isKinematic = false;
			}
		}

		disabledRigidbodies.Clear();
	}
}
