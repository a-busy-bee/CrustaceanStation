using UnityEngine;

[CreateAssetMenu(fileName = "CrabInfo", menuName = "Scriptable Objects/CrabInfo")]
public class CrabInfo : ScriptableObject
{
	public Sprite sprite;
	public string crabName; // individual name ie Crusty Shawn

	public enum CrabType // for naming purposes
	{
		catfish,
		horseshoe,
		scopeCreep,
		coquette,
		crab,
		shrimp,
		lobster,
		hermit,
		seamonkeys,
		isopod,
		tall,
		ghost,
		isopodTiny,
		ittybitty,
		amphipod,
		axolotl
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
}