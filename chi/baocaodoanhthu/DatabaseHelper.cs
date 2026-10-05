using System;
using System.Data;
using System.Data.SqlClient;

namespace baocaodoanhthu
{
    public static class DatabaseHelper
    {
        // Chuỗi kết nối trỏ trực tiếp tới CSDL BaoCaoThongKeDoanhThuDB
        private static readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=BaoCaoThongKeDoanhThuDB;Integrated Security=True;TrustServerCertificate=True";

        /// <summary>
        /// Hàm dùng chung để thực thi câu lệnh SQL SELECT và trả về DataTable
        /// </summary>
        public static DataTable GetData(string query, SqlParameter[]? parameters = null)
        {
            var dt = new DataTable();

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(query, conn);

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            return dt;
        }
    }
}