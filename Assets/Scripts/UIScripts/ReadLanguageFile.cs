using System.IO;
using UnityEngine;

public class ReadLanguageFile : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    public static string ReadText(string property, string language)
    {
        string text = "", line;
        bool found = false;
        StreamReader read = new StreamReader(Application.streamingAssetsPath + "/Languages/" + language + ".txt", System.Text.Encoding.UTF8);
        while((line = read.ReadLine()) != null && !found)
        {
            if (line.Contains(property + "="))
            {
                text = line.Substring(line.IndexOf('=') + 1);
            }
        }
        read.Close();
        return text;
    }
}
