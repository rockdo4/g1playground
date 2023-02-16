using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System;

public static class SaveLoadSystem
{
    public enum Modes
    {
        Json,
        Binary
    }
    public enum Types
    {
        Player,
        Option,
    }

    public static readonly string AesKey = "1234567890";
    public const Modes CurrentMode = Modes.Binary;
    public const int CurrentVersion = 1;
    public static string FilePath = string.Empty;
    public static readonly string DirectoryName = "Save";
    public static readonly string FileNameFormat = "{0}/Save_{1}.{2}";
    public static readonly string[] Extension = { "json", "bin" };

    public static string SaveDirectory
    {
        get => Path.Combine(Application.persistentDataPath, DirectoryName);
    }

    public static string GetSaveFileName(Types type, Modes mode = CurrentMode) => string.Format(FileNameFormat, SaveDirectory, type.ToString(), Extension[(int)mode]);

    public static void Save(Types type, SaveData data)
    {
        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        FilePath = GetSaveFileName(type);

        switch (CurrentMode)
        {
            case Modes.Json:
                JsonSave(data);
                break;
            case Modes.Binary:
                BinarySave(data);
                break;
        }
    }

    public static SaveData Load(Types type)
    {
        FilePath = GetSaveFileName(type);

        switch (CurrentMode)
        {
            case Modes.Json:
                return JsonLoad();
            case Modes.Binary:
                return BinaryLoad();
        }
        return null;
    }


    public static void BinarySave(SaveData data)
    {
        using (var ms = new MemoryStream())
        using (BsonDataWriter writer = new BsonDataWriter(ms))
        using (var file = File.Create(FilePath))
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new Vector3Converter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Serialize(writer, data);

            var bytes = ms.ToArray();
            var plain = Convert.ToBase64String(bytes);
            var cypher = Encrypt(plain, AesKey);
            bytes = Convert.FromBase64String(cypher);
            file.Write(bytes, 0, bytes.Length);
        }
    }

    public static SaveData BinaryLoad()
    {
        SaveData data = null;
        var bytes = File.ReadAllBytes(FilePath);
        var cypher = Convert.ToBase64String(bytes);
        var plain = Decrypt(cypher, AesKey);
        bytes = Convert.FromBase64String(plain);
        using (var ms = new MemoryStream(bytes))
        using (BsonDataReader reader = new BsonDataReader(ms))
        {
            var deserializer = new JsonSerializer();
            data = deserializer.Deserialize(reader, typeof(SaveData)) as SaveData;
        }

        var fileVersion = data.Version;

        bytes = File.ReadAllBytes(FilePath);
        cypher = Convert.ToBase64String(bytes);
        plain = Decrypt(cypher, AesKey);
        bytes = Convert.FromBase64String(plain);
        using (var ms = new MemoryStream(bytes))
        using (BsonDataReader reader = new BsonDataReader(ms))
        {
            System.Type t = typeof(SaveData);
            switch (fileVersion)
            {
                case 1:
                    t = typeof(SaveDataVer1);
                    break;
            }

            var deserializer = new JsonSerializer();
            deserializer.Converters.Add(new Vector3Converter());
            deserializer.Converters.Add(new QuaternionConverter());
            data = deserializer.Deserialize(reader, t) as SaveData;
        }

        while (data.Version < CurrentVersion)
        {
            data = data.VersionUp();
        }

        return data;
    }

    public static void JsonSave(SaveData data)
    {
        using (var file = File.CreateText(FilePath))
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new Vector3Converter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Serialize(file, data);
        }


        using (var ms = new MemoryStream())
        using (BsonDataWriter writer = new BsonDataWriter(ms))
        using (var file = File.Create(FilePath))
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new Vector3Converter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Serialize(writer, data);

            var bytes = ms.ToArray();
            var plain = Convert.ToBase64String(bytes);
            var cypher = Encrypt(plain, AesKey);
            bytes = Convert.FromBase64String(cypher);
            file.Write(bytes, 0, bytes.Length);
        }
    }

    public static SaveData JsonLoad()
    {
        SaveData data = null;
        using (var file = File.OpenText(FilePath))
        {
            var deserializer = new JsonSerializer();
            data = deserializer.Deserialize(file, typeof(SaveData)) as SaveData;
        }
        var fileVersion = data.Version;

        using (var file = File.OpenText(FilePath))
        {
            System.Type t = typeof(SaveData);
            switch (fileVersion)
            {
                case 1:
                    t = typeof(SaveDataVer1);
                    break;
            }

            var deserializer = new JsonSerializer();
            deserializer.Converters.Add(new Vector3Converter());
            deserializer.Converters.Add(new QuaternionConverter());
            data = deserializer.Deserialize(file, t) as SaveData;
        }

        while (data.Version < CurrentVersion)
        {
            data = data.VersionUp();
        }

        return data;
    }

    public static string Decrypt(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
            len = keyBytes.Length;
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

        return Encoding.UTF8.GetString(plainText);
    }

    public static string Encrypt(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
            len = keyBytes.Length;
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }
}
