using Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;



/// 最终目标是用一个脚本完成对所有Excel表的读取
public static class ExcelRead
{
    /// 获取excel表格里面的数据
    /// 
    /*
    public static LanguagePack[] ReadLanguagePackExcel(string filePath)
    {
        //这里的用List存储 可避免一些空行的保存
        List<LanguagePack> list = new List<LanguagePack>();
        int columnNum = 0, rowNum = 0;//excel 行数 列数
        DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);

        //这里i从1开始遍历， 因为第一行是标签名
        for (int i = 1; i < rowNum; i++)
        {
            //如果改行是空行 不保存
            if (IsEmptyRow(collect[i], columnNum)) continue;

            LanguagePack lp = new LanguagePack();
            lp.ID = collect[i][0].ToString();
            lp.CHS = collect[i][1].ToString();
            lp.CHT = collect[i][2].ToString();
            lp.EN = collect[i][3].ToString();
            lp.JP = collect[i][4].ToString();
            lp.Person = collect[i][5].ToString();
            list.Add(lp);
        }
        
        return list.ToArray();
    }
    */ 

    public static void LoadExcels()
    {
        string root = Application.dataPath + "/Excels/" + "Init.xlsx";
        /// 开始读Init表
        int columnNum = 0, rowNum = 0;//excel 行数 列数
        DataRowCollection collect = ReadExcel(root, ref rowNum, ref columnNum);
        // allExcels = AllExcels.GetInstance.GetExcelDict
        Debug.Log("列数：" + rowNum + "  行数：" + columnNum);
        Debug.Log(root);
        for (int i = 1; i < columnNum; i++)
        {
            Debug.Log("加载表:" + collect[i][1].ToString());
            PerExcel excel = Read(Application.dataPath + collect[i][0].ToString(), collect[i][1].ToString(), collect[i][2].ToString());
            AllExcels.GetInstance.GetExcelDict.Add(collect[i][1].ToString(), excel);
        }
        Debug.Log("全部Excel加载完成！");
    }

    public static PerExcel Read(string _filePath, string _className, string _type)
    {
        int columnNum = 0, rowNum = 0;//excel 行数 列数
        DataRowCollection collect = ReadExcel(_filePath, ref columnNum, ref rowNum);
        Debug.Log(_className + "初始化完毕！");
        PerExcel table = new PerExcel(_className, collect, _type, columnNum, rowNum);
        return table;
    }


    //判断是否是空行
    static bool IsEmptyRow(DataRow collect, int columnNum)
    {
        for (int i = 0; i < columnNum; i++)
        {
            if (!collect.IsNull(i)) return false;
        }
        return true;
    }

    /// <summary>
    /// 读取excel文件内容获取行数 列数 方便保存
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="columnNum">行数</param>
    /// <param name="rowNum">列数</param>
    /// <returns></returns>
    public static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();
        //Tables[0] 下标0表示excel文件中第一张表的数据
        columnNum = result.Tables[0].Columns.Count;
        rowNum = result.Tables[0].Rows.Count;
        return result.Tables[0].Rows;
    }
}
