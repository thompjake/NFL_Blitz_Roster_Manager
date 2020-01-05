using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFL_Blitz_2000_Roster_Manager.Models;
using System.IO;
using System.Globalization;

namespace NFL_Blitz_2000_Roster_Manager.Helpers
{
    public static class RomEditor
    {
        public static Teams ReadRom(string filePath, BlitzGame gameSystem, int numberOfTeams)
        {
            // Hack, set number of teams to 160
            //This part below was added to work with Zoinkity's patches which allows expandable teams
            if (gameSystem.ExpandableTeams)
            {
                numberOfTeams = 160;
            }
            Teams teams = new Teams();
            BlitzGame gameSystemClone = Clone.DeepClone<BlitzGame>(gameSystem);
            if (gameSystemClone.PlayerPerTeam == 0)
            {
                gameSystemClone.PlayerPerTeam = 16;
            }
            for (int x = 0; x < numberOfTeams; x++)
            {
                using (var fs = new FileStream(filePath,
   FileMode.Open,
   FileAccess.Read))
                {
                    //Get Team Name
                    Team team = new Team();
                    team.TeamName = ReadStringTo00(fs, gameSystemClone.TeamNameOffsetStart + x * gameSystemClone.TeamNameOffsetIncrement);
                    if (gameSystemClone.TeamCityOffsetStart != 0)
                        team.City = ReadStringTo00(fs, gameSystemClone.TeamCityOffsetStart + x * gameSystemClone.TeamCityOffsetIncrement);
                    if (gameSystemClone.TeamSelectScreenNameOffsetStart != 0)
                        team.SelectScreenName = ReadStringTo00(fs, gameSystemClone.TeamSelectScreenNameOffsetStart + x * gameSystemClone.TeamSelectScreenNameOffsetIncrement);
                    if (gameSystemClone.TeamNameAbbreviationOffsetStart != 0)
                        team.TeamAbbreviation = ReadStringTo00(fs, gameSystemClone.TeamNameAbbreviationOffsetStart + x * gameSystemClone.TeamNameAbbreviationOffsetIncrement);
                    if (gameSystemClone.TeamCityAbbreviationOffsetStart != 0)
                        team.CityAbbreviation = ReadStringTo00(fs, gameSystemClone.TeamCityAbbreviationOffsetStart + x * gameSystemClone.TeamCityAbbreviationOffsetIncrement);
                    // If we don't have team name it means we've reached the end
                    if (string.IsNullOrEmpty(team.TeamName))
                        break;
                    long endingPosition = fs.Position;
                    fs.Position = gameSystemClone.PassingRatingOffsetStart + x * gameSystemClone.PassingRatingOffsetIncrement;
                    team.PassingRatingOffset = fs.Position.ToString();
                    team.PassingRating = int.Parse(BitConverter.ToString(new byte[] { (byte)fs.ReadByte() }));

                    fs.Position = gameSystemClone.DefenseRatingOffsetStart + x * gameSystemClone.DefenseRatingOffsetIncrement;
                    team.DefenseRatingOffset = fs.Position.ToString();
                    team.DefenseRating = int.Parse(BitConverter.ToString(new byte[] { (byte)fs.ReadByte() }));

                    fs.Position = gameSystemClone.LinemenRatingOffsetStart + x * gameSystemClone.LinemenRatingOffsetIncrement;
                    team.LinemenRatingOffset = fs.Position.ToString();
                    team.LinemenRating = int.Parse(BitConverter.ToString(new byte[] { (byte)fs.ReadByte() }));

                    fs.Position = gameSystemClone.RushingRatingOffsetStart + x * gameSystemClone.RushingRatingOffsetIncrement;
                    team.RushingRatingOffset = fs.Position.ToString();
                    team.RushingRating = int.Parse(BitConverter.ToString(new byte[] { (byte)fs.ReadByte() }));

                    fs.Position = gameSystemClone.SpecialTeamsRatingOffsetStart + x * gameSystemClone.SpecialTeamsRatingOffsetIncrement;
                    team.SpecialTeamsRatingOffset = fs.Position.ToString();
                    team.SpecialTeamsRating = int.Parse(BitConverter.ToString(new byte[] { (byte)fs.ReadByte() }));

                    team.TeamPlayers = new Players();
                    teams.Add(team);
                    fs.Position = endingPosition;
                }
            }
            int offsetCount = 0;
            for (int z = 0; z < teams.Count; z++)
            {
                for (int y = 0; y < gameSystemClone.PlayerPerTeam; y++)
                {
                    Player player = new Player();
                    bool isLineman = false;
                    using (var fs = new FileStream(filePath,
        FileMode.Open,
        FileAccess.Read))
                    {
                        byte thisByte = 01;
                        List<Byte> listOfBytes = new List<byte>();

                        //Check for player name, if none its a lineman
                        fs.Position = gameSystemClone.PlayerLastNameOffsetStart + offsetCount * gameSystemClone.PlayerLastNameOffsetIncrement;
                        thisByte = byte.Parse(fs.ReadByte().ToString());
                        if (thisByte == 00)
                        {
                            isLineman = true;
                        }

                        if (!isLineman)
                        {
                            if (gameSystemClone.PlayerFirstNameOffsetStart != 0)
                            {
                                //Get First Name
                                player.FirstName = ReadStringTo00(fs, gameSystemClone.PlayerFirstNameOffsetStart + offsetCount * gameSystemClone.PlayerFirstNameOffsetIncrement);
                            }
                            //Get Last Name
                            player.LastName = ReadStringTo00(fs, gameSystemClone.PlayerLastNameOffsetStart + offsetCount * gameSystemClone.PlayerLastNameOffsetIncrement);
                        }
                        //get Jersey Number
                        fs.Position = gameSystemClone.PlayerNumberOffsetStart + offsetCount * gameSystemClone.PlayerNumberOffsetIncrement;
                        player.NumberOffset = fs.Position.ToString();
                        player.Number = int.Parse(BitConverter.ToString(new byte[] { (byte)fs.ReadByte() }));

                        //get size
                        fs.Position = gameSystemClone.PlayerWeightOffsetStart + offsetCount * gameSystemClone.PlayerWeightOffsetIncrement;
                        player.WeightOffset = fs.Position;
                        player.Weight = Byte.Parse((BitConverter.ToString(new byte[] { (byte)fs.ReadByte() })));

                        //get skin color
                        fs.Position = gameSystemClone.PlayerSkinColorOffsetStart + offsetCount * gameSystemClone.PlayerSkinColorOffsetIncrement;
                        player.SkinColorOffset = fs.Position.ToString();
                        player.SkinColor = Byte.Parse((BitConverter.ToString(new byte[] { (byte)fs.ReadByte() })));

                        //get luck
                        fs.Position = gameSystemClone.PlayerLuckOffsetStart + offsetCount * gameSystemClone.PlayerLuckOffsetIncrement;
                        player.Luck = Byte.Parse((BitConverter.ToString(new byte[] { (byte)fs.ReadByte() })));

                        //add to team
                        teams[z].TeamPlayers.Add(player);
                    }
                    offsetCount++;
                }

                //This part below was added to work with Zoinkity's patches
                if (gameSystemClone.TeamOffsetIncrement > 0)
                {
                    offsetCount = 0;
                    //   gameSystem.PlayerFirstNameOffsetStart += gameSystem.TeamOffsetIncrement;
                    gameSystemClone.PlayerLastNameOffsetStart += gameSystemClone.TeamOffsetIncrement;
                    gameSystemClone.PlayerLuckOffsetStart += gameSystemClone.TeamOffsetIncrement;
                    gameSystemClone.PlayerNumberOffsetStart += gameSystemClone.TeamOffsetIncrement;
                    gameSystemClone.PlayerSkinColorOffsetStart += gameSystemClone.TeamOffsetIncrement;
                    gameSystemClone.PlayerWeightOffsetStart += gameSystemClone.TeamOffsetIncrement;
                }
            }

            return teams;
        }


