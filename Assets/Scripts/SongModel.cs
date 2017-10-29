using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongModel{
	public string Title {get;set;}
	public string SubTitle{get;set;}
	public string Artist{get;set;}
	public string Genre {get;set;}
	public string Designer{get;set;}
	public int PlayLevel {get;set;}
	public int DifficultyType{get;set;}
	public string ExtraDifficulty{get;set;}
	public string SongId{get;set;}
	public string WaveFileName{get;set;}
	public float WaveOffset{get;set;}
	public string JacketFileName{get;set;}
	public float BaseBPM{get;set;}

	public override string ToString()
	{
		return "Title          : " + Title + 
			"\nSubTitle       : " + SubTitle + 
			"\nArtist         : " + Artist + 
			"\nGenre          : " + Genre +
			"\nDesigner       : " + Designer + 
			"\nPlayLevel      : " + PlayLevel + 
			"\nDifficultyType : " + DifficultyType + 
			"\nExtraDifficulty: " + ExtraDifficulty + 
			"\nSongId         : " + SongId + 
			"\nWave           : " + WaveFileName + 
			"\nWaveOffset     : " + WaveOffset + 
			"\nJacket         : " + JacketFileName + 
			"\nBaseBPM        : " + BaseBPM;
	}
}
