using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRecorder : MonoBehaviour
{
	public static List<TimeRecord> records = new List<TimeRecord>();
	public static List<TimeRecorder> recorders = new List<TimeRecorder>();

	public class TimeRecord
	{
		public float time;
		public Record[] records;

		public TimeRecord(float time, Record[] records)
		{
			this.time = time;
			this.records = records;
		}
	}



	public static void RecordAllTimes()
	{
		float time = GetTime();

		Record[] newRecords = new Record[recorders.Count];
		for (int i = 0; i < recorders.Count; i++)
		{
			TimeRecorder timeRecorder = recorders[i];

			Record newRecord;
			if (timeRecorder == null) newRecord = new DestroyRecord();
			else newRecord = timeRecorder.RecordTime();

			newRecords[i] = newRecord;
		}

		TimeRecord newTimeRecord = new TimeRecord(time, newRecords);
		records.Insert(0, newTimeRecord);

		while (records[records.Count - 1].time - GetTime() > 4f) records.RemoveAt(records.Count - 1);
	}
	private static float GetTime()
	{
		return Time.time;
	}

	private static void RecordDestory() { }

	// Start is called before the first frame update
	void Start()
	{
		if (recorders == null)
		{
			recorders = new List<TimeRecorder>();
		}

		recorders.Add(this);
	}



	public Record RecordTime()
	{
		return new WorldRecord(transform);
	}
}
