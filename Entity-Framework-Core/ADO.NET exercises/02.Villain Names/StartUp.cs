using _02.Villain_Names;
using Microsoft.Data.SqlClient;
using System.Text;

public class StartUp
{
    public static async Task Main(string[] args)
    {
        await using SqlConnection sqlConnection = new SqlConnection(Config.connectionString);
        await sqlConnection.OpenAsync();

        int[] minionIds = Console.ReadLine()
            .Split()
            .Select(int.Parse)
            .ToArray();
        

    }

    //2
    static async Task<string> GetVillainNamesAndNumOfMinionsAsync(SqlConnection connection)
    {
        StringBuilder sb = new StringBuilder();

        SqlCommand sqlCommand = new SqlCommand(SqlQueries.getVillainNames, connection);
        SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

        while (reader.Read())
        {


            string villainName = (string)reader["Name"];
            int minionCount = (int)reader["MinionsCount"];
            sb.AppendLine($"{villainName} - {minionCount}");
        }

        return sb.ToString().TrimEnd();
    }

    //3
    static async Task<string> GetMinionNameAndAgeAsync(SqlConnection connection, int villainId)
    {


        SqlCommand getVillainNameCmd = new SqlCommand(SqlQueries.getVillainNameById, connection);
        getVillainNameCmd.Parameters.AddWithValue("@Id", villainId);

        object? villain = await getVillainNameCmd.ExecuteScalarAsync();


        if (villain == null)
        {
            return $"No villain with {villainId} exists in the database.";
        }


        string villainName = (string)villain;

        SqlCommand getMinionNameAndAge = new SqlCommand(SqlQueries.getMinionNamesAndAgeByVillainId, connection);
        getMinionNameAndAge.Parameters.AddWithValue("@Id", villainId);

        SqlDataReader minionReader = await getMinionNameAndAge.ExecuteReaderAsync();

        string output = GetVillainWithMinionsOutput(villainName, minionReader);

        return output;
    }

    private static string GetVillainWithMinionsOutput(string villainName, SqlDataReader minionReader)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Villain: {villainName}");

        if (!minionReader.HasRows)
        {
            sb.AppendLine("(no minions)");
        }

        else
        {

            while (minionReader.Read())
            {
                long rowNum = (long)minionReader["RowNum"];
                string minionName = (string)minionReader["Name"];
                int minionAge = (int)minionReader["Age"];
                sb.AppendLine($"{rowNum}. {minionName} {minionAge}");
            }

        }

