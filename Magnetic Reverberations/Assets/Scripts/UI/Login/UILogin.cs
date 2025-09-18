using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UILogin : MonoBehaviour
{
    public TMP_InputField Email;
    public TMP_InputField Password;


    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField passwordConfirm;

    public GameObject uiLogin;
    public void OnButton()
    {
        if (string.IsNullOrEmpty(this.Email.text))
        {
            MessageBox.Show("�������˺�");
            return;
        }
        if (string.IsNullOrEmpty(this.Password.text))
        {
            MessageBox.Show("����������");
            return;
        }
        if (Logins(this.Email.text, this.Password.text))
        {
            
            User.Instance.UserSet(PlayerPrefs.GetInt("user_" + Email.text), "user_"+this.Email.text);
            SceneManager.Instance.LoadScene("LoadingSence");
            UIManager.Instance.currentScene = UIManager.Scene.LoadingSence;
        }
    }
    public bool Logins(string username, string password)
    {
        if (!PlayerPrefs.HasKey("user_" + username))
        {
            Debug.LogError("�û�������");
            return false;
        }
        return password == PlayerPrefs.GetString(PlayerPrefs.GetInt("user_" + username).ToString()); ;
    }


    public void OnClickRegister()
    {
        PlayerPrefs.DeleteAll();
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("�������˺�");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("����������");
            return;
        }
        if (string.IsNullOrEmpty(this.passwordConfirm.text))
        {
            MessageBox.Show("������ȷ������");
            return;
        }
        if (this.password.text != this.passwordConfirm.text)
        {
            MessageBox.Show("������������벻һ��");
            return;
        }
        if(Register(this.username.text, this.password.text))
        {
            MessageBox.Show("ע��ɹ�,���¼", "��ʾ", MessageBoxType.Information).OnYes = this.CloseRegister;
        }
        
    }
    public bool Register(string username, string password)
    {
        if (PlayerPrefs.HasKey("user_" + username))
        {
            Debug.LogError("�û����Ѵ���");
            return false;
        }
        int randomID;
        do
        {
            randomID = UnityEngine.Random.Range(1, 1000);
        } while (PlayerPrefs.HasKey(randomID.ToString()));
        PlayerPrefs.SetInt("user_" + username, randomID);
        PlayerPrefs.SetString(randomID.ToString(), password);
        PlayerPrefs.Save();
        return true;
    }
    void CloseRegister()
    {
        this.gameObject.SetActive(true);
        uiLogin.SetActive(false);
    }
}