using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineModel {
    public int measure;
    public int type;
    public int start;
    public string id;

    public string data;

    public LineModel(int m,int t,int s,string d)
    {
        measure = m;
        type = t;
        start = s;
        data = d;
    }

    public LineModel(int m, int t, int s,string i, string d)
    {
        measure = m;
        type = t;
        start = s;
        id = i;
        data = d;
    }
}
