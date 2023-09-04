using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ApiTransation.Models;

namespace SuaAplicacao.Controllers
{
    [Route("api/cartoescredito")]
    [ApiController]
    public class CartaoCreditoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CartaoCreditoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para criar um novo cartão de crédito assíncrono
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CartaoCredito cartaoCredito)
        {
            if (cartaoCredito == null)
            {
                return BadRequest();
            }

            using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() => dbConnection.Open());
            var transaction = dbConnection.BeginTransaction();

            try
            {
                // Execute a operação de criação dentro da transação
                var query = "INSERT INTO CartoesCredito (NumeroCartao, Titular, DataValidade) VALUES (@NumeroCartao, @Titular, @DataValidade); SELECT CAST(SCOPE_IDENTITY() as int)";
                cartaoCredito.Id = await dbConnection.ExecuteScalarAsync<int>(query, cartaoCredito, transaction);

                // Commit da transação
                transaction.Commit();

                return CreatedAtAction(nameof(Get), new { id = cartaoCredito.Id }, cartaoCredito);
            }
            catch (Exception)
            {
                // Em caso de erro, faça rollback da transação
                transaction.Rollback();
                return StatusCode(500);
            }
        }

        // Implemente outros métodos CRUD (Get, Put, Delete) seguindo uma abordagem semelhante.

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() => dbConnection.Open()); // Abra a conexão usando Task.Run
            var transaction = dbConnection.BeginTransaction();

            try
            {
                // Execute sua consulta dentro da transação
                var query = "SELECT * FROM CartoesCredito";
                var cartoesCredito = await dbConnection.QueryAsync<CartaoCredito>(query, transaction: transaction);

                // Commit da transação
                transaction.Commit();

                return Ok(cartoesCredito);
            }
            catch (Exception)
            {
                // Em caso de erro, faça rollback da transação
                transaction.Rollback();
                return StatusCode(500);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CartaoCredito cartao)
        {
            cartao.Id = id;

            using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() => dbConnection.Open()); // Abra a conexão usando Task.Run
            var transaction = dbConnection.BeginTransaction();

            try
            {
                // Execute a operação de atualização dentro da transação
                var query = "UPDATE CartoesCredito SET NumeroCartao = @NumeroCartao, Titular = @Titular, DataValidade = @DataValidade WHERE Id = @Id";
                await dbConnection.ExecuteAsync(query, cartao, transaction);

                // Commit da transação
                transaction.Commit();

                return NoContent();
            }
            catch (Exception)
            {
                // Em caso de erro, faça rollback da transação
                transaction.Rollback();
                return StatusCode(500);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await Task.Run(() => dbConnection.Open()); // Abra a conexão usando Task.Run
            var transaction = dbConnection.BeginTransaction();

            try
            {
                // Execute a operação de exclusão dentro da transação
                var query = "DELETE FROM CartoesCredito WHERE Id = @Id";
                await dbConnection.ExecuteAsync(query, new { Id = id }, transaction);

                // Commit da transação
                transaction.Commit();

                return NoContent();
            }
            catch (Exception)
            {
                // Em caso de erro, faça rollback da transação
                transaction.Rollback();
                return StatusCode(500);
            }
        }
    }
}
