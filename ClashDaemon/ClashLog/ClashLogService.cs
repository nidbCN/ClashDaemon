﻿using ClashDaemon.ClashLog;
using ClashDaemon.ClashLog.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashDaemon
{
    public class ClashLogService : IClashLogService
    {
        private readonly ILogger<ClashLogService> _logger;

        public ClashLogService(ILogger<ClashLogService> logger)
        {
            _logger = logger;
        }

        public void HandleLog(string? origin)
        {
            if (origin is null) return;

            var originSpan = origin.AsSpan();

            var logDict = new Dictionary<string, string?>();
            var currentPos = 0;
            while (currentPos < originSpan.Length)
            {
                (currentPos, var key, var value)
                    = ReadKeyValuePair(currentPos, originSpan);
                if (currentPos == -1)
                {
                    _logger.LogError("Can not format: {origin}", origin);
                    return;
                }

                logDict.Add(key, value);
                currentPos++;
            }

            switch (logDict["level"])
            {
                case "info":
                    _logger.LogInformation("Clash msg:{msg}",logDict["msg"]);
                    break;
                case "warning":
                    _logger.LogWarning("Clash msg:{msg}", logDict["msg"]);
                    break;
                case "error":
                    _logger.LogError("Clash msg:{msg}", logDict["msg"]);
                    break;
                default:
                    _logger.LogWarning("UnKnow level in: {origin}", origin);
                    break;
            }
        }

        private static (int endPos, string key, string value)
            ReadKeyValuePair(int start, ReadOnlySpan<char> originStr)
        {
            var keyEndIndex = originStr[start..].IndexOf('=');
            if (keyEndIndex == -1)
                return (-1, string.Empty, string.Empty);
            
            keyEndIndex += start;
            var key = originStr[start..keyEndIndex];

            var valueStartIndex = keyEndIndex + 1;
            var (valueEndIndex, value) = ReadValue(valueStartIndex, originStr);

            return (valueEndIndex, key.ToString(), value);
        }

        private static (int pos, string value)
            ReadValue(int start, ReadOnlySpan<char> originStr)
        {
            var valStart = -1;
            var valEnd = -1;
            var spaceEnd = -1;
            for (int i = start; i < originStr.Length; i++)
            {
                var ch = originStr[i];
                if (ch == '\"')
                {
                    if (valStart == -1)
                    {
                        valStart = i;
                    }
                    else
                    {
                        valEnd = i;
                        break;
                    }
                }
                else if (ch == ' ' && valStart == -1)
                {
                    spaceEnd = i;
                    break;
                }
            }

            if (valEnd == -1)
            {
                if (spaceEnd == -1)
                    return (start, "");

                return (spaceEnd, originStr[start..spaceEnd].ToString());
            }

            return (valEnd + 1, originStr[valStart..(valEnd + 1)].ToString());
        }
    }
}
