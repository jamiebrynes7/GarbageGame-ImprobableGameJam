using System.Collections;
using System.Collections.Generic;
using Assets.Gamelogic.Player;
using Improbable.Core;
using Improbable.Player;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class BinmanDrunkVisualiser : MonoBehaviour {

    public AudioClip drunkSound;

    [Require]
    private BinmanInfo.Reader binmanInfoReader;

    [Require]
    private ClientAuthorityCheck.Writer authorityCheck;

	private AudioSource audioSource;

    private void OnEnable()
    {
        binmanInfoReader.IsDrunkUpdated.Add(IsDrunkChanged);
        LoadAudio();
    }

	private void OnDisable()
	{
		binmanInfoReader.IsDrunkUpdated.Remove(IsDrunkChanged);
		if (audioSource != null)
		{
			audioSource.Stop();
			Destroy(audioSource);
		}
	}

    private void IsDrunkChanged(bool isDrunk){
        GetComponent<ThirdPersonPlayerControls>().SetIsDrunk(isDrunk);
        if(isDrunk){
            audioSource.Play();
        }
    }

	private void LoadAudio()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = drunkSound;
        audioSource.loop = false;
	}
}
