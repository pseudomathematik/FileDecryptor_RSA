using RSA_Decryptor;
using System;
using System.Numerics;
using System.Text;
using System.Windows;
using WpfApp_Decrypt_Dialogs;



namespace WpfApp_Decrypt_RSA
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		private readonly DefaultDialogService _dialogService;
		public MainWindow()
		{

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			InitializeComponent();
			_dialogService = new DefaultDialogService();
		}
		private void AddLog(string message)
		{
			Dispatcher.Invoke(() =>
			{
				textBox_DecryptOutput.Text += message;
			});
		}
		private void button_SelectFileForDecrypt_Click(object sender, RoutedEventArgs e)
		{
			if (_dialogService.OpenFileDialog())
			{
				textBox_DecryptFileName.Text = _dialogService.FilePath;
			}
		}
		private void button_DecryptFile_Click(object sender, RoutedEventArgs e)
		{
			textBox_DecryptOutput.Clear();
			try
			{
				string encFile = textBox_DecryptFileName.Text;
				if(String.IsNullOrEmpty(textBox_DecryptFileName.Text))
				{
					MessageBox.Show("Файл для расшифровки не выбран!", "Предупреждение", MessageBoxButton.OK);
					return;
				}
				if(string.IsNullOrEmpty(textBox_PrivateKey_d.Text) || string.IsNullOrEmpty(textBox_PrivateKey_n.Text))
				{
					MessageBox.Show("Закрытый ключ не введен,или введен не полностью!", "Предупреждение", MessageBoxButton.OK);
					return;
				}
				BigInteger d = BigInteger.Parse(textBox_PrivateKey_d.Text);
				BigInteger n = BigInteger.Parse(textBox_PrivateKey_n.Text);


				FileDecryptor decryptor = new FileDecryptor(d, n,AddLog);
				decryptor.DecryptFile(encFile);

				
			}
			catch (Exception ex)
			{
				textBox_DecryptOutput.Text += ($"Ошибка: {ex.Message}\n");
			}
		}
	}
}

