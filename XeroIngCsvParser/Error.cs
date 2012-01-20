﻿using System;

namespace XeroIngCsvParser
{
    public static class Errors
    {
        public const string IncorrectFile = "Invalid file '{0}' given.";
        public const string IncorrectArguments = "Invalid parameters given; expected a single argument - the filename of a .csv file";
    }
}