using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerbaseScraper.Classes
{
    public class Player
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Position { get; set; }

        public string Team { get; set; }

        public Player(string[] names, string team, string position)
        {
            SetNames(names);
            SetTeam(team);
            SetPosition(position);
        }

        private void SetNames(string[] names)
        {
            FirstName = names[0];

            if (names.Length > 1)
            {
                SecondName = names[1];
            }
            if (names.Length > 2)
            {
                SecondName = string.Format("{0} {1}", SecondName, names[2]);
            }
        }

        private void SetTeam(string team)
        {
            Team = team;
        }

        private void SetPosition(string position)
        {
            switch (position)
            {
                case "G":
                    Position = "GK";
                    break;
                case "D":
                    Position = "DEF";
                    break;
                case "M":
                    Position = "MID";
                    break;
                case "F":
                    Position = "FWD";
                    break;
                default:
                    Position = position;
                    break;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1} {2} {3}", SecondName, FirstName, Position, Team);
        }
    }

}
