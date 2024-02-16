using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Utils
{
	private static string m_SavePath;

	public static int[] StandardScreenSize;

	static Utils()
	{
		StandardScreenSize = new int[2] { 480, 320 };
		string persistentDataPath = Application.persistentDataPath;
		persistentDataPath += "/Documents";
		if (!Directory.Exists(persistentDataPath))
		{
			Directory.CreateDirectory(persistentDataPath);
		}
		m_SavePath = persistentDataPath;
	}

	public static bool CreateDocumentSubDir(string dirname)
	{
		string path = m_SavePath + "/" + dirname;
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
			return true;
		}
		return false;
	}

	public static void DeleteDocumentDir(string dirname)
	{
		string path = m_SavePath + "/" + dirname;
		if (Directory.Exists(path))
		{
			Directory.Delete(path, true);
		}
	}

	public static string SavePath()
	{
		return m_SavePath;
	}

	public static string GetTextAsset(string txt_name)
	{
		TextAsset textAsset = Resources.Load(txt_name) as TextAsset;
		if (null != textAsset)
		{
			return textAsset.text;
		}
		return string.Empty;
	}

	public static void FileSaveString(string name, string content)
	{
		string path = SavePath() + "/" + name;
		try
		{
			FileStream fileStream = new FileStream(path, FileMode.Create);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(content);
			streamWriter.Close();
			fileStream.Close();
		}
		catch
		{
		}
	}

	public static void FileGetString(string name, ref string content)
	{
		string path = SavePath() + "/" + name;
		if (!File.Exists(path))
		{
			return;
		}
		try
		{
			FileStream fileStream = new FileStream(path, FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			content = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
		}
		catch
		{
		}
	}

	public static void DeleteFile(string name)
	{
		if (CheckFileExists(name))
		{
			string path = SavePath() + "/" + name;
			File.Delete(path);
		}
	}

	public static bool CheckFileExists(string name)
	{
		string path = SavePath() + "/" + name;
		if (!File.Exists(path))
		{
			return false;
		}
		return true;
	}

	public static bool IsRetina()
	{
		StandardScreenSize[0] = 960;
		StandardScreenSize[1] = 640;
		return true;
	}

	public static bool ProbabilityIsRandomHit(float rate)
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		float num2 = rate / 2f;
		float num3 = UnityEngine.Random.Range(num2, 1f - num2);
		if (num3 - num2 < num && num < num3 + num2)
		{
			return true;
		}
		return false;
	}

	public static bool IsChineseLetter(string input)
	{
		for (int i = 0; i < input.Length; i++)
		{
			int num = Convert.ToInt32(Convert.ToChar(input.Substring(i, 1)));
			if (num >= 128)
			{
				return true;
			}
		}
		return false;
	}

	public static string PhotoKey()
	{
		return DateTime.Now.ToString("yyyyMMddHHmmssfff", DateTimeFormatInfo.InvariantInfo) + UnityEngine.Random.Range(1, 10000);
	}

	public static void AvataTakeTempPhoto()
	{
		string filename = "TempPhoto.png";
		ScreenCapture.CaptureScreenshot(filename);
	}

	public static void AvataTakePhoto(string photo_key)
	{
		AvataTakePhotoWin32(photo_key);
	}

	public static void AvataTakePhotoWin32(string photo_key)
	{
		try
		{
			Texture2D texture2D = new Texture2D(GameScreen.width, GameScreen.height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0f, 0f, GameScreen.width, GameScreen.height), 0, 0);
			texture2D.Apply();
			byte[] buffer = texture2D.EncodeToPNG();
			string path = SavePath() + "/Avatar/" + photo_key + "_photo.png";
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(buffer);
			binaryWriter.Close();
			fileStream.Close();
			UnityEngine.Object.Destroy(texture2D);
		}
		catch
		{
		}
	}

	private static string UTF8ByteArrayToString(byte[] characters)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(characters);
	}

	private static byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetBytes(pXmlString);
	}

	public static string SerializeObject(object pObject, Type type)
	{
		string empty = string.Empty;
		MemoryStream stream = new MemoryStream();
		XmlSerializer xmlSerializer = new XmlSerializer(type);
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		xmlSerializer.Serialize(xmlTextWriter, pObject);
		stream = (MemoryStream)xmlTextWriter.BaseStream;
		return UTF8ByteArrayToString(stream.ToArray());
	}

	public static object DeserializeObject(string pXmlizedString, Type type)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(type);
		MemoryStream stream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		return xmlSerializer.Deserialize(stream);
	}
}
