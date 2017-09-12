using System.Collections;
using System.Collections.Generic;
using Improbable.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class BinbagClientNPCInfo : MonoBehaviour {

    [Require]
    private ClientAuthorityCheck.Reader authorityCheck;

    public bool IsNPCBinBag(){
        return authorityCheck == null;
    }

	public string GetClientId() {
		return authorityCheck.Data.clientid;
	}

}
