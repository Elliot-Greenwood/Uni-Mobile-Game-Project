using UnityEngine;

public static class PhoneVibration 
{
    //CREDIT GOES TO :Comp-3 Interactive on YouTube
    //https://www.youtube.com/watch?v=o6xVLzs1kVk
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif
    
    //====================================================================
    //Create Static Voids here for custom types
    public static void ExplosionVibration(long Milliseconds = 500) 
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate", Milliseconds);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    //=====================================================================
    public static void Cancel()
    {
        if (IsAndroid())
        {
            vibrator.Call("cancel");
        }
    }
    public static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
