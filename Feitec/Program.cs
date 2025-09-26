using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Feitec
{
    class Visitante
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string WhatsApp { get; set; }

        public string Cursos { get; set; }

        public string DataVisita { get; set; }


        public override string ToString()
        {
            return $"{Nome}, {Email}, {WhatsApp}, {Cursos},{DataVisita}";
        }
        class Program
        {
            static string caminhoArquivo = @"C:\Feitec2025\feitec2025.csv";

            static List<string> cursosDisponiveis = new List<string>
        {
            "Administração",
            "Agronomia",
            "Análise de Desenvolvimento de Sistemas",
            "Arquitetura e Urbanismo",
            "Biomedcina",
            "Bacharelado Interdisciplinar em Saúde",
            "Ciências Contábeis",
            "Direito",
            "Enfermagem",
            "Engenharia Civil",
            "Engenharia Química",
            "Outros"
        };

            static List<Visitante> visitantes = new List<Visitante>();

            static void Main(string[] args)
            {

                //Vaiaveis



                //Comandos

                CarregarDados();

                int opcao;

                do
                {
                    Console.Clear();

                    Console.WriteLine("*** SISTEMA DE CADASTRO DE VISITANTES ***");
                    Console.WriteLine();
                    Console.WriteLine("1 - Cadastrar Visitante");
                    Console.WriteLine("2 - Listar Visitantes");
                    Console.WriteLine("0 - Encerrar Sistema");
                    Console.Write("Escolha uma opção: ");

                    if (!int.TryParse(Console.ReadLine(), out opcao))
                    {
                        opcao = -1;
                    }

                    switch (opcao)
                    {
                        case 0:
                            Console.WriteLine("Encerrando Sistema");
                            break;

                        case 1:
                            CadastrarVisitantes(); //metodo cadastrar
                            break;

                        case 2:
                            ListarVisitantes(); //metodo listar
                            break;

                        default:
                            Console.WriteLine("Opção Inválida");
                            break;
                    }
                    if (opcao != 0)
                    {
                        Console.WriteLine("Pressione <ENTER> para continuar");
                        Console.ReadLine();
                    }
                } while (opcao != 0);
            }

            static void CadastrarVisitantes()
            {
                string nome, email, whatsapp, curso, cursos, data;

                Console.Write("Nome Completo: ");
                nome = Console.ReadLine().ToUpper();

                if (nome == "")
                {
                    return;
                }

                Console.Write("E-mail: ");
                email = Console.ReadLine().ToLower();

                Console.Write("WhatsApp: ");
                whatsapp = LerTelefone();

                Console.WriteLine("Cursos de Interesse");
                Console.WriteLine("(Informe sepsrados por vírgula)");
                Console.WriteLine("Exemplo: 1,3,4");

                for (int i = 0; i < cursosDisponiveis.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {cursosDisponiveis[i]}");
                }

                Console.WriteLine("Digite o número dos cursos (1,3,4)");
                curso = Console.ReadLine();

                List<string> cursoSelecionado = new List<string>();
                foreach (var item in curso.Split(','))
                {
                    if (int.TryParse(item.Trim(), out int indice) && indice >= 1 && indice <= cursosDisponiveis.Count)
                    {
                        cursoSelecionado.Add(cursosDisponiveis[indice - 1]);
                    }
                }

                cursos = string.Join(",", cursoSelecionado);
                data = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                visitantes.Add(new Visitante
                {
                    Nome = nome,
                    Email = email,
                    WhatsApp = whatsapp,
                    Cursos = cursos,
                    DataVisita = data
                });

                //Salvar imediatamente após o cadastro

                SalvarDados();

            }

            static void ListarVisitantes()
            {
                Console.WriteLine("\n--- Lista de Visitantes ---");
                if (visitantes.Count == 0)
                {
                    Console.WriteLine("Nenhum visitante cadastrado.");
                    return;
                }

                int i = 1;
                foreach (var v in visitantes)
                {
                    Console.WriteLine($"{i++} - {v.Nome} | {v.Email} | {v.WhatsApp} | {v.Cursos} | {v.DataVisita}");

                }
            }
            static void SalvarDados()
            {
                try
                {
                    // Garante que a pasta existe
                    string pasta = Path.GetDirectoryName(caminhoArquivo);
                    if (!Directory.Exists(pasta))
                    {
                        Directory.CreateDirectory(pasta);
                    }
                    //Grava os dados no arquivo (criando se não existir)
                    using (StreamWriter sw = new StreamWriter(caminhoArquivo))
                    {
                        foreach (var v in visitantes)
                        {
                            sw.WriteLine($"{v.Nome};{v.Email};{v.WhatsApp};{v.Cursos};{v.DataVisita} ");
                        }
                    }



                    Console.WriteLine("Visitante cadastrado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao salvar os dados:  {ex.Message}");
                }
            }

            static void CarregarDados()
            {
                if (!File.Exists(caminhoArquivo))
                {
                    return;
                }
                visitantes.Clear(); // evita duplicação cas carregue ais de uma vez
                using (StreamReader sr = new StreamReader(caminhoArquivo))
                {
                    string linha;
                    while ((linha = sr.ReadLine()) != null)
                    {
                        var partes = linha.Split(';');
                        if (partes.Length == 5)
                        {
                            visitantes.Add(new Visitante
                            {
                                Nome = partes[0],
                                Email = partes[1],
                                WhatsApp = partes[2],
                                Cursos = partes[3],
                                DataVisita = partes[4],
                            });
                        }
                    }
                }
            }
            static string LerTelefone()
            {
                string telefone;
                Regex regexTelefone = new Regex(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$");
                //teste
                do
                {
                    Console.Write("(formato: (99)99999-9999): ");
                    telefone = Console.ReadLine();

                    if (!regexTelefone.IsMatch(telefone))
                    {
                        Console.WriteLine("Telefone inválido! Tente novamente");
                    }

                } while (!regexTelefone.IsMatch(telefone));

                return telefone;
            }
        }
    }
}