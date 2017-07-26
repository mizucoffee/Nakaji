using System;
using System.Collections.Generic;

public class NotesModel {
    public int start = 0;
    public int width = 0;
    
    public int type = 0;
    public int hold = 0;
    public List<SlideModel> slide = new List<SlideModel>();

    public int measure = 0;
    public int position = 0;
    public int split = 0;



    public NotesModel(int start, string width, int type, int measure, int position, int split)
    {
        this.start = start;
        this.width = ChangeZto35(width);
        this.type = type;
        this.measure = measure;
        this.position = position;
        this.split = split;
    }

    public NotesModel(int start, int width, int type, int measure, int position, int split)
    {
        this.start = start;
        this.width = width;
        this.type = type;
        this.measure = measure;
        this.position = position;
        this.split = split;
    }

    public NotesModel(int start, string width, int type, int measure, int position, int split, int hold)
    {
        this.start = start;
        this.width = ChangeZto35(width);
        this.type = type;
        this.measure = measure;
        this.position = position;
        this.split = split;
        this.hold = hold;
    }

    public NotesModel(int start, string width, int type, int measure, int position, int split,List<SlideModel> slide)
    {
        this.start = start;
        this.width = ChangeZto35(width);
        this.type = type;
        this.measure = measure;
        this.position = position;
        this.split = split;
        this.slide = slide;
    }
    public NotesModel(int start, int width, int type, int measure, int position, int split,  List<SlideModel> slide)
    {
        this.start = start;
        this.width = width;
        this.type = type;
        this.measure = measure;
        this.position = position;
        this.split = split;
        this.slide = slide;
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
