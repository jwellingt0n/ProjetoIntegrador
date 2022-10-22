using System;
using System.Data.SqlClient;

namespace ProjetoIntegrador
{
    internal class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public bool EhAdmin { get; set; }
        public int Saldo { get; set; }

        // METODO PARA CHECAR O LOGIN DO ADMIN
        public void ChecarLoginAdmin(ConexaoBD bd)
        {
            bool loginFeito = false;

            while (loginFeito != true)
            {
                Console.WriteLine("Para prosseguirmos, insira o seu CPF para acessar o painel administrativo:");
                string cpfUsuario = Console.ReadLine();

                if (Program.ChecarCpf(cpfUsuario) == false)
                    Program.MostrarMensagemErro("CPF no formato inválido!\nEx: 000.000.000-00");
                else
                {
                    cpfUsuario = $"cpf = '{cpfUsuario}'";
                    ObterDados(bd, cpfUsuario);
                    if (this.Nome != "" && this.Nome != null)
                    {
                        if (this.EhAdmin)
                        {
                            bool senhaCorreta = false;
                            while (senhaCorreta != true)
                            {
                                Console.WriteLine("\nInsira sua senha de administrador:");
                                string senhaUsuario = Console.ReadLine();
                                if (Program.ChecarSenha(senhaUsuario))
                                {
                                    if (this.Senha == senhaUsuario)
                                    {
                                        Console.Clear();
                                        senhaCorreta = true;
                                        loginFeito = true;
                                        Program.Menu(bd, this);
                                    }
                                    else
                                        Program.MostrarMensagemErro("Senha incorreta!");
                                }
                                else
                                    Program.MostrarMensagemErro("Senha invalida!");
                            }
                        }
                        else
                            Program.MostrarMensagemErro("ERRO! Esse usuário não é administrador!");
                    }
                    else
                        Program.MostrarMensagemErro("Nenhum usuario encontrado!");
                }
            }
        }

