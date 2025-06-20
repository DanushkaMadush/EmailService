using EmailService.Application.DTOs;
using EmailService.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmailService.Infrastructure.Database.StoredProcedures
{
    public class EmailRepository : IEmailRepository
    {
        private readonly AppDbContext _context;

        public EmailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> LogEmailAsync(string from, List<string> to, string subject, string body)
        {
            var toList = string.Join(",", to);
            var connection = _context.Database.GetDbConnection();

            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_LogEmail";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@From", from));
            command.Parameters.Add(new SqlParameter("@To", toList));
            command.Parameters.Add(new SqlParameter("@Subject", subject));
            command.Parameters.Add(new SqlParameter("@Body", body));

            var returnParameter = new SqlParameter("@EmailId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(returnParameter);

            await command.ExecuteNonQueryAsync();

            return (int)returnParameter.Value;
        }

        public async Task<bool> SaveAttachmentAsync(int emailId, AttachmentDto attachment)
        {
            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_SaveAttachment";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@EmailId", emailId));
            command.Parameters.Add(new SqlParameter("@FileName", attachment.FileName));
            command.Parameters.Add(new SqlParameter("@FileData", attachment.FileData));
            command.Parameters.Add(new SqlParameter("@ContentType", attachment.ContentType));

            await command.ExecuteNonQueryAsync();
            return true;
        }
    }
}
