using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class DataManager : Singleton<DataManager> {
	public SongData[] songData;
	
	public void Load()
	{
		FileStream readFile = File.OpenRead("Save/player.binary");
		BinaryFormatter formatter = new BinaryFormatter();
		songData = formatter.Deserialize(readFile) as SongData[];
		readFile.Close();
	}

	public void Save()
	{
		FileStream saveFile = File.Create("Save/player.binary");
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(saveFile,songData);
		saveFile.Close();
	}
	public void SaveTemplate()
	{
		FileStream saveFile = File.Create("Save/template.binary");
		BinaryFormatter formatter = new BinaryFormatter();
		//formatter.Serialize(saveFile,playerData);
		saveFile.Close();
	}
	public void Reset()
	{
		FileStream readFile = File.OpenRead("Save/template.binary");
		BinaryFormatter formatter = new BinaryFormatter();
		//playerData = formatter.Deserialize(readFile) as PlayerData[];
		readFile.Close();
	}
}
