using System;

namespace ProjetoIntegrador
{

    internal class Program
    {
        // CONSTANTES
        public static int TAMANHO_CPF = 14;
        public static int TAMANHO_SENHA = 30;

        // OBJETO DO BANCO DE DADOS
        public static ConexaoBD bd = new ConexaoBD();

        // CORAÇÃO DO SOFTWARE
        static void Main(string[] args)
        {
            bd.Conectar();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n  /$$$$$$        /$$$$$$$$       /$$   /$$        /$$$$$$         /$$$$$$ \r\n /$$__  $$      | $$_____/      | $$$ | $$       /$$__  $$       /$$__  $$\r\n| $$  \\__/      | $$            | $$$$| $$      | $$  \\ $$      | $$  \\__/\r\n|  $$$$$$       | $$$$$         | $$ $$ $$      | $$$$$$$$      | $$      \r\n \\____  $$      | $$__/         | $$  $$$$      | $$__  $$      | $$      \r\n /$$  \\ $$      | $$            | $$\\  $$$      | $$  | $$      | $$    $$\r\n|  $$$$$$/      | $$$$$$$$      | $$ \\  $$      | $$  | $$      |  $$$$$$/\r\n \\______/       |________/      |__/  \\__/      |__/  |__/       \\______/ \r\n                                                                          \r\n                                                                          \r\n                                                                          \r\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bem-Vindo ao Sistema de Impressões do SENAC!");
            Console.ForegroundColor = ConsoleColor.White;

            Usuario admin = new Usuario();
            admin.ChecarLoginAdmin(bd);
        }

        // METODO REFERENTE AO MENU
        public static void Menu(ConexaoBD bd, Usuario adm)
        {
            int opcao = 0;

            while (opcao != 6)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\r\n  /$$$$$$  /$$$$$$$$ /$$   /$$  /$$$$$$   /$$$$$$         /$$$$$$  /$$$$$$$  /$$      /$$ /$$$$$$ /$$   /$$\r\n /$$__  $$| $$_____/| $$$ | $$ /$$__  $$ /$$__  $$       /$$__  $$| $$__  $$| $$$    /$$$|_  $$_/| $$$ | $$\r\n| $$  \\__/| $$      | $$$$| $$| $$  \\ $$| $$  \\__/      | $$  \\ $$| $$  \\ $$| $$$$  /$$$$  | $$  | $$$$| $$\r\n|  $$$$$$ | $$$$$   | $$ $$ $$| $$$$$$$$| $$            | $$$$$$$$| $$  | $$| $$ $$/$$ $$  | $$  | $$ $$ $$\r\n \\____  $$| $$__/   | $$  $$$$| $$__  $$| $$            | $$__  $$| $$  | $$| $$  $$$| $$  | $$  | $$  $$$$\r\n /$$  \\ $$| $$      | $$\\  $$$| $$  | $$| $$    $$      | $$  | $$| $$  | $$| $$\\  $ | $$  | $$  | $$\\  $$$\r\n|  $$$$$$/| $$$$$$$$| $$ \\  $$| $$  | $$|  $$$$$$/      | $$  | $$| $$$$$$$/| $$ \\/  | $$ /$$$$$$| $$ \\  $$\r\n \\______/ |________/|__/  \\__/|__/  |__/ \\______/       |__/  |__/|_______/ |__/     |__/|______/|__/  \\__/\r\n                                                                                                           \r\n                                                                                                           \r\n                                                                                                           \r\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================");
                Console.WriteLine($"Sessão do Admin {adm.Nome} ({adm.Cpf}) ativa!");
                Console.WriteLine("Opções de navegação: \n");
                Console.WriteLine("[1] Estoque");
                Console.WriteLine("[2] Gerenciar Usuário(s)");
                Console.WriteLine("[3] Vender Ticket");
                Console.WriteLine("[4] Realizar Impressão");
                Console.WriteLine("[5] Listar Pedidos");
                Console.WriteLine("[6] Encerrar sessão");
                Console.WriteLine("===============================");
                Console.WriteLine("Insira o número referente a opção que deseja acessar: ");

                string o = Console.ReadLine();
              
                if (ChecarCampo(o))
                    opcao = Convert.ToInt32(o);
                else
                    opcao = 0;

                switch (opcao)
                {
                    case 1:
                        Vendas.Estoque(bd);
                        break;

                    case 2:
                        Usuario.GerenciarUsuario(bd);
                        break;

                    case 3:
                        Vendas.VenderTicket(bd);
                        break;

                    case 4:
                        Vendas.CadastroImpressao(bd);
                        break;

                    case 5:
                        Vendas.ListarPedidos(bd);
                        break;

                    case 6:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\r\n  ______ ______ _____ _    _          _   _ _____   ____     _____ ______  _____ _____         ____                \r\n |  ____|  ____/ ____| |  | |   /\\   | \\ | |  __ \\ / __ \\   / ____|  ____|/ ____/ ____|  /\\   / __ \\               \r\n | |__  | |__ | |    | |__| |  /  \\  |  \\| | |  | | |  | | | (___ | |__  | (___| (___   /  \\ | |  | |              \r\n |  __| |  __|| |    |  __  | / /\\ \\ | . ` | |  | | |  | |  \\___ \\|  __|  \\___ \\\\___ \\ / /\\ \\| |  | |              \r\n | |    | |___| |____| |  | |/ ____ \\| |\\  | |__| | |__| |  ____) | |____ ____) |___) / ____ \\ |__| |  _ _ _ _ _ _ \r\n |_|    |______\\_____|_|  |_/_/    \\_\\_| \\_|_____/ \\____/  |_____/|______|_____/_____/_/    \\_\\____/  (_|_|_|_|_|_)\r\n                                                                                                                   \r\n                                                                                                                   \r\n");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Sessão finalizada!");
                        bd.Desconectar();
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Verificacoes para contornar uns fatal errors
        public static bool ChecarCampo(string campo)
        {
            if (campo == " ")
                return false;
            else if (campo == "")
                return false;
            else if (campo == "0")
                return false;
            else if (campo == null)
                return false;
            else
                return true;
        }

        // METODO PARA CHECAR O FORMATO DO CPF
        public static bool ChecarCpf(string cpf)
        {
            if (cpf.Length == TAMANHO_CPF)
            {
                char[] characteres = cpf.ToCharArray();
                if (characteres[3] == '.' && characteres[7] == '.' && characteres[11] == '-')
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        // METODO PARA CHECAR O FORMATO DA SENHA
        public static bool ChecarSenha(string senha)
        {
            if (senha.Length > TAMANHO_SENHA || senha.Length < 6)
                return false;
            else
                return true;
        }

        // METODO PARA MOSTRAR UMA MENSAGEM DE ERRO COM A COR VERMELHA
        public static void MostrarMensagemErro(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        // METODO PARA RETORNAR O ADMIN PRO MENU
        public static void RetornarMenu()
        {
            Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR");
            Console.ReadLine();
        }
    }
}
