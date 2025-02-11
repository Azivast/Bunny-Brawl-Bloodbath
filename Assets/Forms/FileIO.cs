using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public static class FileIO
{
    public static void SaveData(string data, string filename)
    {
        string destination = Application.persistentDataPath + filename + ".txt";
        FileStream stream = null;
        try
        {
            // Create a FileStream with mode CreateNew
            stream = new FileStream(destination, FileMode.OpenOrCreate);
            // Create a StreamWriter from FileStream
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8 ))
            {
                writer.WriteLine(data);
            }
        }
        finally
        {
            if (stream != null) stream.Dispose();
        }
    }
}