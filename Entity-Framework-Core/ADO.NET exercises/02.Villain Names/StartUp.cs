using _02.Villain_Names;
using Microsoft.Data.SqlClient;
using System.Text;

public class StartUp
{
    public static async Task Main(string[] args)
    {
        await using SqlConnection sqlConnection = new SqlConnection(Config.connectionString);
        await sqlConnection.OpenAsync();

        string countryName = Console.ReadLine();

        await ChangeTownNameCasingAsync(sqlConnection, countryName);
        string result = await GetChangedTownNames(sqlConnection, countryName);

        Console.WriteLine(result);
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
}