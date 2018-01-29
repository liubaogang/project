using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Net.Boot.Configs
{
    internal class ConfigsLoad : IBootStrategy
    {
        public void Run()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "microsoftnet.properties");
            if (!File.Exists(file)) return;
            var data = new List<string>();
            try
            {
                using (var reader = new StreamReader(file, Encoding.UTF8))
                {
                    while (reader.Peek() > -1)
                    {
                        var line = reader.ReadLine();

                        if (string.IsNullOrWhiteSpace(line) || Regex.IsMatch(line, "^\\s*#"))
                            continue;
                        data.Add(line.Trim());
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format("读取配置文件信息时出现异常,文件路径为{0}", file), e);
            }
            if (data.Count == 0) return;
            foreach (var line in data)
            {
                var items = Split(line);
                if (items == null || string.IsNullOrWhiteSpace(items.Item1) || string.IsNullOrWhiteSpace(items.Item2))
                    throw new InvalidOperationException(string.Format("读取配置文件信息时出现异常,信息“{0}”不是有效的配置项；配置文件路径为{1}", line, file));
                if (ConfigsData.Instance.Contains(items.Item1))
                    throw new InvalidOperationException(string.Format("读取配置文件信息时出现异常,存在相同的名称“{0}”；配置文件路径为{1}", items.Item1, file));
                ConfigsData.Instance.Add(items.Item1, items.Item2);
            }
        }

        internal static Tuple<string, string> Split(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;
            var signIndex = line.IndexOf('=');
            return signIndex == -1 ? null : Tuple.Create(line.Substring(0, signIndex), signIndex == line.Length + 1 ? string.Empty : line.Substring(signIndex + 1));
        }
    }
}
