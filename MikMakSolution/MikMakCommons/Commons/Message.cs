using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MikMakCommons
{
    /// <summary>
    /// This class is used for displaying a human readable state of the Game.
    /// You are always free to use a classic message or implement your own.
    /// </summary>
    public class Message
    {
        public int Id { get; set; }
        public String Information { get; set; }

        public static Message GetMessage(Enum classicMessage)
        {
            return new Message { Id = Convert.ToInt32(classicMessage), Information = classicMessage.Description() };
        }
    }

    public enum ClassicMessage
    {
        [Description("Default Message.")]
        Default,
        [Description("Move ok.")]
        MoveOk,
        [Description("Invalid move, It is not your turn.")]
        NotYourTurn,
        [Description("Invalid move, Position not empty.")]
        NotEmptyPosition,
        [Description("Game finished")]
        GameFinished
    }

    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute),
                                                       false);
            return attributes.Length == 0
                ? value.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}

