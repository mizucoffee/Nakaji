using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public GameObject notesPrefub;
    public GameObject tapPrefub;
    public GameObject extapPrefub;
    public TextAsset textAsset;

    public int speed;

    int measureNum = 0;

    List<NotesModel> notes = new List<NotesModel>();

    List<LineModel> lines = new List<LineModel>();

    Dictionary<int, float> bpmlist = new Dictionary<int, float>(); // int 定義番号, float BPM
    Dictionary<int, float> bpms = new Dictionary<int, float>(); // int 小節番号, float BPM
    Dictionary<int, float> beats = new Dictionary<int, float>(); // int 小節番号, float Beats

    Dictionary<string, string> meta = new Dictionary<string, string>();

    private void Start()
    {
        new SusAnalyzer("white");
        //TextAsset textAsset = Resources.Load("umiyurikaiteitan-expert") as TextAsset;
        
        //foreach (string s in textAsset.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
        //{
        //    if (!s.StartsWith("#")) continue;
        //    string line = s.Substring(1);

        //    if (!char.IsNumber(line.ToCharArray()[0]))
        //    {
        //        if (line.StartsWith("BPM"))
        //        {
        //            bpmlist.Add(int.Parse(line.Split(':')[0].Substring(4)), float.Parse(line.Split(':')[1]));
        //            continue;
        //        }
        //        meta.Add(line.Split(' ')[0], line.Split(' ')[1]);
        //        continue;
        //    }
        //    else
        //    {
        //        string info = line.Split(':')[0];
        //        string data = line.Split(':')[1];

        //        int measure = int.Parse(info.Substring(0, 3));
        //        int type = int.Parse(info.Substring(3, 1));

        //        if(type == 0) // Meta
        //        {
        //            int t = int.Parse(info.Substring(4, 1));
        //            if (t == 2) // 拍子指定
        //            {
        //                beats.Add(measure,float.Parse(data));
        //                continue;
        //            }
        //            if (t == 8) // BPM呼び出し
        //            {
        //                bpms.Add(measure,bpmlist[int.Parse(data)]);
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            int start = int.Parse(info.Substring(4, 1), System.Globalization.NumberStyles.HexNumber);
        //            if (measureNum < measure) measureNum = measure; 
        //            if (type == 2 || type == 3 || type == 4)
        //                lines.Add(new LineModel(measure, type, start, info.Substring(5, 1), data));
        //            else
        //                lines.Add(new LineModel(measure, type, start, data));
        //        }
        //    }
        //}

        

        //for(int i = 0;i < measureNum;i++)
        //{

        //    Dictionary<string, List<SlideModel>> slideTmp = new Dictionary<string, List<SlideModel>>(); // string ID, SlideModel Data
        //    List<string> slideFinished = new List<string>();

        //    foreach (LineModel lm in lines)
        //        if (lm.measure == i)
        //        {
        //            Debug.Log(lm.measure);
        //            int split = lm.data.Length / 2;

        //            int pos = 0;
        //            Debug.Log(lm.data);
        //            foreach (string n in lm.data.SubstringAtCount(2))
        //            {
        //                if (n.ToCharArray()[1] == '0')
        //                {
        //                    pos++;
        //                    continue;
        //                }

        //                switch (lm.type)
        //                {
        //                    case 1: // 地を這うショートノーツ用レーン
        //                        if (n.ToCharArray()[0] == '1')
        //                            notes.Add(new NotesModel(lm.start, n.ToCharArray()[1].ToString(), 0, lm.measure, pos, split));
        //                        if (n.ToCharArray()[0] == '2')
        //                            switch (n.ToCharArray()[0])
        //                            {
        //                                case '1':
        //                                    notes.Add(new NotesModel(lm.start, n.ToCharArray()[1].ToString(), 0, lm.measure, pos, split));
        //                                    break;
        //                                case '2':
        //                                    notes.Add(new NotesModel(lm.start, n.ToCharArray()[1].ToString(), 1, lm.measure, pos, split));
        //                                    break;
        //                            }

        //                        break;
        //                    case 2: // Hold用レーン
        //                        if (!slideTmp.ContainsKey(lm.id))
        //                            slideTmp[lm.id] = new List<SlideModel>();

        //                        switch (n.ToCharArray()[0])
        //                        {
        //                            case '1':
        //                            case '3':
        //                                slideTmp[lm.id].Add(new SlideModel(lm.start, n.ToCharArray()[1].ToString(), 2, lm.measure, pos, split));
        //                                break;
        //                            case '2':
        //                                // nmに変換してnullにする
        //                                slideFinished.Add(lm.id);
        //                                slideTmp[lm.id].Add(new SlideModel(lm.start, n.ToCharArray()[1].ToString(), 2, lm.measure, pos, split));
        //                                break;
        //                        }
        //                        break;
        //                    case 3: // Slide用レーン
        //                            //string identifier = info.Substring(6, 1);

        //                        break;
        //                    case 4: // Air-Action用レーン

        //                        break;
        //                    case 5: // Airノーツ用レーン

        //                        break;
        //                    case 0: // 虚無(定義系)

        //                        break;
        //                }
        //                pos++;
        //            }
        //        }
        //    foreach(string s in slideFinished)
        //    {
        //        notes.Add(new NotesModel(slideTmp[s][0].start, slideTmp[s][0].width, slideTmp[s][0].type, slideTmp[s][0].measure, slideTmp[s][0].position, slideTmp[s][0].split, slideTmp[s]));
        //        slideTmp[s] = null;
        //    }
            
        //}

        //foreach (NotesModel nm in notes)
        //{
        //    Vector3 v = new Vector3(0, 0.01f, 30.5f + (nm.measure* 4 * 192 + 4 * 192f / nm.split * nm.position) * speed / 48f);
        //    Debug.Log("Measure:" + nm.measure + " Split:" + nm.split + " Position:" + nm.position + " Type:" + nm.type);
        //    if (nm.type == 0)
        //    {
        //        v.y += 0.01f;
        //        GameObject n = Instantiate(tapPrefub, v, transform.rotation);
        //        Notes r = n.AddComponent<Notes>();
        //        r.Create(nm, speed);
        //    }
        //    else if (nm.type == 1)
        //    {
        //        v.y += 0.01f;
        //        GameObject n = Instantiate(extapPrefub, v, transform.rotation);
        //        Notes r = n.AddComponent<Notes>();
        //        r.Create(nm, speed);
        //    }
        //    else
        //    {
        //        GameObject n = Instantiate(notesPrefub, v, transform.rotation);
        //        Notes r = n.GetComponent<Notes>();
        //        r.Create(nm, speed);
        //    }


        //}

    }

    void Update()
    {
    }
}

public static class StringExtensions
{
    public static string[] SubstringAtCount(this string self, int count)
    {
        var result = new List<string>();
        var length = (int)Math.Ceiling((double)self.Length / count);

        for (int i = 0; i < length; i++)
        {
            int start = count * i;
            if (self.Length <= start)
            {
                break;
            }
            if (self.Length < start + count)
            {
                result.Add(self.Substring(start));
            }
            else
            {
                result.Add(self.Substring(start, count));
            }
        }

        return result.ToArray();
    }
}