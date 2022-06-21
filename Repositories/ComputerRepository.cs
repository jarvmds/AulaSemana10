using LabManager.Database;
using LabManager.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace LabManager.Repositories;

class ComputerRepository
{
    private DatabaseConfig databaseConfig;
    
    public ComputerRepository(DatabaseConfig databaseConfig) 
    {
        this.databaseConfig = databaseConfig;
    }

    public IEnumerable<Computer> GetAll()
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        var computers = connection.Query<Computer>("SELECT * FROM Computers");
        connection.Close();
        return computers;
    }

    public Computer Save(Computer computer)
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        connection.Execute("INSERT INTO Computers VALUES(@Id, @Ram, @Processor)",computer);
        connection.Close();
        return computer;
    }

   public Computer GetById(int id)
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        var computer = connection.QueryFirstOrDefault<Computer>("SELECT * FROM Computers WHERE id = @Id;", new {Id = @id});
        connection.Close();
        return computer;        
    }

    public Computer Update(Computer computer)
    {   
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        connection.Execute("UPDATE Computers SET ram = @Ram, processor = @Processor WHERE id = @Id;",computer);
        connection.Close();
        return computer;
    }

    public void Delete(int id)
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        connection.Execute("DELETE FROM Computers WHERE id = @Id;",new {Id = @id});
        connection.Close();
    }

    public bool ExistsById(int id) //devolve se computador existe ou não no banco de dados
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString); 
        connection.Open();
        var result = connection.ExecuteScalar<Boolean>("SELECT COUNT(id) FROM Computers WHERE id=@id;", new { Id = @id });

        return result; 
    }

    private Computer readerToComputer(SqliteDataReader reader)
    {
        var Computer = new Computer(reader.GetInt32(0),reader.GetString(1),reader.GetString(2));

        return Computer;
    }
}
