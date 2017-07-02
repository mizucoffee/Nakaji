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

    public SlideModel(int start, int width, int type, int measure, int position, int split)
    {
        this.position = position;
        this.measure = measure;
        this.split = split;
        this.start = start;
        this.width = width;
        this.type = type;
    }
}
