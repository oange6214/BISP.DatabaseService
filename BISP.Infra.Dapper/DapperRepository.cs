using BISP.Service.IRepository;
using Dapper;
using MySqlConnector;
using Npgsql;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace BISP.Infra.Dapper;

public enum DatabaseProvider
{
    PostgreSQL,
    MariaDB,
    SQLServer,
    MySQL,
    Oracle
}


public class DapperRepository<T> : IRepository<T> where T : class
{
    private string _connectionString;
    private readonly string _databaseProvider;

    public DapperRepository(string connectionString, string databaseProvider)
    {
        _connectionString = connectionString;
        _databaseProvider = databaseProvider;
    }

    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        using var conn = GetDbConnection();
        return await conn.QueryAsync<T>($"SELECT * FROM {GetTableName(typeof(T))}");
    }

    public async Task<T> GetByIdAsync(Guid guid)
    {
        using var conn = GetDbConnection();
        return await conn.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {GetTableName(typeof(T))} WHERE guid = @guid", new { guid });
    }

    public async Task InsertAsync(T entity)
    {
        using var conn = GetDbConnection();
        await conn.ExecuteAsync(GenerateInsertSql(), entity);
    }

    public async Task InsertRangeAsync(IEnumerable<T> entities)
    {
        using var conn = GetDbConnection();
        await conn.ExecuteAsync(GenerateInsertSql(), entities);
    }

    public async Task Update(T entity)
    {
        using var conn = GetDbConnection();
        await conn.ExecuteAsync(GenerateUpdateSql(), entity);
    }

    public async Task Delete(Guid guid)
    {
        using var conn = GetDbConnection();
        await conn.ExecuteAsync($"DELETE FROM {GetTableName(typeof(T))} WHERE guid = @guid", new { guid });
    }

    public async Task DeleteRange(IEnumerable<T> entities)
    {
        using var conn = GetDbConnection();
        await conn.ExecuteAsync($"DELETE FROM {GetTableName(typeof(T))} WHERE guid = @guid", entities.Select(guid => new { guid }).ToArray());
    }

    /// <summary>
    /// Generate Insert SQL syntax
    /// </summary>
    /// <returns></returns>
    private string GenerateInsertSql()
    {
        var properties = typeof(T).GetProperties();
        var columns = GetColumnNames(properties);
        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
        return $"INSERT INTO {GetTableName(typeof(T))} ({columns}) VALUES ({values})";
    }

    /// <summary>
    /// Get property name.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    private string GetPropertyNames(PropertyInfo[] properties)
    {
        return string.Join(", ", properties.Select(p => p.Name));
    }

    /// <summary>
    /// Join column attribute values.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    private string GetColumnNames(PropertyInfo[] properties)
    {
        return string.Join(", ", properties.Select(p => $"`{GetColumnName(p)}`"));
    }

    private IDbConnection GetDbConnection()
    {
        IDbConnection conn;

        switch (_databaseProvider)
        {
            case nameof(DatabaseProvider.PostgreSQL):
                conn = new NpgsqlConnection(_connectionString);
                break;
            case nameof(DatabaseProvider.MariaDB):
                conn = new MySqlConnection(_connectionString);
                break;
            case nameof(DatabaseProvider.SQLServer):
                throw new InvalidOperationException("SQL Server not supported.");
                break;
            default:
                throw new InvalidOperationException("Invalid database provider specified.");
        }

        conn.Open();
        return conn;
    }

    /// <summary>
    /// Generate Update SQL syntax
    /// </summary>
    /// <returns></returns>
    private string GenerateUpdateSql()
    {
        var properties = typeof(T).GetProperties().Where(p => p.Name != "CreateAt");
        var updateColumns = string.Join(", ", properties.Select(p => $"`{GetColumnName(p)}` = @{p.Name}"));
        return $"UPDATE {GetTableName(typeof(T))} SET {updateColumns} WHERE guid = @guid";
    }

    /// <summary>
    /// Get column attribute value.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private string GetColumnName(PropertyInfo property)
    {
        var columnAttribute = property.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() as ColumnAttribute;
        return columnAttribute?.Name ?? property.Name;
    }

    private string GetTableName(Type type)
    {
        var tableAttribute = type.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        return tableAttribute?.Name ?? type.Name;
    }
}