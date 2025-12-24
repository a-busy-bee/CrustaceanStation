using UnityEngine;

public class DebuggerShop : MonoBehaviour
{
    void OnEnable()
    {
        //Debug.Log("enabled" + StackTraceUtility.ExtractStackTrace());
    }
    void OnDisable()
    {
       //Debug.Log("disabled" + StackTraceUtility.ExtractStackTrace());
	}
}
