using System;
using Microsoft.Data.SqlClient;

namespace FixSp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = "Data Source=localhost;Initial Catalog=vedic_ai;Persist Security Info=True;User ID=sa;Password=Samsung#123;TrustServerCertificate=True;";
            Console.WriteLine("Connecting to database for schema migration...");

            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    Console.WriteLine("Connected successfully!");



                    // 5. Clean up old-pattern treatment plans
                    Console.WriteLine("\nCleaning up old-pattern treatment plans...");
                    string cleanupSql = @"
                        DELETE FROM TREATMENTOUTCOMES 
                        WHERE TreatmentPlanId IN (SELECT Id FROM TREATMENTPLANS WHERE DoctorName IS NULL);

                        DELETE FROM TREATMENTPLANS 
                        WHERE DoctorName IS NULL;
                    ";
                    using (var cmd = new SqlCommand(cleanupSql, connection))
                    {
                        int deleted = cmd.ExecuteNonQuery();
                        Console.WriteLine($"✓ Old pattern plans cleaned. Rows affected: {deleted}");
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nMigration succeeded!");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error migrating database: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}
