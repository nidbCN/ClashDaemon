using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashDaemon
{
    public class ClashLogService
    {
        private readonly ILogger<ClashLogService> _logger;

        public ClashLogService(ILogger<ClashLogService> logger)
        {
            _logger = logger;
        }

        public void HandleLog(object sender, DataReceivedEventArgs e)
        {
            var originLine = e.Data;
            if (originLine is null) return;

            var originSpan = originLine.AsSpan();

            var logDict = new Dictionary<string, string?>();
            var keyPos = 0;
            var currentPos = 0;

            do
            {
                var (keyEndPos, key) = ReadKey(originLine, currentPos);
                if (keyEndPos <= 0)
                {
                    _logger.LogError("Cannot parse clash log, origin message: {origin}", originLine);
                    return;
                }
                var (valEndPos, val) = 

            }while (currentPos < originLine.Length);

            while (currentPos < originLine.Length)
            {


                if (originLine[currentPos] == '=')
                {
                    logDict.Add(originLine.Substring(keyPos, currentPos - 1), null);
                }

                currentPos++;
            }
        }

        private static (int endPos, KeyValuePair<string, string> keyValuePair) 
            ReadKeyValuePair(ReadOnlySpan<char> originStr, int startPos)
        {
            var keyEndIndex = originStr[startPos..].IndexOf('=');
            var key = originStr[startPos..(keyEndIndex - 1)];
            
            var valueEndIndex = originStr[keyEndIndex..].IndexOf(' ');
            var value = originStr[keyEndIndex..(valueEndIndex - 1)];

            return (valueEndIndex, new(key.ToString(), value.ToString()));
        }
    }
}
