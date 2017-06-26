
using System.Collections.Generic;
using UnityEngine;

public class FileLoader{

    private string data;

    private string songname;
    private string artist;
    private int bpm;
    private int maxrhythm;

    private int start = 0;
    private int end = 0;
    private int notes = 0;

    private List<List<NotesModel>> list;

    public FileLoader(string text)
    {
        list = new List<List<NotesModel>>();
        data = text;
        string[] split = data.Split('\n');
        // この辺にちゃんとしたチェック挟む

        for (int i = 0; i < split.Length; ++i)
        {
            if (split[i] == ":START")
            {
                start = i + 1;
            }
            if (split[i] == ":END")
            {
                end = i;
            }
            if (split[i].StartsWith(":SONGNAME="))
            {
                songname = split[i].Substring(10);
            }
            if (split[i].StartsWith(":ARTIST="))
            {
                artist = split[i].Substring(8);
            }
            if (split[i].StartsWith(":BPM="))
            {
                // 数字チェック
                bpm = int.Parse(split[i].Substring(5));
            }
            if (split[i].StartsWith(":MAXRHYTHM="))
            {
                // 数字チェック
                maxrhythm = int.Parse(split[i].Substring(11));
            }
        }
        notes = end - start + 1;

        int rhythm = 0;


        int now = start;
        int count = 0;
        while (now < end)
        {
            list.Add(new List<NotesModel>());
            string target = split[now];
            string tmp = "";
            int size = 0;
            int pos = 0;
            
            for (int i = 0; i < target.Length; ++i)
            {
                switch (tmp)
                {
                    case "":
                        switch (target.ToCharArray()[i])
                        {
                            case '-':
                                continue;
                            case 'R':
                                tmp = "R";
                                size = 1;
                                pos = i;
                                break;
                            case 'Y':
                                tmp = "Y";
                                size = 1;
                                pos = i;
                                break;
                            case 'L':
                                tmp = "L";
                                size = 1;
                                pos = i;
                                break;
                            case 'S':
                                tmp = "S";
                                pos = i;
                                break;
                            case ':':
                                if (target.StartsWith(":RHYTHM="))
                                {
                                    rhythm = int.Parse(target.Substring(8));
                                    i = target.Length;
                                    continue;
                                }
                                break;
                        }
                        break;
                    case "R":
                        switch (target.ToCharArray()[i])
                        {
                            case '2':
                            case 'R':
                                size++;
                                //list[count].Add(new NotesModel(size, pos,0,bpm));
                                size = 0;
                                tmp = "";
                                break;
                            case '=':
                                size++;
                                break;
                            default:
                                //list[count].Add(new NotesModel(size, pos, 0, bpm));
                                size = 0;
                                tmp = "";
                                break;
                        }
                        break;
                    case "Y":
                        switch (target.ToCharArray()[i])
                        {
                            case '2':
                            case 'Y':
                                size++;
                                //list[count].Add(new NotesModel(size, pos, 1, bpm));
                                size = 0;
                                tmp = "";
                                break;
                            case '=':
                                size++;
                                break;
                            default:
                                //list[count].Add(new NotesModel(size, pos, 1, bpm));
                                size = 0;
                                tmp = "";
                                break;
                        }
                        break;
                    case "L":
                        switch (target.ToCharArray()[i])
                        {
                            case '2':
                            case 'L':
                                size++;
                                int hold = 1;
                                for(int j = now + 1; j < end; j++)
                                {
                                    if (split[j].ToCharArray()[pos] == 'H' ||
                                        split[j].ToCharArray()[i] == 'H')
                                    {
                                        hold++;
                                        continue;
                                    }
                                    if (split[j].ToCharArray()[pos] == 'N' ||
                                       split[j].ToCharArray()[i] == 'N')
                                    {
                                        break;
                                    }
                                }
                                
                                hold = hold * (maxrhythm / rhythm);

                                //NotesModel nm = new NotesModel(size, pos, 2, bpm);
                                //nm.hold = hold;
                               // list[count].Add(nm);
                                size = 0;
                                tmp = "";
                                break;
                            case '=':
                                size++;
                                break;
                            default:
                                //list[count].Add(new NotesModel(size, pos, 0, bpm));
                                size = 0;
                                tmp = "";
                                break;
                        }
                        break;
                    case "S":
                        switch (target.ToCharArray()[i])
                        {
                            case '2':
                            case 'S':
                                List <int[]> intArray = new List<int[]>();
                                int[] l = {pos,i,0};
                                intArray.Add(l);

                                int line = 0;

                                bool finished = false;
                                for (int j = now + 1; j < end; j++)
                                {
                                    line++;

                                    bool breakFlag = false;
                                    bool clapFlag = false;
                                    bool endFlag = false;
                                    int start = 0;
                                    for (int k = 0; k < split[j].Length; ++k)
                                    {
                                        if (breakFlag)
                                        {
                                            if (split[j].ToCharArray()[k] == 'B' || split[j].ToCharArray()[k] == '2')
                                            {
                                                int[] ll = { start, k ,line};
                                                intArray.Add(ll);
                                            }
                                        }
                                        else if (clapFlag)
                                        {
                                            if (split[j].ToCharArray()[k] == 'C' || split[j].ToCharArray()[k] == '2')
                                            {
                                                int[] ll = { start, k, line };
                                                intArray.Add(ll);
                                                finished = true;
                                                break;
                                            }
                                        }
                                        else if (endFlag)
                                        {
                                            if (split[j].ToCharArray()[k] == 'E' || split[j].ToCharArray()[k] == '2')
                                            {
                                                int[] ll = { start, k, line };
                                                intArray.Add(ll);
                                                finished = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (split[j].ToCharArray()[k] == 'B')
                                            {
                                                breakFlag = true;
                                                start = k;
                                            }
                                            if (split[j].ToCharArray()[k] == 'C')
                                            {
                                                clapFlag = true;
                                                start = k;
                                            }
                                            if (split[j].ToCharArray()[k] == 'E')
                                            {
                                                endFlag = true;
                                                start = k;
                                            }
                                        }
                                    }
                                    if (finished) { break; }
                                        
                                }
                                
                                NotesModel nm = new NotesModel(bpm);
                               // nm.slide = intArray;
                                list[count].Add(nm);
                                tmp = "";
                                break;
                        }
                        break;
                }
            }
            for (int i = 0; i < maxrhythm / rhythm - 1; i++)
            {
                if (target.StartsWith(":")) break;
                Debug.Log(target);
                list.Add(new List<NotesModel>());
                count++;
            }
            now++;
            count++;
        }
    }
    public List<List<NotesModel>> getList()
    {
        return list;
    }
}