        public static void SaveToRom(string filePath, BlitzGame gameSystem, Teams teams)
        {
            BlitzGame gameSystemClone = Clone.DeepClone<BlitzGame>(gameSystem);

            if (gameSystemClone.PlayerPerTeam == 0)
            {
                gameSystemClone.PlayerPerTeam = 16;
            }

            using (var fs = new FileStream(filePath,
FileMode.OpenOrCreate,
FileAccess.ReadWrite))
            {
                for (int x = 0; x < gameSystem.GameTeamCount; x++)
                {
                    WriteStringToFile(fs, teams[x].TeamName, filePath, gameSystemClone.TeamNameOffsetStart + x * gameSystemClone.TeamNameOffsetIncrement);
                    if (gameSystemClone.TeamCityOffsetStart != 0)
                        WriteStringToFile(fs, teams[x].City, filePath, gameSystemClone.TeamCityOffsetStart + x * gameSystemClone.TeamCityOffsetIncrement);
                    if (gameSystemClone.TeamSelectScreenNameOffsetStart != 0)
                        WriteStringToFile(fs, teams[x].SelectScreenName, filePath, gameSystemClone.TeamSelectScreenNameOffsetStart + x * gameSystemClone.TeamSelectScreenNameOffsetIncrement);
                    if (gameSystemClone.TeamNameAbbreviationOffsetStart != 0)
                        WriteStringToFile(fs, teams[x].TeamAbbreviation, filePath, gameSystemClone.TeamNameAbbreviationOffsetStart + x * gameSystemClone.TeamNameAbbreviationOffsetIncrement);
                    if (gameSystemClone.TeamCityAbbreviationOffsetStart != 0)
                        WriteStringToFile(fs, teams[x].CityAbbreviation, filePath, gameSystemClone.TeamCityAbbreviationOffsetStart + x * gameSystemClone.TeamCityAbbreviationOffsetIncrement);
                    WriteIntToFile(fs, teams[x].PassingRating, filePath, gameSystemClone.PassingRatingOffsetStart + x * gameSystemClone.PassingRatingOffsetIncrement);
                    WriteIntToFile(fs, teams[x].LinemenRating, filePath, gameSystemClone.LinemenRatingOffsetStart + x * gameSystemClone.LinemenRatingOffsetIncrement);
                    WriteIntToFile(fs, teams[x].DefenseRating, filePath, gameSystemClone.DefenseRatingOffsetStart + x * gameSystemClone.DefenseRatingOffsetIncrement);
                    WriteIntToFile(fs, teams[x].RushingRating, filePath, gameSystemClone.RushingRatingOffsetStart + x * gameSystemClone.RushingRatingOffsetIncrement);
                    WriteIntToFile(fs, teams[x].SpecialTeamsRating, filePath, gameSystemClone.SpecialTeamsRatingOffsetStart + x * gameSystemClone.SpecialTeamsRatingOffsetIncrement);
                }
                int offsetCount = 0;
                for (int z = 0; z < gameSystem.GameTeamCount; z++)
                {
                    for (int y = 0; y < gameSystemClone.PlayerPerTeam; y++)
                    {
                        bool isLineman = false;

                        if (string.IsNullOrEmpty(teams[z].TeamPlayers[y].LastName))
                        {
                            isLineman = true;
                        }


                        if (!isLineman)
                        {
                            if (gameSystemClone.PlayerFirstNameOffsetStart > 0)
                            {
                                //Write First Name
                                WriteStringToFile(fs, teams[z].TeamPlayers[y].FirstName, filePath, gameSystemClone.PlayerFirstNameOffsetStart + offsetCount * gameSystemClone.PlayerFirstNameOffsetIncrement);
                            }

                            //Write Last Name
                            WriteStringToFile(fs, teams[z].TeamPlayers[y].LastName, filePath, gameSystemClone.PlayerLastNameOffsetStart + offsetCount * gameSystemClone.PlayerLastNameOffsetIncrement);
                        }

                        //Write Jersey Number
                        WriteIntToFile(fs, teams[z].TeamPlayers[y].Number, filePath, gameSystemClone.PlayerNumberOffsetStart + offsetCount * gameSystemClone.PlayerNumberOffsetIncrement);

                        //Write size
                        WriteIntToFile(fs, teams[z].TeamPlayers[y].Weight, filePath, gameSystemClone.PlayerWeightOffsetStart + offsetCount * gameSystemClone.PlayerWeightOffsetIncrement);

                        //Write skin color
                        WriteIntToFile(fs, teams[z].TeamPlayers[y].SkinColor, filePath, gameSystemClone.PlayerSkinColorOffsetStart + offsetCount * gameSystemClone.PlayerSkinColorOffsetIncrement);

                        //Write Luck
                        WriteIntToFile(fs, teams[z].TeamPlayers[y].Luck, filePath, gameSystemClone.PlayerLuckOffsetStart + offsetCount * gameSystemClone.PlayerLuckOffsetIncrement);

                        offsetCount++;
                    }
                    //This part below was added to work with Zoinkity's patches
                    if (gameSystemClone.TeamOffsetIncrement > 0)
                    {
                        offsetCount = 0;
                        gameSystemClone.PlayerLastNameOffsetStart += gameSystemClone.TeamOffsetIncrement;
                        gameSystemClone.PlayerLuckOffsetStart += gameSystemClone.TeamOffsetIncrement;
                        gameSystemClone.PlayerNumberOffsetStart += gameSystemClone.TeamOffsetIncrement;
                        gameSystemClone.PlayerSkinColorOffsetStart += gameSystemClone.TeamOffsetIncrement;
                        gameSystemClone.PlayerWeightOffsetStart += gameSystemClone.TeamOffsetIncrement;
                    }
                }
            }
        }

