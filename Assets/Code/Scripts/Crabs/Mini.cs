using UnityEngine;

[CreateAssetMenu(fileName = "Mini", menuName = "Scriptable Objects/Mini")]
public class Mini : ScriptableObject
{
	public enum Type
	{
		empty,
		predator,
		prey,
		toxic
	}
	public enum Strength
	{
		weak,
		average,
		strong
	}

	[Header("Cart Popup")]
	public CartPopup.MiniType miniType;
	public Sprite miniSprite;

	public bool isMultiple;
	public Sprite multSprite;
	public bool isWipeOut; // for seagulls

	[Header("Pred-Prey Relationships")]
	public Type type;
	public Strength strength;
	
}
