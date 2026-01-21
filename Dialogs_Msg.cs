using Microsoft.Win32;
using System;
using System.Windows;

namespace WpfApp_Decrypt_Dialogs
{
	public class DefaultDialogService
	{
		public string FilePath { get; private set; }

		public bool OpenFileDialog()
		{
			try
			{
				OpenFileDialog dialog = new OpenFileDialog
				{
					Title = "Выберите файл",
					Filter = "Encrypted files (*.enc)|*.enc",
					Multiselect = false
				};

				if (dialog.ShowDialog() == true)
				{
					FilePath = dialog.FileName;
					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					ex.Message,
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}

			return false;
		}

	}
}
