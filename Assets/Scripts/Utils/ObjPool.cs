using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 对象池，目前更换算法只有LRU，之后看要不要加其他算法
/// @author Folivora Li
/// AddObject(): 添加一个值为_Val, 键为_key的对象
/// RemoveObject(): 删除一个键为_key的对象
/// GetObject(): 得到一个键为_key的对象(如果对象池中有的话), 如果没有会返回提示， 这里需要调用者自己做判断，建议先调用IsHaveObject再调用这个
/// IsHaveObject(): 返回对象池中是否有这个对象
/// MaxSize: 对象池最大对象数量
/// Size: 对象池当前对象数量
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
