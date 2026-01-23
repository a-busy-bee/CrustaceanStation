using UnityEngine;

[CreateAssetMenu(fileName = "CrabInfo", menuName = "Scriptable Objects/CrabInfo")]
public class CrabInfo : ScriptableObject
{
	public Sprite sprite;
	public float kioskHeight = 89;
	public string crabName; // individual name ie Crusty Shawn

	public enum CrabType // for naming purposes
	{
		Multiple,
		IttyBitty,
		SeaSheep,
		ScopeCreep,
		BlueLobster,
		Coquette,
		Axolotl,
		Conch,
		Horseshoe,
		Nautilus,
		Catfish,
		Fish,
		Pillbug,
		Isopod,
		Amphipod,
		Lobster,
		Hermit,
		Ghost,
		Shrimp,
		Crab,
		Tall,
		Bird,
		SeaLion,
		SeaLemon
	}

	public CrabType type;

	// WEATHER
	public enum WeatherType
	{
		Sunny,
		LightRain,
		DarkRain,
		Fog
	}

	public WeatherType[] favoriteWeatherTypes;

	[Header("Crabdex")]
	public string crabdexName; // general name used in crabdex, ie soldier
	public bool isVariant;
	public string variantName; // variant name used in crabdex, ie teen_blue

	//TODO LATER
	// sfx

	[Header("Cart Popup")]
	public Mini mini;
	public bool isMultiple;
}