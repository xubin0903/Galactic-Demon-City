using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class FileDataHandler 
{
   private string filePath;
   private string fileName;
    public FileDataHandler(string filePath, string fileName)
    {
        this.filePath = filePath;
        this.fileName = fileName;
    }
    public void SaveData(GameData _data)
    {
        string fullPath = Path.Combine(filePath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));//创建文件夹如果文件夹存在保持现状
            string dataToStore = JsonUtility.ToJson(_data, true);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))//创建文件如果文件存在先删除再创建
            {

                using (StreamWriter writer = new StreamWriter(fileStream))//在刚刚创建的文件中写入数据
                {
                    writer.Write(dataToStore);
                }
            }
        }
        
        catch (Exception e)
        {
            Debug.LogError("Error while saving data: "+e.Message);
        }
    }
    public GameData LoadData()
    {
        string fullPath = Path.Combine(filePath, fileName);
        GameData loadData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    
                    }
                    
                }
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading data: " + e.Message);
            }
        }

        return loadData;
    }
    public void DeleteData()
    {
        string fullPath = Path.Combine(filePath, fileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
   
}
