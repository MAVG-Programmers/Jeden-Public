using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeden.Engine
{
    public class ConfigFileParser
    {
        Dictionary<String, String> KeyValueMap;

        public ConfigFileParser(String filename)
        {
            KeyValueMap = new Dictionary<string, string>();

            using(StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line[0] == '#')
                        continue;
                    
                    string[] pair = line.Split('=');
                    if (pair.Count() == 2)
                    {
                        pair[0].Trim();
                        pair[1].Trim();
                        KeyValueMap[pair[0]] = pair[1];
                    }
                }
            }
        }

        public float GetAsFloat(String key)
        {
            return float.Parse(KeyValueMap[key]);
        }

        public int GetAsInt(String key)
        {
            return int.Parse(KeyValueMap[key]);
        }

        public String GetAsString(String key)
        {
            return KeyValueMap[key];
        }
    }
}
