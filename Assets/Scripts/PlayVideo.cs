using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour {

    public RawImage rawImage;
    private List<VideoClip> movies = new List<VideoClip>();
    public int selectedIndex;

    private VideoPlayer videoPlayer;

    void Start () {
        videoPlayer = GetComponent<VideoPlayer>();
        Play(GameController.spriteInt);
	}
    	

    public void Play(int index)
    {
        StartCoroutine(PlayVid(index));
    }

    IEnumerator PlayVid(int index)
    {
        string url = System.IO.Path.Combine(Application.streamingAssetsPath, "ult" + index + ".mp4");
        videoPlayer.url = url;
        videoPlayer.Prepare();
        
        while (!videoPlayer.isPrepared)
        {
            yield return null;            
        }

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();       
    }

}
