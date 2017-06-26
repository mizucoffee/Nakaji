using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideModel{

    public int step = 0;
    public int start = 0;
    public int end = 0;
    public string type = "";

    public SlideModel(int step,int start,int end,string type)
    {
        this.step = step;
        this.start = start;
        this.end = end;
        this.type = type;
    }
}
