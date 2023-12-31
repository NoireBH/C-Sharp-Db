﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.Villain_Names
{
    public class SqlQueries
    {
        public const string getVillainNames =
     @" SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
        FROM Villains AS v 
        JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
        GROUP BY v.Id, v.Name 
        HAVING COUNT(mv.VillainId) > 3 
        ORDER BY COUNT(mv.VillainId)";

        public const string getVillainNameById =
            "SELECT Name FROM Villains WHERE Id = @Id";

        public const string getMinionNamesAndAgeByVillainId =

         @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

        public const string getMinionIdByName =
            @"  SELECT Id FROM Minions WHERE Name = @Name";

        public const string addNewMinion = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

        public const string setMinionToVillain = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

        public const string getVillainIdByName = @"SELECT Id FROM Villains WHERE Name = @Name";

        public const string addVillainWithDefaultVillainFactor = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

        public const string getTownIdByName =
            @"SELECT Id FROM Towns WHERE Name = @townName";

        public const string addNewTown =
            @"INSERT INTO Towns (Name) VALUES (@townName);";

        public const string changeTownNameCasingByCountry =
            @"UPDATE Towns
               SET Name = UPPER(Name)
             WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

        public const string getTownNames =
            @"SELECT t.Name 
               FROM Towns as t
               JOIN Countries AS c ON c.Id = t.CountryCode
              WHERE c.Name = @countryName";

        public const string removeVillainById =          
            @"DELETE FROM Villains
              WHERE Id = @villainId";

        public const string releaseMinionsFromVillain =
            @"DELETE FROM MinionsVillains 
              WHERE VillainId = @villainId";

        public const string printMinionNames = @"SELECT Name FROM Minions";

        public const string increaseMinionAge =
            @"UPDATE Minions
               SET Name = LOWER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
             WHERE Id = @Id";

        public const string printMinionNameAndAge = @"SELECT Name, Age FROM Minions";

        public const string increaseAgeStoredProcedure =
            @"GO
            CREATE PROC usp_GetOlder @id INT
            AS
            UPDATE Minions
               SET Age += 1
             WHERE Id = @id

            SELECT Name, Age FROM Minions WHERE Id = @Id";
    }
}
