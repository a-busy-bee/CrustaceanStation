using UnityEngine;

[CreateAssetMenu(fileName = "DecorItems", menuName = "Scriptable Objects/DecorItems")]
public class DecorItems : ScriptableObject
{
    public Sprite sprite;
    public bool isTopItem;
    public int cost;
    public bool bought;
    public bool placed;

}
 