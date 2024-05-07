using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuth : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, profilePanel, forgetPasswordPanel;

    public TMP_InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUserName, forgetPassEmail;

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);

    }
    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);


    }
    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);


    }

    public void OpenForgetPassPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);


    }

    public void LoginUser()
    {
        if(string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
        {
            return;
        }

        // Do Login
    }

    public void SignUpUser()
    {
        if(string.IsNullOrEmpty(signupEmail.text) &&  string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPassword.text) && string.IsNullOrEmpty(signupUserName.text))
        {
            return;
        }

        // Do SignUp
    }

    public void forgetPass()
    {
        if (string.IsNullOrEmpty(forgetPassEmail.text))
        { 
            return;
        }
    }
}
