using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Excel;

public class ExcelData
{
    /// <summary>
    /// Excel表
    /// </summary>
    public DataSet dataSet;

    /// <summary>
    /// 总行数,读取到的比实际的多一行，实际最后一个是rows - 2
    /// </summary>
    public int rows;

    /// <summary>
    /// 总列数，读取到的比实际的多一列
    /// </summary>
    public int Columns;
}

public class PersonData
{
    public string Type;
    public string Name;
    public string Sex;
    public string Msg;
}

public enum LaoMoType
{
    省级 = 0,
    市级 = 1,
    全国 = 2,
}

public class ExcelControl : MonoBehaviour
{
    public static ExcelControl Instance;

    private string ShengJi = "新劳模信息/（省级）省部级劳模名单.xlsx";
    private string ShiJi = "新劳模信息/（市级）西安市总工会(City)市劳模名单524.xlsx";
    private string QuanGuo = "新劳模信息/（全国）省部级以上劳模名单.xlsx";

    public ExcelData ShengJiData;
    public ExcelData ShiJiData;
    public ExcelData GuoJiaJiData;

    private Dictionary<string, PersonData> ShengJiMsg = new Dictionary<string, PersonData>();
    private Dictionary<string, PersonData> ShiJiMsg = new Dictionary<string, PersonData>();
    private Dictionary<string, PersonData> QuanGuoMsg = new Dictionary<string, PersonData>();

    private void Awake()
    {
        Instance = this;

        ShengJiData = LoadData(ShengJi);
        ShiJiData = LoadData(ShiJi);
        GuoJiaJiData = LoadData(QuanGuo);
    }

