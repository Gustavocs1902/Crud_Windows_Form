using CrudWindowsForm.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;


namespace CrudWindowsForm
{
    public class FuncRepositorio
    {
        public string connectionString;

        public FuncRepositorio()
        {
            connectionString = ConfigurationManager.ConnectionStrings["CrudConnection"].ConnectionString;
        }
        
        //Inserir

       public bool Inserir(Funcionarios funcionario)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Funcionarios (Nome, Telefone, RG, Endereco, Salario, HorasExtras)
                             VALUES
                             (@Nome, @Telefone, @RG, @Endereco, @Salario, @HorasExtras)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nome", funcionario.Nome);
                cmd.Parameters.AddWithValue("@Telefone", funcionario.Telefone);
                cmd.Parameters.AddWithValue("@RG", funcionario.RG);
                cmd.Parameters.AddWithValue("@Endereco", funcionario.Endereco);
                cmd.Parameters.AddWithValue("@Salario", funcionario.Salario);
                cmd.Parameters.AddWithValue("@HorasExtras", funcionario.HorasExtras);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;

            }

        }
        //Buscar
        public List<Funcionarios> Buscar(string q)
        {
            var funcionario = new List<Funcionarios>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Funcionarios WHERE Nome LIKE @Buscar OR RG LIKE @Buscar OR Salario LIKE @Buscar";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Buscar", $"%{q}%");

                conn.Open();
                using(SqlDataReader ler = cmd.ExecuteReader())
                {
                    while (ler.Read())
                    {
                        funcionario.Add(new Funcionarios
                        {
                            ID = Convert.ToInt32(ler["Id"]),
                            Nome = ler["Nome"].ToString(),
                            Telefone = ler["Telefone"].ToString(),
                            RG = ler["RG"].ToString(),
                            Endereco = ler["Endereco"].ToString(),
                            Salario = Convert.ToDecimal(ler["Salario"]),
                            HorasExtras = Convert.ToInt32(ler["HorasExtras"])

                        });
                    }
                }

            }
            return funcionario;
        }

        public bool Atualizar(Funcionarios funcionarios)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Funcionarios SET 
                            Nome = @Nome,
                            Telefone = @Telefone, 
                            RG = @RG, 
                            Endereco = @Endereco, 
                            Salario = @Salario, 
                            HorasExtras = @HorasExtras 
                            WHERE Id = @ID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", funcionarios.ID);
                cmd.Parameters.AddWithValue("@Nome", funcionarios.Nome);
                cmd.Parameters.AddWithValue("@Telefone", funcionarios.Telefone);
                cmd.Parameters.AddWithValue("@RG", funcionarios.RG);
                cmd.Parameters.AddWithValue("@Endereco", funcionarios.Endereco);
                cmd.Parameters.AddWithValue("@Salario", funcionarios.Salario);
                cmd.Parameters.AddWithValue("@HorasExtras", funcionarios.HorasExtras);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool Deletar(int id) 
        {
            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                string sql = "DELETE FROM Funcionarios WHERE Id = @ID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;

            }
        }
    }
}
