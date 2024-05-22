using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szyfrowanie4
{
    public partial class Form1 : Form
    {
        private string filePath;
        private RSAParameters publicKey;
        private RSAParameters privateKey;
        public Form1()
        {
            InitializeComponent();
            GenerateKeys();
        }
        private void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    txtFilePath.Text = filePath;
                }
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please select a file first.");
                return;
            }

            try
            {
                byte[] dataToEncrypt = File.ReadAllBytes(filePath);
                byte[] encryptedData = EncryptData(dataToEncrypt, publicKey);
                File.WriteAllBytes(filePath + ".encrypted.txt", encryptedData);
                txtOutput.Text = "File encrypted successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encryption failed: " + ex.Message);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please select a file first.");
                return;
            }

            try
            {
                byte[] dataToDecrypt = File.ReadAllBytes(filePath + ".encrypted.txt");
                byte[] decryptedData = DecryptData(dataToDecrypt, privateKey);
                File.WriteAllBytes(filePath + ".decrypted.txt", decryptedData);
                txtOutput.Text = "File decrypted successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Decryption failed: " + ex.Message);
            }
        }
        private byte[] EncryptData(byte[] data, RSAParameters rsaKeyInfo)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(rsaKeyInfo);
                return rsa.Encrypt(data, false);
            }
        }

        private byte[] DecryptData(byte[] data, RSAParameters rsaKeyInfo)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(rsaKeyInfo);
                return rsa.Decrypt(data, false);
            }
        }
    }
}
