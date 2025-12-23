using UnityEngine;

[CreateAssetMenu(fileName = "KioskStyle", menuName = "Scriptable Objects/KioskStyle")]
public class KioskStyle : ScriptableObject
{
	public Sprite sprite;
	public int cost;
	public bool bought;

	public Vector2 position;
	public Vector2 positionChildren;
	public float crabposition;
}
