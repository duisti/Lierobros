using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayRandomSound : MonoBehaviour
{

	public List<AudioClip> clips = new List<AudioClip>();
	float audioDur = 10f;
	AudioSource source;

	bool expiring = false;
    // Start is called before the first frame update
    void Start()
    {
		if (clips.Count == 0) {
			Destroy(this.gameObject);
		}
		source = GetComponent<AudioSource>();
		source.clip = clips[Random.Range(0, clips.Count - 1)];
		audioDur = source.clip.length * 1.2f;
		source.Play();
    }

    // Update is called once per frame
    void Update()
    {
		if (expiring) {
			return;
		}

		//test for expiring
		/*
		if (Input.GetKeyDown(KeyCode.Space) & Application.isEditor) {
			Expire(5f);
		}
		*/

		//if it's a looping sound source, then never expire
		if (source.loop) {
			return;
		}
		if (audioDur <= 0) Expire(0);
		audioDur -= Time.deltaTime;
    }

	public void Expire(float fadeDur) {
		expiring = true;
		if (fadeDur != 0) {
			StartCoroutine(FadeCoroutine(fadeDur));
		}
		else Destroy(this.gameObject);
	}

	IEnumerator FadeCoroutine(float fadeDur) {
		var timer = fadeDur;
		var start = source.volume;
		while (timer > 0) {
			timer -= Time.deltaTime;
			source.volume = Mathf.Lerp(0, start, timer / fadeDur);
			yield return null;
		}
		yield return null;
		Destroy(this.gameObject);
	}
}
