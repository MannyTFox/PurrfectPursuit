using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs : MonoBehaviour
{
    public static readonly string DemoHighScore = "DemoHighScore";
    public static readonly string DemoProgress = "DemoProgress";


    // Volume prefs
    public static readonly string MasterVolume = "masterVolume";
    public static readonly string MusicVolume = "musicVolume";
    public static readonly string SFXVolume = "sfxVolume";
    public static readonly string VoiceFXVolume = "voicefxVolume";

    // Options prefs
    // fullscreen
    public static readonly string FullscreenOption = "fullscreenPref";

    // resolution
    public static readonly string ResolutionWidth = "resolutionW";
    public static readonly string ResolutionHeight = "resolutionH";
}
