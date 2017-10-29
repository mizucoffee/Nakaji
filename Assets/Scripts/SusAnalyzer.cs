using System;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class SusAnalyzer
{
	// イニシャライズ時解析

	private Regex metaReg = new Regex (@"^#(?<name>\w+)+\s+(?<data>.*)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	private Regex scoreReg = new Regex (@"^#(?<measure>\w{3})(?<lane>\w{2,3}):\s*(?<data>.*)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
	private SongModel songData = new SongModel ();

	private List<SusRawNoteData> notes = new List<SusRawNoteData>();
	private Dictionary<int,float> bpmDef = new Dictionary<int,float>();
	private Dictionary<int,float> beatDef = new Dictionary<int,float>();
	private int ticksPerBeat = 192;

	public SusAnalyzer (string susName)
	{
		TextAsset textAsset = Resources.Load (susName) as TextAsset;
		int i = 0;
		foreach (string s in textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None)) {
			i++;
			Match m = metaReg.Match (s);
			Match m2 = scoreReg.Match (s);

			if (s.Length == 0)
				continue;
			if (!s.StartsWith ("#"))
				continue;
			else if (m.Success)
				ProcessCommand (m, i);
			else if (m2.Success)
				ProcessData (m2,i);
			else
				Debug.Log ("SUS有効行ですが解析できませんでした Line: " + i);
		}

		Debug.Log ("META: " + songData.ToString ());

		foreach (SusRawNoteData note in notes) {
			Debug.Log ("NOTE: " + note.ToString());
		}
	}

	private void ProcessData (Match score, int line)
	{
		string measure = score.Groups ["measure"].Value.ToUpper ();
		string lane = score.Groups ["lane"].Value;
		string data = score.Groups ["data"].Value.Replace(" ","");

		/*
	     判定順について
	     0. #...** (BPMなど)
	     1. #---0* (特殊データ、定義分割不可)
	     2. #---1* (Short)
	     3. #---5* (Air)
	     4. #---[234]*. (Long)
	     ※参考用にSeaurchinの写し
	    */

		int notesCount = data.Length / 2;

		int measureInt;
		if (!int.TryParse (measure, out measureInt)) {
			// コマンド
			switch (measure) {
			case "BPM":
				float bpm;
				if (float.TryParse (data, out bpm)) {
					Debug.Log ("BPM change");
					bpmDef.Add (RadixConvert.ToInt32 (lane, 36), bpm);
				} else
					Debug.Log ("BPMの値が不正です Line: " + line);
				break;
			case "TIL":
				Debug.LogError ("Warn: TILはsusフォーマットv2.7.1現在定義されていますが未実装です Line: " + line);
				break;
			default:
				Debug.LogError ("Warn: " + measure + "は未実装です Line: " + line);
				break;
			}
		} else if (lane [0] == '0') {
			int step = (int)((ticksPerBeat * getBeatsAt (measureInt)) / (notesCount == 0 ? 1 : notesCount));

			switch (lane [1]) {
			case '2':
				// 小節長
				float pat;
				if (float.TryParse (data, out pat))
					beatDef [measureInt] = pat;
				else
					Debug.Log ("Beatの値が不正です Line: " + line);
				break;
			case '8':
				// BPM
				for (int i = 0; i < notesCount; i++) {
					string note = data.Substring (i * 2, 2);
					SusRawNoteData noteData = new SusRawNoteData ();
					noteData.time = new SusRelativeNoteTime (measureInt, step * i);
					noteData.type = SusNoteType.Undefined;
					noteData.defNumber = RadixConvert.ToInt32 (note, 36);
					notes.Add (noteData);
				}
				break;
			default:
				Debug.LogError ("Warn: 不正なデータコマンドです Line: " + line);
				break;
			}
		} else if (lane [0] == '1') {
			int step = (int)((ticksPerBeat * getBeatsAt (measureInt)) / (notesCount == 0 ? 1 : notesCount));

			for (int i = 0; i < notesCount; i++) {
				string note = data.Substring (i * 2, 2);
				SusRawNoteData noteData = new SusRawNoteData ();
				noteData.time = new SusRelativeNoteTime (measureInt, step * i);
				noteData.startLane = RadixConvert.ToInt32 (lane.Substring (1, 1), 36);
				noteData.length = RadixConvert.ToInt32 (note.Substring (1, 1), 36);

				if (note [1] == '0')
					continue;

				switch (note [0]) {
				case '1':
					noteData.type = SusNoteType.Tap;
					break;
				case '2':
					noteData.type = SusNoteType.ExTap;
					break;
				case '3':
					noteData.type = SusNoteType.Flick;
					break;
				case '4':
					noteData.type = SusNoteType.HellTap;
					break;
				default: 
					Debug.LogError ("Warn: ショートレーンの指定が不正です Line: " + line);
					break;
				}

				notes.Add (noteData);
			}
		} else if (lane [0] == '5') {
			int step = (int)((ticksPerBeat * getBeatsAt (measureInt)) / (notesCount == 0 ? 1 : notesCount));

			for (int i = 0; i < notesCount; i++) {
				string note = data.Substring (i * 2, 2);
				SusRawNoteData noteData = new SusRawNoteData ();
				noteData.time = new SusRelativeNoteTime (measureInt, step * i);
				noteData.startLane = RadixConvert.ToInt32 (lane.Substring (1, 1), 36);
				noteData.length = RadixConvert.ToInt32 (note.Substring (1, 1), 36);
				noteData.type = SusNoteType.Air;

				if (note [1] == '0')
					continue;

				switch (note [0]) {
				case '1':
					noteData.type |= SusNoteType.Up;
					break;
				case '2':
					noteData.type |= SusNoteType.Down;
					break;
				case '3':
					noteData.type |= SusNoteType.Up | SusNoteType.Left; 
					break;
				case '4':
					noteData.type |= SusNoteType.Up | SusNoteType.Right; 
					break;
				case '5':
					noteData.type |= SusNoteType.Down | SusNoteType.Right; 
					break;
				case '6':
					noteData.type |= SusNoteType.Down | SusNoteType.Left; 
					break;
				default: 
					Debug.LogError ("Warn: Airレーンの指定が不正です Line: " + line);
					break;
				}

				notes.Add (noteData);
			}
		} else if (lane.Length == 3) {
			int step = (int)((ticksPerBeat * getBeatsAt (measureInt)) / (notesCount == 0 ? 1 : notesCount));

			for (int i = 0; i < notesCount; i++) {
				string note = data.Substring (i * 2, 2);
				SusRawNoteData noteData = new SusRawNoteData ();
				noteData.time = new SusRelativeNoteTime (measureInt, step * i);
				noteData.startLane = RadixConvert.ToInt32 (lane.Substring (1, 1), 36);
				noteData.length = RadixConvert.ToInt32 (note.Substring (1, 1), 36);
				noteData.extra = RadixConvert.ToInt32 (lane.Substring (2, 1), 36);

				if (note [1] == '0')
					continue;

				switch (lane [0]) {
				case '2':
					noteData.type = SusNoteType.Hold;
					break;
				case '3':
					noteData.type = SusNoteType.Slide; 
					break;
				case '4':
					noteData.type = SusNoteType.AirAction;
					break;
				default: 
					Debug.LogError ("Warn: ロングレーンの指定が不正です Line: " + line);
					break;
				}

				switch (note [0]) {
				case '1':
					noteData.type |= SusNoteType.Start;
					break;
				case '2':
					noteData.type |= SusNoteType.End;
					break;
				case '3':
					noteData.type |= SusNoteType.Step; 
					break;
				case '4':
					noteData.type |= SusNoteType.Control;
					break;
				case '5':
					noteData.type |= SusNoteType.Invisible;
					break;
				default: 
					Debug.LogError ("Warn: ロングレーンのノーツ種類の指定が不正です Line: " + line);
					break;
				}

				notes.Add (noteData);
			}
		} else {
			Debug.LogError ("Warn: 不正なデータ定義です Line: " + line);
		}

	}

	private void ProcessCommand (Match meta, int i)
	{
		string name = meta.Groups ["name"].Value.ToUpper ();
		string data = meta.Groups ["data"].Value;
        
		if (Regex.IsMatch (data, @"^"".*""$"))
			data = data.Substring (1, data.Length - 2);

		switch (name) {
		case "TITLE":
			songData.Title = data;
			break;
		case "SUBTITLE":
			songData.SubTitle = data;
			break;
		case "ARTIST":
			songData.Artist = data;
			break;
		case "GENRE":
			songData.Genre = data;
			break;
		case "DESIGNER":
		case "SUBARTIST":
			songData.Designer = data;
			break;
		case "PLAYLEVEL":
			int j;
			if (Int32.TryParse (data, out j))
				songData.PlayLevel = j;
			else
				Debug.LogError ("PlayLevelの値が不正です Line: " + i);
			break;
		case "DIFFICULTY":
			int k;
			if (Int32.TryParse (data, out k))
				songData.DifficultyType = k;
			else
				Debug.LogError ("Difficultyの値が不正です Line: " + i);
			break;
		case "SONGID":
			songData.SongId = data;
			break;
		case "WAVE":
			songData.WaveFileName = data;
			break;
		case "WAVEOFFSET":
			float l;
			if (float.TryParse (data, out l))
				songData.WaveOffset = l;
			else
				Debug.LogError ("WAVEOFFSETの値が不正です Line: " + i);
			break;
		case "JACKET":
			songData.JacketFileName = data;
			break;
		case "REQUEST":
				//未実装
			Debug.LogError ("Warn: REQUESTはsusフォーマットv2.7.1現在定義されていますが未実装です Line: " + i);
			break;
		case "BASEBPM":
			float m;
			if (float.TryParse (data, out m))
				songData.BaseBPM = m;
			else
				Debug.LogError ("BASEBPMの値が不正です Line: " + i);
			break;
		case "HISPEED":
				//未実装
			Debug.LogError ("Warn: HISPEEDはsusフォーマットv2.7.1現在定義されていますが未実装です Line: " + i);
			break;
		case "NOSPEED":
				//未実装
			Debug.LogError ("Warn: NOSPEEDはsusフォーマットv2.7.1現在定義されていますが未実装です Line: " + i);
			break;
		default:
			Debug.LogError ("Warn: " + name + "は未実装です Line: " + i);
			break;
		}

	}

	private float getBeatsAt(int measure){
		float result = 4;
		int last = 0;
		foreach (KeyValuePair<int,float> pair in beatDef) {
			if (pair.Key >= last && pair.Key <= measure) {
				result = pair.Value;
				last = pair.Key;
			}
		}
		return result;
	}
}
