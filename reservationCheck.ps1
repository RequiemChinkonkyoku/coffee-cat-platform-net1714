# Connect to SQL Server
$server = "localhost"
$database = "CoffeeCatDB"
$username = "sa"
$password = "12345"
$connectionString = "Server=$server;Database=$database;User ID=$username;Password=$password;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Execute stored procedure
$command = $connection.CreateCommand()
$command.CommandText = "EXEC UpdateReservationStatusProcedure"
$command.ExecuteNonQuery()

# Close connection
$connection.Close()
