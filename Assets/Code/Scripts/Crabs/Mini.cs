using UnityEngine;

[CreateAssetMenu(fileName = "Mini", menuName = "Scriptable Objects/Mini")]
public class Mini : ScriptableObject
{
	[Header("Cart Popup")]
	public CartPopup.MiniType miniType;
	public Sprite miniSprite;
	public bool isWipeOut; // for seagulls

	public CartPopup.MiniType[] preyTo;
	public CartPopup.MiniType[] neutralTo;
	public CartPopup.MiniType[] predatorTo;
	
}
