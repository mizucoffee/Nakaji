using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum SusNoteType{
	Undefined = 1 << 0,
	Tap       = 1 << 1,
	ExTap     = 1 << 2,
	Flick     = 1 << 3,
	Air       = 1 << 4,
	HellTap   = 1 << 5,

	Hold      = 1 << 6,
	Slide     = 1 << 7,
	AirAction = 1 << 8,

	Start     = 1 << 9,
	Step      = 1 << 10,
	Control   = 1 << 11,
	End       = 1 << 12,

	Up        = 1 << 13,
	Down      = 1 << 14,
	Left      = 1 << 15,
	Right     = 1 << 16,

	Injection = 1 << 17,
	Invisible = 1 << 18,
	Unused3   = 1 << 19,
}

public class SusRawNoteData {

	public SusNoteType type;
	public int defNumber; // = BPMの定義番号
	public int startLane;
	public int length;
	public int extra; // = レーンID(ロング系)
	public SusRelativeNoteTime time;

	public override string ToString()
	{
		return 
			"\nType         : " + type + 
			"\nDefNumber  : " + defNumber + 
			"\nStartLane  : " + startLane + 
			"\nLength     : " + length +
			"\nExtra      : " + extra + 
			"\nMeasure    : " + time.measure +
			"\nTick       : " + time.tick;
	}

}

public class SusRelativeNoteTime {
	public int measure;
	public int tick;

	public SusRelativeNoteTime(int measure,int tick){
		this.measure = measure;
		this.tick = tick;
	}

	public static bool operator < (SusRelativeNoteTime a, SusRelativeNoteTime b){
		if(a.measure < b.measure)
			return true;
		else if(a.measure == b.measure && a.tick < b.tick)
		    return true;
		return false;
	}

	public static bool operator > (SusRelativeNoteTime a, SusRelativeNoteTime b){
		if(a.measure > b.measure)
			return true;
		else if(a.measure == b.measure && a.tick > b.tick)
			return true;
		return false;
	}

	public static bool operator == (SusRelativeNoteTime a, SusRelativeNoteTime b){
		return a.measure == b.measure && a.tick == b.tick;
	}

	public static bool operator != (SusRelativeNoteTime a, SusRelativeNoteTime b){
		return a.measure != b.measure || a.tick != b.tick;
	}
}

/// <summary>
/// Enum 型の拡張メソッドを管理するクラス
/// </summary>
public static class EnumExtensions
{
	/// <summary>
	/// 現在のインスタンスで 1 つ以上のビット フィールドが設定されているかどうかを判断します
	/// </summary>
	public static bool HasFlag( this Enum self, Enum flag )
	{
		if ( self.GetType() != flag.GetType() )
		{
			throw new ArgumentException( "flag の型が、現在のインスタンスの型と異なっています。" );
		}

		var selfValue = Convert.ToUInt64( self );
		var flagValue = Convert.ToUInt64( flag );

		return ( selfValue & flagValue ) == flagValue;
	}
}
