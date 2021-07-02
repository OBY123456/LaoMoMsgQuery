using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// 文件读取
/// </summary>
public class FileHandle
{
    private FileHandle() { }

    public static readonly FileHandle Instance = new FileHandle();

    /// <summary>
    /// 该路径是否存在
    /// </summary>
    /// <param name="filepath">路径</param>
    /// <returns></returns>
    public bool IsExistFile(string filepath)
    {
        return File.Exists(filepath);
    }
    /// <summary>
    /// 保存文件，默认System.IO.FileMode.Create, System.IO.FileAccess.Write
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="FilePath"></param>
    public void SaveFile(string msg, string FilePath)
    {
        var fss = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        var sws = new System.IO.StreamWriter(fss);
        sws.Write(msg);
        sws.Close();
        fss.Close();
    }
    /// <summary>
    /// 判断文件夹是否存在
    /// </summary>
    /// <param name="Folderpath"></param>
    public void IsExisFolder(string Folderpath)
    {
        if (!System.IO.Directory.Exists(Folderpath))
            System.IO.Directory.CreateDirectory(Folderpath);
    }
    /// <summary>
    ///  读取文件内容
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public string FileToString(string filepath)
    {
        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(fs, Encoding.UTF8);
        try
        {
            return reader.ReadToEnd();
        }
        catch
        {
            return string.Empty;
        }
        finally
        {
            fs.Close();
            reader.Close();
        }
    }
    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filepath"></param>
    public void ClearFile(string filepath)
    {
        File.Delete(filepath);
    }

    /// <summary>
    /// //获取unity根目录下的图片文件夹下的所有文件的路径 路径+ 名称全部存储在字符串数组中
    /// </summary>
    /// <returns></returns>
    public List<string> GetImagePath(string Path)
    {
        List<string> filePaths = new List<string>();
        string imgtype = "*.JPG|*.PNG";
        string[] ImageType = imgtype.Split('|');
        for (int i = 0; i < ImageType.Length; i++)
        {
            
            string[] dirs = Directory.GetFiles(Path, ImageType[i]);
            for (int j = 0; j < dirs.Length; j++)
            {
                filePaths.Add(dirs[j]);
            }
            Debug.Log(ImageType[i] + ":一共读取到" + dirs.Length + "张图片");
        }
        return filePaths;
    }

    /// <summary>
    /// 返回Sprite图片
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="width">图片宽</param>
    /// <param name="height">图片高</param>
    /// <returns></returns>
    public Sprite LoadByIO(string path,int width,int height)
    {
        //创建文件读取流
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        //创建Texture
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(bytes);
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
    }

    /// <summary>
    /// 返回Texture2D图片
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public Texture2D LoadByIO(string path)
    {
        //创建文件读取流
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        //创建Texture
        Texture2D texture2D = new Texture2D(0, 0);
        texture2D.LoadImage(bytes);
        texture2D.Apply();
        return texture2D;
    }
}