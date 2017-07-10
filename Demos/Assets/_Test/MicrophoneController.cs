using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UFramework.Design;

/// <summary>
/// 录音
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MicrophoneController : Singleton<MicrophoneController>
{
    private const int RECORD_TIME = 60; // 录制时间上限

    private int sampleCount = 0; // 采样数
    private float recordTime = 0; // 录音时长

    // 得到当前音量
    public float volume
    {
        get
        {
            if (Microphone.IsRecording(null))
            {
                int sampleSize = 128;
                float[] samples = new float[sampleSize];
                int startPosition = Microphone.GetPosition(null) - (sampleSize + 1);
                this.GetComponent<AudioSource>().clip.GetData(samples, startPosition);

                // Getting a peak on the last 128 samples
                float levelMax = 0;
                for (int i = 0; i < sampleSize; ++i)
                {
                    float wavePeak = samples[i];
                    if (levelMax < wavePeak)
                        levelMax = wavePeak;
                }

                return levelMax*99;
            }
            return 0;
        }
    }

    /// <summary>
    /// 开始录音
    /// </summary>
    public void StartRecord()
    {
        if (Microphone.devices.Length == 0)
            return;
        this.GetComponent<AudioSource>().Stop();
        this.GetComponent<AudioSource>().loop = false;
        this.GetComponent<AudioSource>().mute = true;

        this.recordTime = Time.time;
        this.GetComponent<AudioSource>().clip = Microphone.Start(null, false, RECORD_TIME, 44100);

        // Wait until the recording has started
        while (!(Microphone.GetPosition(null) > 0))
        {
        }

        this.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// 停止录音
    /// </summary>
    public void StopRecord(string recordFileName)
    {
        if (Microphone.devices.Length == 0 || !Microphone.IsRecording(null))
            return;

        this.recordTime = Time.time - this.recordTime;
        this.recordTime = this.recordTime > RECORD_TIME ? RECORD_TIME : this.recordTime;

        Microphone.End(null);
        this.GetComponent<AudioSource>().Stop();
        this.GetComponent<AudioSource>().mute = false;

        SaveToBinaryFile(recordFileName, this.GetComponent<AudioSource>().clip);
    }

    /// <summary>
    /// 将 Clip 保存成二进制文件
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="clip"></param>
    private void SaveToBinaryFile(string recordFileName, AudioClip clip)
    {
        var filepath = Path.Combine(Application.persistentDataPath, recordFileName);

        // Make sure directory exists if user is saving to sub dir.
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        using (var fileStream = new FileStream(filepath, FileMode.Create))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ConvertAndWrite(ms, clip);
                ms.WriteTo(fileStream);
            }
        }
    }

    /// <summary>
    /// 将录制的数据存入文件
    /// </summary>
    /// <param name="memStream"></param>
    /// <param name="clip"></param>
    private void ConvertAndWrite(MemoryStream memStream, AudioClip clip)
    {
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        this.sampleCount = (int) (clip.samples*recordTime/RECORD_TIME);
        Array.Copy(samples, samples, this.sampleCount);

        //Debug.Log("sample count " + this.sampleCount);

        Int16[] intData = new Int16[this.sampleCount];
        Byte[] bytesData = new Byte[this.sampleCount*2];

        int rescaleFactor = 32767; //to convert float to Int16

        for (int i = 0; i < this.sampleCount; i++)
        {
            intData[i] = (short) (samples[i]*rescaleFactor);
        }
        Buffer.BlockCopy(intData, 0, bytesData, 0, bytesData.Length);
        memStream.Write(bytesData, 0, bytesData.Length);
        //Debug.Log("write in " + bytesData.Length);

        Destroy(this.GetComponent<AudioSource>().clip);
    }

    /// <summary>
    /// 从文件读取并播放
    /// </summary>
    /// <param name="fileName"></param>
    public AudioClip GetClipFromRecordFile(string fileName)
    {
        var filepath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filepath))
        {
            Debug.Log("haven't find file: " + filepath);
            return null;
        }

        byte[] bytesData;
        using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
        {
            using (BinaryReader br = new BinaryReader(fileStream))
            {
                bytesData = new byte[fileStream.Length];
                bytesData = br.ReadBytes(bytesData.Length);
            }
        }
        if (bytesData == null)
            return null;

        //Debug.Log("read byte " + bytesData.Length);

        Int16[] intData = new Int16[(bytesData.Length + 1)/2];
        Buffer.BlockCopy(bytesData, 0, intData, 0, bytesData.Length);

        //if (bytesData != null && bytesData.Length > 0)
        //{
        //    for (int i = 0; i < bytesData.Length; i += 2)
        //    {
        //        byte[] tempByte = new byte[2];
        //        tempByte[0] = bytesData[i];
        //        tempByte[1] = bytesData[i + 1];
        //        intData[i / 2] = System.BitConverter.ToInt16(tempByte, 0);
        //    }
        //}

        // 从Int16[] 到 float[]
        float[] samples = new float[intData.Length];
        for (int i = 0; i < intData.Length; ++i)
            samples[i] = (float) intData[i]/32767;

        AudioClip clip = AudioClip.Create(fileName, intData.Length, 1, 44100, false, false);
        clip.SetData(samples, 0);
        return clip;
        //Debug.Log("samples " + clip.samples);
        //return clip;
    }


    /// <summary>
    /// 得到合适的频率
    /// </summary>
    /// <returns></returns>
    private int GetAppropriateFrequency()
    {
        if (Microphone.devices.Length == 0)
            return 0;

        // 选择麦克风可支持的频率
        int minHZ, maxHZ, hz;
        Microphone.GetDeviceCaps(null, out minHZ, out maxHZ);
        if ((minHZ == 0 && maxHZ == 0) || maxHZ > 44100)
            hz = 44100;
        else
            hz = maxHZ;
        return hz;
    }
}