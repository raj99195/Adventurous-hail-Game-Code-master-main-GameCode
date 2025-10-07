using System;
using System.Collections;
using UnityEngine;
using System.Runtime.Serialization.Formatters;

public enum AudioType { Music, Sound }
public class AudioAndVibrationManeger : MonoBehaviour
{

    [Header("Sounds of the game")]
    [SerializeField] Sound[] _sounds;

    [Header("Options to start playback and stop music smoothly")]
    [SerializeField] float _timeOfLerpPlayAndStopMusic;
    [SerializeField] float _deltaOfLerpToPlayAndStop;
    Coroutine coroutineOfPlaying, coroutineOfStoping;

    bool _isSoundMute = false;
    bool _isMusicMute = false;
    bool _isVibrationMute = false;

    const string VIBRATION_MUTE = "Vibration mute";
    const string SOUND_MUTE = "Sound mute";
    const string Music_MUTE = "Music mute";

    PlayerPrefsManeger playerPrefs;

    public static AudioAndVibrationManeger Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        playerPrefs = new PlayerPrefsManeger();
        LoadPrefs();
        initSounds();
    }

    void LoadPrefs()
    {

        // Sound mute
        if (playerPrefs.LoadBoolean(SOUND_MUTE))
        {

            _isSoundMute = true;
            UIManeger.Instance.ChangeSoundBtnStatus(true);
        }
        else
        {
            _isSoundMute = false;
            UIManeger.Instance.ChangeSoundBtnStatus(false);
        }

        // Music mute
        if (playerPrefs.LoadBoolean(Music_MUTE))
        {

            _isMusicMute = true;
            UIManeger.Instance.ChangeMusicBtnStatus(true);
        }
        else
        {
            _isMusicMute = false;
            UIManeger.Instance.ChangeMusicBtnStatus(false);
        }

        // For vibration stutos (mute or not mute)
        if (playerPrefs.LoadBoolean(VIBRATION_MUTE))
        {

            _isVibrationMute = true;
            UIManeger.Instance.ChangeVibrationBtnStatus(true);
        }
        else
        {
            _isVibrationMute = false;
            UIManeger.Instance.ChangeVibrationBtnStatus(false);
        }
    }

    void initSounds()
    {

        foreach (Sound s in _sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.clip;
            source.volume = s.volume;
            source.pitch = s.pitch;
            source.loop = s.loop;

            s.source = source;

            if (s.playOnStart)
                s.source.Play();
        }
    }

    #region Music and Audio

    public void play(string soundName)
    {

        Sound s = Array.Find(_sounds, so => so.name == soundName);

        if (s == null)
        {
            Debug.LogError("Sound with name " + soundName + " doesn't exist!");
            return;
        }

        if (s.audioType == AudioType.Music && !_isMusicMute)
        {
            coroutineOfPlaying = StartCoroutine(playWithLerp(s));
        }
        else if (s.audioType == AudioType.Sound && !_isSoundMute)
        {
            s.source.Play();
        }

    }
    public void stop(string soundName)
    {
        Sound s = Array.Find(_sounds, so => so.name == soundName);

        if (s == null)
        {
            Debug.LogError("Sound with name " + soundName + " doesn't exist!");
            return;
        }

        if (s.audioType == AudioType.Music)
        {
            coroutineOfStoping = StartCoroutine(stopWithLerp(s));
        }
        else if (s.audioType == AudioType.Sound)
        {
            s.source.Stop();
        }

    }
    public void SetSoundMute(bool isMute)
    {
        _isSoundMute = isMute;
        playerPrefs.SaveBoolean(SOUND_MUTE, isMute);
    }
    public bool GetIsSoundMute()
    {
        return _isSoundMute;
    }
    public void SetMusicMute(bool isMute)
    {
        _isMusicMute = isMute;
        playerPrefs.SaveBoolean(Music_MUTE, isMute);
    }
    public bool GetIsMusicMute()
    {
        return _isMusicMute;
    }
    IEnumerator playWithLerp(Sound s)
    {
        AudioSource aS = s.source;
        if (aS.isPlaying)
        {
            StopCoroutine(coroutineOfStoping);
        }

        var wait = new WaitForSeconds(_timeOfLerpPlayAndStopMusic);
        float targetVolume = s.volume;

        aS.volume = 0;

        aS.Play();
        bool canlerp = true;
        while (canlerp)
        {
            aS.volume = aS.volume + _deltaOfLerpToPlayAndStop;
            if (aS.volume >= targetVolume)
            {
                canlerp = false;
                aS.volume = targetVolume;
            }
            yield return wait;
        }

    }
    IEnumerator stopWithLerp(Sound s)
    {
        AudioSource As = s.source;
        if (As.isPlaying)
        {
            StopCoroutine(coroutineOfPlaying);
        }

        var wait = new WaitForSeconds(_timeOfLerpPlayAndStopMusic / 3);
        float targetVolume = 0;
        float firstVolume = As.volume;

        bool canlerp = true;
        while (canlerp)
        {
            As.volume = As.volume - _deltaOfLerpToPlayAndStop;
            if (As.volume <= targetVolume)
            {

                canlerp = false;
                As.volume = targetVolume;
                As.Stop();
            }
            yield return wait;
        }
    }


    #region Change music speed playing when collsioned hail by suprise speed or slow motion suprise
    public void SetToActiveSlowMotion(String soundName)
    {
        Sound s = Array.Find(_sounds, so => so.name == soundName);

        if (s == null)
        {
            Debug.LogError("Sound with name " + soundName + " doesn't exist!");
            return;
        }

        AudioSource aS = s.source;
        aS.pitch = 0.7f;
    }

    public void SetToActiveSpeedSuprise(String soundName)
    {
        Sound s = Array.Find(_sounds, so => so.name == soundName);

        if (s == null)
        {
            Debug.LogError("Sound with name " + soundName + " doesn't exist!");
            return;
        }

        AudioSource aS = s.source;
        aS.pitch = 1.2f;
    }

    public void ResetPlayingSpeed(String soundName)
    {
        Sound s = Array.Find(_sounds, so => so.name == soundName);

        if (s == null)
        {
            Debug.LogError("Sound with name " + soundName + " doesn't exist!");
            return;
        }

        AudioSource aS = s.source;
        aS.pitch = 1f;
    }
    #endregion

    #endregion

    #region Vibration

    public void VibrateReceiveHail()
    {
        if (!_isVibrationMute)
        {
            Vibration.Vibrate(new long[] { 20, 30 }, -1);
        }
    }
    public void VibrateWhenLoss()
    {
        if (!_isVibrationMute)
        {
            Vibration.Vibrate(new long[] { 200, 38, 20, 38, 20, 38, 20, 38 }, -1);

        }
    }

    public void VibrateWhenGamePlayedForAlert()
    {
        if (!_isVibrationMute)
        {
            Vibration.Vibrate(new long[] { 200, 38, 20, 38 }, -1);

        }
    }

    public void VibrateWhenCollisionHailWithSuprise()
    {
        if (!_isVibrationMute)
        {
            Vibration.Vibrate(new long[] { 0, 23, 10, 23 }, -1);

        }
    }
    public void SetVibrationMute(bool isMute)
    {
        _isVibrationMute = isMute;
        playerPrefs.SaveBoolean(VIBRATION_MUTE, isMute);
    }
    public bool GetIsVibrationMute()
    {
        return _isVibrationMute;
    }

    #endregion
}


[Serializable]
public class Sound
{
    public string name;
    public AudioType audioType;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;

    [Range(0, 1f)]
    public float volume = 1;

    [Range(-3f, 3f)]
    public float pitch = 1;

    public bool loop = false;
    public bool playOnStart = false;
}
