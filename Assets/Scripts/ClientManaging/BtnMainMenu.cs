using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//SearchButton
public class BtnMainMenu : MonoBehaviour {

    public void SearchOpponent()
    {
        ClientManager.RequestSearch();
    }

}
