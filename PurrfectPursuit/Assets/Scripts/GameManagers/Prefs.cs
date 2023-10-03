using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs : MonoBehaviour
{
    public static readonly string DemoHighScore = "DemoHighScore";


    // Volume prefs
    public static readonly string MasterVolume = "masterVolume";
    public static readonly string MusicVolume = "musicVolume";
    public static readonly string SFXVolume = "sfxVolume";

    // Options prefs
    // fullscreen
    public static readonly string FullscreenOption = "fullscreenPref";

    // resolution
    public static readonly string ResolutionWidth = "resolutionW";
    public static readonly string ResolutionHeight = "resolutionH";
}
