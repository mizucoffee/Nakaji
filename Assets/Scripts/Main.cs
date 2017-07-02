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

    List<NotesModel> notes = new List<NotesModel>();

    Dictionary<int, float> bpmlist = new Dictionary<int, float>(); // int 定義番号, float BPM
    Dictionary<int, float> bpms = new Dictionary<int, float>(); // int 小節番号, float BPM
    Dictionary<int, float> beats = new Dictionary<int, float>(); // int 小節番号, float Beats

    Dictionary<string, string> meta = new Dictionary<string, string>();
    Dictionary<string, List<SlideModel>> slideTmp = new Dictionary<string, List<SlideModel>>(); // int 小節番号, float Beats

    private void Start()
    {
        TextAsset textAsset = Resources.Load("umiyurikaiteitan-expert") as TextAsset;
        
        foreach (string s in textAsset.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (!s.StartsWith("#")) continue;
            string line = s.Substring(1);

            if (!char.IsNumber(line.ToCharArray()[0]))
            {
                if (line.StartsWith("BPM"))
                {
                    bpmlist.Add(int.Parse(line.Split(':')[0].Substring(4)), float.Parse(line.Split(':')[1]));
                    continue;
                }
                meta.Add(line.Split(' ')[0], line.Split(' ')[1]);
                continue;
            }
            else
            {
                string info = line.Split(':')[0];
                string data = line.Split(':')[1];

                int measure = int.Parse(info.Substring(0, 3));
                int type = int.Parse(info.Substring(3, 1));

                if(type == 0) // Meta
                {
                    int t = int.Parse(info.Substring(4, 1));
                    if (t == 2) // 拍子指定
                    {
                        beats.Add(measure,float.Parse(data));
                        continue;
                    }
                    if (t == 8) // BPM呼び出し
                    {
                        bpms.Add(measure,bpmlist[int.Parse(data)]);
                        continue;
                    }
                }

                int start = int.Parse(info.Substring(4, 1), System.Globalization.NumberStyles.HexNumber);
                int split = data.Length / 2;

                int pos = 0;
                foreach(string n in data.SubstringAtCount(2))
                {
                    if (n.ToCharArray()[1] == '0') {
                        pos++;
                        continue;
                    }

                    switch (type)
                    {
                        case 1: // 地を這うショートノーツ用レーン
                            if (n.ToCharArray()[0] == '1')
                                notes.Add(new NotesModel(start, int.Parse(n.ToCharArray()[1].ToString()),0,measure,pos,split));
                            if (n.ToCharArray()[0] == '2')
                                switch (n.ToCharArray()[0])
                                {
                                    case '1':
                                        notes.Add(new NotesModel(start, int.Parse(n.ToCharArray()[1].ToString()), 0, measure, pos, split));
                                        break;
                                    case '2':
                                        notes.Add(new NotesModel(start, int.Parse(n.ToCharArray()[1].ToString()), 1, measure, pos, split));
                                        break;
                                }

                            break;
                        case 2: // Hold用レーン
                            List<SlideModel> l = slideTmp[info.Substring(5, 1)];
                            if (l == null) l = new List<SlideModel>();

                            switch (n.ToCharArray()[0])
                            {
                                case '1':
                                case '3':
                                    l.Add(new SlideModel(start, int.Parse(n.ToCharArray()[1].ToString()),2,measure,pos,split));
                                    break;// 上から順番に処理をしていくため多分後続のレーンが処理されない
                                case '2':
                                    // nmに変換してnullにする
                                    break;
                            }
                            break;
                        case 3: // Slide用レーン
                                //string identifier = info.Substring(6, 1);

                            break;
                        case 4: // Air-Action用レーン

                            break;
                        case 5: // Airノーツ用レーン

                            break;
                        case 0: // 虚無(定義系)

                            break;
                    }
                    pos++;
                }
                
            }

            
            //if (s.StartsWith("#")) continue;
            //if (s.StartsWith(":BPM=")) bpm = int.Parse(s.Substring(5));
            //if (s.StartsWith(":SPLIT=")) split = int.Parse(s.Substring(7));
            //if (s.StartsWith(":")) continue;



            //string[] sp = s.Split(',');

            //int pos = int.Parse(sp[0]);
            //string type = sp[1];
            //int x_1 = int.Parse(sp[2]) - 1;
            //int x_2 = int.Parse(sp[3]) - 1;

            //if (type == "T")
            //    list[pos].Add(new NotesModel(x_1, x_2, 0, bpm,split));
            //if (type == "E")
            //    list[pos].Add(new NotesModel(x_1, x_2, 1, bpm, split));
            //if (type == "H")
            //    list[pos].Add(new NotesModel(x_1, x_2, 2, bpm, split, int.Parse(sp[4])));
            //if (type == "S")
            //{
            //    List<SlideModel> smList = new List<SlideModel>();
            //    foreach(String ss in sp[4].Split(':'))
            //    {
            //        smList.Add(new SlideModel(
            //            int.Parse(ss.Split('-')[1]), 
            //            int.Parse(ss.Split('-')[2]), 
            //            int.Parse(ss.Split('-')[3]), 
            //            ss.Split('-')[0]));
            //    }

            //    list[pos].Add(new NotesModel(x_1, x_2, 3, bpm, split, smList));
            //}
            //line++;
        }

        foreach (NotesModel nm in notes)
        {
            Vector3 v = new Vector3(0, 0.01f, 30.5f + (nm.measure * 4 * 192 + 4 * 192f / nm.split * nm.position) * speed / 48f);
            Debug.Log("Measure:" + nm.measure + " Split:" + nm.split + " Position:" + nm.position);
            // posが仕事してない
            if (nm.type == 0)
            {
                v.y += 0.01f;
                GameObject n = Instantiate(tapPrefub, v, transform.rotation);
                Notes r = n.AddComponent<Notes>();
                r.Create(nm, speed);
            }
            else if (nm.type == 1)
            {
                v.y += 0.01f;
                GameObject n = Instantiate(extapPrefub, v, transform.rotation);
                Notes r = n.AddComponent<Notes>();
                r.Create(nm, speed);
            }
            else
            {
                GameObject n = Instantiate(notesPrefub, v, transform.rotation);
                Notes r = n.GetComponent<Notes>();
                r.Create(nm, speed);
            }


        }

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