using LabManager.Models;
using LabManager.Database;
using Microsoft.Data.Sqlite;

namespace LabManager.Repositories;

class ComputerRepository
{
    private DatabaseConfig databaseConfig;
    
    public ComputerRepository(DatabaseConfig databaseConfig) => this.databaseConfig = databaseConfig;
    public List<Computer> GetAll()
    {
        var computers  = new List<Computer>();
        
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Computers;";
        
        var reader = command.ExecuteReader();

        while(reader.Read()) // reader.Read() é o comando utilizado para iniciar uma leitura
        {
            var computer = new Computer(reader.GetInt32(0),reader.GetString(1),reader.GetString(2));

            computers.Add(readerToComputer(reader));
        }
        connection.Close();

        return computers;
    }

    public Computer Save(Computer computer)
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Computers VALUES ($id, $ram, $processor);"; // inserindo comando
        command.Parameters.AddWithValue("$id", computer.Id); // adicionando os valores do parametro para o comando SQL
        command.Parameters.AddWithValue("$ram", computer.Ram);
        command.Parameters.AddWithValue("$processor", computer.Processor);
        
        command.ExecuteNonQuery(); // para quando o método não precisa retornar
        connection.Close(); //fechando conexão

        return computer;
    }

    public Computer GetById(int id)
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Computers WHERE id = $id;";
        command.Parameters.AddWithValue("$id",id);
        
        var reader = command.ExecuteReader();
        reader.Read();
        var computer = readerToComputer(reader);

        connection.Close();

        return computer;
    }

    public bool existsById (int id)
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT count (id) FROM Computers WHERE id = $id;";
        command.Parameters.AddWithValue("$id", id);

        int result = Convert.ToInt32 (command.ExecuteScalar()); 

        return result == 1;
    }

    private Computer readerToComputer(SqliteDataReader reader )
    {
        var computer = new Computer (reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
        return computer;
    }

    public Computer Update(Computer computer)
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Computers SET ram = $ram, processor = $processor WHERE id = $id;";
        command.Parameters.AddWithValue("$id", computer.Id); 
        command.Parameters.AddWithValue("$ram", computer.Ram);
        command.Parameters.AddWithValue("$processor", computer.Processor);

        command.ExecuteNonQuery();
        connection.Close();

        return computer;
    }

    public void Delete(int id)
    {
        var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Computers WHERE id = $id;";
        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery(); //ExecuteNonQuery executa, mas nao devolve valor no banco (oposto ExecuteReader)
        connection.Close();
    }
}

//tarefa: verificar se existe registro no banco antes de apresentar ao usuario (apresentar 0 se nao tiver e vice-versa)
//função count sql
// SELECT count(id) FROM Computers WHERE id = $id;
// chamar no program mas fica no computerrepository
//existsById
