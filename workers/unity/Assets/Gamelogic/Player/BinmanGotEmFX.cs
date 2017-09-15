using System.Collections;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class BinmanGotEmFX : MonoBehaviour {

	public AudioClip gotEmClip;

	[Require]
	private ClientAuthorityCheck.Writer authorityCheck;

	private AudioSource gotEmAudioSource;

	private void OnEnable()
	{
		LoadAudio();
	}

	private void OnDisable()
	{
		if (gotEmAudioSource != null)
		{
			gotEmAudioSource.Stop();
			Destroy(gotEmAudioSource);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
        if (authorityCheck != null && other.tag == "Binbag")
		{
            gotEmAudioSource.Play();
		}
	}

	private void LoadAudio()
	{
		gotEmAudioSource = gameObject.AddComponent<AudioSource>();
		gotEmAudioSource.clip = gotEmClip;
		gotEmAudioSource.loop = false;
	}
}
