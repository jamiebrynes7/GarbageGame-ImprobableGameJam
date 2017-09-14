using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Improbable.Unity;
using Improbable.Unity.Visualizer;
using Improbable.Core;

[WorkerType(WorkerPlatform.UnityClient)]
public class MusicController : MonoBehaviour {

	public AudioClip music;
	private AudioSource audioSource;

	[Require] private ClientAuthorityCheck.Writer crcWriter;

	private void OnEnable() 
	{
		if (crcWriter != null) {
			LoadAudio();
		}
	}

	private void OnDisable() 
	{
		if (audioSource != null) 
		{
			audioSource.Stop();
            Destroy(audioSource);
		}
	}

	private void LoadAudio() 
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = music;
		audioSource.loop = true;
		audioSource.Play();
	}
}
