# Bulkins
Simple bulk insert using SqlBulkCopy

## Usage
```cs
using (var conn = new SqlConnection("Your connection string goes here."))
{
    conn.Open();
    conn.BulkInsert(c => c.From(new SourceFileInfo("SourceFile.txt", true)).To("DESTINATION_TABLE"));
}
```