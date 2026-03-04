using UnityEngine;

[CreateAssetMenu(fileName = "CrabdexEntry", menuName = "Scriptable Objects/CrabdexEntry")]
public class CrabdexEntry : ScriptableObject
{
	public string crabName; // backend name, ie soldier
	public string formalName; // Soldier Crab
	public string scientificName; // Mictyris Longicarpus
	public string description;
	public bool isCrustacean;
	public Sprite typicalSprite;
	public bool generalVariantDiscovered;
	
	[System.Serializable]
	public struct Variant
	{
		public string variantName;  // teen_blue
		public string formalVariantName; // Teen, blue
		public Sprite sprite;
		public bool hasBeenDiscovered;
	}

	public Variant[] variants;

}
