using System;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using Microsoft.Extensions.Configuration;

namespace CoolShop.Core.Extend
{
    public class DbContext
    {
        public IFreeSql Mysql { get; }

        private static string CreateConnection(IConfiguration configuration)
        {
            return $"Data Source={configuration["Mysql:Host"]};Port={configuration["Mysql:Port"]};User ID={configuration["Mysql:User"]};Password={configuration["Mysql:Pass"]};Initial Catalog={configuration["Mysql:Db"]};Charset={configuration["Mysql:Charset"]};SslMode=none;Max pool size={configuration["Mysql:MaxPool"]};";
        }

        private static async Task MonitorCommand(DbCommand command, string str)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"==========={DateTime.Now.ToLocalTime():yyyy-MM-dd HH:mm:ss}===========");
            sb.AppendLine($"{command.CommandText}");
            foreach (DbParameter parameter in command.Parameters)
            {
                sb.AppendLine($"{parameter.ParameterName}:{parameter.Value}");
            }

            sb.AppendLine(str);
            sb.AppendLine("========================================");
            await Console.Out.WriteLineAsync(sb);
        }


        public DbContext(IConfiguration configuration)
        {
            var conn = CreateConnection(configuration);
            Mysql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, conn)
                .UseMonitorCommand(command => { }, async (command, s) => { await MonitorCommand(command, s); })
                .Build();
        }
    }
}