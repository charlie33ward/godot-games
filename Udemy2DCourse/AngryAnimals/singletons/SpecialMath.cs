using Godot;
using System;

public static class SpecialMath
{
	public static float Clamp(float value, float min, float max)
	{
		return Math.Min(Math.Max(value, min), max);
	}
}