    private ExcelData LoadData(string Path)
    {
        // StreamingAssets目录下的  党员信息.xlsx文件的路径：Application.streamingAssetsPath + "/党员信息.xlsx" 
        FileStream fileStream = File.Open(Application.streamingAssetsPath + "/" + Path, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        // 表格数据全部读取到result里(引入：DataSet（
        //using System.Data;） 需引入 System.Data.dll到项目中去)
        DataSet result = excelDataReader.AsDataSet();

        ExcelData data = new ExcelData();
        data.dataSet = result;
        data.rows = result.Tables[0].Rows.Count;
        data.Columns = result.Tables[0].Columns.Count;

        return data;
    }

    public PersonData GetPersonMsg()
    {
        int num = UnityEngine.Random.Range(0,3);
        switch (num)
        {
            case 0:
                return GetPersonMsg(LaoMoType.省级);
            case 1:
                return GetPersonMsg(LaoMoType.市级);
            case 2:
                return GetPersonMsg(LaoMoType.全国);
        }
        return null;
    }

    bool IsRepeat, IsRepeat2, IsRepeat3;
    string st1, st2, st3;
    private PersonData GetPersonMsg(LaoMoType type)
    {
        int num;
        switch (type)
        {
            case LaoMoType.省级:
                num = UnityEngine.Random.Range(1, ShengJiData.rows - 1);

                //这段是去重
                st1 = ShengJiData.dataSet.Tables[0].Rows[num][1].ToString();
                IsRepeat = JudeRepeat(st1);
                while (IsRepeat)
                {
                    num = UnityEngine.Random.Range(1, ShengJiData.rows - 1);
                    st1 = ShengJiData.dataSet.Tables[0].Rows[num][1].ToString();
                    IsRepeat = JudeRepeat(st1);
                }

                if (ShengJiMsg.ContainsKey(st1))
                {
                    return ShengJiMsg[st1];
                }
                else
                {
                    PersonData personData = new PersonData();
                    personData.Type = ShengJiData.dataSet.Tables[0].Rows[num][0].ToString();
                    personData.Name = st1;
                    personData.Sex = "(" + ShengJiData.dataSet.Tables[0].Rows[num][2].ToString() + ")";
                    personData.Msg = Sentence(Replace(ShengJiData.dataSet.Tables[0].Rows[num][3].ToString()), ShengJiData.dataSet.Tables[0].Rows[num][5].ToString(),
                        ShengJiData.dataSet.Tables[0].Rows[num][4].ToString(), ShengJiData.dataSet.Tables[0].Rows[num][8].ToString(), ShengJiData.dataSet.Tables[0].Rows[num][7].ToString());

                    ShengJiMsg.Add(personData.Name, personData);
                    return personData;
                }
            case LaoMoType.市级:
                num = UnityEngine.Random.Range(1, ShiJiData.rows - 1);

                //这段是去重
                st2 = ShiJiData.dataSet.Tables[0].Rows[num][1].ToString();
                IsRepeat2 = JudeRepeat(st2);
                while (IsRepeat2)
                {
                    num = UnityEngine.Random.Range(1, ShiJiData.rows - 1);
                    st2 = ShiJiData.dataSet.Tables[0].Rows[num][1].ToString();
                    IsRepeat2 = JudeRepeat(st2);
                }

                if (ShiJiMsg.ContainsKey(st2))
                {
                    return ShiJiMsg[st2];
                }
                else
                {
                    PersonData personData = new PersonData();
                    personData.Type = ShiJiData.dataSet.Tables[0].Rows[num][0].ToString();
                    personData.Name = st2;
                    personData.Sex = "(" + ShiJiData.dataSet.Tables[0].Rows[num][2].ToString() + ")";
                    personData.Msg = Sentence(Replace(ShiJiData.dataSet.Tables[0].Rows[num][3].ToString()), ShiJiData.dataSet.Tables[0].Rows[num][7].ToString(),
                        ShiJiData.dataSet.Tables[0].Rows[num][5].ToString(), ShiJiData.dataSet.Tables[0].Rows[num][11].ToString(), ShiJiData.dataSet.Tables[0].Rows[num][10].ToString()); ;

                    ShiJiMsg.Add(personData.Name, personData);
                    return personData;
                }
            case LaoMoType.全国:
                num = UnityEngine.Random.Range(1, GuoJiaJiData.rows - 1);

                //这段是去重
                st3 = GuoJiaJiData.dataSet.Tables[0].Rows[num][1].ToString();
                IsRepeat3 = JudeRepeat(st3);
                while (IsRepeat3)
                {
                    num = UnityEngine.Random.Range(1, GuoJiaJiData.rows - 1);
                    st3 = GuoJiaJiData.dataSet.Tables[0].Rows[num][1].ToString();
                    IsRepeat3 = JudeRepeat(st3);
                }

                if (QuanGuoMsg.ContainsKey(st3))
                {
                    return QuanGuoMsg[st3];
                }
                else
                {
                    PersonData personData = new PersonData();
                    personData.Type = GuoJiaJiData.dataSet.Tables[0].Rows[num][0].ToString();
                    personData.Name = st3;
                    personData.Sex = "(" + GuoJiaJiData.dataSet.Tables[0].Rows[num][2].ToString() + ")";
                    personData.Msg = Sentence(Replace(GuoJiaJiData.dataSet.Tables[0].Rows[num][3].ToString()), GuoJiaJiData.dataSet.Tables[0].Rows[num][5].ToString(),
                        GuoJiaJiData.dataSet.Tables[0].Rows[num][4].ToString(), GuoJiaJiData.dataSet.Tables[0].Rows[num][8].ToString(), GuoJiaJiData.dataSet.Tables[0].Rows[num][7].ToString()); ;

                    QuanGuoMsg.Add(personData.Name, personData);
                    return personData;
                }
        }
        return null;
    }

    private bool JudeRepeat(string Name)
    {
        return WaitPanel.Instance.CurrentListName.Contains(Name);
    }

    /// <summary>
    /// 拼接句子
    /// </summary>
    /// <param name="Name">姓名</param>
    /// <param name="Date">出生日期</param>
    /// <param name="Address">工作地址</param>
    /// <param name="Position">职位</param>
    /// <param name="Date2">获得荣誉日期</param>
    /// <param name="Honor">所获荣誉</param>
    /// <returns>一句话：出生日期:19XX年X月X日 在XXXXX任XX 19XX年获得XXX X</returns>
    private string Sentence(string Date,string Address,string Position,string Date2,string Honor)
    {
        if(Position.Contains("0") || Position.Contains("退休") || Position.Contains("离休") || Position.Contains("无"))
        {
            Position = null;
        }
        else
        {
            Position = "任" + Position;
        }

        string msg = "出生日期:" + Date + " 在" + Address + Position + " " + Date2 + "年获得" + Honor;

        return msg;
    }

    /// <summary>
    /// 对出生日期进行处理，去掉/
    /// </summary>
    /// <param name="Date">出生日期</param>
    /// <returns></returns>
    private string Replace(string Date)
    {
        int num = Date.IndexOf("/");
        Date = Date.Remove(num,1);
        Date = Date.Insert(num, "年");
        num = Date.IndexOf("/");
        Date = Date.Remove(num,1);
        Date = Date.Insert(num, "月");
        Date = Date + "日";
        return Date;
    }
}
