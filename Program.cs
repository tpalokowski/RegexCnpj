using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RegexCnpj
{
    class Program
    {
        static bool VerificarEntradaCnpj(string cnpjString)
        {
            string padraoCnpjRegex = @"^\d\d\d\d\d\d\d\d\d\d\d\d\d\d$";
            if (Regex.IsMatch(cnpjString, padraoCnpjRegex))
            {
                System.Console.WriteLine("Entrada válida");
                return true;
            }
            System.Console.WriteLine("Entrada inválida");
            return false;
        }

        static int[] GerarPesoCnpj(int verificaDigito, int auxiliar)
        {
            int[] cnpjAuxiliar = new int[14];
            for (int i = 0; i < cnpjAuxiliar.Length - verificaDigito; i++) //laço for do tamanho do array - 2 casas dos digitos verificadores
            {
                cnpjAuxiliar[i] = auxiliar;
                auxiliar--;
                if (auxiliar == 1)
                {
                    auxiliar = 9;
                }
            }
            return cnpjAuxiliar;
        }
        static int[] ValidarDigitoVerificador(int[] cnpj, int[] cnpjAuxiliar, int verificaDigito)
        {
            int soma = 0, acumulador = 0, digitoResultado = 0, digitoResto = 0;
            for (int i = 0; i < cnpj.Length - verificaDigito; i++) //laço for do tamanho do array - 2 casas pois é como será validado o primeiro digito verificador
            {
                soma = cnpj[i] * cnpjAuxiliar[i]; //multiplica o valor do cnpj pelo peso correspondente
                acumulador = acumulador + soma; //soma os resultados das multiplicações
            }
            digitoResultado = acumulador / 11; //obtem o resultado das multiplicações dividido por 11
            digitoResto = acumulador % 11; //obtem o resto da divisão por 11

            if (digitoResto < 2) // se o resto é menor que 2, o primeiro dígito verificador passa a valer 0
            {
                cnpj[cnpj.Length - verificaDigito] = 0;
            }
            else  // do contrário, pega-se o valor 11 e subtrai o resto, o valor da subtração passa a ser o dígito verificador em questão
            {
                cnpj[cnpj.Length - verificaDigito] = 11 - digitoResto;
            }
            return cnpj;
        }
        static void ImprimirCnpj(int[] cnpj)
        {
            for (int i = 0; i < cnpj.Length; i++)
            {
                Console.Write($"{cnpj[i]} ");
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Insira seu CNPJ (somente números): ");
            string cnpjObter = Console.ReadLine();
            string cnpjString = cnpjObter;
            int[] cnpj = new int[14]; //declara o array a ser usado pelo usuário
            int[] cnpjAuxiliar = new int[14]; //Usado para fazer as validações e somas
            int[] verificaCnpj = new int[14]; //Usado para validar a entrada com os dígitos verificadores

            if (VerificarEntradaCnpj(cnpjString))
            {
                for (int i = 0; i < cnpj.Length; i++)
                {
                    cnpj[i] = (int)char.GetNumericValue(cnpjString, i); // converte a String com números para um array de inteiros
                    verificaCnpj[i] = (int)char.GetNumericValue(cnpjString, i);
                }
                int auxiliar = 5; //variável auxiliar para a criação dos pesos
                cnpjAuxiliar = GerarPesoCnpj(2, auxiliar);
                cnpj = ValidarDigitoVerificador(cnpj, cnpjAuxiliar, 2);

                auxiliar = 6; //atualiza os pesos com novos valores
                cnpjAuxiliar = GerarPesoCnpj(1, auxiliar);
                cnpj = ValidarDigitoVerificador(cnpj, cnpjAuxiliar, 1);

                Console.Write("\nVerificação do CNPJ:\n");
                ImprimirCnpj(verificaCnpj);

                Console.Write("\nO CNPJ é: "); // imprime o CNPJ completo com ambos os dígitos
                ImprimirCnpj(cnpj);
                if (cnpj.SequenceEqual(verificaCnpj))
                {
                    System.Console.Write("\nO CNPJ de entrada é válido!");
                }
                else
                {
                    System.Console.Write("\nO CNPJ de entrada é inválido!");
                }
            }
            else
            {
                System.Console.WriteLine("CNPJ Incorreto");
            }
        }
    }
}
