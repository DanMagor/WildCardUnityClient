using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSearch : MonoBehaviour {


    public void SearchOpponent()
    {
        ClientTCP.PACKAGE_SearchOpponent();
    }

}
