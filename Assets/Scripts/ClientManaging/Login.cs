#pragma warning disable 0649
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Core;

public class Login : MonoBehaviour
{

    [SerializeField]
    private InputField username;
    [SerializeField] private InputField password;

    public void LoginAccount()
    {
        //if (username.text == string.Empty) { Debug.Log("Please enter a username"); return; }
        //if (password.text == string.Empty) { Debug.Log("Please enter a password"); return; }
        try
        {
            ClientTCP.PACKAGE_Login(username.text, password.text);
        }
        catch (Exception e)
        {
            Debug.Log("Trying to initialize connection");
            try
            {
                ClientTCP.clientManager.InitializeClientManager();
                ClientTCP.PACKAGE_Login(username.text, password.text);
            }
            catch (Exception exception)
            {
                Debug.Log("No connection to the server");
                throw;
            }
        }
       
    }
}
