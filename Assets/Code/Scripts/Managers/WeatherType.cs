using UnityEngine;

[CreateAssetMenu(fileName = "WeatherType", menuName = "Scriptable Objects/WeatherType")]
public class WeatherType : ScriptableObject
{
	public string type;
	public CrabInfo.WeatherType weatherType;

	[Header("Foreground")]
	public Color cloudsTop;
	public Color backgroundTop;


	[Header("Background")]
	public Color cloudsBottom;
	public Color backgroundBottom;

	[Header("Fog/Rain")]
	public bool isFoggy;	
	public bool isRainy;
	public enum RainType
	{
		none,
		light,
		dark
	}
	public RainType rainType;
}
