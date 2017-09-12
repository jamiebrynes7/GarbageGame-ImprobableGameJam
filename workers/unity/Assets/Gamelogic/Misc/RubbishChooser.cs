using System.Collections;
using System.Collections.Generic;
using Improbable.Misc;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class RubbishChooser : MonoBehaviour {

    public static int RUBBISH_NUM = 3;

    [Require]
    private RubbishInfo.Reader rubbishInfoReader;

    [SerializeField] private GameObject[] typeGameObjects;

    private void OnEnable()
    {
        for (var i = 0; i < typeGameObjects.Length; i++){
            typeGameObjects[i].SetActive(i == rubbishInfoReader.Data.rubbishIndex);
        }
    }
}
