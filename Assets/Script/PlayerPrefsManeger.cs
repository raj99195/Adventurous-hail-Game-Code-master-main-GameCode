using UnityEngine;

/// <summary>
/// Save and load playerPrefs
/// </summary>
public class PlayerPrefsManeger
{
    /// <summary>
    /// Saves a bool with key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SaveBoolean(string key, bool value)
    {
        int intValue = 0;
        if (value)
            intValue = 1;

        PlayerPrefs.SetInt(key, intValue);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Returns the saved bool value with one key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool LoadBoolean(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return false;
        }

        int intValue = PlayerPrefs.GetInt(key);
        bool returnBool = (intValue == 1);
        return returnBool;
    }

    /// <summary>
    /// Saves a int with key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    /// <summary>
    /// Returns the saved int value with one key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int LoadInt(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return 0;
        }
        return PlayerPrefs.GetInt(key);
    }
}
