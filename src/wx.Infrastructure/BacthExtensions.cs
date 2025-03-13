using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace wx.Infrastructure;

public class Translation
{
    public int Id { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }

    public string Value { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }


    public static class BacthExtensions
    {
        public static bool BatchInsertOrUpdate<T>(IDbConnection connection, IEnumerable<Translation> list, string tableName) 
        {
            const int batchSize = 5000;
            var batches = list.Select((x, i) => new { Index = i, Value = x })
                             .GroupBy(x => x.Index / batchSize)
                             .Select(g => g.Select(x => x.Value));
            if (connection is SqlConnection sqlConn)
            {
                using var transaction = sqlConn.BeginTransaction();
                try
                {
                    foreach (var batch in list.Chunk(batchSize))
                    {
                        var tempTableName = $"#{tableName}_{Guid.NewGuid():N}";
                        sqlConn.Execute($@"
                    SELECT TOP 0 * INTO {tempTableName} FROM {tableName};
                    CREATE UNIQUE INDEX IX_Temp ON {tempTableName} (Path, Name, Language);",
                            transaction: transaction);

                        using var bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, transaction);
                        bulkCopy.DestinationTableName = tempTableName;
                        bulkCopy.BulkCopyTimeout = 600;


                        //bulkCopy.WriteToServer(reader);

                        sqlConn.Execute($@"
                    MERGE {tableName} AS t
                    USING {tempTableName} AS s 
                    ON t.Path = s.Path AND t.Name = s.Name AND t.Language = s.Language
                    WHEN MATCHED THEN 
                        UPDATE SET
                            t.LastBatchNo = t.BatchNo,
                            t.BatchNo = s.BatchNo, 
                            t.UpdateTime = GETUTCDATE()
                    WHEN NOT MATCHED THEN 
                        INSERT (Path, Name, Language, CreateTime, UpdateTime)
                        VALUES (s.Path, s.Name, s.Language, GETUTCDATE(),GETUTCDATE());
                    
                    DROP TABLE {tempTableName};", transaction: transaction);
                    }
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }

            }
            return true;
        }
    }
}