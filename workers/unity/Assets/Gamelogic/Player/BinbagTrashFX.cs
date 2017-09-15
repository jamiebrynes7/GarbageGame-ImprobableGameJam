using System.Collections;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class BinbagTrashFX : MonoBehaviour {

	public AudioClip trashClip;

	[Require]
	private ClientAuthorityCheck.Writer authorityCheck;

	private AudioSource trashAudioSource;

	private void OnEnable()
	{
		LoadAudio();
	}

	private void OnDisable()
	{
		if (trashAudioSource != null)
		{
			trashAudioSource.Stop();
			Destroy(trashAudioSource);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
        if (authorityCheck != null && other.tag == "RubbishTipWtf")
		{
            trashAudioSource.Play();
		}
	}

	private void LoadAudio()
	{
		trashAudioSource = gameObject.AddComponent<AudioSource>();
		trashAudioSource.clip = trashClip;
		trashAudioSource.loop = false;
	}
}
