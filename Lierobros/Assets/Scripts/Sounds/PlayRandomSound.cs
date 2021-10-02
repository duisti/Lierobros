using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayRandomSound : MonoBehaviour
{

	public List<AudioClip> clips = new List<AudioClip>();
	float audioDur = 10f;
    // Start is called before the first frame update
    void Start()
    {
		if (clips.Count == 0) {
			Destroy(this);
		}
		AudioSource source = GetComponent<AudioSource>();
		source.clip = clips[Random.Range(0, clips.Count - 1)];
		audioDur = source.clip.length * 1.2f;
		source.Play();
    }

    // Update is called once per frame
    void Update()
    {
		if (audioDur <= 0) Destroy(this);
		audioDur -= Time.deltaTime;
    }
}
