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



    public NotesModel(int start, int width, int type, int measure, int position, int split)
    {
        this.start = start;
        this.width = width;
        this.type = type;
        this.measure = measure;
        this.position = position;
        this.split = split;
    }

    public NotesModel(int start, int width, int type, int measure, int position, int split, int hold)
    {
        this.start = start;
        this.width = width;
        this.type = type;
        this.measure = measure;
        this.position = position;
        this.split = split;
        this.hold = hold;
    }

    public NotesModel(int start, int end, int type,List<SlideModel> slide)
    {
        this.start = start;
        this.width = end;
        this.type = type;
        this.slide = slide;
    }

}
