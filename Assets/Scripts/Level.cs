using UnityEngine;

public class Level
{
	public float destroyTime;
	public int levelNo;
	public LevelPiece[] pieces;
	public LevelEnd end;
	public LevelGenerator levelGenerator;

	//Used for piece percentage calculation
	private float totalDistance;

	public Level(LevelGenerator levelGenerator, LevelPiece[] pieces, LevelEnd end, float destroyTime)
	{
		this.levelGenerator = levelGenerator;
		this.end = end;
		this.pieces = pieces;
		this.destroyTime = destroyTime;

		foreach (LevelPiece piece in pieces)
		{
			piece.level = this;
			totalDistance += piece.length;
		}

		float distanceSoFar = 0;
		foreach (LevelPiece piece in pieces)
		{
			piece.percentage = distanceSoFar / totalDistance;

			piece.weight = piece.length / totalDistance;

			distanceSoFar += piece.length;
		}

		levelGenerator.levels.Add(this);
		end.tripwire.OnTripped += DestroySelf;
	}

	private void DestroySelf(GameObject caller)
	{
		foreach (LevelPiece piece in this.pieces)
		{
			MonoBehaviour.Destroy(piece, destroyTime);
		}

		levelGenerator.levels.Remove(this);
	}
}