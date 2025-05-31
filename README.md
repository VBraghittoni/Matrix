# Matrix: Encoding Reality
![image](https://github.com/user-attachments/assets/a1699701-be25-4d80-8478-2135d6304123)

**Matrix** is a project that uses matrices to encrypt and decrypt messages.  
It was developed during the first semester of Computer Engineering to apply our knowledge of algorithms and linear algebra.

---

## 👥 Collaborators

- [Vítor Braghittoni](https://github.com/VBraghittoni)  
- [Nicolas Birochi](https://github.com/nicholasbirochi)  
- [Henrico Birochi](https://github.com/henricobirochi)  
- [Edgar Camacho](https://github.com/Edgarcsr)

---

## 🚀 How to Run

1. Navigate to the `Executable files` folder.
2. Select your preferred language.
3. Download the `.exe` file.
4. Run the executable.

---

## 🛠️ How It Works

### 🔐 Encoding a Message

1. The message is received, converted into numbers, and split into multiple 2x2 matrices (adding spaces at the end if necessary).
2. The password is converted into a 2x2 matrix using **Jacob’s theorem** (*see note below*).
3. Each message matrix is multiplied by the password matrix.
4. The resulting cryptographed matrices are converted back into the encoded message and returned to the user.

---

### 🔓 Decoding a Message

1. Given an encoded message and a password, the program transforms both into matrices.
2. The password matrix is converted into its **inverse**.
3. The encoded message matrices are multiplied by the **inverse** of the password matrix, decoding the message.
4. The resulting matrices are transformed back into readable text and returned to the user.

---

### ℹ️ Note

> This process ensures that the determinant of the password matrix equals **1**,  
> which is essential for correctly converting between numbers and characters.

---

## 🧮 Key Concept

The project leverages linear algebra concepts, particularly matrix multiplication and inversion, to perform encryption and decryption securely and efficiently.