        // METODO PARA OBTER DADOS DO USUARIO
        public bool ObterDados(ConexaoBD bd, string condicao = "")
        {
            string sql;
            if (condicao != "")
                sql = $"SELECT * FROM Usuario WHERE {condicao}";
            else
                sql = $"SELECT * FROM Usuario";

            SqlCommand comando = new SqlCommand(sql, bd.conn);

            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    this.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    this.Nome = reader.GetString(reader.GetOrdinal("nome"));
                    this.Senha = reader.GetString(reader.GetOrdinal("senha"));
                    this.Cpf = reader.GetString(reader.GetOrdinal("cpf"));
                    this.EhAdmin = reader.GetBoolean(reader.GetOrdinal("ehAdmin"));
                    this.Saldo = reader.GetInt32(reader.GetOrdinal("saldo"));

                }
            }

            if (this.Id > 0)
                return true;
            else
                return false;
        }

        // METODO PARA CADASTRAR USUARIO
        public static void CadastrarUsuario(ConexaoBD bd)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n   _____          _____           _____ _______ _____            _____     _    _  _____ ______ _____  \r\n  / ____|   /\\   |  __ \\   /\\    / ____|__   __|  __ \\     /\\   |  __ \\   | |  | |/ ____|  ____|  __ \\ \r\n | |       /  \\  | |  | | /  \\  | (___    | |  | |__) |   /  \\  | |__) |  | |  | | (___ | |__  | |__) |\r\n | |      / /\\ \\ | |  | |/ /\\ \\  \\___ \\   | |  |  _  /   / /\\ \\ |  _  /   | |  | |\\___ \\|  __| |  _  / \r\n | |____ / ____ \\| |__| / ____ \\ ____) |  | |  | | \\ \\  / ____ \\| | \\ \\   | |__| |____) | |____| | \\ \\ \r\n  \\_____/_/    \\_\\_____/_/    \\_\\_____/   |_|  |_|  \\_\\/_/    \\_\\_|  \\_\\   \\____/|_____/|______|_|  \\_\\\r\n                                                                                                       \r\n                                                                                                       \r\n");
            Console.ForegroundColor = ConsoleColor.White;

            bool cadastroConcluido = false;
            string NomeUsuario = "";
            string CpfUsuario = "";
            string SenhaUsuario = "";
            string EmailUsuario = "";
            int ehAdminUsuario = 0;

            while (cadastroConcluido != true)
            {
                Console.WriteLine("\nINFORME NOME DO USUÁRIO:");
                NomeUsuario = Console.ReadLine();
                if (NomeUsuario.Length < 3)
                    Program.MostrarMensagemErro("\nNome usuario muito pequeno!");
                else
                {
                    Console.WriteLine("\nINFORME O CPF:");
                    CpfUsuario = Console.ReadLine();
                    if(!Program.ChecarCpf(CpfUsuario))
                        Program.MostrarMensagemErro("\nCPF no formato incorreto!\nEx: 000.000.000-00");
                    else
                    {
                        Console.WriteLine("\nINFORME A SENHA:");
                        SenhaUsuario = Console.ReadLine();
                        if(!Program.ChecarSenha(SenhaUsuario))
                            Program.MostrarMensagemErro($"\nA senha deve conter no minimo 6 caracteres e no maximo {Program.TAMANHO_SENHA} caracteres!");
                        else
                        {
                            Console.WriteLine("\nINFORME O E-MAIL:");
                            EmailUsuario = Console.ReadLine();

                            if (EmailUsuario.Length < 6)
                                Program.MostrarMensagemErro("\nEmail no formato incorreto!\nEx:email@gmail.com");
                            else
                            {
                                Console.WriteLine("\n"+ NomeUsuario + " é ADMIN? (1) SIM / (0) NÃO");
                                string a = Console.ReadLine();
                                if (Program.ChecarCampo(a))
                                    ehAdminUsuario = int.Parse(Console.ReadLine());
                                else
                                    ehAdminUsuario = 0;

                                cadastroConcluido = true;
                            }
                        }
                    }
                }
            }

            string sqlx = $"INSERT INTO Usuario (Nome, Senha, Cpf, ehAdmin, saldo) VALUES ('{NomeUsuario}','{SenhaUsuario}', '{CpfUsuario}', '{ehAdminUsuario}', {0})";
            string sqly = $"Select id from Usuario where Cpf = '{CpfUsuario}'";
            SqlCommand comandoX = new SqlCommand(sqlx, bd.conn);
            comandoX.ExecuteNonQuery();
            SqlCommand comandoY = new SqlCommand(sqly, bd.conn);
            int IdUsuarioCadastrado;
            using (var reader1 = comandoY.ExecuteReader())
            {
                reader1.Read();
                IdUsuarioCadastrado = reader1.GetInt32(reader1.GetOrdinal("id"));
            }
            string sqlz = $"INSERT INTO Email (idUsuario, email) VALUES ('{IdUsuarioCadastrado}','{EmailUsuario}')";
            SqlCommand comandoZ = new SqlCommand(sqlz, bd.conn);
            comandoZ.ExecuteNonQuery();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nUsuario cadastrado com sucesso!");
            Console.ForegroundColor = ConsoleColor.White;
            Program.RetornarMenu();
        }

        // METODO PARA ATUALIZAR DADOS DO USUARIO
        public static bool AtualizarUsuario(ConexaoBD bd, string condicao)
        {
            string sql = $"UPDATE Usuario SET {condicao}";
            SqlCommand comando = new SqlCommand(sql, bd.conn);
            if (comando.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }

        // METODO PARA CONSULTAR O SALDO DO CLIENTE
        public static void ConsultarSaldoCliente(ConexaoBD bd)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n   _____ ____  _   _  _____ _    _ _   _______       _____     _____         _      _____   ____  \r\n  / ____/ __ \\| \\ | |/ ____| |  | | | |__   __|/\\   |  __ \\   / ____|  /\\   | |    |  __ \\ / __ \\ \r\n | |   | |  | |  \\| | (___ | |  | | |    | |  /  \\  | |__) | | (___   /  \\  | |    | |  | | |  | |\r\n | |   | |  | | . ` |\\___ \\| |  | | |    | | / /\\ \\ |  _  /   \\___ \\ / /\\ \\ | |    | |  | | |  | |\r\n | |___| |__| | |\\  |____) | |__| | |____| |/ ____ \\| | \\ \\   ____) / ____ \\| |____| |__| | |__| |\r\n  \\_____\\____/|_| \\_|_____/ \\____/|______|_/_/    \\_\\_|  \\_\\ |_____/_/    \\_\\______|_____/ \\____/ \r\n                                                                                                  \r\n                                                                                                  \r\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInsira o CPF do cliente:");
            string CpfUsuario = Console.ReadLine();

            if (!Program.ChecarCpf(CpfUsuario))
            {
                Program.MostrarMensagemErro("CPF no formato incorreto!");
                Program.MostrarMensagemErro("Ex: 000.000.000-00");

                Program.RetornarMenu();

            }
            else
            {
                string sql = $"SELECT * FROM Usuario WHERE cpf = '{CpfUsuario}'";
                SqlCommand comando = new SqlCommand(sql, bd.conn);
                using (var reader = comando.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        var saldoDb = reader.GetInt32(reader.GetOrdinal("saldo"));
                        var nomeDb = reader.GetString(reader.GetOrdinal("nome"));
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n{nomeDb} possui {saldoDb} de saldo.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Program.MostrarMensagemErro("\nUsuário não encontrado!");

                }

                Program.RetornarMenu();
            }
        }

        // METODO PARA GERENCIAR USUARIO
        public static void GerenciarUsuario(ConexaoBD bd)
        {
            int opcao = 0;
            while (opcao != 6)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\r\n /$$   /$$  /$$$$$$  /$$   /$$  /$$$$$$  /$$$$$$$  /$$$$$$  /$$$$$$ \r\n| $$  | $$ /$$__  $$| $$  | $$ /$$__  $$| $$__  $$|_  $$_/ /$$__  $$\r\n| $$  | $$| $$  \\__/| $$  | $$| $$  \\ $$| $$  \\ $$  | $$  | $$  \\ $$\r\n| $$  | $$|  $$$$$$ | $$  | $$| $$$$$$$$| $$$$$$$/  | $$  | $$  | $$\r\n| $$  | $$ \\____  $$| $$  | $$| $$__  $$| $$__  $$  | $$  | $$  | $$\r\n| $$  | $$ /$$  \\ $$| $$  | $$| $$  | $$| $$  \\ $$  | $$  | $$  | $$\r\n|  $$$$$$/|  $$$$$$/|  $$$$$$/| $$  | $$| $$  | $$ /$$$$$$|  $$$$$$/\r\n \\______/  \\______/  \\______/ |__/  |__/|__/  |__/|______/ \\______/ \r\n                                                                    \r\n                                                                    \r\n                                                                    \r\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Opções de navegação: \n");
                Console.WriteLine("[1] Cadastrar Usuário");
                Console.WriteLine("[2] Cadastrar e-mail alternativo");
                Console.WriteLine("[3] Consultar Saldo do Usuário");
                Console.WriteLine("[4] Alterar Senha do Usuário");
                Console.WriteLine("[5] Consultar pelo e-mail do Usuário");
                Console.WriteLine("[6] Voltar Para Menu Principal");
                Console.WriteLine("===============================");
                Console.WriteLine("Insira o número referente a opção que deseja acessar: ");
                string o = Console.ReadLine();
                if (Program.ChecarCampo(o))
                    opcao = Convert.ToInt32(o);
                else
                    opcao = 0;
                switch (opcao)
                {
                    case 1:
                        CadastrarUsuario(bd);
                        break;
                    case 2:
                        CadastroEmailAlternativo(bd);
                        break;
                    case 3:
                        ConsultarSaldoCliente(bd);
                        break;
                    case 4:
                        AlterarSenha(bd);
                        break;
                    case 5:
                        ConsultarEmailCliente(bd);
                        break;
                    case 6:
                        // Voltar menu principal
                        Console.Clear();
                        break;
                }
            }
        }

        // METODO PARA CADASTRAR EMAIL ALTERNATIVO
        public static void CadastroEmailAlternativo(ConexaoBD bd)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n   _____          _____           _____ _______ _____            _____      ______ __  __          _____ _      \r\n  / ____|   /\\   |  __ \\   /\\    / ____|__   __|  __ \\     /\\   |  __ \\    |  ____|  \\/  |   /\\   |_   _| |     \r\n | |       /  \\  | |  | | /  \\  | (___    | |  | |__) |   /  \\  | |__) |   | |__  | \\  / |  /  \\    | | | |     \r\n | |      / /\\ \\ | |  | |/ /\\ \\  \\___ \\   | |  |  _  /   / /\\ \\ |  _  /    |  __| | |\\/| | / /\\ \\   | | | |     \r\n | |____ / ____ \\| |__| / ____ \\ ____) |  | |  | | \\ \\  / ____ \\| | \\ \\    | |____| |  | |/ ____ \\ _| |_| |____ \r\n  \\_____/_/    \\_\\_____/_/    \\_\\_____/   |_|  |_|  \\_\\/_/    \\_\\_|  \\_\\   |______|_|  |_/_/    \\_\\_____|______|\r\n                                                                                                                \r\n                                                                                                                \r\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Informe o CPF do usuário:");
            string CpfUsuario = Console.ReadLine();
            int idDb;
            string sql = $"Select * from Usuario where cpf = '{CpfUsuario}'";
            SqlCommand comando = new SqlCommand(sql, bd.conn);
            using (var reader = comando.ExecuteReader())
            {
                if (reader.Read() == false)
                {
                    Program.MostrarMensagemErro("\nUsuário não cadastrado!");
                    Program.RetornarMenu();
                    return;
                }
                else
                    idDb = reader.GetInt32(reader.GetOrdinal("id"));
            }
            Console.WriteLine("\nInforme o email adicional do usuário:");
            string EmailUsuario = Console.ReadLine();
            Console.Clear();
            string sqlX = $"INSERT INTO Email (idUsuario,email) VALUES ({idDb},'{EmailUsuario}')";
            SqlCommand comandoX = new SqlCommand(sqlX, bd.conn);
            comandoX.ExecuteNonQuery();
            Console.WriteLine("Email cadastrado com sucesso!");
            Program.RetornarMenu();
        }

        // METODO PARA ALTERAR A SENHA
        public static void AlterarSenha(ConexaoBD bd)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n           _   _______ ______ _____            _____       _____ ______ _   _ _    _          \r\n     /\\   | | |__   __|  ____|  __ \\     /\\   |  __ \\     / ____|  ____| \\ | | |  | |   /\\    \r\n    /  \\  | |    | |  | |__  | |__) |   /  \\  | |__) |   | (___ | |__  |  \\| | |__| |  /  \\   \r\n   / /\\ \\ | |    | |  |  __| |  _  /   / /\\ \\ |  _  /     \\___ \\|  __| | . ` |  __  | / /\\ \\  \r\n  / ____ \\| |____| |  | |____| | \\ \\  / ____ \\| | \\ \\     ____) | |____| |\\  | |  | |/ ____ \\ \r\n /_/    \\_\\______|_|  |______|_|  \\_\\/_/    \\_\\_|  \\_\\   |_____/|______|_| \\_|_|  |_/_/    \\_\\\r\n                                                                                              \r\n                                                                                              \r\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Informe o CPF do usuário:");
            string CpfUsuario = Console.ReadLine();
            string senhaDb;
            string sql = $"Select * from Usuario where cpf = '{CpfUsuario}'";
            SqlCommand comando = new SqlCommand(sql, bd.conn);
            using (var reader = comando.ExecuteReader())
            {
                if (reader.Read() == false)
                {
                    Program.MostrarMensagemErro("\nUsuário não cadastrado!");
                    Program.RetornarMenu();
                    return;
                }
                else
                {
                    senhaDb = reader.GetString(reader.GetOrdinal("senha"));
                    CpfUsuario = reader.GetString(reader.GetOrdinal("cpf"));
                }
            }
            Console.WriteLine("\nInforme a senha atual:");
            string SenhaAtual = Console.ReadLine();
            string NovaSenha = "1";
            string SenhaCheck = "0";
            if (SenhaAtual == senhaDb)
            {
                while (NovaSenha != SenhaCheck)
                {
                    Console.WriteLine("\nInforma a nova senha:");
                    NovaSenha = Console.ReadLine();
                    Console.WriteLine("\nInforme novamente a nova senha:");
                    SenhaCheck = Console.ReadLine();
                    Console.Clear();
                    if (NovaSenha != SenhaCheck)
                        Program.MostrarMensagemErro("Senhas informadas precisam ser iguais!");
                }
            }
            else
            {
                Program.MostrarMensagemErro("Senha incorreta. Alteração de senha não aprovada!");
                Program.RetornarMenu();
                return;
            }
            string sqlX = $"UPDATE Usuario SET senha='{NovaSenha}' WHERE cpf='{CpfUsuario}'";
            SqlCommand comandoX = new SqlCommand(sqlX, bd.conn);
            comandoX.ExecuteNonQuery();
            Console.WriteLine("\nAlteração feita com sucesso!");
            Program.RetornarMenu();
        }

        // METODO PARA CONSULTAR PELO EMAIL DO CLIENTE DADOS
        public static void ConsultarEmailCliente(ConexaoBD bd)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n   _____ ____  _   _  _____ _    _ _   _______          ______      __  __          _____ _      \r\n  / ____/ __ \\| \\ | |/ ____| |  | | | |__   __|/\\      |  ____|    |  \\/  |   /\\   |_   _| |     \r\n | |   | |  | |  \\| | (___ | |  | | |    | |  /  \\     | |__ ______| \\  / |  /  \\    | | | |     \r\n | |   | |  | | . ` |\\___ \\| |  | | |    | | / /\\ \\    |  __|______| |\\/| | / /\\ \\   | | | |     \r\n | |___| |__| | |\\  |____) | |__| | |____| |/ ____ \\   | |____     | |  | |/ ____ \\ _| |_| |____ \r\n  \\_____\\____/|_| \\_|_____/ \\____/|______|_/_/    \\_\\  |______|    |_|  |_/_/    \\_\\_____|______|\r\n                                                                                                 \r\n                                                                                                 \r\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Informe o E-mail para realizar a busca no Banco de Dados");
            string EmailUsuario = Console.ReadLine();
            string NomeUsuario;
            int SaldoUsuario;
            string CpfUsuario;
            int idUsuario;
            string sql = $"Select * from Email where email = '{EmailUsuario}'";
            SqlCommand comando = new SqlCommand(sql, bd.conn);
            using (var reader = comando.ExecuteReader())
            {
                if (reader.Read() == false)
                {
                    Program.MostrarMensagemErro("\nUsuário não cadastrado!");
                    Program.RetornarMenu();
                    return;
                }
                else
                    idUsuario = reader.GetInt32(reader.GetOrdinal("idUsuario"));
            }
            string sqlX = $"Select * from Usuario where id = {idUsuario}";
            SqlCommand comandoX = new SqlCommand(sqlX, bd.conn);
            using (var reader = comandoX.ExecuteReader())
            {
                reader.Read();
                NomeUsuario = reader.GetString(reader.GetOrdinal("nome"));
                SaldoUsuario = reader.GetInt32(reader.GetOrdinal("saldo"));
                CpfUsuario = reader.GetString(reader.GetOrdinal("cpf"));
            }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nNome do Usuário: " + NomeUsuario);
            Console.WriteLine("CPF do Usuário: " + CpfUsuario);
            Console.WriteLine("Saldo do Usuário: " + SaldoUsuario);
            Console.ResetColor();
            Program.RetornarMenu();
        }
    }

}
