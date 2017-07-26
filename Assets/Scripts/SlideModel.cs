using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideModel{
    
    public int position = 0;
    public int start = 0;
    public int width = 0;
    public int type = 0;
    public int measure = 0;
    public int split = 0;
    // 2: HOLD
    // 3: SLIDE
    // 4: AIR-ACTION

    public SlideModel(int start, string width, int type, int measure, int position, int split)
    {
        this.position = position;
        this.measure = measure;
        this.split = split;
        this.start = start;
        this.width = ChangeZto35(width);
        this.type = type;
    }
    private int ChangeZto35(string str)
    {
        if (str == null) return 0;

        int ChangeZto35 = 0;
        if (str != "")
        {
            char c = Convert.ToChar(str.Substring(0, 1));
            ChangeZto35 = (int)c - 48;
            if (ChangeZto35 > 10) ChangeZto35 -= 7;
            if (ChangeZto35 < 0) ChangeZto35 = 0;
            if (ChangeZto35 > 35) ChangeZto35 = 0;
        }
        return ChangeZto35;
    }
}
