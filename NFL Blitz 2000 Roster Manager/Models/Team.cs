using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFL_Blitz_2000_Roster_Manager.Models
{
    [Serializable]
    public class Team
    {
        #region vars and matching properties

        /// <summary>
        /// Name of the Blitz team
        /// </summary>
        public string TeamName { get;  set; }
        public string City { get; set; }
        public string SelectScreenName { get; set; }
        public string TeamAbbreviation { get; set; }
        public string CityAbbreviation  {get; set;}

        /// <summary>
        /// Passing stat/rating of the Blitz team
        /// </summary>
        public int PassingRating { get; set; }
        public string PassingRatingOffset { get; set; }

        /// <summary>
        /// Rushing stat/rating of the Blitz team
        /// </summary>
        public int RushingRating { get; set; }
        public string RushingRatingOffset { get; set; }

        /// <summary>
        /// Linemen stat/rating of the Blitz team
        /// </summary>
        public int LinemenRating { get; set; }
        public string LinemenRatingOffset { get; set; }

        /// <summary>
        /// Defense stat/rating of the Blitz team
        /// </summary>
        public int DefenseRating { get; set; }
        public string DefenseRatingOffset { get; set; }

        /// <summary>
        /// Special Team stat/rating of the Blitz team
        /// </summary>
        public int SpecialTeamsRating { get; set; }
        public string SpecialTeamsRatingOffset { get; set; }

        /// <summary>
        /// All the players on the team 
        /// </summary>
        public Players TeamPlayers { get; set; }

        #endregion



    }
}
