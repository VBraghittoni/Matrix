using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Criptografia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Vetor com os caracteres. O programa foi feito para ser possível adicionar caracteres nesse vetor sem precisar consertar mais nada no resto do código.
            char[] alfabeto = { '!', ' ', '@', '#', '$', '%', '&', '*', '(', ')', '<', '>', '.', ',', ';', ':', '/', '?', '[', ']', '{', '}', '=', '+', '-', '_', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'á', 'Á', 'à', 'À', 'ã', 'Ã', 'ñ', 'Ñ', 'é', 'É', 'ê', 'Ê', 'í', 'Í', 'ó', 'Ó' };

            //Funções:
            //Dado um caracter e um vetor, essa função acha a posição na qual esse caracter aparece pela primeira vez no vetor.
            float posicaoCaracterNoVetor(char caracter, char[] vetor)
            {
                for (int posicao = 0; posicao < vetor.Length; posicao++)
                {
                    if (caracter == vetor[posicao])
                    {
                        return posicao;
                    }
                }
                return 666;
            }

            //Acha o tamanho do texto que será usado para fazer a matriz. Note que esse tamanho será necessariamente múltiplo de quatro, visto que o texto a ser criptografado será dividido em várias matrizes 2 x 2.
            int tamanhoMatriz(string texto)
            {
                int i = texto.Length;

                while (true)
                {
                    if (i % 4 == 0)
                    {
                        return i;
                    }
                    i++;
                }
            }

            //Essa função adiciona ' ' ao texto até que o comprimento desse texto seja igual ao tamanho (múltiplo de quatro) encontrado na função anterior
            string preenchedor(string texto)
            {
                int k = tamanhoMatriz(texto);
                while (true)
                {
                    if (texto.Length == k)
                    {
                        return texto;
                    }
                    texto += " ";
                }
            }

            //Transforma a matriz númerica em uma palavra. Transforma número "n" no caracter que ocupa o índice n % alfabeto.Lenght no alfabeto
            string matrizParaPalavra(float[,,] matriz)
            {
                string palavrinha = "";
                for (int indiceMatriz = 0; indiceMatriz < matriz.GetLength(0); indiceMatriz++)
                {
                    for (int linha = 0; linha < matriz.GetLength(1); linha++)
                    {
                        for (int coluna = 0; coluna < matriz.GetLength(2); coluna++)
                        {
                            if (matriz[indiceMatriz, linha, coluna] >= 0)
                            {
                                palavrinha += alfabeto[Convert.ToInt64((matriz[indiceMatriz, linha, coluna]) % alfabeto.Length)];
                            }
                            else
                            {
                                double negatividade = Math.Ceiling((-matriz[indiceMatriz, linha, coluna]) / alfabeto.Length);
                                palavrinha += alfabeto[Convert.ToInt64((matriz[indiceMatriz, linha, coluna] + negatividade * alfabeto.Length) % alfabeto.Length)];
                            }
                        }
                    }
                }
                return palavrinha;
            }

            //Multiplica cada matriz no conjunto de matrizes por uma matriz específica (a que será usada como chave)
            float[,,] multiplicarMatrizes(float[,,] matriz1, float[,] matriz2)
            {
                float[,,] produto = new float[matriz1.GetLength(0), matriz1.GetLength(1), matriz1.GetLength(2)];
                for (int indiceMatriz = 0; indiceMatriz < matriz1.GetLength(0); indiceMatriz++)
                {
                    for (int linha = 0; linha < matriz1.GetLength(1); linha++)
                    {
                        for (int coluna = 0; coluna < matriz1.GetLength(2); coluna++)
                        {
                            for (int numSoma = 0; numSoma < matriz1.GetLength(2); numSoma++)
                            {
                                produto[indiceMatriz, linha, coluna] += matriz1[indiceMatriz, linha, numSoma] * matriz2[numSoma, coluna];
                            }
                        }
                    }
                }
                return produto;
            }

            //Transforma a palavra dada em quantas matrizes 2x2 forem necessárias
            float[,,] geradorDeMatrizes(string texto)
            {
                float[,,] matrizGerada = new float[preenchedor(texto).Length / 4, 2, 2];
                for (int nDeMatrizes = 0; nDeMatrizes < (preenchedor(texto).Length) / 4; nDeMatrizes++)
                {
                    for (int linha = 0; linha < 2; linha++)
                    {
                        for (int coluna = 0; coluna < 2; coluna++)
                        {
                            matrizGerada[nDeMatrizes, linha, coluna] = posicaoCaracterNoVetor(texto[nDeMatrizes * 4 + 2 * linha + coluna], alfabeto);
                        }
                    }
                }
                return matrizGerada;
            }

            //Queremos uma matriz resultante cujo determinante é 1. Sabendo disso, usamos o Teorema de Jacobi, que diz que é possível somar/subtrair os números de uma linha na outra quantas vezes quisermos sem mudar o determinante. Assim, partindo de uma matriz cujo determinante é 1, somamos "n" vezes a linha 1 na linha linha 2 para obtermos a matriz chave. Para definirmos "n", somamos os índices de cada um dos caracteres
            float[,] matrizChave(string texto)
            {
                float[,] matrizGerada = new float[2, 2];
                long multiplicador = 1;
                for (int pos = 0; pos < texto.Length; pos++)
                {
                    multiplicador += Convert.ToInt64(posicaoCaracterNoVetor(texto[pos], alfabeto));
                }
                matrizGerada[0, 0] = 4;
                matrizGerada[0, 1] = 11;
                matrizGerada[1, 0] = 1 + multiplicador * matrizGerada[0, 0];
                matrizGerada[1, 1] = 3 + multiplicador * matrizGerada[0, 1];

                return matrizGerada;
            }

            //Faz o processo padrão de matriz inversa de uma matriz 2 x 2
            float[,] matrizInversa(float[,] matriz)
            {
                float determinante = Convert.ToInt32(matriz[0, 0] * matriz[1, 1] - matriz[0, 1] * matriz[1, 0]);
                if (determinante == 0)
                {
                    Console.WriteLine("PROBLEMA!! Determinante igual a 0!!");
                }
                float[,] inversa = new float[matriz.GetLength(0), matriz.GetLength(1)];
                inversa[0, 0] = matriz[1, 1] / determinante;
                inversa[0, 1] = -matriz[0, 1] / determinante;
                inversa[1, 0] = -matriz[1, 0] / determinante;
                inversa[1, 1] = matriz[0, 0] / determinante;

                return inversa;
            }


            //Criando variáveis e interagindo com o usuário:
            string palavra, chave, crip, descrip, opcaoIntermediario;
            char opcao, opcao2, resposta;
            bool funcionando = true;
            Console.WriteLine("AVISO: palavras criptografadas e senhas geradas por nós estarão delimitadas no seguinte formato: |palavra|");
            while (funcionando)
            {
                funcionando = false;
                do
                {
                    Console.WriteLine("Digite 'A' ou 'a' para criptografar ou digite 'B' ou 'b' para decodificar uma mensagem: ");
                    opcaoIntermediario = Console.ReadLine();
                    opcao = Convert.ToChar(opcaoIntermediario[0]);
                } while (opcao != 'a' && opcao != 'A' && opcao != 'b' && opcao != 'B');

                if (opcao == 'A' || opcao == 'a')
                {
                    Console.WriteLine("Escreva a palavra a ser criptografada: ");
                    palavra = Console.ReadLine();

                    do
                    {
                        Console.WriteLine("Digite 'A' para inserir uma chave ou digite 'B' para que criemos uma: ");
                        opcaoIntermediario = Console.ReadLine();
                        opcao2 = Convert.ToChar(opcaoIntermediario[0]);
                    } while (opcao2 != 'a' && opcao2 != 'A' && opcao2 != 'b' && opcao2 != 'B');

                    if (opcao2 == 'A' || opcao2 == 'a')
                    {
                        Console.WriteLine("Digite uma chave: ");
                        chave = Console.ReadLine();
                        crip = matrizParaPalavra(multiplicarMatrizes(geradorDeMatrizes(preenchedor(palavra)), matrizChave(chave)));
                        Console.WriteLine("Sua palavra criptografada é |{0}|", crip);
                    }
                    else if (opcao2 == 'B' || opcao2 == 'b')
                    {
                        Console.WriteLine("Quantos caracteres você deseja que tenha a chave que criaremos para você?");
                        int tamanhoChave = Convert.ToInt32(Console.ReadLine());
                        chave = "";
                        Random aleatorio = new Random();
                        while (chave.Length < tamanhoChave)
                        {
                            int valor = aleatorio.Next(alfabeto.Length);
                            chave += alfabeto[Convert.ToInt32(valor.ToString())];
                        }

                        Console.WriteLine("A sua chave gerada é: |{0}|", chave);
                        crip = matrizParaPalavra(multiplicarMatrizes(geradorDeMatrizes(preenchedor(palavra)), matrizChave(chave)));
                        Console.WriteLine("Sua palavra criptografada com a chave que geramos é |{0}|", crip);
                    }

                }

                else if (opcao == 'B' || opcao == 'b')
                {
                    Console.WriteLine("Digite a palavra a ser decodificada: ");
                    palavra = Console.ReadLine();
                    Console.WriteLine("Digite a chave: ");
                    chave = Console.ReadLine();
                    descrip = matrizParaPalavra(multiplicarMatrizes(geradorDeMatrizes(preenchedor(palavra)), matrizInversa(matrizChave(chave))));
                    Console.WriteLine("Sua palavra decodificada é |{0}|", descrip);
                }

                do
                {
                    Console.WriteLine("Quer continuar criptografando ou decodificando palavras? (S ou N)");
                    opcaoIntermediario = Console.ReadLine();
                    resposta = Convert.ToChar(opcaoIntermediario[0]);
                } while (resposta != 's' && resposta != 'S' && resposta != 'n' && resposta != 'N');

                if (resposta == 'S' || resposta == 's')
                {
                    funcionando = true;
                }
            }

            Console.ReadKey();
        }
    }
}
