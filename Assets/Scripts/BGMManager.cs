using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 之后换地方
/// </summary>
// string bgmNodePath = "";

/// <summary>
/// 开局时自己建一个bgm节点，然后维护BGM即可
/// </summary>
public class BGMManager : MonoBehaviour
{
    private string localBGM = "test8";
    private AudioSource bgmNode;
    // 音效节点的表
    private PerExcel bgmPack;
    List<AudioSource> listBGM;
    ObjPool<AudioClip> poolBGM;
    int maxBGMNum = 5;
    private string defaultBGM = "test8";
    // Start is called before the first frame update

    private static BGMManager instance;
    public static BGMManager GetInstance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        instance = this;
        bgmNode = GameObject.Find("Main Camera").gameObject.GetComponent<AudioSource>();
        listBGM = new List<AudioSource>();
        poolBGM = new ObjPool<AudioClip>(maxBGMNum);
        
    }

    private void Start()
    {
        bgmPack = AllExcels.GetInstance.GetExcel("BGMPack");
        SetBGM(defaultBGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetBGM(string _newBgm)
    {
        AudioClip clip;
        if (poolBGM.IsHaveObject(_newBgm))
        {
            clip = poolBGM.GetObject(_newBgm);
        }
        else
        {
            string clipPath = bgmPack.GetPack[_newBgm]["Path"];
            // clipPath = "Audios/SE/SE2D/test1";

            clip = Resources.Load<AudioClip>(clipPath);
            poolBGM.AddObject(clip, _newBgm);
        }
        bgmNode.clip = clip;
        bgmNode.Play();
    }

    private void PauseBGM()
    {
        bgmNode.Pause();
    }

    private void RePlayBGM()
    {
        bgmNode.UnPause();
    }
    


    public void ChangeBGM(string _newBgm, bool _enforceChange = false)
    {
        if(_enforceChange)
        {
            SetBGM(_newBgm);
        }
        else
        {
            if(localBGM != _newBgm)
            {
                SetBGM(_newBgm);
            }
        }
    }
}
