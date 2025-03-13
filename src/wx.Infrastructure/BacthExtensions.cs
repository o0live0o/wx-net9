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

                        var dataTable = new DataTable();
                        dataTable.Columns.Add("Id", typeof(int));
                        dataTable.Columns.Add("Path", typeof(string));
                        dataTable.Columns.Add("Name", typeof(string));
                        dataTable.Columns.Add("Value", typeof(string));
                        dataTable.Columns.Add("CreateTime", typeof(DateTime));
                        dataTable.Columns.Add("UpdateTime", typeof(DateTime));
                        foreach (var item in batch)
                        {
                            dataTable.Rows.Add(item.Id, item.Path, item.Name, item.Value, item.CreateTime, item.UpdateTime);
                        }
                        bulkCopy.WriteToServer(dataTable);

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


// 自定义 DataReader 实现
public class TranslationDataReader : IDataReader
{
    private readonly IEnumerator<Translation> _enumerator;
    private readonly int _fieldCount = 6;
    private bool _isClosed = false;
    private int _depth = 0;
    private bool _hasRows = false;

    public TranslationDataReader(IEnumerable<Translation> translations)
    {
        _enumerator = translations.GetEnumerator();
        _hasRows = translations.GetEnumerator().MoveNext();
    }

    public int Depth => _depth;

    public bool IsClosed => _isClosed;

    public int RecordsAffected => -1;

    public int FieldCount => _fieldCount;

    public object this[int i] => GetValue(i);

    public object this[string name] => GetValue(GetOrdinal(name));

    public void Close()
    {
        _isClosed = true;
        _enumerator.Dispose();
    }

    public bool GetBoolean(int i)
    {
        throw new InvalidCastException($"Column {i} is not a boolean type.");
    }

    public byte GetByte(int i)
    {
        throw new InvalidCastException($"Column {i} is not a byte type.");
    }

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
        throw new InvalidCastException($"Column {i} is not a byte array type.");
    }

    public char GetChar(int i)
    {
        throw new InvalidCastException($"Column {i} is not a char type.");
    }

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
        throw new InvalidCastException($"Column {i} is not a char array type.");
    }

    public IDataReader GetData(int i)
    {
        throw new NotSupportedException("GetData method is not supported.");
    }

    public string GetDataTypeName(int i)
    {
        switch (i)
        {
            case 0: return "Int32";
            case 1: return "String";
            case 2: return "String";
            case 3: return "String";
            case 4: return "DateTime";
            case 5: return "DateTime";
            default: throw new IndexOutOfRangeException();
        }
    }

    public DateTime GetDateTime(int i)
    {
        switch (i)
        {
            case 4:
            case 5:
                return (DateTime)GetValue(i);
            default:
                throw new InvalidCastException($"Column {i} is not a DateTime type.");
        }
    }

    public decimal GetDecimal(int i)
    {
        throw new InvalidCastException($"Column {i} is not a decimal type.");
    }

    public double GetDouble(int i)
    {
        throw new InvalidCastException($"Column {i} is not a double type.");
    }

    public Type GetFieldType(int i)
    {
        switch (i)
        {
            case 0: return typeof(int);
            case 1: return typeof(string);
            case 2: return typeof(string);
            case 3: return typeof(string);
            case 4: return typeof(DateTime);
            case 5: return typeof(DateTime);
            default: throw new IndexOutOfRangeException();
        }
    }

    public float GetFloat(int i)
    {
        throw new InvalidCastException($"Column {i} is not a float type.");
    }

    public Guid GetGuid(int i)
    {
        throw new InvalidCastException($"Column {i} is not a Guid type.");
    }

    public short GetInt16(int i)
    {
        if (i == 0)
        {
            return (short)(int)GetValue(i);
        }
        throw new InvalidCastException($"Column {i} is not a short type.");
    }

    public int GetInt32(int i)
    {
        if (i == 0)
        {
            return (int)GetValue(i);
        }
        throw new InvalidCastException($"Column {i} is not an int type.");
    }

    public long GetInt64(int i)
    {
        if (i == 0)
        {
            return (long)(int)GetValue(i);
        }
        throw new InvalidCastException($"Column {i} is not a long type.");
    }

    public string GetName(int i)
    {
        switch (i)
        {
            case 0: return "Id";
            case 1: return "Path";
            case 2: return "Name";
            case 3: return "Value";
            case 4: return "CreateTime";
            case 5: return "UpdateTime";
            default: throw new IndexOutOfRangeException();
        }
    }

    public int GetOrdinal(string name)
    {
        switch (name)
        {
            case "Id": return 0;
            case "Path": return 1;
            case "Name": return 2;
            case "Value": return 3;
            case "CreateTime": return 4;
            case "UpdateTime": return 5;
            default: throw new IndexOutOfRangeException($"Column {name} not found.");
        }
    }

    public string GetString(int i)
    {
        switch (i)
        {
            case 1:
            case 2:
            case 3:
                return (string)GetValue(i);
            default:
                throw new InvalidCastException($"Column {i} is not a string type.");
        }
    }

    public object GetValue(int i)
    {
        var current = _enumerator.Current;
        switch (i)
        {
            case 0: return current.Id;
            case 1: return current.Path;
            case 2: return current.Name;
            case 3: return current.Value;
            case 4: return current.CreateTime;
            case 5: return current.UpdateTime;
            default: throw new IndexOutOfRangeException();
        }
    }

    public int GetValues(object[] values)
    {
        var current = _enumerator.Current;
        values[0] = current.Id;
        values[1] = current.Path;
        values[2] = current.Name;
        values[3] = current.Value;
        values[4] = current.CreateTime;
        values[5] = current.UpdateTime;
        return 6;
    }

    public bool IsDBNull(int i)
    {
        var value = GetValue(i);
        return value == null || value == DBNull.Value;
    }

    public bool NextResult()
    {
        return false;
    }

    public bool Read()
    {
        return _enumerator.MoveNext();
    }

    public DataTable GetSchemaTable()
    {
        var schemaTable = new DataTable();
        schemaTable.Columns.Add("ColumnName", typeof(string));
        schemaTable.Columns.Add("ColumnOrdinal", typeof(int));
        schemaTable.Columns.Add("ColumnSize", typeof(int));
        schemaTable.Columns.Add("DataType", typeof(Type));

        schemaTable.Rows.Add("Id", 0, 0, typeof(int));
        schemaTable.Rows.Add("Path", 1, 0, typeof(string));
        schemaTable.Rows.Add("Name", 2, 0, typeof(string));
        schemaTable.Rows.Add("Value", 3, 0, typeof(string));
        schemaTable.Rows.Add("CreateTime", 4, 0, typeof(DateTime));
        schemaTable.Rows.Add("UpdateTime", 5, 0, typeof(DateTime));

        return schemaTable;
    }

    public void Dispose()
    {
        Close();
    }

    public IEnumerator<Translation> GetEnumerator()
    {
        throw new NotSupportedException("Enumeration is not supported for this data reader.");
    }
}