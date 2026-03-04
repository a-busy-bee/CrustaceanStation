using UnityEngine;

[CreateAssetMenu(fileName = "TrainPosition", menuName = "Scriptable Objects/TrainPosition")]
public class TrainPosition : ScriptableObject
{
	public Rail.RailDirection direction;
	public float rotation; // if north, it's 180 on z-axis
	public float startingPosArrive; // where the train is before it moves into the station
	public float startingPosDepart; // also endPosArrive
	public float endPosDepart; // where the train goes to be completely offscreen
	

}
