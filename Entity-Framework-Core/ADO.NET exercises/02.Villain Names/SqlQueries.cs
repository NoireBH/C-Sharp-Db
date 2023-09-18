using System;
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

        public const string getMinionNames = 
     @"SELECT Name FROM Villains WHERE Id = @Id

        SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

        public const string addMinion =
            @"SELECT Id FROM Villains WHERE Name = @Name
                SELECT Id FROM Minions WHERE Name = @Name
                INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)
                INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)
                INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)
                INSERT INTO Towns (Name) VALUES (@townName)
                SELECT Id FROM Towns WHERE Name = @townName";

        public const string changeTownNameCasing =
            @"UPDATE Towns
               SET Name = UPPER(Name)
             WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)

             SELECT t.Name 
               FROM Towns as t
               JOIN Countries AS c ON c.Id = t.CountryCode
              WHERE c.Name = @countryName";

        public const string removeVillain =
            @"SELECT Name FROM Villains WHERE Id = @villainId

        DELETE FROM MinionsVillains 
              WHERE VillainId = @villainId

        DELETE FROM Villains
              WHERE Id = @villainId";

        public const string printMinionNames = @"SELECT Name FROM Minions";

        public const string increaseMinionAge =
            @"UPDATE Minions
               SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
             WHERE Id = @Id 
                SELECT Name, Age FROM Minions";

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