        return sb.ToString().TrimEnd();
    }

    //4

    static async Task<string> AddMinionToDbAsync(SqlConnection connection, string[] minionInfo, string villainName)
    {
        StringBuilder sb = new StringBuilder();

        string minionName = minionInfo[1];
        int minionAge = int.Parse(minionInfo[2]);
        string minionTownName = minionInfo[3];


        int townId = await GetTownIdOrAddByNameAsync(connection, sb, minionTownName);
        int villainId = await GetVillainIdOrAddByNameAsync(connection, sb, villainName);
        int minionId = await AddMinionAndReturnIdAsync(connection, minionName, minionAge, townId);

        await SetMinionToVillainAsync(connection, minionId, villainId);
        sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

        return sb.ToString().TrimEnd();
    }

    private static async Task<int> GetTownIdOrAddByNameAsync(SqlConnection connection, StringBuilder sb, string minionTownName)
    {
        SqlCommand getTownId = new SqlCommand(SqlQueries.getTownIdByName, connection);
        getTownId.Parameters.AddWithValue("@townName", minionTownName);

        object? townIdObj = await getTownId.ExecuteScalarAsync();

        if (townIdObj == null)
        {
            SqlCommand addTownToDb = new SqlCommand(SqlQueries.addNewTown, connection);
            addTownToDb.Parameters.AddWithValue("@townName", minionTownName);

            await addTownToDb.ExecuteNonQueryAsync();
            townIdObj = await getTownId.ExecuteScalarAsync();
            sb.AppendLine($"Town {minionTownName} was added to the database.");
        }

        return (int)townIdObj;
    }

    private static async Task<int> GetVillainIdOrAddByNameAsync(SqlConnection connection, StringBuilder sb, string villainName)
    {
        SqlCommand getVillainId = new SqlCommand(SqlQueries.getVillainIdByName, connection);
        getVillainId.Parameters.AddWithValue("@Name", villainName);

        int? villainId = (int?)await getVillainId.ExecuteScalarAsync();

        if (!villainId.HasValue)
        {
            SqlCommand addVillainToDb = new SqlCommand(SqlQueries.addVillainWithDefaultVillainFactor, connection);
            addVillainToDb.Parameters.AddWithValue("@villainName", villainName);

            await addVillainToDb.ExecuteNonQueryAsync();
            villainId = (int?)await getVillainId.ExecuteScalarAsync();
            sb.AppendLine($"Villain {villainName} was added to the database.");
        }

        return (int)villainId;
    }

    private static async Task<int> AddMinionAndReturnIdAsync
        (SqlConnection connection, string minionName, int minionAge, int townId)
    {
        SqlCommand addMinionCmd = new SqlCommand(SqlQueries.addNewMinion, connection);

        addMinionCmd.Parameters.AddWithValue("@name", minionName);
        addMinionCmd.Parameters.AddWithValue("@age", minionAge);
        addMinionCmd.Parameters.AddWithValue("@townId", townId);

        await addMinionCmd.ExecuteNonQueryAsync();

        SqlCommand getMinionIdCmd = new SqlCommand(SqlQueries.getMinionIdByName, connection);
        getMinionIdCmd.Parameters.AddWithValue("@Name", minionName);

        int minionId = (int)await getMinionIdCmd.ExecuteScalarAsync();

        return minionId;
    }

    private static async Task SetMinionToVillainAsync
        (SqlConnection connection, int minionId, int villainId)
    {
        SqlCommand addMinionCmd = new SqlCommand(SqlQueries.setMinionToVillain, connection);
        addMinionCmd.Parameters.AddWithValue("@minionId", minionId);
        addMinionCmd.Parameters.AddWithValue("@villainId", villainId);

        await addMinionCmd.ExecuteNonQueryAsync();
    }

    //5

    static async Task ChangeTownNameCasingAsync(SqlConnection connection, string countryName)
    {
        SqlCommand changeTownNameCmd = new SqlCommand(SqlQueries.changeTownNameCasingByCountry, connection);
        changeTownNameCmd.Parameters.AddWithValue("@countryName", countryName);
        int numberOfTownsChanged = await changeTownNameCmd.ExecuteNonQueryAsync();

        if (numberOfTownsChanged == 0)
        {
            Console.WriteLine("No town names were affected.");
        }

        else
        {
            Console.WriteLine($"{numberOfTownsChanged} town names were affected.");
        }
    }

    static async Task<string> GetChangedTownNames(SqlConnection connection, string countryName)
    {
        StringBuilder sb = new StringBuilder();
        List<string> names = new List<string>();

        SqlCommand getTownNamesCmd = new SqlCommand(SqlQueries.getTownNames, connection);
        getTownNamesCmd.Parameters.AddWithValue("@countryName", countryName);
        SqlDataReader reader = await getTownNamesCmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            string changedTownName = (string)reader["Name"];
            names.Add(changedTownName);
        }

        sb.Append(string.Join(", ", names));

        return sb.ToString().TrimEnd();
    }

    //6

    static async Task<string> RemoveVillainAsync(SqlConnection connection, int villainId)
    {
        StringBuilder sb = new StringBuilder();

        var transaction = connection.BeginTransaction();
        try
        {
            bool villainExists = await GetVillainNameAsync(connection, sb, villainId, transaction);

            if (!villainExists)
            {
                await transaction.CommitAsync();
                return sb.ToString().TrimEnd();
            }

            await ReleaseMinionsFromVillainAsync(connection, sb, villainId, transaction);

            SqlCommand removeVillainCmd = new SqlCommand(SqlQueries.removeVillainById, connection, transaction);
            removeVillainCmd.Parameters.AddWithValue("@villainId", villainId);

            await removeVillainCmd.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
        }

        return sb.ToString().TrimEnd();

    }

    private static async Task<bool> GetVillainNameAsync(SqlConnection connection, StringBuilder sb, int villainId, SqlTransaction transaction)
    {

        string villainName = string.Empty;

        SqlCommand getVillainNameCmd = new SqlCommand(SqlQueries.getVillainNameById, connection, transaction);
        getVillainNameCmd.Parameters.AddWithValue("@Id", villainId);

        object? villainNameObj = await getVillainNameCmd.ExecuteScalarAsync();

        if (villainNameObj == null)
        {
            sb.AppendLine("No such villain was found.");
            return false;
        }

        else
        {
            villainName = (string)villainNameObj;
            sb.AppendLine($"{villainName} was deleted.");
            return true;
        }
    }

    private static async Task ReleaseMinionsFromVillainAsync(SqlConnection connection, StringBuilder sb, int villainId, SqlTransaction transaction)
    {
        SqlCommand releaseMinionsFromVillainCmd = new SqlCommand(SqlQueries.releaseMinionsFromVillain, connection, transaction);
        releaseMinionsFromVillainCmd.Parameters.AddWithValue("@villainId", villainId);

        int minionsDeletedCount = await releaseMinionsFromVillainCmd.ExecuteNonQueryAsync();

        sb.AppendLine($"{minionsDeletedCount} minions were released.");
    }

    //7

    static async Task PrintMinionNamesAsync(SqlConnection connection)
    {
        List<string> minionNames = new List<string>();

        SqlCommand getMinionNamesCmd = new SqlCommand(SqlQueries.printMinionNames, connection);
        SqlDataReader nameReader = await getMinionNamesCmd.ExecuteReaderAsync();

        while (nameReader.Read())
        {
            string minionName = (string)nameReader["Name"];
            minionNames.Add(minionName);
        }

        while (minionNames.Count > 0)
        {
            Console.WriteLine(minionNames[0]);

            if (minionNames.Count == 1)
            {
                minionNames.Remove(minionNames[0]);
                return;
            }

            Console.WriteLine(minionNames[minionNames.Count - 1]);
            minionNames.Remove(minionNames[0]);
            minionNames.Remove(minionNames[minionNames.Count - 1]);
        }
    }

    //8

    static async Task IncreaseMinionAgeAndLowerCaseFirstLetterAsync(SqlConnection connection, int[] minionIds)
    {

        foreach (var minionId in minionIds)
        {
            SqlCommand increaseAgeCmd = new SqlCommand(SqlQueries.increaseMinionAge, connection);
            increaseAgeCmd.Parameters.AddWithValue("@Id", minionId);
            await increaseAgeCmd.ExecuteNonQueryAsync();
        }

        await PrintNameAndAgeOfMinionsAsync(connection);
    }

    private static async Task PrintNameAndAgeOfMinionsAsync(SqlConnection connection)
    {
        SqlCommand printNameAndAgeCmd = new SqlCommand(SqlQueries.printMinionNameAndAge, connection);

        SqlDataReader reader = await printNameAndAgeCmd.ExecuteReaderAsync();

        while (reader.Read())
        {
            string name = (string)reader["Name"];
            int age = (int)reader["Age"];

            Console.WriteLine($"{name} {age}");
        }
    }

    //9



}