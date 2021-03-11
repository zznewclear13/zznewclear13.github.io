using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    public enum SoundKey
    {
        C, Db, D, Eb, E, F, Gb, G, Ab, A, Bb, B
    }

    public Text text;

    private const string PATH_TO_WAV = "Assets/1230/output/Piano.ff.";
    private AudioSource audioSource;

    public int pitchOffset = 3;
    public SoundKey soundKey = SoundKey.C;

    public float fadeTime = 3.0f;

    private Color originColor;
    private Color grayColor = Color.gray;
    private MeshRenderer keyMeshRenderer;

    private bool isHolding = false;
    private float currentRotate = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        keyMeshRenderer = GetComponent<MeshRenderer>();
        originColor = keyMeshRenderer.sharedMaterial.color;
        //LoadAudioClip();
    }
    public void LoadAudioClip()
    {
        AudioClip clip = (AudioClip)AssetDatabase.LoadAssetAtPath(PATH_TO_WAV + soundKey.ToString() + pitchOffset + ".wav", typeof(AudioClip));
        audioSource.clip = clip;
        text.text = soundKey.ToString() + pitchOffset;
    }

    private void Update()
    {
        //LoadAudioClip();
    }

    public void Play()
    {
        StopAllCoroutines();
        audioSource.volume = 1.0f;
        audioSource.Play();
        //StartCoroutine(FadeEffect(fadeTime));
        StartCoroutine(PressRotate());
    }

    public void OnHold()
    {
        if(!isHolding)
        {
            isHolding = true;
            Play();
        }
    }

    public void StopHolding()
    {
        StopAllCoroutines();
        isHolding = false;
        StartCoroutine(FadeEffect(fadeTime));
        StartCoroutine(BounceBackRotate());
    }


    IEnumerator FadeEffect(float _fadeTime)
    {
        float tempTime = 0;
        while (tempTime < _fadeTime)
        {
            tempTime += Time.deltaTime;
            float fadeVolumn = Mathf.SmoothStep(1.0f, 0, tempTime / _fadeTime);
            audioSource.volume = fadeVolumn;
            yield return null;
        }
        yield return null;
    }

    IEnumerator PressRotate()
    {
        float maxAngle = 4.0f;
        float speed = 30.0f;
        while (currentRotate < maxAngle)
        {
            currentRotate += speed * Time.deltaTime;
            transform.parent.rotation = Quaternion.Euler(0, 0, -currentRotate);
            yield return null;
        }
        yield return null;
    }

    IEnumerator BounceBackRotate()
    {
        //float maxAngle = 4.0f;
        float speed = 30.0f;
        while (currentRotate > 0)
        {
            currentRotate -= speed * Time.deltaTime;
            if (currentRotate < 0)
            {
                currentRotate = 0;
            }
            transform.parent.rotation = Quaternion.Euler(0, 0, -currentRotate);
            yield return null;
        }
        yield return null;
    }

    IEnumerator PingPongRotate()
    {
        float totalTime = 0.2f;
        float maxAngle = 8.0f; //事实上是4
        float tempTime = 0;
        while (tempTime < totalTime)
        {
            tempTime += Time.deltaTime;
            float ratio = Mathf.Abs(0.5f - tempTime / totalTime) - 0.5f;
            transform.parent.rotation = Quaternion.Euler(0, 0, ratio * maxAngle);
            yield return null;
        }
        yield return null;
    }

    public void SetActive()
    {
        keyMeshRenderer.material.color = originColor;
    }

    public void SetInactive()
    {
        keyMeshRenderer.material.color = grayColor;
    }

}
