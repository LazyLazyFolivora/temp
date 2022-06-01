using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.IO;
using System;
using LitJson;

public class PerExcel
{
    private string excelName;
    private string type;
    private int colNum;
    private int rowNum;
    private dynamic pack;
    
    public dynamic GetPack
    {
        get
        {
            return pack;
        }
    }

    private List<string> keys = new List<string>();
    public dynamic GetKeys
    {
        get
        {
            return keys;
        }
    }

    // Ŀǰ֧��4�����ͣ�string, bool, int, double
    public PerExcel(string _excelName, DataRowCollection _pack, string _type, int _colNum, int _rowNum)
    {
        excelName = _excelName;
        type = _type.ToLower();
        colNum = _colNum;
        rowNum = _rowNum - 3;
        UnityEngine.Debug.Log(colNum);
        UnityEngine.Debug.Log(rowNum);
        // ��¼�¼��� �ڵڶ���
        for(int i = 0; i < colNum; i++)
        {
            UnityEngine.Debug.Log(_pack[1][i]);
            keys.Add(_pack[1][i].ToString());
        }


        // ����һ���µ��ֵ�
        if (_type.ToLower() == "dict")
        {
            pack = new Dictionary<string, Hashtable>();
            for (int i = 3; i < _rowNum; i++)
            {
                Hashtable row = new Hashtable();
                for (int j = 0; j < _colNum; j++)
                {
                    row.Add(_pack[1][j], GetValue(_pack[0][j].ToString(), _pack[i][j]));
                }
                if (pack.ContainsKey(_pack[i][0].ToString()))
                {
                    UnityEngine.Debug.LogError("���ظ��������޸ļ������ظ�������" + _pack[i][0]);
                }
                pack.Add(_pack[i][0].ToString(), row);
            }
        }
        else if (_type.ToLower() == "vector")
        {
            pack = new List<Hashtable>();
            for (int i = 3; i < _rowNum; i++)
            {
                Hashtable row = new Hashtable();
                for (int j = 0; j < _colNum; j++)
                {
                    row.Add(_pack[1][j], GetValue(_pack[0][j].ToString().ToLower(), _pack[i][j]));
                }

                pack.Add(row);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Init���type��������" + "��������:" + _type);
        }
    }
    
    dynamic GetValue(string _type, dynamic _value)
    {
        if (_type == "int")
        {
            return Convert.ToInt32(_value.ToString());
        }
        else if (_type == "double")
        {
            return Convert.ToDouble(_value.ToString());
        }
        else if (_type == "string")
        {
            return _value.ToString();
        }
        else if (_type == "bool")
        {
            return Convert.ToBoolean(_value.ToString());
        }
        else if (_type == "hashtable")
        {
            string json = _value.ToString();
            Hashtable jd = JsonMapper.ToObject<Hashtable>(json);
            foreach (DictionaryEntry de in jd)
            {
                UnityEngine.Debug.Log(de.Value);
            }
            return jd;
        }
        else
        {
            UnityEngine.Debug.LogError("Ŀǰ����ת����" + _value.ToString() + "���͵����ݣ��������");
            return "error";
        }
    }

    public void PrintKeys()
    {
        for(int i = 0; i < keys.Count; i++)
        {
            // ֮��дһ��Debugģ�� Ŀǰ��д��
            UnityEngine.Debug.Log("key name: " + keys[i]);
        }
    }
}


public class AllExcels
{
    private static AllExcels instance;
    private static Dictionary<string, PerExcel> excelDict;
    private static Mutex mutex = new Mutex();
    private AllExcels()
    {
        if(excelDict == null)
        {
            excelDict = new Dictionary<string, PerExcel>();
        }
    }
    public static AllExcels GetInstance
    {
        get
        {
            // lock
            mutex.WaitOne();
            if (instance == null)
            {
                instance = new AllExcels();
            }
            // unlock;
            mutex.ReleaseMutex();
            return instance;
        }
    }
    public ref Dictionary<string, PerExcel> GetExcelDict
    {
        get
        {
            return ref excelDict;
        }
        
    }

    public PerExcel GetExcel(string _name)
    {

        if(!excelDict.ContainsKey(_name))
        {
            UnityEngine.Debug.LogError("Init�����޸ñ�" + _name);
        }

        return excelDict[_name];
    }


}