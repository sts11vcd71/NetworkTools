using System;
using System.Collections.Generic;

namespace FlexibleProxy.Utilities
{
    public class ArgumentsParser
    {
        private Dictionary<string, object> AllArguments { get; set; }
  
        public ArgumentsParser(string[] arguments)
        {
            AllArguments = new Dictionary<string, object>();
            var argsMerged = String.Join(" ", arguments);
            argsMerged = " " + argsMerged;
            argsMerged = argsMerged.Replace(" -", "\t");
            argsMerged = argsMerged.Replace(" /", "\t");
            var argsChunks = argsMerged.Split('\t');
            foreach (var argsChunk in argsChunks)
            {
                var firstSpacePosition = argsChunk.IndexOf(' ');
                if (firstSpacePosition == -1)
                {
                    AllArguments.Add(argsChunk, true);
                }
                else
                {
                    AllArguments.Add(argsChunk.Substring(0, firstSpacePosition), argsChunk.Substring(firstSpacePosition+1));
                }
            }
        }

        public T GetValue<T>(string argumentName)
        {
            object val;
            if (!AllArguments.TryGetValue(argumentName, out val)) return default(T);
            if (val.GetType() == typeof (T))
                return (T) val;
            return (T)Convert.ChangeType(val, typeof (T));
        }
    }
}
