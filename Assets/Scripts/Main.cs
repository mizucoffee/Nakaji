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
    
    List<List<NotesModel>> list;
    int notes = 0;

    int bpm = 0;
    int split = 0;

    private void Start()
    {
        list = new List<List<NotesModel>>();
        TextAsset textAsset = Resources.Load("umiyurikaiteitan-expert") as TextAsset;

        string body = "";
        int line = 0;

        for(int i = 0; i < 100; i++)
            list.Add(new List<NotesModel>());


        foreach (string s in textAsset.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
        {                   
            if (s.StartsWith("#")) continue;
            if (s.StartsWith(":BPM=")) bpm = int.Parse(s.Substring(5));
            if (s.StartsWith(":SPLIT=")) split = int.Parse(s.Substring(7));
            if (s.StartsWith(":")) continue;

            

            string[] sp = s.Split(',');

            int pos = int.Parse(sp[0]);
            string type = sp[1];
            int x_1 = int.Parse(sp[2]) - 1;
            int x_2 = int.Parse(sp[3]) - 1;

            if (type == "T")
                list[pos].Add(new NotesModel(x_1, x_2, 0, bpm,split));
            if (type == "E")
                list[pos].Add(new NotesModel(x_1, x_2, 1, bpm, split));
            if (type == "H")
                list[pos].Add(new NotesModel(x_1, x_2, 2, bpm, split, int.Parse(sp[4])));
            if (type == "S")
            {
                List<SlideModel> smList = new List<SlideModel>();
                foreach(String ss in sp[4].Split(':'))
                {
                    smList.Add(new SlideModel(
                        int.Parse(ss.Split('-')[1]), 
                        int.Parse(ss.Split('-')[2]), 
                        int.Parse(ss.Split('-')[3]), 
                        ss.Split('-')[0]));
                }
                
                list[pos].Add(new NotesModel(x_1, x_2, 3, bpm, split, smList));
            }
            line++;
        }

        foreach (List<NotesModel> lnm in list)
        {
            foreach (NotesModel nm in lnm)
            {
                Vector3 v = new Vector3(0, 0.01f, 30.5f + notes * speed * 1.5f);

                if(nm.type == 0)
                {
                    v.y += 0.01f;
                    GameObject n = Instantiate(tapPrefub, v, transform.rotation);
                    Notes r = n.AddComponent<Notes>();
                    r.Create(nm, speed);
                }else if (nm.type == 1)
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
            notes++;
        }

    }

    void Update()
    {
    }
}