        public static string ReadStringTo00(FileStream fs, long offset)
        {
            //Get Team Name
            byte thisByte = 01;
            List<Byte> listOfBytes = new List<byte>();
            fs.Position = offset;
            while (true)
            {
                thisByte = byte.Parse(fs.ReadByte().ToString());
                if (thisByte == 00)
                {
                    break;
                }
                listOfBytes.Add(thisByte);
            }
            return System.Text.Encoding.ASCII.GetString(listOfBytes.ToArray());
        }

        public static void WriteStringToFile(FileStream fs, string toBeWritten, string filePath, long offset, bool writeToBlank = true)
        {
            if (string.IsNullOrEmpty(toBeWritten))
                return;

            fs.Position = offset;
            foreach (char letter in toBeWritten)
            {
                fs.WriteByte(Convert.ToByte(letter));
            }
            //if write to blank is set to true we will write blanks until we hit a blank
            while (true)
            {
                long currentPostiton = fs.Position;
                byte thisByte = byte.Parse(fs.ReadByte().ToString());
                if (thisByte == 00)
                {
                    break;
                }
                fs.Position = currentPostiton;
                fs.WriteByte(00);
            }
        }



        private static void WriteIntToFile(FileStream fs, int toBeWritten, string filePath, long offset)
        {
            fs.Position = offset;
            var numberByte = Byte.Parse(toBeWritten.ToString(), NumberStyles.HexNumber);
            fs.WriteByte((byte)numberByte);
        }


        public static void ReadFileTable(int toBeWritten, string filePath, long offset)
        {

            using (var fs = new FileStream(filePath,
FileMode.Open,
FileAccess.ReadWrite))
            {
                fs.Position = offset;
                var numberByte = Byte.Parse(toBeWritten.ToString(), NumberStyles.HexNumber);
                fs.WriteByte((byte)numberByte);
                fs.Close();
            }
        }


        public static bool ByteArrayToFile(string fileName, byte[] byteArray, int offset)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                int index = offset;
                foreach (byte replacementByte in byteArray)
                {
                    fileBytes[index] = replacementByte;
                    index++;
                }
                File.WriteAllBytes(fileName, fileBytes);
                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }

    }
}
