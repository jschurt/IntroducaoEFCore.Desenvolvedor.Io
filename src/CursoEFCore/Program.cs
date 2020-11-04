using CursoEFCore.Data;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {

            using var db = new Data.ApplicationContext();

            //Verificando se existem migracoes pendentes
            var existeMigracaoPendente = db.Database.GetPendingMigrations().Any();
            if (existeMigracaoPendente)
            { 
                //Fazer algo. (Exemplo abortar) 
            }

            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();

            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();

            //AtualizarDados();

            RemoverDados();

        } //Main


        private static void InserirDados()
        {

            var produto = new Produto { 
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new ApplicationContext();
            
            //Opcao 1 (recomendado)
            //db.Produtos.Add(produto);

            //Opcao 2 (recomendado)
            db.Set<Produto>().Add(produto);

            //Opcao 3 (nao recomendado)
            //db.Entry(produto).State = EntityState.Added;

            //Opcao 4 (nao recomendado)
            //db.Add(produto);

            //Save changes retorna numero de registros afetados
            var numRegistros = db.SaveChanges();

            Console.WriteLine($"Total registro(s): {numRegistros}");

        } //InserirDados

        private static void InserirDadosEmMassa()
        {

            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Rafael Almeida",
                CEP = "9999900",
                Cidade = "Itabaina",
                Estado = "SE",
                Telefone = "9900001111"
            };

            using var db = new ApplicationContext();

            db.AddRange(produto, cliente);

            //Save changes retorna numero de registros afetados
            var numRegistros = db.SaveChanges();

            Console.WriteLine($"Total registro(s): {numRegistros}");


            var listaClientes = new[]
            {
                new Cliente
                {
                    Nome = "Cliente 01",
                    CEP = "9999900",
                    Cidade = "Itabaina",
                    Estado = "SE",
                    Telefone = "9900001111"
                },
                new Cliente
                {
                    Nome = "Cliente 02",
                    CEP = "9999900",
                    Cidade = "Itabaina",
                    Estado = "SE",
                    Telefone = "9900001111"
                }

            };

            //Opcao 1
            //db.Clientes.AddRange(listaClientes);

            //Opcao 2
            db.Set<Cliente>().AddRange(listaClientes);

            //Save changes retorna numero de registros afetados
            numRegistros = db.SaveChanges();

            Console.WriteLine($"Total registro(s): {numRegistros}");

        } //InserirDadosEmMassa

        private static void AtualizarDados()
        {

            using var db = new ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            cliente.Nome = "Cliente alterado passo 1a";

            //Opcao 1 
            // Se eu utilizar o "Update", a instrucao sql de update ira conter todos os campos, tendo eles sido alterados
            // ou nao. Se eu nao utilizar o comando e usar apenas o SaveChanges, a instrucao sql update CONTERA APENAS 
            // OS CAMPOS QUE FORAM MODIFICADOS. (Comente a linha abaixo e observe no console)
            // Observe 
            //db.Set<Cliente>().Update(cliente);

            //Opcao 2
            //db.Entry(cliente).State = EntityState.Modified;

            //Save changes retorna numero de registros afetados
            var numRegistros = db.SaveChanges();

            Console.WriteLine($"Total registro(s): {numRegistros}");

            //======== IMPORTANTE =================================================================
            //Numa situacao onde quero atualizar um dado "desconectado", isto eh, nao estou atualizando
            //a informacao apos uma leitura previa do dado no banco de dados, posso fazer da forma abaixo)

            //Passo 1. Tenho um objeto qualquer (no caso um objeto anonimo)
            var clienteDesconectado = new { 
                Nome = "Cliente desconectado",
                Telefone = "304050",
                CampoInexistente = "Qualquer coisa"
            };

            //Passo 2. Usando o objeto desconectado eu atualizo o objeto cliente com o comando abaixo.
            // Sera gerado um update APENAS com os campos que forem alterados no objeto cliente.
            // O campo "CampoInexistente", sera ignorado, pois ele nao existe na classe "Cliente"
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);
            db.SaveChanges();

            //======== IMPORTANTE 2 ==============================================================
            //No exemplo anterior, fizemos um update do objeto cliente que pode ter sido previamente
            //carregado do banco, mas temos a opcao de dar um update em um objeto (registro) que sequer
            //Foi lido previamente do banco.

            //Passo 1. Crio um objeto Cliente com a chave do objeto que quero atualizar
            var clienteFake = new Cliente { Id = 3 };

            //Passo 2. Faco o "attachment" deste objeto para que ele possa ser rastreado.
            db.Set<Cliente>().Attach(clienteFake);

            //Passo 3. Usando o objeto desconectado eu atualizo o objeto cliente com o comando abaixo.
            // Sera gerado um update APENAS com os campos que forem alterados no objeto cliente.
            // O campo "CampoInexistente", sera ignorado, pois ele nao existe na classe "Cliente"
            db.Entry(clienteFake).CurrentValues.SetValues(clienteDesconectado);
            db.SaveChanges();


        } //AtualizarDados

        private static void RemoverDados() {

            using var db = new ApplicationContext();

            //var cliente = db.Clientes.Find(12);

            //Opcao 1. 
            //db.Clientes.Remove(cliente);

            //Opcao 2.
            //db.Remove(cliente);

            //Opcao 3.
            //db.Entry(cliente).State = EntityState.Deleted;

            //db.SaveChanges();

            //======== IMPORTANTE ================================================================
            //Podemos fazer uma remocao de um objeto desconectado, poupando assim uma leitura no banco.

            //Passo 1.
            var clienteDesconectado = new Cliente { Id = 12 };

            //Passo 2.
            db.Entry(clienteDesconectado).State = EntityState.Deleted;
            db.SaveChanges();



        } //RemoverDados


        private static void ConsultarDados()
        {

            using var db = new ApplicationContext();

            // Opcao 1. Linq
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();

            //Opcao 2. Lamba Expressions
            //Sempre que os dados sao carregados em memoria, EF vai manter uma copia dos objetos em memoria e 
            //ira rastrar suas modificacoes.
            var consultaPorMetodo = db.Clientes.AsNoTracking().Where(c => c.Id > 0).OrderBy(c => c.Nome).ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando cliente: {cliente.Id}");

                //Find primeiro procura objetos em memoria. Se nao encontrar em memoria, ele ira buscar no banco de dados.
                //Se o dado tiver sido carregado anteriormente com "AsNoTracking", nao havera dados em memoria e o dado sera
                //lido sempre da base de dados.
                //db.Clientes.Find(cliente.Id);

                //FirstOrDefault ira buscar na base de dados INDEPENDENTE se os dados estiverem em memoria ou nao
                db.Clientes.FirstOrDefault(c => c.Id == cliente.Id);

            }

        } //ConsultarDados

        private static void CadastrarPedido()
        {

            using var db = new ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Items = new List<PedidoItem> { 
                    
                    new PedidoItem { 
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 2,
                        Valor = 10
                    } 
                }
            };


            db.Set<Pedido>().Add(pedido);

            //Save changes retorna numero de registros afetados
            var numRegistros = db.SaveChanges();

            Console.WriteLine($"Total registro(s): {numRegistros}");

        } //CadastrarPedido

        private static void ConsultarPedidoCarregamentoAdiantado()
        {

            using var db = new ApplicationContext();

            //O metodo include, faz o carreamento adiantado
            var pedidos = db.Pedidos
                .Include(p => p.Items)              //Primeiro nivel
                    .ThenInclude(i => i.Produto )   //Segundo nivel
                .ToList();

            Console.WriteLine(pedidos.Count);

        } //ConsultarDados


    } //class
} //namespace
