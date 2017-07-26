# Bulkins
Simple bulk insert using SqlBulkCopy

## Usage
### Bulk Insert
```cs
using (var conn = new SqlConnection("Your connection string goes here."))
{
    conn.Open();
    var bulkins = new BulkInsertOperation();
    bulkins.From("SourceFileTest.txt").To("DESTINATION_TABLE");
    conn.ExecuteBulkInsert(bulkins);
}
```

### Export to file

```cs
using (var conn = new SqlConnection("Your connection string goes here."))
{
    conn.Open();
    using (var cmd = conn.CreateCommand())
    {
        cmd.CommandText = @"SELECT * FROM SOURCE_TABLE";
        cmd.ExportTo("Result.csv");
    }
}
```