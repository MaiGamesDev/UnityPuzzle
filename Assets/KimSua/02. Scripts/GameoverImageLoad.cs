using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameoverImageLoad : MonoBehaviour
{
    public RawImage img;

    void OnEnable()
    {
        StartCoroutine(TextureLoad());
    }

    IEnumerator TextureLoad()
    {
        string url = "https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdna%2FbJnJTa%2FbtsPMDoc9ag%2FAAAAAAAAAAAAAAAAAAAAAN6THsyTYZdWEqHHIZsn9RE1LyqE221F608S3aWkV5te%2Fimg.png%3Fcredential%3DyqXZFxpELC7KVnFOS48ylbz2pIh7yKj8%26expires%3D1756652399%26allow_ip%3D%26allow_referer%3D%26signature%3DjmJCiq8WyBs5G%252BN96ecIxVoe0vI%253D";
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
