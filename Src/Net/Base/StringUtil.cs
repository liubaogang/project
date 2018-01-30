
namespace Net.Base
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringUtil
    {
        public static readonly char[] LetterChars = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F',
            'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z'
        };
        private static readonly Cicada.Core.LimitedCharFilter LimitedCharFilter = new Cicada.Core.LimitedCharFilter(true, null);
        public static readonly char[] LowerLetterChars = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        public static readonly char[] NumberChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static readonly char[] NumberLetterChars = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f',
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
            'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };
        private static readonly Lazy<Dictionary<char, int>> NumeralRadixCacheLazy = new Lazy<Dictionary<char, int>>(new Func<Dictionary<char, int>>(<> c.<> 9.<.cctor > b__111_0));
        public static readonly char[] NumeralRadixChars = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f',
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
            'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '+', '/'
        };
        public static readonly char[] UpperLetterChars = new char[] {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };
        public static readonly char[] UsualChineseChars = "的一是在不了有和人这中大为上个国我以要他时来用们生到作地于出就分对成会可主发年动同工也能下过子说产种面而方后多定行学法所民得经十三之进着等部度家电力里如水化高自二理起小物现实加量都两体制机当使点从业本去把性好应开它合还因由其些然前外天政四日那社义事平形相全表间样与关各重新线内数正心反你明看原又么利比或但质气第向道命此变条只没结解问意建月公无系军很情者最立代想已通并提直题党程展五果料象员革位入常文总次品式活设及管特件长求老头基资边流路级少图山统接知较将组见计别她手角期根论运农指几九区强放决西被干做必战先回则任取据处队南给色光门即保治北造百规热领七海口东导器压志世金增争济阶油思术极交受联什认六共权收证改清己美再采转更单风切打白教速花带安场身车例真务具万每目至达走积示议声报斗完类八离华名确才科张信马节话米整空元况今集温传土许步群广石记需段研界拉林律叫且究观越织装影算低持音众书布复容儿须际商非验连断深难近矿千周委素技备半办青省列习响约支般史感劳便团往酸历市克何除消构府称太准精值号率族维划选标写存候毛亲快效斯院查江型眼王按格养易置派层片始却专状育厂京识适属圆包火住调满县局照参红细引听该铁价严".ToCharArray();

        static StringUtil()
        {
            LimitedCharFilter.AddLetterChars();
            LimitedCharFilter.AddLowerChars();
            LimitedCharFilter.AddNumberChars();
            LimitedCharFilter.AddChinese();
        }

        public static string ConvertNumberBase(string source, int fromBase, int toBase)
        {
            return Convert.ToString(Convert.ToInt64(source, fromBase), toBase);
        }

        public static string DefaultIfNull(this string src, string defaultValue)
        {
            return (src ?? defaultValue);
        }

        public static string DefaultIfNullOrEmpty(this string src, string defaultValue)
        {
            if (!string.IsNullOrEmpty(src))
            {
                return src;
            }
            return defaultValue;
        }

        public static string FixWidth(this string str, int width, string sufix = "")
        {
            string str2 = null;
            if (width >= (str.Length * 2))
            {
                return str;
            }
            for (int i = 0; i < str.Length; i++)
            {
                width -= GetCharWidth(str[i]);
                if (width <= 0)
                {
                    if (width == 0)
                    {
                        str2 = string.Format("{0}{1}", str.Substring(0, i + 1), sufix);
                    }
                    else if (width == -1)
                    {
                        str2 = string.Format("{0} {1}", str.Substring(0, i), sufix);
                    }
                    break;
                }
            }
            return (str2 ?? str);
        }

        public static string FromBase64String(string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetString(Convert.FromBase64String(str));
        }

        public static char[] GetCharArray(CharsScope scope)
        {
            List<char> source = new List<char>();
            if (scope.HasFlag(CharsScope.Numbers))
            {
                source.AddRange(NumberChars);
            }
            if (scope.HasFlag(CharsScope.LowerLetters))
            {
                source.AddRange(LowerLetterChars);
            }
            if (scope.HasFlag(CharsScope.UpperLetters))
            {
                source.AddRange(UpperLetterChars);
            }
            if (scope.HasFlag(CharsScope.UsualChinese))
            {
                source.AddRange(UsualChineseChars);
            }
            return source.ToArray<char>();
        }

        public static int GetCharWidth(char chr)
        {
            if (chr >= '\x00ff')
            {
                return 2;
            }
            return 1;
        }

        public static string GetGuidPureString()
        {
            return Guid.NewGuid().ToPureString();
        }

        public static int GetStrWidth(this string str)
        {
            return str.Sum<char>(((Func<char, int>)(chr => GetCharWidth(chr))));
        }

        public static byte[] HexToBytes(this string hex)
        {
            if ((hex.Length % 2) != 0)
            {
                throw new ArgumentException("16进制字符串长度必须是2的倍数。", "hex");
            }
            int num = hex.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                buffer[i] = (byte)Convert.ToInt32(hex.Substring(i * 2, 2), 0x10);
            }
            return buffer;
        }

        public static string HexToStr(this string hex, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;
            return encode.GetString(hex.HexToBytes());
        }

        public static bool IsAscii(this char chr)
        {
            return (chr <= '\x007f');
        }

        public static bool IsAscii(string str)
        {
            return MatchCharacterSetOfAll(str, new Func<char, bool>(StringUtil.IsAscii));
        }

        public static bool IsChinese(char chr)
        {
            return ((chr >= '一') && (chr <= 0x9fa5));
        }

        public static bool IsChinese(string text)
        {
            return text.All<char>(new Func<char, bool>(StringUtil.IsChinese));
        }

        public static bool IsChineseOrLetterOrNumber(string text)
        {
            return LimitedCharFilter.IsValid(text);
        }

        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool IsGb18030(this char chr)
        {
            if (chr.IsAscii())
            {
                return true;
            }
            char[] chars = new char[] { chr };
            byte[] bytes = Encoding.GetEncoding(0xd698).GetBytes(chars);
            if (bytes.Length == 2)
            {
                byte num = bytes[0];
                byte num2 = bytes[1];
                return ((((num >= 0x81) && (num <= 0xfe)) && ((num2 >= 0x40) && (num2 <= 0x7e))) || ((((num >= 0x81) && (num <= 0xfe)) && (num2 >= 0x80)) && (num2 <= 0xfe)));
            }
            if (bytes.Length == 4)
            {
                byte num3 = bytes[0];
                byte num4 = bytes[1];
                byte num5 = bytes[2];
                byte num6 = bytes[3];
                return (((((num3 >= 0x81) && (num3 <= 0xfe)) && ((num4 >= 0x30) && (num4 <= 0x39))) && ((num5 >= 0x81) && (num5 <= 0xfe))) && ((num6 >= 0x30) && (num6 <= 0x39)));
            }
            return false;
        }

        public static bool IsGb2312(this char chr)
        {
            if (chr.IsAscii())
            {
                return true;
            }
            char[] chars = new char[] { chr };
            byte[] bytes = Encoding.GetEncoding(0x51c8).GetBytes(chars);
            if (bytes.Length == 2)
            {
                byte num = bytes[0];
                byte num2 = bytes[1];
                return ((((num >= 0xb0) && (num <= 0xf7)) && (num2 >= 160)) && (num2 <= 0xfe));
            }
            return false;
        }

        public static bool IsGb2312(string str)
        {
            return MatchCharacterSetOfAll(str, new Func<char, bool>(StringUtil.IsGb2312));
        }

        public static bool IsGbk(this char chr)
        {
            if (chr.IsAscii())
            {
                return true;
            }
            char[] chars = new char[] { chr };
            byte[] bytes = Encoding.GetEncoding(0x3a8).GetBytes(chars);
            if (bytes.Length == 2)
            {
                byte num = bytes[0];
                byte num2 = bytes[1];
                return ((((num >= 0x81) && (num <= 0xfe)) && (num2 >= 0x40)) && (num2 <= 0xfe));
            }
            return false;
        }

        public static bool IsGbk(string str)
        {
            return MatchCharacterSetOfAll(str, new Func<char, bool>(StringUtil.IsGbk));
        }

        public static bool IsInteger(string input)
        {
            return Regex.IsMatch(input, @"^-?\d+$");
        }

        public static bool IsIpAddress(string ip)
        {
            return Regex.IsMatch(ip, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }

        public static bool IsNegativeInteger(string input)
        {
            return Regex.IsMatch(input, "^-[0-9]*[1-9][0-9]*$");
        }

        public static bool IsNotNegativeInteger(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        public static bool IsNotNegativeNumber(string input)
        {
            return Regex.IsMatch(input, @"^\d+[.]?\d*$");
        }

        public static bool IsNotPositiveInteger(string input)
        {
            return Regex.IsMatch(input, @"^((-\d+)|(0+))$");
        }

        public static bool IsNumber(string input)
        {
            return Regex.IsMatch(input, @"^[-]?\d+[.]?\d*$");
        }

        public static bool IsPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^[1]+\d{10}$");
        }

        public static bool IsPositiveInteger(string input)
        {
            return Regex.IsMatch(input, "^[0-9]*[1-9][0-9]*$");
        }

        public static bool IsPostcode(string postcode)
        {
            return Regex.IsMatch(postcode, @"[1-9]\d{5}(?!\d)");
        }

        public static bool IsTel(string tel)
        {
            return Regex.IsMatch(tel, @"^(\d{3,4})?\d{6,8}$");
        }

        public static bool IsUrl(string url)
        {
            return Regex.IsMatch(url, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        public static int LevenshteinDistance(string source, string target, bool ignoreCase = true)
        {
            int length = source.Length;
            int num2 = target.Length;
            int[,] numArray = new int[length + 1, num2 + 1];
            if (length == 0)
            {
                return num2;
            }
            if (num2 == 0)
            {
                return length;
            }
            if (ignoreCase)
            {
                source = source.ToLower();
                target = target.ToLower();
            }
            int num3 = 0;
            while (num3 <= length)
            {
                numArray[num3, 0] = num3++;
            }
            int num4 = 0;
            while (num4 <= num2)
            {
                numArray[0, num4] = num4++;
            }
            for (int i = 1; i <= length; i++)
            {
                for (int j = 1; j <= num2; j++)
                {
                    int num7 = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    numArray[i, j] = Math.Min(Math.Min((int)(numArray[i - 1, j] + 1), (int)(numArray[i, j - 1] + 1)), numArray[i - 1, j - 1] + num7);
                }
            }
            return numArray[length, num2];
        }

        private static bool MatchCharacterSetOfAll(string str, Func<char, bool> func)
        {
            return str.All<char>(func);
        }

        public static string RadixToString(ulong num, uint radix)
        {
            if (num == 0)
            {
                return "0";
            }
            string str = string.Empty;
            while (num > 0L)
            {
                str = NumeralRadixChars[(int)((IntPtr)(num % ((ulong)radix)))].ToString() + str;
                num /= (ulong)radix;
            }
            return str;
        }

        public static string ReplaceWhiteSpace(this string src, char newChar, bool repeated = true)
        {
            return src.ReplaceWhiteSpace(newChar.ToString(CultureInfo.InvariantCulture), repeated);
        }

        public static string ReplaceWhiteSpace(this string src, string newValue, bool repeated = false)
        {
            if (string.IsNullOrEmpty(src))
            {
                return src;
            }
            int num = -2147483648;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < src.Length; i++)
            {
                char c = src[i];
                if (char.IsWhiteSpace(c))
                {
                    if (repeated)
                    {
                        builder.Append(newValue);
                    }
                    else
                    {
                        if (num != (i - 1))
                        {
                            builder.Append(newValue);
                        }
                        num = i;
                    }
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        public static ulong StringToRadix(this string value, uint radix)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0L;
            }
            ulong num = 0L;
            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                num += ((ulong)NumeralRadixCache[ch]) * ((ulong)Math.Pow((double)radix, (double)((value.Length - i) - 1)));
            }
            return num;
        }

        public static string StrToHex(this string str, Encoding encode = null)
        {
            return BitConverter.ToString((encode ?? Encoding.UTF8).GetBytes(str)).Replace("-", "");
        }

        public static T To<T>(this string src)
        {
            return (T)Convert.ChangeType(src, typeof(T), null);
        }

        public static T To<T>(this string src, T defaultValue)
        {
            try
            {
                return src.To<T>();
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T To<T>(this string src, Func<string, T> converter)
        {
            converter = converter ?? new Func<string, T>(StringUtil.To<T>);
            return converter(src);
        }

        public static T To<T>(this string src, IFormatProvider provider)
        {
            return (T)Convert.ChangeType(src, typeof(T), provider);
        }

        public static T To<T>(this string src, T defaultValue, Func<string, T> converter)
        {
            try
            {
                return src.To<T>(converter);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T To<T>(this string src, T defaultValue, IFormatProvider provider)
        {
            try
            {
                return src.To<T>(provider);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ToBase64String(string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            return Convert.ToBase64String(encoding.GetBytes(str));
        }

        public static bool ToBoolean(this string src)
        {
            return src.To<bool>(new Func<string, bool>(bool.Parse));
        }

        public static bool ToBoolean(this string src, bool defaultValue)
        {
            return src.To<bool>(defaultValue, new Func<string, bool>(StringUtil.ToBoolean));
        }

        public static bool? ToBooleanN(this string src, bool defaultValue)
        {
            return src.ToN<bool>(defaultValue, new Func<string, bool>(StringUtil.ToBoolean));
        }

        public static byte ToByte(this string src)
        {
            return src.To<byte>(new Func<string, byte>(byte.Parse));
        }

        public static byte ToByte(this string src, byte defaultValue)
        {
            return src.To<byte>(defaultValue, new Func<string, byte>(StringUtil.ToByte));
        }

        public static byte? ToByteN(this string src)
        {
            return src.ToN<byte>(new Func<string, byte>(StringUtil.ToByte));
        }

        public static byte? ToByteN(this string src, byte defaultValue)
        {
            return src.ToN<byte>(defaultValue, new Func<string, byte>(StringUtil.ToByte));
        }

        public static char ToChar(this string src)
        {
            return src.To<char>(new Func<string, char>(char.Parse));
        }

        public static char ToChar(this string src, char defaultValue)
        {
            return src.To<char>(defaultValue, new Func<string, char>(StringUtil.ToChar));
        }

        public static char? ToCharN(this string src)
        {
            return src.ToN<char>(new Func<string, char>(StringUtil.ToChar));
        }

        public static char? ToCharN(this string src, char defaultValue)
        {
            return src.ToN<char>(defaultValue, new Func<string, char>(StringUtil.ToChar));
        }

        public static DateTime ToDateTime(this string src)
        {
            return src.To<DateTime>(new Func<string, DateTime>(DateTime.Parse));
        }

        public static DateTime ToDateTime(this string src, DateTime defaultValue)
        {
            return src.To<DateTime>(defaultValue, new Func<string, DateTime>(StringUtil.ToDateTime));
        }

        public static DateTime ToDateTime(this string src, string format)
        {
            return DateTime.ParseExact(src, format, null);
        }

        public static DateTime ToDateTime(this string src, string format, DateTime defaultValue)
        {
            try
            {
                return (string.IsNullOrEmpty(src) ? defaultValue : src.ToDateTime(format));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime? ToDateTimeN(this string src)
        {
            return src.ToN<DateTime>(new Func<string, DateTime>(StringUtil.ToDateTime));
        }

        public static DateTime? ToDateTimeN(this string src, DateTime defaultValue)
        {
            return src.ToN<DateTime>(defaultValue, new Func<string, DateTime>(StringUtil.ToDateTime));
        }

        public static DateTime? ToDateTimeN(this string src, string format)
        {
            DateTime? nullable = null;
            if (!string.IsNullOrEmpty(src))
            {
                nullable = new DateTime?(src.ToDateTime(format));
            }
            return nullable;
        }

        public static DateTime? ToDateTimeN(this string src, string format, DateTime defaultValue)
        {
            DateTime? nullable = null;
            if (!string.IsNullOrEmpty(src))
            {
                nullable = new DateTime?(src.ToDateTime(format, defaultValue));
            }
            return nullable;
        }

        public static decimal ToDecimal(this string src)
        {
            return src.To<decimal>(new Func<string, decimal>(decimal.Parse));
        }

        public static decimal ToDecimal(this string src, decimal defaultValue)
        {
            return src.To<decimal>(defaultValue, new Func<string, decimal>(StringUtil.ToDecimal));
        }

        public static decimal? ToDecimalN(this string src)
        {
            return src.ToN<decimal>(new Func<string, decimal>(StringUtil.ToDecimal));
        }

        public static decimal? ToDecimalN(this string src, decimal defaultValue)
        {
            return src.ToN<decimal>(defaultValue, new Func<string, decimal>(StringUtil.ToDecimal));
        }

        public static double ToDouble(this string src)
        {
            return src.To<double>(new Func<string, double>(double.Parse));
        }

        public static double ToDouble(this string src, double defaultValue)
        {
            return src.To<double>(defaultValue, new Func<string, double>(StringUtil.ToDouble));
        }

        public static double? ToDoubleN(this string src)
        {
            return src.ToN<double>(new Func<string, double>(StringUtil.ToDouble));
        }

        public static double? ToDoubleN(this string src, double defaultValue)
        {
            return src.ToN<double>(defaultValue, new Func<string, double>(StringUtil.ToDouble));
        }

        public static T ToEnum<T>(this string src) where T : struct
        {
            return (T)Enum.Parse(typeof(T), src, true);
        }

        public static T ToEnum<T>(this string src, T defaultValue) where T : struct
        {
            return src.To<T>(defaultValue, new Func<string, T>(StringUtil.ToEnum<T>));
        }

        public static T? ToEnumN<T>(this string src) where T : struct
        {
            return src.ToN<T>(new Func<string, T>(StringUtil.ToEnum<T>));
        }

        public static T? ToEnumN<T>(this string src, T defaultValue) where T : struct
        {
            return src.ToN<T>(defaultValue, new Func<string, T>(StringUtil.ToEnum<T>));
        }

        public static short ToInt16(this string src)
        {
            return src.To<short>(new Func<string, short>(short.Parse));
        }

        public static short ToInt16(this string src, short defaultValue)
        {
            return src.To<short>(defaultValue, new Func<string, short>(StringUtil.ToInt16));
        }

        public static short? ToInt16N(this string src)
        {
            return src.ToN<short>(new Func<string, short>(StringUtil.ToInt16));
        }

        public static short? ToInt16N(this string src, short defaultValue)
        {
            return src.ToN<short>(defaultValue, new Func<string, short>(StringUtil.ToInt16));
        }

        public static int ToInt32(this string src)
        {
            return src.To<int>(new Func<string, int>(int.Parse));
        }

        public static int ToInt32(this string src, int defaultValue)
        {
            return src.To<int>(defaultValue, new Func<string, int>(StringUtil.ToInt32));
        }

        public static int? ToInt32N(this string src)
        {
            return src.ToN<int>(new Func<string, int>(StringUtil.ToInt32));
        }

        public static int? ToInt32N(this string src, int defaultValue)
        {
            return src.ToN<int>(defaultValue, new Func<string, int>(StringUtil.ToInt32));
        }

        public static long ToInt64(this string src)
        {
            return src.To<long>(new Func<string, long>(long.Parse));
        }

        public static long ToInt64(this string src, long defaultValue)
        {
            return src.To<long>(defaultValue, new Func<string, long>(StringUtil.ToInt64));
        }

        public static long? ToInt64N(this string src)
        {
            return src.ToN<long>(new Func<string, long>(StringUtil.ToInt64));
        }

        public static long? ToInt64N(this string src, long defaultValue)
        {
            return src.ToN<long>(defaultValue, new Func<string, long>(StringUtil.ToInt64));
        }

        public static T? ToN<T>(this string src) where T : struct
        {
            T? nullable = null;
            if (src != null)
            {
                nullable = new T?(src.To<T>());
            }
            return nullable;
        }

        public static T? ToN<T>(this string src, IFormatProvider provider) where T : struct
        {
            T? nullable = null;
            if (src != null)
            {
                nullable = new T?(src.To<T>(provider));
            }
            return nullable;
        }

        public static T? ToN<T>(this string src, T defaultValue) where T : struct
        {
            T? nullable = null;
            if (src != null)
            {
                nullable = new T?(src.To<T>(defaultValue));
            }
            return nullable;
        }

        public static T? ToN<T>(this string src, Func<string, T> converter) where T : struct
        {
            T? nullable = null;
            if (src != null)
            {
                nullable = new T?(src.To<T>(converter));
            }
            return nullable;
        }

        public static T? ToN<T>(this string src, T defaultValue, Func<string, T> converter) where T : struct
        {
            T? nullable = null;
            if (src != null)
            {
                nullable = new T?(src.To<T>(defaultValue, converter));
            }
            return nullable;
        }

        public static T? ToN<T>(this string src, T defaultValue, IFormatProvider provider) where T : struct
        {
            T? nullable = null;
            if (src != null)
            {
                nullable = new T?(src.To<T>(defaultValue, provider));
            }
            return nullable;
        }

        public static sbyte ToSByte(this string src)
        {
            return src.To<sbyte>(new Func<string, sbyte>(sbyte.Parse));
        }

        public static sbyte ToSByte(this string src, sbyte defaultValue)
        {
            return src.To<sbyte>(defaultValue, new Func<string, sbyte>(StringUtil.ToSByte));
        }

        public static sbyte? ToSByteN(this string src)
        {
            return src.ToN<sbyte>(new Func<string, sbyte>(StringUtil.ToSByte));
        }

        public static sbyte? ToSByteN(this string src, sbyte defaultValue)
        {
            return src.ToN<sbyte>(defaultValue, new Func<string, sbyte>(StringUtil.ToSByte));
        }

        public static float ToSingle(this string src)
        {
            return src.To<float>(new Func<string, float>(float.Parse));
        }

        public static float ToSingle(this string src, float defaultValue)
        {
            return src.To<float>(defaultValue, new Func<string, float>(StringUtil.ToSingle));
        }

        public static float? ToSingleN(this string src)
        {
            return src.ToN<float>(new Func<string, float>(StringUtil.ToSingle));
        }

        public static float? ToSingleN(this string src, float defaultValue)
        {
            return src.ToN<float>(defaultValue, new Func<string, float>(StringUtil.ToSingle));
        }

        public static ushort ToUInt16(this string src)
        {
            return src.To<ushort>(new Func<string, ushort>(ushort.Parse));
        }

        public static ushort ToUInt16(this string src, ushort defaultValue)
        {
            return src.To<ushort>(defaultValue, new Func<string, ushort>(StringUtil.ToUInt16));
        }

        public static ushort? ToUInt16N(this string src)
        {
            return src.ToN<ushort>(new Func<string, ushort>(StringUtil.ToUInt16));
        }

        public static ushort? ToUInt16N(this string src, ushort defaultValue)
        {
            return src.ToN<ushort>(defaultValue, new Func<string, ushort>(StringUtil.ToUInt16));
        }

        public static uint ToUInt32(this string src)
        {
            return src.To<uint>(new Func<string, uint>(uint.Parse));
        }

        public static uint ToUInt32(this string src, uint defaultValue)
        {
            return src.To<uint>(defaultValue, new Func<string, uint>(StringUtil.ToUInt32));
        }

        public static uint? ToUInt32N(this string src)
        {
            return src.ToN<uint>(new Func<string, uint>(StringUtil.ToUInt32));
        }

        public static uint? ToUInt32N(this string src, uint defaultValue)
        {
            return src.ToN<uint>(defaultValue, new Func<string, uint>(StringUtil.ToUInt32));
        }

        public static ulong ToUInt64(this string src)
        {
            return src.To<ulong>(new Func<string, ulong>(ulong.Parse));
        }

        public static ulong ToUInt64(this string src, ulong defaultValue)
        {
            return src.To<ulong>(defaultValue, new Func<string, ulong>(StringUtil.ToUInt64));
        }

        public static ulong? ToUInt64N(this string src)
        {
            return src.ToN<ulong>(new Func<string, ulong>(StringUtil.ToUInt64));
        }

        public static ulong? ToUInt64N(this string src, ulong defaultValue)
        {
            return src.ToN<ulong>(defaultValue, new Func<string, ulong>(StringUtil.ToUInt64));
        }

        public static string TrimEnd(this string src, string end, bool ignoreCase = true)
        {
            string str = src;
            while (str.EndsWith(end, ignoreCase, null))
            {
                str = str.Substring(0, str.Length - end.Length);
            }
            return str;
        }

        public static string TrimStart(this string src, string start, bool ignoreCase = true)
        {
            string str = src;
            while (str.StartsWith(start, ignoreCase, null))
            {
                str = str.Substring(start.Length);
            }
            return str;
        }

        public static Dictionary<char, int> NumeralRadixCache
        {
            get
            {
                return NumeralRadixCacheLazy.Value;
            }
        }
    }
}
