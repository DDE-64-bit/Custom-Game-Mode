using System;
using System.Collections.Generic;
using System.IO;

namespace CustomGamemode
{
    public class IniParser
    {
        private readonly Dictionary<string, Dictionary<string, string>> data;

        public IniParser(string filePath)
        {
            data = new Dictionary<string, Dictionary<string, string>>();
            Load(filePath);
        }

        private void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Configuration file not found", filePath);
            }

            string currentSection = string.Empty;
            foreach (var line in File.ReadAllLines(filePath))
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue; // Negeer lege regels en opmerkingen
                }

                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();
                    if (!data.ContainsKey(currentSection))
                    {
                        data[currentSection] = new Dictionary<string, string>();
                    }
                }
                else
                {
                    var kvp = trimmedLine.Split(new[] { '=' }, 2);
                    if (kvp.Length == 2)
                    {
                        var key = kvp[0].Trim();
                        var value = kvp[1].Trim();

                        if (!data.ContainsKey(currentSection))
                        {
                            data[currentSection] = new Dictionary<string, string>();
                        }
                        data[currentSection][key] = value;
                    }
                }
            }
        }

        public bool GetValue(string section, string key, bool defaultValue)
        {
            if (data.ContainsKey(section) && data[section].ContainsKey(key))
            {
                if (bool.TryParse(data[section][key], out var result))
                {
                    return result;
                }
            }

            return defaultValue;
        }

        public float GetValue(string section, string key, float defaultValue)
        {
            if (data.ContainsKey(section) && data[section].ContainsKey(key))
            {
                if (float.TryParse(data[section][key], out var result))
                {
                    return result;
                }
            }

            return defaultValue;
        }

        public string GetValue(string section, string key, string defaultValue)
        {
            if (data.ContainsKey(section) && data[section].ContainsKey(key))
            {
                return data[section][key];
            }

            return defaultValue;
        }
    }
}
