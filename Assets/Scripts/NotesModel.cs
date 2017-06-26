using System.Collections.Generic;

public class NotesModel {
    public int start = 0;
    public int end = 0;
    
    public int type = 0;
    // 0 red
    // 1 yellow
    // 2 hold
    public int hold = 0;
    public List<int[]> slide = new List<int[]>();

    public int bpm = 0;
    public int split = 0;

    public NotesModel(int bpm) { this.bpm = bpm; }

    public NotesModel(int start,int end,int type,int bpm,int split)
    {
        this.start = start;
        this.end = end;
        this.type = type;
        this.bpm = bpm;
        this.split = split;
    }

    public NotesModel(int start, int end, int type, int bpm, int split,int hold)
    {
        this.start = start;
        this.end = end;
        this.type = type;
        this.bpm = bpm;
        this.split = split;
        this.hold = hold;
    }

}
