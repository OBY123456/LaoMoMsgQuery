using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class Person
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }
}

public class Data
{
    public string DataType;
    public string Msg;
}

public enum DataType
{
    Check,
    Change,
    Create,
}

public class UnityWebRequestScript : MonoBehaviour
{
    private string url = "http://localhost:52674/api/Values/";
    private string Key = "value";
    Person person = new Person();
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Get());
        //StartCoroutine(Post(1234567891));
        //StartCoroutine(Put());


        person.Id = 1234567895;
        person.Name = "bbcc";
        person.Age = 18;
        //StartCoroutine(Post(person));

        StartCoroutine(Post_Creat(person));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Post(person));
        }
    }

    IEnumerator Get()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);
        List<Person> people = JsonConvert.DeserializeObject<List<Person>>(request.downloadHandler.text);
        foreach (Person item in people)
        {
            Debug.Log(item.Id);
        }
        
    }

    /// <summary>
    ///  验证账号密码登录
    /// </summary>
    /// <param name="Account">账号</param>
    /// <returns></returns>
    IEnumerator Post(int Account)
    {
        Data data = new Data();
        data.DataType = DataType.Check.ToString();
        data.Msg = Account.ToString();

        WWWForm form = new WWWForm();
        form.AddField(Key, JsonConvert.SerializeObject(data));
        //form.AddField(Key, Account.ToString());
        UnityWebRequest request = UnityWebRequest.Post(url,form );

        yield return request.SendWebRequest();

        if(request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error:" + request.error);
            yield break;
        }

        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);
        Person person = new Person();
        person = JsonConvert.DeserializeObject<Person>(request.downloadHandler.text);
        if (person.Age == 12)
        {
            Debug.Log(true);
        }
        else
        {
            Debug.Log(person.Age);
        }
    }

    IEnumerator Post(string Url,Action<string> Callback)
    {

        WWWForm form = new WWWForm();
        form.AddField(Key, "aaa");
        UnityWebRequest request = UnityWebRequest.Post(Url, form);

        yield return request.SendWebRequest();

        //Debug.Log(request.error);
        //Debug.Log(request.responseCode);
        //Debug.Log(request.downloadHandler.text);

        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error:" + request.error);
            yield break;
        }

        if(Callback!=null)
        {
            Callback(request.downloadHandler.text);
        }
    }

    /// <summary>
    /// 注册账号密码
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    IEnumerator Post(Person person)
    {
        Data data = new Data();
        data.DataType = DataType.Change.ToString();
        data.Msg = JsonConvert.SerializeObject(person);

        WWWForm form = new WWWForm();
        form.AddField(Key, JsonConvert.SerializeObject(data));
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        yield return request.SendWebRequest();
        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);
        if(request.downloadHandler.text.Contains("true"))
        {
            Debug.Log("创建成功！！！");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    IEnumerator Post_Creat(Person person)
    {
        Data data = new Data();
        data.DataType = DataType.Create.ToString();
        data.Msg = JsonConvert.SerializeObject(person);

        WWWForm form = new WWWForm();
        form.AddField(Key, JsonConvert.SerializeObject(data));
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        yield return request.SendWebRequest();
        Debug.Log(request.error);
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerable GetPic(string Url ,Action<String> Callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(Url);
        yield return request.SendWebRequest();

        if(request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }

        if(request.isDone)
        {
            Callback(request.downloadHandler.text);
        }
    }

    private IEnumerable PostPic(string Url,Action<String> Callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("aaa", 123);
        UnityWebRequest request = UnityWebRequest.Post(Url, form);

        yield return request.SendWebRequest();

        if(request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }

        if(request.isDone)
        {
            Callback(request.downloadHandler.text);
        }
    }
}
