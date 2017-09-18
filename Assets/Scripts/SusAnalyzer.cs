using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusAnalyzer
{
    public SusAnalyzer(string susName)
    {
        TextAsset textAsset = Resources.Load(susName) as TextAsset;
        foreach (string s in textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (s.Length == 0) continue;
            if (!s.StartsWith("#")) continue;
            else if (System.Text.RegularExpressions.Regex.IsMatch(s, @"^#(\w)+(\s+.+)*$"))
            {
                parseMeta(s);
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(s, @"^#\w{3}\w{2,3}:\s*\w+$")) //seaは最後の\wが任意の文字
            {
                Debug.Log("SCORE: " + s);
            }
            else
            {
                Debug.Log("SUS有効行ですが解析できませんでした。");
            }
        }
    }

    private void parseMeta(string meta)
    {
        meta = meta.Substring(1).ToUpper();
        if (meta.StartsWith("BPM"))
        {
            //TODO: 途中変更
            return;
        }
        Debug.Log("META: " + meta);
    }
}
