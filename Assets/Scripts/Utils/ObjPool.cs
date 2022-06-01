using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����أ�Ŀǰ�����㷨ֻ��LRU��֮��Ҫ��Ҫ�������㷨
/// @author Folivora Li
/// AddObject(): ���һ��ֵΪ_Val, ��Ϊ_key�Ķ���
/// RemoveObject(): ɾ��һ����Ϊ_key�Ķ���
/// GetObject(): �õ�һ����Ϊ_key�Ķ���(�����������еĻ�), ���û�л᷵����ʾ�� ������Ҫ�������Լ����жϣ������ȵ���IsHaveObject�ٵ������
/// IsHaveObject(): ���ض�������Ƿ����������
/// MaxSize: ���������������
/// Size: ����ص�ǰ��������
/// </summary>
/// <typeparam name="T"></typeparam>

public class ObjPool<T>
{
    LRUList<T> pool;
    public ObjPool(int _maxSize, string _name = "")
    {
        pool = new LRUList<T>(_maxSize);
    }

    public void AddObject(T _val, string _key)
    {
        pool.Insert(_key, _val);
    }
    public void RemoveObject(string _key)
    {
        pool.Remove(_key);
    }

    public bool IsHaveObject(string _key)
    {
        return pool.IsHaveObject(_key);
    }

    public T GetObject(string _key)
    {
        T res = pool.GetValue(_key);
        if (EqualityComparer<T>.Default.Equals(res))
        {
            return default;
        }
        return res;
    }

    public int Maxsize
    {
        get
        {
            return pool.Maxsize;
        }
    }

    public int Size
    {
        get
        {
            return pool.Size;
        }
    }

}
