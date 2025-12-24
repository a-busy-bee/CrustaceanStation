using UnityEngine;

[CreateAssetMenu(fileName = "KioskStyle", menuName = "Scriptable Objects/KioskStyle")]
public class KioskStyle : ScriptableObject
{
	public Sprite sprite;
	public string styleName;
	public int cost;
	public bool bought;

	[Header("In-Game Positions")]
	public Vector2 position;
	public Vector2 positionChildren;
	public float crabposition;

	[Header("In-Shop Positions")]
	public Vector2 shopPosition;
	public Vector2 shopPositionChildren;


	// 37.6
	// 22.96
}
