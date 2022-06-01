using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ListNode<T>
{
    public T val;
    public ListNode<T> next;
    public ListNode<T> pre;
    public string key;
    public ListNode(T _val, string _key, ListNode<T> _pre = null, ListNode<T> _next = null)
    {
        val = _val;
        pre = _pre;
        next = _next;
        key = _key;
    }


}

/// <summary>
/// c# 双向链表
/// author Folivora Li
/// </summary>
/// <typeparam name="T"></typeparam>
public class MyList<T>
{
    public ListNode<T> head;
    public ListNode<T> tail;
    int maxSize;
    int size;
    public MyList(int _val)
    {
        maxSize = _val > 1 ? _val : 2;
        head = null;
        tail = null;
    }

    void myDebug(string con)
    {
#if true
        Debug.Log(con);
#endif
    }
    /// <summary>
    /// 尾部插入
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_key"></param>
    /// <param name="res"></param>
    /// <returns></returns>
    public dynamic push_back(T _val, string _key, ref string res)
    {
        if(head == null)
        {
            ListNode<T> node = new ListNode<T>(_val, _key);
            head = node;
            tail = node;
            size = 1;
            myDebug("add key:" + _key);
            return node;
        }
        else
        {
            
            ListNode<T> node = new ListNode<T>(_val, _key, tail);
            tail.next = node;
            tail = node;
            myDebug("add key:" + _key);
            if (size >= maxSize)
            {
                res = head.key;
                head = head.next;
                head.pre = null;
                myDebug("remove key:" + res);
                // isFull = true;
                return node;
            }
            size++;
            return node;
        }

        
    }

    /// <summary>
    /// 将一个已知节点挪到尾部
    /// </summary>
    /// <param name="_val"></param>
    public void ToBack(ListNode<T> _val)
    {
        if(_val == head)
        {
            
            head = _val.next;
            head.pre = null;
            tail.next = _val;
            _val.pre = tail;
            _val.next = null;
            tail = _val;
        }
        else if(_val == tail)
        {
            // 啥也不干
        }
        else
        {
            _val.pre.next = _val.next;
            _val.next = _val.pre;
            tail.next = _val;
            _val.pre = tail;
            tail = _val;
        }
        myDebug("move to tail");
    }

    public int Size
    {
        get
        {
            return size;
        }
    }
    public int MaxSize
    {
        get
        {
            return maxSize;
        }
    }
    /// <summary>
    /// 删除一个已知节点
    /// </summary>
    /// <param name="val"></param>
    public void Delete(ListNode<T> val)
    {
        if(val == head)
        {
            
            head = head.next;
            head.pre = null;
            val.pre = null;
            val.next = null;
        }
        else if(val == tail)
        {
            tail.pre.next = null;
            val.next = null;
            val.pre = null;
        }
        else
        {
            ListNode<T> tmp = val.pre;
            tmp.next = val.next;
            val.next.pre = tmp;
            val.next = null;
            val.pre = null;
        }
        myDebug("delete key: " + val.key);
        size--;
    }
    

}

/// <summary>
/// Insert(): 检查链表中是否已经有_key为键的对象，若有则把对象放在链表的最后一位, 没有则需要提供对象，插在尾部
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class LRUList<T>
{
    public MyList<T> lruList;
    public Hashtable cacheHash;
    int maxSize;
    // public Hashtable<string, int> s;
    // 
    // public Hashtable<T, ListNode<T>> cacheHash;
    public LRUList(int _maxsize)
    {
        maxSize = _maxsize;
        lruList = new MyList<T>(_maxsize);
        cacheHash = new Hashtable();
    }

    public void Insert(string _key, T _val = default)
    {
        if (cacheHash.Contains(_key))
        {
            lruList.ToBack((ListNode<T>)cacheHash[_key]);
        }
        else
        {
            string res = "";
            // bool isFull = false;
            ListNode<T> node = lruList.push_back(_val, _key, ref(res));
            cacheHash.Add(_key, node);
            if(res != "")
            {
                cacheHash.Remove(_key);
            }
            

        }
    }

    public void Remove(string _key)
    {
        if(cacheHash.Contains(_key))
        {
            lruList.Delete((ListNode<T>)cacheHash[_key]);
            cacheHash.Remove(_key);

        }
    }

    public int Maxsize
    {
        get
        {
            return maxSize;
        }
    }

    public int Size
    {
        get
        {
            return lruList.Size;
        }
    }

    ListNode<T> Get(string _key)
    {
        if(!cacheHash.ContainsKey(_key))
        {
            Debug.Log("没有键为_key的对象！可能是池满了或没有添加");
            return default;
        }
        return (ListNode<T>)cacheHash[_key];
    }

    public T GetValue(string _key)
    {
        ListNode<T> tmp = Get(_key);

        // 比较类是否为default
        if (EqualityComparer<T>.Default.Equals(tmp))
        {
            return default;
        }
        return tmp.val;
    }


    public bool IsHaveObject(string _key)
    {
        bool res = cacheHash.Contains(_key);
        if(res)
        {
            Insert(_key);
        }
        return res;
    }

}
