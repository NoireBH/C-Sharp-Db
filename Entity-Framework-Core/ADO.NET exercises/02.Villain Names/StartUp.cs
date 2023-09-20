using _02.Villain_Names;
using Microsoft.Data.SqlClient;
using System.Text;

public class StartUp
{
    public static async Task Main(string[] args)
    {
        await using SqlConnection sqlConnection = new SqlConnection(Config.connectionString);
        await sqlConnection.OpenAsync();

        int villainId = int.Parse(Console.ReadLine());

        string result = await GetMinionNameAndAgeAsync(sqlConnection, villainId);
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

    //3


}