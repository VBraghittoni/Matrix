# Matrix: encoding reality

"Matrix" is a project that uses matrices to cryptograph messages.
It was made in the first semester of Computer Engineering to put to use our knowledge of algorithms and algebra.

## Collaborators

Vítor Braghittoni, Nicolas Birochi, Henrico Birochi, and Edgar Camacho created this project.

## How it works

### Encoding a message:
1. The message is received, converted into numbers, and transformed into several 2x2 matrices (with spaces in the end, if necessary).
2. Password is converted into a 2x2 matrix using Jacob´s theorem*.
3. Matrices of step 1 are all multiplied by the matrix of step 2, resulting in cryptographed matrices that are converted to the encoded message and returned to the user.

### Decoding a message:
1. Given an encoded message and a password, the program transforms both into matrices.
2. The password matrix is transformed into its inverse.
3. The encoded message matrices are multiplied by the INVERSE of the password, decoding the message.
4. The resulting matrices are transformed into words, and those are returned to the user.

*This process maintains the determinant of the password matrix equaling one, which is necessary for the functionality of transforming the numbers into characters and vice-versa.
