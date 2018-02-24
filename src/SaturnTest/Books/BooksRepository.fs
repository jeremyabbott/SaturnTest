namespace Books

open Database
open Microsoft.Data.Sqlite
open System.Threading.Tasks

module Database =
  let getAll connectionString : Task<Result<Book seq, exn>> =
    use connection = new SqliteConnection(connectionString)
    query connection "SELECT id, title, author FROM Books" None

  let getById connectionString id : Task<Result<Book option, exn>> =
    use connection = new SqliteConnection(connectionString)
    querySingle connection "SELECT id, title, author FROM Books WHERE id=@id" (Some <| dict ["id" => id])

  let update connectionString v : Task<Result<int,exn>> =
    use connection = new SqliteConnection(connectionString)
    execute connection "UPDATE Books SET id = @id, title = @title, author = @author WHERE id=@id" v

  let insert connectionString v : Task<Result<int,exn>> =
    use connection = new SqliteConnection(connectionString)
    execute connection "INSERT INTO Books(id, title, author) VALUES (@id, @title, @author)" v

  let delete connectionString id : Task<Result<int,exn>> =
    use connection = new SqliteConnection(connectionString)
    execute connection "DELETE FROM Books WHERE id=@id" (dict ["id" => id])

