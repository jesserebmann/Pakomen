using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundExtracter : MonoBehaviour
{
    public int count = 890;
    public AudioSource cry;
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(GetAudioClip(i.ToString()));
        }
    }

    IEnumerator GetAudioClip(string count)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip($"https://japeal.com/wordpress/wp-content/themes/total/PKM/Cries/{count.PadLeft(3,'0')}.wav", AudioType.WAV))
        {
            //Debug.Log($"https://japeal.com/wordpress/wp-content/themes/total/PKM/Cries/{count.PadLeft(3,'0')}.wav");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var test = DownloadHandlerAudioClip.GetContent(www);
                //cry.clip = test;
                //cry.loop = true;
                //cry.Play();
                
                SavWav.Save(Application.persistentDataPath + $"/StreamingAssets/Audio/ {count.PadLeft(3,'0')}", test);
                //EncodeMP3.convert ( myClip,  Application.persistentDataPath + "/StreamingAssets/Audio/" + "001" + ".wav",  44100);
            }
        }
    }
}
