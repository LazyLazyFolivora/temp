using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音效管理模块，采用LRU维护音效对象池
/// </summary>
/// 


public class SEManager: MonoBehaviour
{
    List<AudioSource> list2D;
    List<AudioSource> list3D;
    Hashtable list2DCache;
    ObjPool<AudioClip> pool2D;
    ObjPool<AudioClip> pool3D;
    int max2DNum = 10;
    int max3DNum = 10;
    PerExcel pack2D;
    private static SEManager instance;
    public static SEManager GetInstance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        list2DCache = new Hashtable();
        list2D = new List<AudioSource>();
        list3D = new List<AudioSource>();
        for (int i = 0; i < max2DNum; i++)
        {
            //在世界中创建节点 3D的到时候再说
            list2D.Add(GameObject.Find("Main Camera").gameObject.AddComponent<AudioSource>());

        }

        pool2D = new ObjPool<AudioClip>(max2DNum);
        pool3D = new ObjPool<AudioClip>(max3DNum);
        
    }

    private void Start()
    {
        pack2D = AllExcels.GetInstance.GetExcel("SE2D");
    }

    /// 先看对象池有没有这个对象, 如果有就直接拿对象池的这个对象即可
    /// 先加进对象池, 由对象池判断并管理所有AudioClip
    private AudioClip Add2DClip(string _name)
    {
        AudioClip clip;
        if (pool2D.IsHaveObject(_name))
        {
            clip = pool2D.GetObject(_name);
            
        }
        else
        {
            /// 坑爹啊，必须放在Resources文件夹下，而且不能带文件扩展名，前缀也不要   
            /// e.g.:Audios/SE/SE2D/test1

            string clipPath = pack2D.GetPack[_name]["Path"];
            // clipPath = "Audios/SE/SE2D/test1";
                
            clip = Resources.Load<AudioClip>(clipPath);
            pool2D.AddObject(clip, _name);
        }

        return clip;
        // a.clip = clip;

    }

    private void Add3DClip(string _name, string _clipPath)
    {
        AudioClip clip;
        if (pool3D.IsHaveObject(_name))
        {
            clip = pool3D.GetObject(_name);
        }
        else
        {
            clip = Resources.Load<AudioClip>(_clipPath);
            pool3D.AddObject(clip, _name);
        }
    }

    private void Remove2DClip(string _name)
    {
        pool2D.RemoveObject(_name);

    }

    private void Remove3DClip(string _name)
    {
        pool3D.RemoveObject(_name);
    }

    public void Play2DSE(string _name)
    {
        AudioClip clip = Add2DClip(_name);
        for(int i = 0; i < max2DNum; i++)
        {
            if(!list2D[i].isPlaying)
            {
                list2D[i].clip = clip;
                // print(pack2D.GetPack[_name]["IsLoop"]);
                list2D[i].loop = pack2D.GetPack[_name]["IsLoop"];
                list2D[i].name = _name;
                list2D[i].Play();
                list2DCache[_name] = i;
                Debug.Log(clip);
                break;
            }
            
        }

    }

    public void Stop2DSE(string _name)
    {
        if (list2D[(int)list2DCache[_name]].name == _name)
        {
            list2D[(int)list2DCache[_name]].Stop();
            list2DCache.Remove(_name);
        }
    }


}
