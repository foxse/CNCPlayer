using System;
using System.Globalization;

namespace GCode.Core
{
    public static class BaseParser
    {
        private static int _line = 1;

        public static GCommand Parse(string text, float scale, bool lineNumbers)
        {
            try
            {
                var result = new GCommand();

                var arr = text.Split(' ');

                if (arr.Length == 1)
                {
                    lineNumbers = text.StartsWith('G');

                    for (int i=0; i < text.Length; i++)
                    {
                        if (char.IsLetter(text[i]))
                        {
                            text = text.Replace($"{text[i]}", $" {text[i]}");
                            i++;
                        }
                    }

                    arr = text.Split(' ');
                }

                if (lineNumbers && int.TryParse(arr[0].Trim('N'), out var ln))
                    result.LineNumber = ln;
                else
                    result.LineNumber = _line++;

                ParseCommandType(arr[lineNumbers ? 1 : 0]);

                if (_lastGType.HasValue)
                    result.CommandType = _lastGType.Value;
                else
                    return null;

                string coordinates = string.Empty;

                for (int i = lineNumbers ? 2 : 1; i < arr.Length; i++)
                {
                    if (arr[i].Length == 0)
                        continue;

                    var val = arr[i].Trim().ToUpper();

                    switch (val[0])
                    {
                        case 'X':
                            val = val.Trim('X');
                            result.DestinationX = float.Parse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo) * scale;
                            break;
                        case 'Y':
                            val = val.Trim('Y');
                            result.DestinationY = float.Parse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo) * scale;
                            break;
                        case 'Z':
                            val = val.Trim('Z');
                            result.DestinationZ = float.Parse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo) * scale;
                            break;
                        case 'R':
                            val = val.Trim('R');
                            result.ArcRadius = float.Parse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo) * scale;
                            break;
                        default:
                            break;
                    }
                }

                if (result.DestinationX.HasValue || result.DestinationX.HasValue || result.DestinationZ.HasValue || result.ArcRadius != 0)
                { 
                    return result; 
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void DropCounters()
        {
            _line = 1;
        }

        private static GCommandType? _lastGType;

        static GCommandType? ParseCommandType(string text)
        {
            switch (text.ToUpper())
            {
                case "G0":
                    _lastGType = GCommandType.G0;
                    break;
                case "G1":
                    _lastGType = GCommandType.G1;
                    break;
                case "G01":
                    _lastGType = GCommandType.G01;
                    break;
                case "G02":
                    _lastGType = GCommandType.G02;
                    break;
                case "G03":
                    _lastGType = GCommandType.G03;
                    break;
                case "":
                    break;
                default:
                    _lastGType = null;
                    break;
            }
            return _lastGType;
        }
    }
}
