using Caliburn.Micro;
using GCode.Core;

namespace GCodePlayer3D.ViewModels
{
    public class GCommandViewModel : Screen
    {
        public GCommand Command { get; }

        public GCommandViewModel(GCommand command) 
        {
            Command = command;
        }

        public string CommandType => Command.CommandType.ToString();

        public float? X => Command.DestinationX;
        public float? Y => Command.DestinationY;
        public float? Z => Command.DestinationZ;

        public string CurrentPos => Command.CurrentPos?.ToString();
    }
}