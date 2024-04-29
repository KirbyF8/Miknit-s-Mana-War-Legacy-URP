using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float masterVolume;
    private float sfxValue;
    private float voicesValue;
    private float musicValue;

    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource voices;
    [SerializeField] private AudioSource music;

    private bool mirror;
    private bool gridShow;
    private bool party;
    private bool autoend;

    public void ChangeMaster(float x)
    {
        masterVolume = x;

        ChangeSFX(sfxValue);
        ChangeMusic(musicValue);
        ChangeVoices(voicesValue);
    }

    public void ChangeSFX(float x)
    {
        sfxValue = x;
        sfx.volume = (sfxValue + masterVolume) / 2;
    }

    public void ChangeMusic(float x)
    {
        musicValue = x;
        music.volume = (musicValue+masterVolume)/2;
    }

    public void ChangeVoices(float x)
    {
        voicesValue = x;
        voices.volume = (voicesValue + masterVolume) / 2;
    }

    public void ShowGrid(bool x)
    {
        gridShow = x;
    }

    public void PartyMode(bool x)
    {
        party = x;
    }

    public void MirrorStats(bool x)
    {
        mirror = x;
    }

    public void AutoEndOnOff(bool x)
    {
        autoend = x;
    }

}
