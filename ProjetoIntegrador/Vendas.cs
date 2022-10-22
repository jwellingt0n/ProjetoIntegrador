using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjetoIntegrador
{
    internal class Vendas
    {
        // METODO PARA CADASTRAR IMPRESSAO E DESCONTAR DO ESTOQUE
        // METODO PARA CADASTRAR IMPRESSAO E DESCONTAR DO ESTOQUE
        public static void CadastroImpressao(ConexaoBD bd)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n  _____ __  __ _____  _____  ______  _____ _____         ____  \r\n |_   _|  \\/  |  __ \\|  __ \\|  ____|/ ____/ ____|  /\\   / __ \\ \r\n   | | | \\  / | |__) | |__) | |__  | (___| (___   /  \\ | |  | |\r\n   | | | |\\/| |  ___/|  _  /|  __|  \\___ \\\\___ \\ / /\\ \\| |  | |\r\n  _| |_| |  | | |    | | \\ \\| |____ ____) |___) / ____ \\ |__| |\r\n |_____|_|  |_|_|    |_|  \\_\\______|_____/_____/_/    \\_\\____/ \r\n                                                               \r\n                                                               \r\n");
            Console.ForegroundColor = ConsoleColor.White;
        inicio:
            Console.WriteLine("\nInsira o CPF do usuário:");
            string cpfUsuario = Console.ReadLine();
            int saldoUsuario;
            int idUsuario;
            if (!Program.ChecarCpf(cpfUsuario))
            {
                Program.MostrarMensagemErro("CPF no formato incorreto!");
                Program.MostrarMensagemErro("Ex: 000.000.000-00");
                goto inicio;
            }
            else
            {
                string sql = $"SELECT * FROM Usuario WHERE cpf = '{cpfUsuario}'";
                SqlCommand comando = new SqlCommand(sql, bd.conn);
                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read() == false)
                    {
                        Program.MostrarMensagemErro("CPF não encontrado.");
                        goto inicio;
                    }
                    else
                    {
                        Console.WriteLine("\nInsira a senha do cliente:");
                        string senhaUsuario = Console.ReadLine();
                        string senhaDB = reader.GetString(reader.GetOrdinal("senha"));
                        saldoUsuario = reader.GetInt32(reader.GetOrdinal("saldo"));
                        idUsuario = reader.GetInt32(reader.GetOrdinal("id"));
                        if (senhaDB != senhaUsuario)
                        {
                            Program.MostrarMensagemErro("Senha incorreta!");
                            goto inicio;
                        }
                        else
                            goto sequencia;
                    }
                }
            sequencia:
                Console.WriteLine("\nÉ frente e verso? (1) SIM / (0) NÃO");
                string fv = Console.ReadLine();
                int ehFrenteVerso;
                if (Program.ChecarCampo(fv))
                    ehFrenteVerso = Convert.ToInt32(fv);
                else
                    ehFrenteVerso = 0;
                Console.WriteLine("\nQuantas folhas deseja imprimir?");
                string qntd = Console.ReadLine();
                int qntdImpressao;
                if (Program.ChecarCampo(qntd))
                    qntdImpressao = Convert.ToInt32(qntd);
                else
                    qntdImpressao = 0;
                if (saldoUsuario < qntdImpressao)
                    Program.MostrarMensagemErro("O usuário não possui saldo suficiente para imprimir essa quantidade");
                else
                {
                    string sql1 = $"SELECT * FROM EstoqueA4";
                    SqlCommand comando1 = new SqlCommand(sql1, bd.conn);
                    int quantidadeEstoqueA4 = 0;
                    using (var reader1 = comando1.ExecuteReader())
                    {
                        reader1.Read();
                        quantidadeEstoqueA4 = reader1.GetInt32(reader1.GetOrdinal("quantidade"));
                    }
                    if (quantidadeEstoqueA4 >= qntdImpressao)
                    {
                        string sql2 = $"UPDATE EstoqueA4 SET quantidade={quantidadeEstoqueA4 - qntdImpressao}";
                        SqlCommand comando2 = new SqlCommand(sql2, bd.conn);
                        comando2.ExecuteNonQuery();
                        string condicao = $"saldo = {saldoUsuario - qntdImpressao} WHERE cpf = '{cpfUsuario}'";
                        Usuario.AtualizarUsuario(bd, condicao);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nImpressões aprovadas!");
                        Console.ForegroundColor = ConsoleColor.White;
                        DateTime data = DateTime.Now;
                        string anoAtual = data.Year.ToString();
                        string mesAtual = data.Month.ToString();
                        string diaAtual = data.Day.ToString();
                        string dataFinal = $"{anoAtual}-{mesAtual}-{diaAtual}";
                        string sql3 = $"INSERT INTO Pedido (idUsuario,dataPedido, qntdImpressao,ehFrenteVerso,idEstoqueA4) VALUES ({idUsuario},'{dataFinal}',{qntdImpressao},{ehFrenteVerso},1)";
                        SqlCommand comando3 = new SqlCommand(sql3, bd.conn);
                        comando3.ExecuteNonQuery();
                    }
                    else
                    {
                        Console.WriteLine($"\nEstoque insuficiente. Estoque atual: {quantidadeEstoqueA4}.");
                        goto sequencia;
                    }
                }
                Program.RetornarMenu();
            }
        }

        // METODO PARA VENDER TICKET AO CLIENTE
        public static void VenderTicket(ConexaoBD bd)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n __      ________ _   _ _____            _______ _____ _____ _  ________ _______ \r\n \\ \\    / /  ____| \\ | |  __ \\   /\\     |__   __|_   _/ ____| |/ /  ____|__   __|\r\n  \\ \\  / /| |__  |  \\| | |  | | /  \\       | |    | || |    | ' /| |__     | |   \r\n   \\ \\/ / |  __| | . ` | |  | |/ /\\ \\      | |    | || |    |  < |  __|    | |   \r\n    \\  /  | |____| |\\  | |__| / ____ \\     | |   _| || |____| . \\| |____   | |   \r\n     \\/   |______|_| \\_|_____/_/    \\_\\    |_|  |_____\\_____|_|\\_\\______|  |_|   \r\n                                                                                 \r\n                                                                                 \r\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInsira o CPF do cliente:");
            string cpfCliente = Console.ReadLine();
            while (Program.ChecarCpf(cpfCliente) != true)
            {
                Program.MostrarMensagemErro("CPF no formato inválido!\nEx: 000.000.000-00");
                Console.WriteLine("Por favor, insira o CPF do cliente:");
                cpfCliente = Console.ReadLine();
            }

            cpfCliente = $"cpf = '{cpfCliente}'";
            Usuario cliente = new Usuario();
            cliente.ObterDados(bd, cpfCliente);

            if (cliente.Nome != "" && cliente.Nome != null)
            {
                int opcao = 0;
                int saldoEscolhido = 0;

                while (opcao != 3)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\r\n  _______ _____ _____ _  ________ _______ _  _____ \r\n |__   __|_   _/ ____| |/ /  ____|__   __( )/ ____|\r\n    | |    | || |    | ' /| |__     | |  |/| (___  \r\n    | |    | || |    |  < |  __|    | |     \\___ \\ \r\n    | |   _| || |____| . \\| |____   | |     ____) |\r\n    |_|  |_____\\_____|_|\\_\\______|  |_|    |_____/ \r\n                                                   \r\n                                                   \r\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Qual ticket o cliente deseja comprar?");
                    Console.WriteLine("[1] Ticket 25 de saldo");
                    Console.WriteLine("[2] Ticket 50 de saldo");
                    Console.WriteLine("[3] Retornar ao Menu");
                    string o = Console.ReadLine();

                    if (Program.ChecarCampo(o))
                        opcao = Convert.ToInt32(o);
                    else
                        opcao = 0;

                    if (opcao == 1 || opcao == 2)
                    {
                        if (opcao == 1)
                            saldoEscolhido = 25;
                        else if (opcao == 2)
                            saldoEscolhido = 50;

                        opcao = 3;

                        List<Pagamento> pagamentos = Pagamento.ListarFormasPagamento(bd);
                        int forma;
                        bool escolheu = false;

                        while (escolheu != true)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\r\n  _____        _____          __  __ ______ _   _ _______ ____  \r\n |  __ \\ /\\   / ____|   /\\   |  \\/  |  ____| \\ | |__   __/ __ \\ \r\n | |__) /  \\ | |  __   /  \\  | \\  / | |__  |  \\| |  | | | |  | |\r\n |  ___/ /\\ \\| | |_ | / /\\ \\ | |\\/| |  __| | . ` |  | | | |  | |\r\n | |  / ____ \\ |__| |/ ____ \\| |  | | |____| |\\  |  | | | |__| |\r\n |_| /_/    \\_\\_____/_/    \\_\\_|  |_|______|_| \\_|  |_|  \\____/ \r\n                                                                \r\n                                                                \r\n");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("\nEscolha a forma de pagamento:");
                            for (int i = 0; i < pagamentos.Count; i++)
                            {
                                Console.WriteLine($"[{i + 1}] {pagamentos[i].Nome}");
                            }

                            Console.WriteLine($"[{pagamentos.Count + 1}] Retornar ao Menu");

                            string f = Console.ReadLine();

                            if (Program.ChecarCampo(f))
                                forma = Convert.ToInt32(f);
                            else
                                forma = 0;

                            if (forma > 0)
                                escolheu = true;

                            switch (forma)
                            {
                                case 1:
                                    // PIX
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\r\n  _____ _______   __\r\n |  __ \\_   _\\ \\ / /\r\n | |__) || |  \\ V / \r\n |  ___/ | |   > <  \r\n | |    _| |_ / . \\ \r\n |_|   |_____/_/ \\_\\\r\n                    \r\n                    \r\n");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Forma de pagamento escolhida: {pagamentos[forma - 1].Nome}");
                                    Console.WriteLine("QRCode para pagamento: \n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(" ▄▄▄▄▄▄▄ ▄▄▄▄ ▄▄▄ ▄   ▄ ▄   ▄▄ ▄▄     ▄  ▄ ▄▄▄▄▄▄▄\r\n █ ▄▄▄ █ ▄ ▄▄▀█▄ ▄▄ ▄▀▀▀█ █  ▀▄▀▀█▀ ▀▄▄▀██ █ ▄▄▄ █\r\n █ ███ █ ██▀ █▀█   █▄▄▀▄████▄ ▄▀▄▄ ▀▄▀█ ▀  █ ███ █\r\n █▄▄▄▄▄█ ▄▀█ █▀▄▀█ ▄▀▄▀█ ▄ █ █▀█▀▄▀▄▀▄▀█ ▄ █▄▄▄▄▄█\r\n ▄▄▄▄  ▄ ▄ █  ▄▀▄  ▀▀█▀█▄▄▄███▀▄█   █ ▀ ▀█▄  ▄▄▄ ▄\r\n   ▄  ▀▄▀▀   █▀█▀▄█▄▄ ▄ ▄ ▀ █▄ █ ▄▄ ▀ ▀▄▄▄█▀▀ ▀▄ ▀\r\n ▀ ▄ ▀█▄ ▄▄▄█▄  █  ▄█▄ █▄▄ ▄  █  ▀▀▀▀█▀▄█▄▄▄ ▀ █ ▀\r\n ▄██  ▄▄▀▄▄  ▄▀▀▀▀ █▄▀█▀   █▀██▄  ▄   ██  █▄▄▀█▀ ▀\r\n █ █ ▄ ▄██▀▀▄▀▀▄ ██▀▄  ▄▀ ▄▄█ ▀▀▀█▄ █ █▄  █▄▄▄▄ ▄▀\r\n  ▀▀▀▄█▄▀▀█▄ ▀▄█▄▀▀▀█ ▄█ █ ▄██▀  ▄▀▀ ▄▄ ▀█▀▄█▄▄▀▄▄\r\n █▀█▄▄█▄   ▄ █ ▄▄▄▀ ▀█▄ █ █▀▄█▀ █ █▄ ▀ ██   █ ▄ ▄ \r\n ▄▀▄▀██▄▄▄██▄▀  █ █▄▄ ▄▄▄▄█▄ ▄ █▀▄█▄█▀▀ ▄▄██▄▄█▄█ \r\n  ████ ▄ █▀ ▀▀▀  █ ▄█▄▄█ ▄ █ ▀█▄ ▀ █▀██▄▀█ ▄ ██▀ ▄\r\n ▄█▄██▄▄▄█ █▄ █ ▄▄▄▀▄▀██▄▄▄█ ▄ ▄▄ █▄▀ ▄█▀█▄▄▄█▄▄ ▀\r\n  ▀ ▄▀▄▄▀▄ ▀▀▄▀▄▀ ▄ ▄ ▀ ▀ ▄ █▀▄█▀█  █   ▀▀   ▄█▄▄ \r\n  █ █  ▄ ▄ █▀█  █▄▄▄▀▄ ▀█▄ ▀ █▄ ▄  █ ▄▀▄▄▀██  ▀▀▀█\r\n  ▄███▀▄▀ ▀▀█▀▄▀█▀▀▀▀█▄▀ ▀  ▀█  █ ▄▄ ▀▄▀▀▀  ▀▄  ▀ \r\n █ █▀█ ▄█  ▄▄▄█▀█ ▀▄ ▄█▀▀▀███▀▀▀ █▄   █▄ ▀  ▄▄▀▄█ \r\n  ▀ ███▄██ ▀▄▄██▀▀▄▀█▄▄▄▀▄▀   ▄▄ ▀▀█▀█ ██▀██   █▀ \r\n ▀▄▀ ▀▄▄█▀█▄█ ▄▀▄▄█ ▄█▄▄▄▄ ▀ ▄█▄██▄ ▀▄▀████ ▀▀ ▄ ▀\r\n ▄██▀  ▄▄▄▀  █▄██▄█▀▄ ███▄█▄ ▀▄▄▀█▄ █ ▀  █▄██▄▄ ▄▀\r\n ▄▄▄▄▄▄▄ ▀██  ▀ ▄▀▀█▀ ▄█ ▄ █▀▀▀ █▀█▀█▄   █ ▄ ███▀█\r\n █ ▄▄▄ █   █▀ █▀ ▄   █▄█▄▄▄███▄ █ ▄█▄▀▄▀██▄▄▄█▄ ▄ \r\n █ ███ █ █▀▄█▄▀▄ █▀▀ ▄   ▀█   █▀▀ █▄██▀ █ ▄█▀█▄▄█▄\r\n █▄▄▄▄▄█ █ ▄███▀▀▄ ▄██▄▀▀█  ▀ █▀ ▀▀▀▄▄█▀▀▄  ▄ █▀▄▄");
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\nChave alternativa: +5547999516147");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    FinalizarCompra(bd, cliente, saldoEscolhido, pagamentos[forma - 1].Id);
                                    break;

                                case 2:
                                    // Cartao de Credito
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\r\n   _____          _____ _______       ____  \r\n  / ____|   /\\   |  __ \\__   __|/\\   / __ \\ \r\n | |       /  \\  | |__) | | |  /  \\ | |  | |\r\n | |      / /\\ \\ |  _  /  | | / /\\ \\| |  | |\r\n | |____ / ____ \\| | \\ \\  | |/ ____ \\ |__| |\r\n  \\_____/_/    \\_\\_|  \\_\\ |_/_/    \\_\\____/ \r\n                                            \r\n                                            \r\n");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Forma de pagamento escolhida: {pagamentos[forma - 1].Nome}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    FinalizarCompra(bd, cliente, saldoEscolhido, pagamentos[forma - 1].Id);
                                    break;

                                case 3:
                                    // Cartao de Debito
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\r\n   _____          _____ _______       ____  \r\n  / ____|   /\\   |  __ \\__   __|/\\   / __ \\ \r\n | |       /  \\  | |__) | | |  /  \\ | |  | |\r\n | |      / /\\ \\ |  _  /  | | / /\\ \\| |  | |\r\n | |____ / ____ \\| | \\ \\  | |/ ____ \\ |__| |\r\n  \\_____/_/    \\_\\_|  \\_\\ |_/_/    \\_\\____/ \r\n                                            \r\n                                            \r\n");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Forma de pagamento escolhida: {pagamentos[forma - 1].Nome}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    FinalizarCompra(bd, cliente, saldoEscolhido, pagamentos[forma - 1].Id);
                                    break;

                                case 4:
                                    // Dinheiro
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\r\n  _____ _____ _   _ _    _ ______ _____ _____   ____  \r\n |  __ \\_   _| \\ | | |  | |  ____|_   _|  __ \\ / __ \\ \r\n | |  | || | |  \\| | |__| | |__    | | | |__) | |  | |\r\n | |  | || | | . ` |  __  |  __|   | | |  _  /| |  | |\r\n | |__| || |_| |\\  | |  | | |____ _| |_| | \\ \\| |__| |\r\n |_____/_____|_| \\_|_|  |_|______|_____|_|  \\_\\\\____/ \r\n                                                      \r\n                                                      \r\n");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Forma de pagamento escolhida: {pagamentos[forma - 1].Nome}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    FinalizarCompra(bd, cliente, saldoEscolhido, pagamentos[forma - 1].Id);
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                Program.MostrarMensagemErro("Cliente não cadastrado!");
                Program.MostrarMensagemErro("Retorne ao menu e cadastre esse cliente para prosseguir.");
                Program.RetornarMenu();
            }
        }

        // METODO PARA FINALIZAR A COMPRA
        public static void FinalizarCompra(ConexaoBD bd, Usuario cliente, int ticket, int pagId)
        {
            int opcao = 0;
            bool NumeroCerto = false;
            while (NumeroCerto != true)
            {
                Console.WriteLine("O pagamento foi concluido com sucesso?");
                Console.WriteLine("[1] Sim");
                Console.WriteLine("[2] Não");
                string o = Console.ReadLine();
                if (Program.ChecarCampo(o))
                    opcao = Convert.ToInt32(o);
                else
                    opcao = 0;

                if (opcao == 1 || opcao == 2)
                    NumeroCerto = true;
            }

            if (opcao == 1)
            {
                cliente.Saldo += ticket;
                string condicao = $"saldo = {cliente.Saldo} WHERE cpf = '{cliente.Cpf}'";
                if (Usuario.AtualizarUsuario(bd, condicao))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nO saldo de {ticket} foi adicionado ao cliente {cliente.Nome}");
                    Console.WriteLine($"Saldo atual de {cliente.Nome}: {cliente.Saldo}");
                    Console.ForegroundColor = ConsoleColor.White;

                    string sql;
                    if (ticket == 25)
                        sql = $"1, 10, {cliente.Id}, {pagId}";
                    else
                        sql = $"0, 20, {cliente.Id}, {pagId}";

                    InserirTicket(bd, sql);
                    Program.RetornarMenu();
                }
                else
                {
                    Program.MostrarMensagemErro("Ocorreu algum problema no sistema ao adicionar o saldo!");
                    Program.MostrarMensagemErro("Contacte a equipe de T.I!");
                    Program.RetornarMenu();
                }
            }
            else
                Program.RetornarMenu();
        }

        // METODO PARA INSERIR O TICKET
        public static void InserirTicket(ConexaoBD bd, string valores)
        {
            string sql = $"INSERT INTO Ticket (eh25, valor, idUsuario, idPagamento) VALUES ({valores})";
            SqlCommand comando = new SqlCommand(sql, bd.conn);
            comando.ExecuteNonQuery();
        }

        // METODO PARA LISTAR OS PEDIDOS
        public static void ListarPedidos(ConexaoBD bd)
        {
            Console.Clear();
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.WriteLine("\r\n  ______ _____ _   _______ _____            _____     _____  ______ _____ _____ _____   ____   _____ \r\n |  ____|_   _| | |__   __|  __ \\     /\\   |  __ \\   |  __ \\|  ____|  __ \\_   _|  __ \\ / __ \\ / ____|\r\n | |__    | | | |    | |  | |__) |   /  \\  | |__) |  | |__) | |__  | |  | || | | |  | | |  | | (___  \r\n |  __|   | | | |    | |  |  _  /   / /\\ \\ |  _  /   |  ___/|  __| | |  | || | | |  | | |  | |\\___ \\ \r\n | |     _| |_| |____| |  | | \\ \\  / ____ \\| | \\ \\   | |    | |____| |__| || |_| |__| | |__| |____) |\r\n |_|    |_____|______|_|  |_|  \\_\\/_/    \\_\\_|  \\_\\  |_|    |______|_____/_____|_____/ \\____/|_____/ \r\n                                                                                                     \r\n                                                                                                     \r\n");
            Console.ForegroundColor= ConsoleColor.White;       
            bool escolheu = false;
            while(escolheu != true)
            {
                Console.WriteLine("\nQual filtro gostaria de utilizar no listamento dos pedidos?");
                Console.WriteLine("\n[1] Listar Pedidos de um data específica");
                Console.WriteLine("[2] Listar Pedidos dos últimos 7 dias");
                Console.WriteLine("[3] Listar Pedidos do último mês");
                Console.WriteLine("[4] Retornar ao menu");
                string o = Console.ReadLine();
                int opcao;

                if (Program.ChecarCampo(o))
                {
                    opcao = Convert.ToInt32(o);
                    escolheu = true;
                }
                else
                    opcao = 0;

                if (opcao == 1)
                {
                    bool consultaFeita = false;
                    while (consultaFeita != true)
                    {
                        Console.WriteLine("\nDigite a data desejada para realizar a consulta:");
                        string dataInserida = Console.ReadLine();
                        //0000-00-00
                        char[] dataChar = dataInserida.ToCharArray();
                        if (dataInserida.Length == 10 && dataChar[4] == '-' && dataChar[7] == '-')
                        {
                            if (dataChar[0] == '0' && dataChar[1] == '0' && dataChar[2] == '0' &&
                                dataChar[3] == '0' && dataChar[6] == '0' && dataChar[9] == '0')
                                Program.MostrarMensagemErro("Você precisa colocar uma data real!");
                            else
                            {
                                consultaFeita = true;
                                string banner = "\r\n   _____ ____  _   _  _____ _    _ _   _______       \r\n  / ____/ __ \\| \\ | |/ ____| |  | | | |__   __|/\\    \r\n | |   | |  | |  \\| | (___ | |  | | |    | |  /  \\   \r\n | |   | |  | | . ` |\\___ \\| |  | | |    | | / /\\ \\  \r\n | |___| |__| | |\\  |____) | |__| | |____| |/ ____ \\ \r\n  \\_____\\____/|_| \\_|_____/ \\____/|______|_/_/    \\_\\\r\n                                                     \r\n                                                     \r\n";
                                string sql = $"SELECT u.nome, FORMAT(p.dataPedido, 'dd/MM/yyyy') dataP, p.ehFrenteVerso, p.qntdImpressao FROM Pedido p JOIN Usuario u ON p.idUsuario = u.id WHERE p.dataPedido = '{dataInserida}'";
                                FiltrarPedido(bd, banner, sql);
                            }
                        }
                        else
                            Program.MostrarMensagemErro("Formato de data incorreta! Coloque primeiro o Ano, Mês e depois Dia.\nEx: 2022-12-01");

                    }
                }
                else if(opcao == 2)
                {
                    DateTime data = DateTime.Now;
                    int semanaPassada = Convert.ToInt32(data.Day.ToString());
                    semanaPassada = semanaPassada - 7;
                    string anoAtual = data.Year.ToString();
                    string mesAtual = data.Month.ToString();
                    string dataFinal = $"{anoAtual}-{mesAtual}-{semanaPassada}";

                    string banner = "\r\n  _    _ _   _______ _____ __  __  ____     __  __ ______  _____ \r\n | |  | | | |__   __|_   _|  \\/  |/ __ \\   |  \\/  |  ____|/ ____|\r\n | |  | | |    | |    | | | \\  / | |  | |  | \\  / | |__  | (___  \r\n | |  | | |    | |    | | | |\\/| | |  | |  | |\\/| |  __|  \\___ \\ \r\n | |__| | |____| |   _| |_| |  | | |__| |  | |  | | |____ ____) |\r\n  \\____/|______|_|  |_____|_|  |_|\\____/   |_|  |_|______|_____/ \r\n                                                                 \r\n                                                                 \r\n";

                    string sql = $"SELECT u.nome, FORMAT(p.dataPedido, 'dd/MM/yyyy') dataP, p.ehFrenteVerso, p.qntdImpressao FROM Pedido p JOIN Usuario u ON p.idUsuario = u.id WHERE p.dataPedido >= '{dataFinal}' AND p.dataPedido <= GETDATE()";
                    FiltrarPedido(bd, banner, sql);
                }
                else if(opcao == 3)
                {
                    DateTime data = DateTime.Now;
                    int mesPassado = Convert.ToInt32(data.Month.ToString());
                    mesPassado = mesPassado - 1;
                    string anoAtual = data.Year.ToString();
                    string diaAtual = data.Day.ToString();
                    string dataFinal = $"{anoAtual}-{mesPassado}-{diaAtual}";

                    string banner = "\r\n  _    _ _   _______ _____ __  __  ____     __  __ ______  _____ \r\n | |  | | | |__   __|_   _|  \\/  |/ __ \\   |  \\/  |  ____|/ ____|\r\n | |  | | |    | |    | | | \\  / | |  | |  | \\  / | |__  | (___  \r\n | |  | | |    | |    | | | |\\/| | |  | |  | |\\/| |  __|  \\___ \\ \r\n | |__| | |____| |   _| |_| |  | | |__| |  | |  | | |____ ____) |\r\n  \\____/|______|_|  |_____|_|  |_|\\____/   |_|  |_|______|_____/ \r\n                                                                 \r\n                                                                 \r\n";

                    string sql = $"SELECT u.nome, FORMAT(p.dataPedido, 'dd/MM/yyyy') dataP, p.ehFrenteVerso, p.qntdImpressao FROM Pedido p JOIN Usuario u ON p.idUsuario = u.id WHERE p.dataPedido >= '{dataFinal}' AND p.dataPedido <= GETDATE()";
                    FiltrarPedido(bd, banner, sql);
                }
            }
        }

        // Filtrar Pedido
        public static void FiltrarPedido(ConexaoBD bd, string banner, string filtro)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(banner);
            Console.ForegroundColor = ConsoleColor.White;

            SqlCommand select = new SqlCommand(filtro, bd.conn);

            using (var reader = select.ExecuteReader())
            {
                bool semResultados = true;

                while (reader.Read())
                {
                    semResultados = false;
                    string nomeCliente = reader.GetString(reader.GetOrdinal("nome"));
                    int qntdImpressao = reader.GetInt32(reader.GetOrdinal("qntdImpressao"));
                    bool ehFrenteVerso = reader.GetBoolean(reader.GetOrdinal("ehFrenteVerso"));
                    string dataPedido = reader.GetString(reader.GetOrdinal("dataP"));

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine($"Nome do cliente: {nomeCliente}");
                    Console.WriteLine($"Data da impressão: {dataPedido}");
                    if (ehFrenteVerso)
                        Console.WriteLine($"Folha(s) frente e verso: Sim");
                    else
                        Console.WriteLine($"Folha(s) frente e verso: Não");

                    Console.WriteLine($"Quantidade de impressão: {qntdImpressao}");
                    Console.WriteLine("-----------------------------\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (!reader.Read() && semResultados)
                    Program.MostrarMensagemErro("Nenhum registro encontrado!");

                Program.RetornarMenu();
            }
        }

        //Estoque
        public static void Estoque(ConexaoBD bd)
        {
            int quantidadeEstoqueA4 = ChecarEstoque(bd);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n  ______  _____ _______ ____   ____  _    _ ______ \r\n |  ____|/ ____|__   __/ __ \\ / __ \\| |  | |  ____|\r\n | |__  | (___    | | | |  | | |  | | |  | | |__   \r\n |  __|  \\___ \\   | | | |  | | |  | | |  | |  __|  \r\n | |____ ____) |  | | | |__| | |__| | |__| | |____ \r\n |______|_____/   |_|  \\____/ \\___\\_\\\\____/|______|\r\n                                                   \r\n                                                   \r\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\nEstoque atual de folhas A4 = {quantidadeEstoqueA4}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInforme a quantidade de folhas A4 para adicionar em estoque:");
            string o = Console.ReadLine();
            if (Program.ChecarCampo(o))
            {
                int qntdAdd = Convert.ToInt32(o);
                string sql = $"UPDATE EstoqueA4 SET quantidade=({quantidadeEstoqueA4 + qntdAdd})";
                SqlCommand comando = new SqlCommand(sql, bd.conn);
                comando.ExecuteNonQuery();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ESTOQUE ATUALIZADO!");
                Console.ForegroundColor = ConsoleColor.White;
                Program.RetornarMenu();
            }
            else
            {
                Program.MostrarMensagemErro("Você inseriu algum valor errado!");
                Program.RetornarMenu();
            }
        }

        // Checar Estoque
        public static int ChecarEstoque(ConexaoBD bd)
        {
            string sql1 = $"Select * from EstoqueA4";
            SqlCommand comando1 = new SqlCommand(sql1, bd.conn);
            int quantidadeEstoqueA4;
            using (var reader1 = comando1.ExecuteReader())
            {
                reader1.Read();
                quantidadeEstoqueA4 = reader1.GetInt32(reader1.GetOrdinal("quantidade"));
            }
            return quantidadeEstoqueA4;
        }
    }
}
