using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class ServerImageLoad : MonoBehaviour
{
    public RawImage img;


    void Start()
    {
        StartCoroutine(TextureLoad());
    }

    IEnumerator TextureLoad()
    {
        string url = "https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdna%2FbnejLS%2FbtsPJ28POVs%2FAAAAAAAAAAAAAAAAAAAAAP8Wr4-kuEP3Qbfz057-DDTgl3LdELW0lJrv38GEF0Sv%2Fimg.png%3Fcredential%3DyqXZFxpELC7KVnFOS48ylbz2pIh7yKj8%26expires%3D1756652399%26allow_ip%3D%26allow_referer%3D%26signature%3DTCfE1iEld2NcUCo9L6IoEwcXPM0%253D";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.LogError(www.error);
        else
        {
            Texture2D tex = DownloadHandlerTexture.GetContent(www);
            img.texture = tex;
        }            
    }
}
