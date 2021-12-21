using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �F�̕ϊ����s���N���X
/// Original Source https://github.com/baba-s/UniColorUtils/blob/master/Scripts/ColorUtils.cs
/// </summary>
public static class ColorUtils
{
	//================================================================================
	// �֐�(static)
	//================================================================================
	/// <summary>
	/// <para>�e�F������ 0 ���� 255 �͈̔͂Ŏw�肵�� Color �^�̃C���X�^���X���쐬���܂�</para>
	/// <para>�A���t�@�l�� 255 �������Őݒ肳��܂�</para>
	/// </summary>
	public static Color FromRGB(byte r, byte g, byte b)
	{
		return new Color(r / 255f, g / 255f, b / 255f);
	}

	/// <summary>
	/// �e�F������ 0 ���� 255 �͈̔͂Ŏw�肵�� Color �^�̃C���X�^���X���쐬���܂�
	/// </summary>
	public static Color FromRGBA(byte r, byte g, byte b, byte a)
	{
		return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
	}

	/// <summary>
	/// �e�F������ 0 ���� 255 �͈̔͂Ŏw�肵�� Color �^�̃C���X�^���X���쐬���܂�
	/// </summary>
	public static Color FromARGB(byte a, byte r, byte g, byte b)
	{
		return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
	}

	/// <summary>
	/// <para>16 �i���̐��l���� Color �^�̃C���X�^���X���쐬���܂�</para>
	/// <para>ColorUtils.FromRGB( 0xFF8000 ) -> // RGBA( 1.0, 0.5, 0.0, 1.0 )</para>
	/// </summary>
	public static Color FromRGB(long value)
	{
		var inv = 1f / 255f;
		var color = Color.black;
		color.r = inv * ((value >> 16) & 0xFF);
		color.g = inv * ((value >> 8) & 0xFF);
		color.b = inv * (value & 0xFF);
		color.a = 1f;
		return color;
	}

	/// <summary>
	/// <para>16 �i���̐��l���� Color �^�̃C���X�^���X���쐬���܂�</para>
	/// <para>ColorUtils.FromRGBA( 0xFF8000FF ) -> // RGBA( 1.0, 0.5, 0.0, 1.0 )</para>
	/// </summary>
	public static Color FromRGBA(long value)
	{
		var inv = 1f / 255f;
		var color = Color.black;
		color.r = inv * ((value >> 24) & 0xFF);
		color.g = inv * ((value >> 16) & 0xFF);
		color.b = inv * ((value >> 8) & 0xFF);
		color.a = inv * (value & 0xFF);
		return color;
	}

	/// <summary>
	/// <para>16 �i���̐��l���� Color �^�̃C���X�^���X���쐬���܂�</para>
	/// <para>ColorUtils.FromARGB( 0xFFFF8000 ) -> // RGBA( 1.0, 0.5, 0.0, 1.0 )</para>
	/// </summary>
	public static Color FromARGB(long value)
	{
		var inv = 1f / 255f;
		var color = Color.black;
		color.a = inv * ((value >> 24) & 0xFF);
		color.r = inv * ((value >> 16) & 0xFF);
		color.g = inv * ((value >> 8) & 0xFF);
		color.b = inv * (value & 0xFF);
		return color;
	}

	/// <summary>
	/// <para>16 �i����\�������񂩂� Color �^�̃C���X�^���X���쐬���܂�</para>
	/// <para>ColorUtils.FromRGB( "#FF8000" ) -> // RGBA( 1.0, 0.5, 0.0, 1.0 )</para>
	/// </summary>
	public static Color FromRGB(string htmlString)
	{
		ColorUtility.TryParseHtmlString(htmlString, out var color);
		return color;
	}

	/// <summary>
	/// <para>16 �i����\�������񂩂� Color �^�̃C���X�^���X���쐬���܂�</para>
	/// <para>ColorUtils.FromRGBA( "#FF8000FF" ) -> // RGBA( 1.0, 0.5, 0.0, 1.0 )</para>
	/// </summary>
	public static Color FromRGBA(string htmlString)
	{
		ColorUtility.TryParseHtmlString(htmlString, out var color);
		return color;
	}

	/// <summary>
	/// 16 �i���̐��l�ɕϊ����܂�
	/// </summary>
	public static long ToRGB(Color color)
	{
		long value = 0;
		value |= (long)Mathf.RoundToInt(color.r * 255) << 16;
		value |= (long)Mathf.RoundToInt(color.g * 255) << 8;
		value |= (long)Mathf.RoundToInt(color.b * 255);
		return value;
	}

	/// <summary>
	/// 16 �i���̐��l�ɕϊ����܂�
	/// </summary>
	public static long ToRGBA(Color color)
	{
		long value = 0;
		value |= (long)Mathf.RoundToInt(color.r * 255) << 24;
		value |= (long)Mathf.RoundToInt(color.g * 255) << 16;
		value |= (long)Mathf.RoundToInt(color.b * 255) << 8;
		value |= (long)Mathf.RoundToInt(color.a * 255);
		return value;
	}

	/// <summary>
	/// 16 �i���̐��l�ɕϊ����܂�
	/// </summary>
	public static long ToARGB(Color color)
	{
		long value = 0;
		value |= (long)Mathf.RoundToInt(color.a * 255) << 24;
		value |= (long)Mathf.RoundToInt(color.r * 255) << 16;
		value |= (long)Mathf.RoundToInt(color.g * 255) << 8;
		value |= (long)Mathf.RoundToInt(color.b * 255);
		return value;
	}

	/// <summary>
	/// <para>16 �i����\�����l�ɕϊ����܂�</para>
	/// <para>ColorUtils.ToRGBHtmlStringLower( Color.red ) -> // ff0000</para>
	/// </summary>
	public static string ToRGBHtmlStringLower(Color color)
	{
		return ToRGB(color).ToString("x6");
	}

	/// <summary>
	/// <para>16 �i����\�����l�ɕϊ����܂�</para>
	/// <para>ColorUtils.ToRGBAHtmlStringLower( Color.red ) -> // ff0000ff</para>
	/// </summary>
	public static string ToRGBAHtmlStringLower(Color color)
	{
		return ToRGBA(color).ToString("x8");
	}

	/// <summary>
	/// <para>16 �i����\�����l�ɕϊ����܂�</para>
	/// <para>ColorUtils.ToARGBHtmlStringLower( Color.red ) -> // ffff0000</para>
	/// </summary>
	public static string ToARGBHtmlStringLower(Color color)
	{
		return ToARGB(color).ToString("x8");
	}

	/// <summary>
	/// <para>16 �i����\�����l�ɕϊ����܂�</para>
	/// <para>ColorUtils.ToRGBHtmlStringUpper( Color.red ) -> // FF0000</para>
	/// </summary>
	public static string ToRGBHtmlStringUpper(Color color)
	{
		return ToRGB(color).ToString("X6");
	}

	/// <summary>
	/// <para>16 �i����\�����l�ɕϊ����܂�</para>
	/// <para>ColorUtils.ToRGBAHtmlStringUpper( Color.red ) -> // FF0000FF</para>
	/// </summary>
	public static string ToRGBAHtmlStringUpper(Color color)
	{
		return ToRGBA(color).ToString("X8");
	}

	/// <summary>
	/// <para>16 �i����\�����l�ɕϊ����܂�</para>
	/// <para>ColorUtils.ToARGBHtmlStringUpper( Color.red ) -> // FFFF0000</para>
	/// </summary>
	public static string ToARGBHtmlStringUpper(Color color)
	{
		return ToARGB(color).ToString("X8");
	}
}