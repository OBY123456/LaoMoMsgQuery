using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Excel;
using Newtonsoft.Json;

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
    public string Birthday;
}

public class HeadData
{
    public string Name;
    public string Birthday;
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

    public Dictionary<HeadData, PersonData> AllPersonMsg = new Dictionary<HeadData, PersonData>();
    public List<HeadData> headDatas = new List<HeadData>();

    public Dictionary<string, List<Texture2D>>  PicGroup = new Dictionary<string, List<Texture2D>>();
    public Dictionary<string, List<string>> VideoGroup = new Dictionary<string, List<string>>();

    private Dictionary<string, string> VideoPath = new Dictionary<string, string>();
    private Dictionary<string, string> PicPath = new Dictionary<string, string>();

    private void Awake()
    {
        Instance = this;

        ShengJiData = LoadData(ShengJi);
        ShiJiData = LoadData(ShiJi);
        GuoJiaJiData = LoadData(QuanGuo);

        for (int i = 1; i < ShengJiData.rows - 1; i++)
        {
            PersonData personData = new PersonData();
            personData.Type = ShengJiData.dataSet.Tables[0].Rows[i][0].ToString();
            personData.Name = ShengJiData.dataSet.Tables[0].Rows[i][1].ToString();
            personData.Sex = "(" + ShengJiData.dataSet.Tables[0].Rows[i][2].ToString() + ")";
            personData.Birthday = Replace(ShengJiData.dataSet.Tables[0].Rows[i][3].ToString());
            personData.Msg = Sentence(personData.Birthday, ShengJiData.dataSet.Tables[0].Rows[i][5].ToString(),
                ShengJiData.dataSet.Tables[0].Rows[i][4].ToString(), ShengJiData.dataSet.Tables[0].Rows[i][8].ToString(), ShengJiData.dataSet.Tables[0].Rows[i][7].ToString());

            HeadData headData = new HeadData();
            headData.Name = personData.Name;
            headData.Birthday = personData.Birthday;

            AllPersonMsg.Add(headData, personData);
            headDatas.Add(headData);
        }

        for (int j = 1; j < ShiJiData.rows - 1; j++)
        {
            PersonData personData = new PersonData();
            personData.Type = ShiJiData.dataSet.Tables[0].Rows[j][0].ToString();
            personData.Name = ShiJiData.dataSet.Tables[0].Rows[j][1].ToString(); 
            personData.Sex = "(" + ShiJiData.dataSet.Tables[0].Rows[j][2].ToString() + ")";
            personData.Birthday = Replace(ShiJiData.dataSet.Tables[0].Rows[j][3].ToString());
            personData.Msg = Sentence(personData.Birthday, ShiJiData.dataSet.Tables[0].Rows[j][7].ToString(),
                ShiJiData.dataSet.Tables[0].Rows[j][5].ToString(), ShiJiData.dataSet.Tables[0].Rows[j][11].ToString(), ShiJiData.dataSet.Tables[0].Rows[j][10].ToString());

            HeadData headData = new HeadData();
            headData.Name = personData.Name;
            headData.Birthday = personData.Birthday;

            AllPersonMsg.Add(headData, personData);
            headDatas.Add(headData);
        }

        for (int k = 1; k < GuoJiaJiData.rows - 1; k++)
        {
            PersonData personData = new PersonData();
            personData.Type = GuoJiaJiData.dataSet.Tables[0].Rows[k][0].ToString();
            personData.Name = GuoJiaJiData.dataSet.Tables[0].Rows[k][1].ToString();
            personData.Sex = "(" + GuoJiaJiData.dataSet.Tables[0].Rows[k][2].ToString() + ")";
            personData.Birthday = Replace(GuoJiaJiData.dataSet.Tables[0].Rows[k][3].ToString());
            personData.Msg = Sentence(personData.Birthday, GuoJiaJiData.dataSet.Tables[0].Rows[k][5].ToString(),
                GuoJiaJiData.dataSet.Tables[0].Rows[k][4].ToString(), GuoJiaJiData.dataSet.Tables[0].Rows[k][8].ToString(), GuoJiaJiData.dataSet.Tables[0].Rows[k][7].ToString());

            HeadData headData = new HeadData();
            headData.Name = personData.Name;
            headData.Birthday = personData.Birthday;

            AllPersonMsg.Add(headData, personData);
            headDatas.Add(headData);
        }

        PicPath = FileHandle.Instance.GetFolderPath(Application.streamingAssetsPath + "/Picture");
        CheckPicPath();

        VideoPath = FileHandle.Instance.GetFolderPath(Application.streamingAssetsPath + "/Video");
        CheckVideoPath();
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

    /// <summary>
    /// 随机取值
    /// </summary>
    /// <returns></returns>
    public PersonData GetPersonMsg()
    {
        int num = UnityEngine.Random.Range(0, headDatas.Count - 1);
        WaitPanel.Instance.CurrentListName.Add(headDatas[num]);
        if (AllPersonMsg.ContainsKey(headDatas[num]))
        {
            return AllPersonMsg[headDatas[num]];
        }
        else
        {
            return null;
        }
    }

    private bool JudeRepeat(HeadData Name)
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

    /// <summary>
    /// 排查路径，空文件夹就去掉了
    /// </summary>
    private void CheckPicPath()
    {
        List<string> vs = new List<string>();
        if (PicPath.Count > 0)
        {
            foreach (var item in PicPath.Keys)
            {
                int count = FileHandle.Instance.GetImagePath(PicPath[item]).Count;
                if (count <= 0)
                {
                    vs.Add(item);
                }
            }
        }

        if (vs.Count > 0)
        {
            for (int i = 0; i < vs.Count; i++)
            {
                PicPath.Remove(vs[i]);
            }
        }
        GetAllPicture();
    }

    /// <summary>
    /// 获取所有图片
    /// </summary>
    private void GetAllPicture()
    {
        if(PicPath.Count > 0)
        {
            foreach (var item in PicPath.Keys)
            {
                List<string> vs = new List<string>();
                List<Texture2D> ds = new List<Texture2D>();
                vs = FileHandle.Instance.GetImagePath(PicPath[item]);
                for (int i = 0; i < vs.Count; i++)
                {
                    ds.Add(FileHandle.Instance.LoadByIO(vs[i]));
                }
                PicGroup.Add(item, ds);
            }
        }
    }

    /// <summary>
    /// 排查路径，空文件夹就去掉了
    /// </summary>
    private void CheckVideoPath()
    {
        List<string> vs = new List<string>();
        if (VideoPath.Count > 0)
        {
            foreach (var item in VideoPath.Keys)
            {
                int count = FileHandle.Instance.GetVideoPath(VideoPath[item]).Count;
                if (count <= 0)
                {
                    vs.Add(item);
                }
            }
        }

        if(vs.Count > 0)
        {
            for (int i = 0; i < vs.Count; i++)
            {
                VideoPath.Remove(vs[i]);
            }
        }

        GetAllVideoPath();
    }

    /// <summary>
    /// 获取所有的视频路径
    /// </summary>
    private void GetAllVideoPath()
    {
        if (VideoPath.Count > 0)
        {
            foreach (var item in VideoPath.Keys)
            {
                List<string> ds = new List<string>();
                ds = FileHandle.Instance.GetVideoPath(VideoPath[item]);
                VideoGroup.Add(item, ds);
            }
        }
    }
}
