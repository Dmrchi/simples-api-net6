using Dapper;
using Microsoft.Data.SqlClient;
using Usuarios.API.Infraestrutura.Interface;
using Usuarios.API.Model;
using Usuarios.API.Response;
using Microsoft.Extensions.Configuration;

namespace Usuarios.API.Infraestrutura
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _stringConexao;
        private readonly SqlConnection _conexao;

        

        public UsuarioRepository(
            IConfiguration configuration
            )
        {
            _stringConexao = configuration.GetConnectionString("UsuarioConnection")
                                     ?? throw new InvalidOperationException(
                                         "A string de conexão 'conexaoSQL' não foi encontrada. Verifique o appsettings.json."
                                     );
            _conexao = new SqlConnection(_stringConexao);

            //_conexao = new SqlConnection(_stringConexao);
        }

        /// <summary>
        /// Cadastra um novo usuário usando Dapper de forma segura e assíncrona.
        /// </summary>
        public async Task<bool> CadastrarUsuarioAsync(Usuario usuario)
        {
            string sql = @"INSERT INTO Usuario (Nome, Email, Senha, Telefone) VALUES(@Nome, @Email, @Senha, @Telefone);";


            try
            {
                //using var conexao = new SqlConnection(_stringConexao);
                //await _conexao.OpenAsync();
                var linhasAfetadas = await _conexao.ExecuteAsync(sql, new
                  {
                      usuario.Nome,
                      usuario.Email,
                      usuario.Senha, //SenhaHash = 
                      usuario.Telefone
                  });

                return linhasAfetadas > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro SQL: {ex.Message}");
                return false; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                return false; 
            }
        }
        /*
        public Resposta<Usuario> LoginValidar(Usuario usuario)
        {

            string sql = $"SELECT ID, NOME, EMAIL, SENHA, PROFISSAO, TELEFONE FROM USUARIO WHERE EMAIL='{usuario.Email}' AND SENHA='{usuario.Senha}'";

            var resultado = _conexao.Query<Usuario>(sql);


            Resposta<Usuario> resposta = new Resposta<Usuario>();

            if (resultado != null && resultado.ToList().Count > 0)
            {
                resposta.Titulo = "Login Ok";
                resposta.Status = 200;
                resposta.Resultado = resultado;

                return resposta;

            }
            return resposta;
        }*/
    }
}