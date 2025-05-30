using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography_Work
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Array with characters. The program is designed to allow adding characters to this array without needing to fix the rest of the code.
            char[] alphabet = { '!', ' ', '@', '#', '$', '%', '&', '*', '(', ')', '<', '>', '.', ',', ';', ':', '/', '?', '[', ']', '{', '}', '=', '+', '-', '_', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'á', 'Á', 'à', 'À', 'ã', 'Ã', 'ñ', 'Ñ', 'é', 'É', 'ê', 'Ê', 'í', 'Í', 'ó', 'Ó' };

            // Functions:
            // Given a character and an array, this function finds the position at which this character first appears in the array.
            float PositionOfCharacterInArray(char character, char[] array)
            {
                for (int position = 0; position < array.Length; position++)
                {
                    if (character == array[position])
                    {
                        return position;
                    }
                }
                return 666;
            }

            // Find the size of the text that will be used to create the matrix. Note that this size will necessarily be a multiple of four, as the text to be encrypted will be divided into several 2 x 2 matrices.
            int MatrixSize(string text)
            {
                int i = text.Length;

                while (true)
                {
                    if (i % 4 == 0)
                    {
                        return i;
                    }
                    i++;
                }
            }

            // This function adds ' ' to the text until its length is equal to the size (a multiple of four) found in the previous function.
            string Filler(string text)
            {
                int k = MatrixSize(text);
                while (true)
                {
                    if (text.Length == k)
                    {
                        return text;
                    }
                    text += " ";
                }
            }

            // Transforms the numeric matrix into a word. Converts the number "n" into the character that occupies the index n % alphabet.Length in the alphabet.
            string MatrixToWord(float[,,] matrix)
            {
                string text = "";
                for (int matrixIndex = 0; matrixIndex < matrix.GetLength(0); matrixIndex++)
                {
                    for (int row = 0; row < matrix.GetLength(1); row++)
                    {
                        for (int column = 0; column < matrix.GetLength(2); column++)
                        {
                            if (matrix[matrixIndex, row, column] >= 0)
                            {
                                text += alphabet[Convert.ToInt64((matrix[matrixIndex, row, column]) % alphabet.Length)];
                            }
                            else
                            {
                                double negativity = Math.Ceiling((-matrix[matrixIndex, row, column]) / alphabet.Length);
                                text += alphabet[Convert.ToInt64((matrix[matrixIndex, row, column] + negativity * alphabet.Length) % alphabet.Length)];
                            }
                        }
                    }
                }
                return text;
            }

            // Multiplies each matrix in the matrix set by a specific matrix (the one used as a key).
            float[,,] MultiplyMatrices(float[,,] matrix1, float[,] matrix2)
            {
                float[,,] product = new float[matrix1.GetLength(0), matrix1.GetLength(1), matrix1.GetLength(2)];
                for (int matrixIndex = 0; matrixIndex < matrix1.GetLength(0); matrixIndex++)
                {
                    for (int row = 0; row < matrix1.GetLength(1); row++)
                    {
                        for (int column = 0; column < matrix1.GetLength(2); column++)
                        {
                            for (int sumNumber = 0; sumNumber < matrix1.GetLength(2); sumNumber++)
                            {
                                product[matrixIndex, row, column] += matrix1[matrixIndex, row, sumNumber] * matrix2[sumNumber, column];
                            }
                        }
                    }
                }
                return product;
            }

            // Transforms the given word into as many 2x2 matrices as needed
            float[,,] MatrixGenerator(string text)
            {
                float[,,] generatedMatrix = new float[Filler(text).Length / 4, 2, 2];
                for (int matrixCount = 0; matrixCount < (Filler(text).Length) / 4; matrixCount++)
                {
                    for (int row = 0; row < 2; row++)
                    {
                        for (int column = 0; column < 2; column++)
                        {
                            generatedMatrix[matrixCount, row, column] = PositionOfCharacterInArray(text[matrixCount * 4 + 2 * row + column], alphabet);
                        }
                    }
                }
                return generatedMatrix;
            }

            // We want a resulting matrix whose determinant is 1. Knowing this, we use Jacobi's Theorem, which says that we can add/subtract the numbers in one row to another as many times as we want without changing the determinant. Thus, starting from a matrix with a determinant of 1, we add "n" times the first row to the second row to obtain the key matrix. To define "n," we add the indices of each of the characters.
            float[,] KeyMatrix(string text)
            {
                float[,] generatedMatrix = new float[2, 2];
                long multiplier = 1;
                for (int pos = 0; pos < text.Length; pos++)
                {
                    multiplier += Convert.ToInt64(PositionOfCharacterInArray(text[pos], alphabet));
                }
                generatedMatrix[0, 0] = 4;
                generatedMatrix[0, 1] = 11;
                generatedMatrix[1, 0] = 1 + multiplier * generatedMatrix[0, 0];
                generatedMatrix[1, 1] = 3 + multiplier * generatedMatrix[0, 1];

                return generatedMatrix;
            }

            // Performs the standard process of finding the inverse matrix of a 2x2 matrix
            float[,] InverseMatrix(float[,] matrix)
            {
                float determinant = Convert.ToInt32(matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]);
                if (determinant == 0)
                {
                    Console.WriteLine("PROBLEM!! Determinant equals 0!!");
                }
                float[,] inverse = new float[matrix.GetLength(0), matrix.GetLength(1)];
                inverse[0, 0] = matrix[1, 1] / determinant;
                inverse[0, 1] = -matrix[0, 1] / determinant;
                inverse[1, 0] = -matrix[1, 0] / determinant;
                inverse[1, 1] = matrix[0, 0] / determinant;

                return inverse;
            }

            // Creating variables and interacting with the user:
            string word, key, encryptedWord, decryptedWord, intermediateOption;
            char option, option2, response;
            bool running = true;
            Console.WriteLine("WARNING: Words encrypted and passwords generated by us will be delimited in the following format: |word|");
            while (running)
            {
                running = false;
                do
                {
                    Console.WriteLine("Enter 'A' or 'a' to encrypt or enter 'B' or 'b' to decode a message: ");
                    intermediateOption = Console.ReadLine();
                    option = Convert.ToChar(intermediateOption[0]);
                } while (option != 'a' && option != 'A' && option != 'b' && option != 'B');

                if (option == 'A' || option == 'a')
                {
                    Console.WriteLine("Write the word to be encrypted: ");
                    word = Console.ReadLine();

                    do
                    {
                        Console.WriteLine("Enter 'A' to input a key or enter 'B' to have us create one for you: ");
                        intermediateOption = Console.ReadLine();
                        option2 = Convert.ToChar(intermediateOption[0]);
                    } while (option2 != 'a' && option2 != 'A' && option2 != 'b' && option2 != 'B');

                    if (option2 == 'A' || option2 == 'a')
                    {
                        Console.WriteLine("Enter a key: ");
                        key = Console.ReadLine();
                        encryptedWord = MatrixToWord(MultiplyMatrices(MatrixGenerator(Filler(word)), KeyMatrix(key)));
                        Console.WriteLine("Your encrypted word is |{0}|", encryptedWord);
                    }
                    else if (option2 == 'B' || option2 == 'b')
                    {
                        Console.WriteLine("How many characters do you want the generated key to have?");
                        int keySize = Convert.ToInt32(Console.ReadLine());
                        key = "";
                        Random random = new Random();
                        while (key.Length < keySize)
                        {
                            int value = random.Next(alphabet.Length);
                            key += alphabet[Convert.ToInt32(value.ToString())];
                        }

                        Console.WriteLine("Your generated key is: |{0}|", key);
                        encryptedWord = MatrixToWord(MultiplyMatrices(MatrixGenerator(Filler(word)), KeyMatrix(key)));
                        Console.WriteLine("Your word encrypted with the generated key is |{0}|", encryptedWord);
                    }

                }

                else if (option == 'B' || option == 'b')
                {
                    Console.WriteLine("Enter the word to be decoded: ");
                    word = Console.ReadLine();
                    Console.WriteLine("Enter the key: ");
                    key = Console.ReadLine();
                    decryptedWord = MatrixToWord(MultiplyMatrices(MatrixGenerator(Filler(word)), InverseMatrix(KeyMatrix(key))));
                    Console.WriteLine("Your decoded word is |{0}|", decryptedWord);
                }

                do
                {
                    Console.WriteLine("Do you want to continue encrypting or decrypting words? (Y or N)");
                    intermediateOption = Console.ReadLine();
                    response = Convert.ToChar(intermediateOption[0]);
                } while (response != 'y' && response != 'Y' && response != 'n' && response != 'N');

                if (response == 'Y' || response == 'y')
                {
                    running = true;
                }
            }

            Console.ReadKey();
        }
    }
}
