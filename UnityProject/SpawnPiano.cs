using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Linq;

public class SpawnPiano : MonoBehaviour
{
    public float offset = 7.0f;

    public AudioMixer audioMixer;
    private float volume = 0.0f;
    [Range(-5.0f, 10.0f)]
    public float m_volume = 0.0f;

    public static float debug = 0.5f;
    public float m_debug = 0.5f;

    public GameObject WhiteKeyPrefab;
    public GameObject BlackKeyPrefab;

    private Mode mode = Mode.None;
    public Mode m_mode = Mode.None;

    public int[] activeList;
    //C Db D Eb E F Gb G Ab A Bb B

    [System.Serializable]
    public enum Mode
    {
        None, CMajor, DMajor, EMajor, FMajor, GMajor, AMajor, BMajor, BFlatMajor, FSharpMajor
    }

    private void Awake()
    {
        ChangeMode();
        for (int i = 1; i < 8; i++)
        {
            SpawnSets(i);
        }
    }

    private void Update()
    {
        if(m_mode != mode)
        {
            mode = m_mode;
            ChangeMode();
            UpdateActiveKeys();
        }

        if (m_volume != volume)
        {
            volume = m_volume;
            ChangeVolume();
        }

        if(m_debug != debug)
        {
            debug = m_debug;
        }
    }

    private void SpawnSets(int pitch)
    {
        foreach (int value in Enum.GetValues(typeof(PlaySound.SoundKey)))
        {
            PlaySound.SoundKey targetSoundKey = (PlaySound.SoundKey)value;
            switch (targetSoundKey)
            {
                case PlaySound.SoundKey.C:
                    SpawnWhiteKey(pitch, 0, targetSoundKey);
                    break;
                case PlaySound.SoundKey.D:
                    SpawnWhiteKey(pitch, 1, targetSoundKey);
                    break;
                case PlaySound.SoundKey.E:
                    SpawnWhiteKey(pitch, 2, targetSoundKey);
                    break;
                case PlaySound.SoundKey.F:
                    SpawnWhiteKey(pitch, 3, targetSoundKey);
                    break;
                case PlaySound.SoundKey.G:
                    SpawnWhiteKey(pitch, 4, targetSoundKey);
                    break;
                case PlaySound.SoundKey.A:
                    SpawnWhiteKey(pitch, 5, targetSoundKey);
                    break;
                case PlaySound.SoundKey.B:
                    SpawnWhiteKey(pitch, 6, targetSoundKey);
                    break;
                case PlaySound.SoundKey.Db:
                    SpawnBlackKey(pitch, 1, targetSoundKey);
                    break;
                case PlaySound.SoundKey.Eb:
                    SpawnBlackKey(pitch, 3, targetSoundKey);
                    break;
                case PlaySound.SoundKey.Gb:
                    SpawnBlackKey(pitch, 7, targetSoundKey);
                    break;
                case PlaySound.SoundKey.Ab:
                    SpawnBlackKey(pitch, 9, targetSoundKey);
                    break;
                case PlaySound.SoundKey.Bb:
                    SpawnBlackKey(pitch, 11, targetSoundKey);
                    break;
                default:
                    break;
            }
            UpdateActiveKeys();
        }
    }

    private PlaySound SpawnWhiteKey(int setID, int soundKeyID, PlaySound.SoundKey targetSoundKey)
    {
        float zOffset = offset * setID + soundKeyID;
        Vector3 position = new Vector3(0, 0, zOffset);
        GameObject whiteKey = Instantiate(WhiteKeyPrefab, position, Quaternion.identity, transform);
        
        PlaySound playSound = whiteKey.GetComponentInChildren<PlaySound>();
        playSound.pitchOffset = setID;
        playSound.soundKey = targetSoundKey;
        playSound.LoadAudioClip();
        return playSound;
    }

    private PlaySound SpawnBlackKey(int setID, int soundKeyID, PlaySound.SoundKey targetSoundKey)
    {
        float zOffset = offset * setID + soundKeyID * 0.5f;
        Vector3 position = new Vector3(0, 0.3f, zOffset);
        GameObject blackKey = Instantiate(BlackKeyPrefab, position, Quaternion.identity, transform);

        PlaySound playSound = blackKey.GetComponentInChildren<PlaySound>();
        playSound.pitchOffset = setID;
        playSound.soundKey = targetSoundKey;
        playSound.LoadAudioClip();
        return playSound;
    }

    public void SetMode(int targetMode)
    {
        m_mode = (Mode)targetMode;
        ChangeMode();
        UpdateActiveKeys();
    }

    private void ChangeMode()
    {
        switch (mode)
        {
            //C Db D Eb E F Gb G Ab A Bb B
            case Mode.None:
                activeList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                break;
            case Mode.CMajor:
                activeList = new int[] { 0, 2, 4, 5, 7, 9, 11 };
                break;
            case Mode.DMajor:
                activeList = new int[] { 1, 2, 4, 6, 7, 9, 11 };
                break;
            case Mode.EMajor:
                activeList = new int[] { 1, 3, 4, 6, 7, 9, 11 };
                break;
            case Mode.FMajor:
                activeList = new int[] { 0, 2, 4, 5, 7, 9, 10 };
                break;
            case Mode.GMajor:
                activeList = new int[] { 0, 2, 4, 6, 7, 9, 11 };
                break;
            case Mode.AMajor:
                activeList = new int[] { 1, 2, 4, 6, 8, 9, 11 };
                break;
            case Mode.BMajor:
                activeList = new int[] { 1, 3, 4, 6, 8, 10, 11 };
                break;
            case Mode.BFlatMajor:
                activeList = new int[] { 0, 2, 3, 5, 7, 9, 10 };
                break;
            case Mode.FSharpMajor:
                activeList = new int[] { 1, 3, 5, 6, 8, 10, 11 };
                break;

            default:
                break;
        }
    }

    private void UpdateActiveKeys()
    {
        PlaySound[] playSounds = GetComponentsInChildren<PlaySound>();
        foreach (PlaySound playSound in playSounds)
        {
            if (activeList.Contains((int)playSound.soundKey))
            {
                playSound.SetActive();
            }
            else
            {
                playSound.SetInactive();
            }
        }


    }

    private void ChangeVolume()
    {
        //float tempVolume;
        //audioMixer.GetFloat("MasterVolume", out tempVolume);
        audioMixer.SetFloat("MasterVolume", volume);
        //Debug.Log(tempVolume);
    }

}
