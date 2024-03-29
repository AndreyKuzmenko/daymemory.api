﻿using System;

namespace DayMemory.Core.Services
{
    public static class StringUtils
    {
        public static string GenerateUniqueString()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}
