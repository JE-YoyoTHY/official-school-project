using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System;

public class SubtitleManager
{
    private string _currentLanguage = "chinese";  // �]�w���嬰�w�]�y��
    private string[] _availableLanguages = {"chinese", "english"};  // �i�λy��
    private Dictionary<string, string> _language_path_pairs = new Dictionary<string, string>
    {
        { "chinese", Application.dataPath + "/Resources/JsonFiles/ch_subtitle.json"},
        { "english", Application.dataPath + "/Resources/JsonFiles/en_subtitle.json"},
    };
    string str_json = string.Empty;  // ���NJSON��r�ɹw�]���šA�æb�禡��Ū��

    //-----------------------------------------------------------------------------\\

    public Subtitle getSubtitleById(string id)
    {
        
        using (StreamReader read = new StreamReader(_language_path_pairs[_currentLanguage]))
        {
            // �̷ӥثe�y��Ū��JSON��
            str_json = read.ReadToEnd();
        }
        // �NJSON��r���ഫ��JSON��
        Dictionary<string, Subtitle> subtitleDict = JsonConvert.DeserializeObject<Dictionary<string, Subtitle>>(str_json);

        // ���o��Ӧr�����t���A���o���U���r����(��ȹ�A��: id, ��: class Subtitle)�A�A�Q��id���o�Ӧr����
        var subtitle = subtitleDict[id];
        


        return subtitle;
    }

    public void changeCurrentLanguage(string lan)
    {
        if (_availableLanguages.Contains(lan))
        {
            _currentLanguage = lan;
        }
        else
        {
           Console.WriteLine("[changeCurrentLanguage]: Not available.");
        }
    }
    public string getCurrentLanguage()
    {
        return _currentLanguage;
    }
    public string[] getAvailableLanguages()
    {
        return _availableLanguages;
    }

    
}
