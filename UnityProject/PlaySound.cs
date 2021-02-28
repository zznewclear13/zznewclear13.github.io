using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    public enum SoundKey
    {
        C, Db, D, Eb, E, F, Gb, G, Ab, A, Bb, B
    }

    public Text text;

    private string PATH_TO_WAV;

    private AudioSource audioSource;

    public int pitchOffset = 3;
    public SoundKey soundKey = SoundKey.C;

    public float fadeTime = 3.0f;

    private Color originColor;
    private Color grayColor = Color.gray;
    private MeshRenderer keyMeshRenderer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        keyMeshRenderer = GetComponent<MeshRenderer>();
        originColor = keyMeshRenderer.sharedMaterial.color;
        //LoadAudioClip();

#if UNITY_EDITOR
        PATH_TO_WAV = "file://" + Application.dataPath + "/Notes/Piano.ff.";
#else
        PATH_TO_WAV = "Notes/Piano.ff.";
#endif
    }


    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(PATH_TO_WAV + soundKey.ToString() + pitchOffset + ".wav", AudioType.WAV))
        {
            request.uri = new Uri(request.uri.AbsoluteUri.Replace("http://localhost", "file:/"));
            request.url = request.url.Replace("http://localhost", "file:/");

            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                audioSource.clip = clip;
            }
            else
            {
                Debug.Log(PATH_TO_WAV + soundKey.ToString() + pitchOffset + ".wav not found.");
            }
        }
    }

    public void LoadAudioClip()
    {
        StartCoroutine(GetAudioClip());
        /*
        AudioClip clip = (AudioClip)Resources.Load(PATH_TO_WAV + soundKey.ToString() + pitchOffset, typeof(AudioClip));
        //AudioClip clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Resources/" + PATH_TO_WAV + soundKey.ToString() + pitchOffset + ".wav", typeof(AudioClip));
        */
        text.text = soundKey.ToString() + pitchOffset;
    }

    private void Update()
    {
        //LoadAudioClip();
    }

    public void Play()
    {
        audioSource.Play();
        StopAllCoroutines();
        StartCoroutine(FadeEffect(fadeTime));
        StartCoroutine(PingPongRotate());
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
