using System.Text;

namespace NotinoHomeWork.Tests.Extensions;

public static class StringExtensions
{
	public static MemoryStream ToStream(this string s) => new(Encoding.ASCII.GetBytes(s));
}