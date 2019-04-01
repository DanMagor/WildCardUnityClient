using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    [SerializeField] private InputField username;
    [SerializeField] private InputField password;

    public void LoginAccount()
    {
        //if (username.text == string.Empty) { Debug.Log("Please enter a username"); return; }
        //if (password.text == string.Empty) { Debug.Log("Please enter a password"); return; }

        ClientTCP.PACKAGE_Login(username.text, password.text);
    }
}
