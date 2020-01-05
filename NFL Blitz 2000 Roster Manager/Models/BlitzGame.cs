using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFL_Blitz_2000_Roster_Manager.Models
{
    [Serializable]
    public class BlitzGame
    {
        public virtual long TeamNameOffsetStart { get; set; }
        public virtual long TeamCityOffsetStart { get; set; }
        public virtual long TeamSelectScreenNameOffsetStart { get; set; }
        public virtual long TeamNameAbbreviationOffsetStart { get; set; }
        public virtual long TeamCityAbbreviationOffsetStart { get; set; }
        public virtual long PassingRatingOffsetStart { get; set; }
        public virtual long RushingRatingOffsetStart { get; set; }
        public virtual long LinemenRatingOffsetStart { get; set; }
        public virtual long DefenseRatingOffsetStart { get; set; }
        public virtual long SpecialTeamsRatingOffsetStart { get; set; }

        public virtual int TeamNameOffsetIncrement { get; set; }
        public virtual int TeamCityOffsetIncrement { get; set; }
        public virtual int TeamSelectScreenNameOffsetIncrement { get; set; }
        public virtual int TeamNameAbbreviationOffsetIncrement { get; set; }
        public virtual int TeamCityAbbreviationOffsetIncrement { get; set; }
        public virtual int PassingRatingOffsetIncrement { get; set; }
        public virtual int RushingRatingOffsetIncrement { get; set; }
        public virtual int LinemenRatingOffsetIncrement { get; set; }
        public virtual int DefenseRatingOffsetIncrement { get; set; }
        public virtual int SpecialTeamsRatingOffsetIncrement { get; set; }

        public virtual long PlayerNumberOffsetStart { get; set; }
        public virtual long PlayerLastNameOffsetStart { get; set; }
        public virtual long PlayerFirstNameOffsetStart { get; set; }
        public virtual long PlayerSkinColorOffsetStart { get; set; }
        public virtual long PlayerWeightOffsetStart { get; set; }
        public virtual long PlayerLuckOffsetStart { get; set; }

        public virtual int PlayerNumberOffsetIncrement { get; set; }
        public virtual int PlayerLastNameOffsetIncrement { get; set; }
        public virtual int PlayerFirstNameOffsetIncrement { get; set; }
        public virtual int PlayerSkinColorOffsetIncrement { get; set; }
        public virtual int PlayerWeightOffsetIncrement { get; set; }
        public virtual int PlayerLuckOffsetIncrement { get; set; }
        public virtual int TeamOffsetIncrement { get; set; }

        public virtual long FileListOffset { get; set; }
        public virtual string SystemName { get; set; }
        public virtual string GameName { get; set; }
        public virtual int GameTeamCount { get; set; }
        public virtual int PlayerPerTeam { get; set; }

        public virtual long BinRomIndex { get; set; }
        public virtual int maxFileNameLenght { get; set; }
        public virtual int filePositionLength { get; set; }
        public virtual int decompressedLenght { get; set; }
        public virtual int compressedLenght { get; set; }

        public virtual bool ExpandableTeams { get; set; }

    }
}
