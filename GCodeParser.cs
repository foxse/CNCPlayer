using System;
using System.Globalization;

namespace SceneSample
{
    public static class GCodeParser
    {
        public static GCommand Parse(string text, float scale = 1)
        {
            try
            {
                var result = new GCommand();

                var arr = text.Split(' ');

                result.LineNumber = int.Parse(arr[0].Trim('N'));
                result.Command = ParseCommandType(arr[1]);

                string coordinates = string.Empty;

                for (int i = 2; i < arr.Length; i++)
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

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static CommandType ParseCommandType(string text)
        {
            switch (text.ToUpper())
            {
                case "G0":
                    return CommandType.G0;
                case "G1":
                    return CommandType.G1;
                case "G01":
                    return CommandType.G01;
                case "G02":
                    return CommandType.G02;
                case "G03":
                    return CommandType.G03;
                default:
                    throw new Exception("Unknown command");
            }
        }
    }
}
