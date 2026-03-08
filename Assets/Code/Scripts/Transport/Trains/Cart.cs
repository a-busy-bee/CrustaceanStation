using UnityEngine;

[CreateAssetMenu(fileName = "Cart", menuName = "Scriptable Objects/Cart")]
public class Cart : ScriptableObject
{
	public enum Type
	{
		Economy,
		Standard,
		Deluxe,
		Null,
		Shuttle,
		Van
	}

	public Type cartType;

	public int ticketCost;

	public float cartHeight;
	public float cartStartingPoint;
}
