using UnityEngine;

[CreateAssetMenu(fileName = "DecorItems", menuName = "Scriptable Objects/DecorItems")]
public class DecorItems : ScriptableObject
{
    public Sprite sprite;
    public Sprite iconSprite; // shop icon
    public bool isRemoveItem;
    public int cost;
    public bool bought;
    public bool placed;

}
 