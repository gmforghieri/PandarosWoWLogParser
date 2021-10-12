using System;
using System.Collections.Generic;
using System.Text;

namespace PandarosWoWLogParser
{
    public class UnitFlags
    {
        public enum FlagType
        {
            Mask = 0x0000FC00,
            Object = 0x00004000,
            Guardian = 0x00002000,
            Pet = 0x00001000,
            Npc = 0x00000800,
            Player = 0x00000400
        }
        public enum Controller
        {
            Mask = 0x00000300,
            Npc = 0x00000200,
            Player = 0x00000100
        }
        public enum Reaction
        {
            Mask = 0x000000F0,
            Hostile = 0x00000040,
            Neutral = 0x00000020,
            Friendly = 0x00000010
        }
        public enum ControllerAffiliation
        {
            Mask = 0x0000000F,
            Outsider = 0x00000008,
            Raid = 0x00000004,
            Party = 0x00000002,
            Mine = 0x00000001
        }
        public enum Special
        {
            Mask = unchecked((int)0xFFFF0000),
            None = unchecked((int)0x80000000),
            Raid8 = 0x08000000,
            Raid7 = 0x04000000,
            Raid6 = 0x02000000,
            Raid5 = 0x01000000,
            Raid4 = 0x00800000,
            Raid3 = 0x00400000,
            Raid2 = 0x00200000,
            Raid1 = 0x00100000,
            MainAssist = 0x00080000,
            MainTank = 0x00040000,
            Focus = 0x00020000,
            Target = 0x00010000
        }

        public int Value { get; private set; }

        public UnitFlags(int value)
        {
            this.Value = value;
        }

        public Controller GetController()
        {
            return (Controller)(Value & (int)Controller.Mask);
        }
        public FlagType GetFlagType()
        {
            return (FlagType)(Value & (int)FlagType.Mask);
        }
        public Reaction GetReaction()
        {
            return (Reaction)(Value & (int)Reaction.Mask);
        }
        public ControllerAffiliation GetControllerAffiliation()
        {
            return (ControllerAffiliation)(Value & (int)ControllerAffiliation.Mask);
        }
        public Special GetSpecial()
        {
            return (Special)(Value & (int)Special.Mask);
        }
    }
}
