using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    public UIManager Manager;

    [Header("Button References")]
    [SerializeField] private Button SingleBtn;
    [SerializeField] private Button OnlineBtn;
    [SerializeField] private Button HelpBtn;
    [SerializeField] private Button llustratedBtn;
    [SerializeField] private Button DeveloperBtn;
    [SerializeField] private Button SettingsBtn;
    [SerializeField] private Button QuitBtn;

    public UIDeveloper uIDeveloper;

    void Start()
    {

    }

    #region ��ť�¼�ʵ��
    public void OnSingle()
    {
        SceneManager.Instance.LoadScene("MainSence");
        UIManager.Instance.currentScene = UIManager.Scene.MainSence;
    }

    public void OnOnline()
    {
        //SceneManager.Instance.LoadScene("CharSelect");
    }

    public void OnHelp()
    {
        //SceneManager.LoadScene("TutorialScene");
    }

    public void Onllustrated()
    {
    }

    public void OnDeveloper()
    {
        uIDeveloper.t = -50;
        uIDeveloper.Rect.content.anchoredPosition = new Vector2(uIDeveloper.Rect.content.anchoredPosition.x, 0);
    }

    

    public void OnQuit()
    {
        Application.Quit();
    }
    #endregion

    #region ��������
    // ����չ�������ơ��������õȹ���
    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
    
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    #endregion
}