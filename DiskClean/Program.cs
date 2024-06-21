using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Para continuarmos, recomenda-se que faça uma breve verificação de disco, você deseja? (S/N)");
            var option = Console.ReadLine().ToUpper();

            if (option == "S")
            {
                string cmdhd = "sfc/scannow";

                Process processoVerificacao = ExecutarCMD(cmdhd);

                if (processoVerificacao != null)
                {
                    processoVerificacao.WaitForExit();
                }
            }
            else
            {
                Console.WriteLine("Vamos começar a liberação de espaço!");
            }

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady)
                {
                    Console.WriteLine($"Drive {d.Name} está pronto. Tipo: {d.DriveType}, Formato: {d.DriveFormat}, Espaço livre: {d.TotalFreeSpace}");

                    string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string tempPath = Path.GetTempPath();

                    string FolderPath1 = Path.Combine(d.Name, @"Windows\Temp");
                    string FolderPath2 = tempPath;
                    string FolderPath3 = Path.Combine(d.Name, @"Windows\Prefetch");
                    string FolderPath4 = Path.Combine(d.Name, @"Windows\SoftwareDistribution\Download");
                    string FolderPath5 = Path.Combine(localAppDataPath, @"Microsoft\Windows\Explorer");
                    string FolderPath6 = Path.Combine(d.Name, @"ProgramData\Microsoft\Windows\WER\ReportQueue");
                    string FolderPath7 = Path.Combine(localAppDataPath, @"Google\Chrome\User Data\Default\Cache");

                    try
                    {
                        ExcluirArquivosEDiretorios(FolderPath1);
                        ExcluirArquivosEDiretorios(FolderPath2);
                        ExcluirArquivosEDiretorios(FolderPath3);
                        ExcluirArquivosEDiretorios(FolderPath4);
                        ExcluirArquivosEDiretorios(FolderPath5);
                        ExcluirArquivosEDiretorios(FolderPath6);

                        FecharProcesso("chrome");
                        ExcluirArquivosEDiretorios(FolderPath7);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ocorreu um erro inesperado ao limpar o drive {d.Name}: {ex.Message}");
                    }
                }
            }

            Console.WriteLine("Digite enter para fechar...");
            Console.ReadLine();
        

    static Process ExecutarCMD(string comando)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + comando,
                    Verb = "runas"
                };
                return Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao executar o comando no CMD: {ex.Message}");
                return null;
            }
        }

        static void ExcluirArquivosEDiretorios(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                        Console.WriteLine($"Arquivo excluído: {file}");
                    }
                    catch (IOException ex) when (
                        ex.Message.Contains("O processo não pode acessar o arquivo")
                        || ex.Message.Contains("está sendo usado por outro processo")
                    )
                    {
                        Console.WriteLine($"Ignorando erro ao excluir arquivo '{file}': {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao excluir o arquivo '{file}': {ex.Message}");
                    }
                }

                string[] directories = Directory.GetDirectories(folderPath);
                foreach (string directory in directories)
                {
                    try
                    {
                        Directory.Delete(directory, true);
                        Console.WriteLine($"Diretório excluído: {directory}");
                    }
                    catch (IOException ex) when (
                        ex.Message.Contains("O processo não pode acessar o arquivo")
                        || ex.Message.Contains("está sendo usado por outro processo")
                    )
                    {
                        Console.WriteLine($"Ignorando erro ao excluir diretório '{directory}': {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao excluir o diretório '{directory}': {ex.Message}");
                    }
                }
            }
        }

        static void FecharProcesso(string nomeProcesso)
        {
            var processos = Process.GetProcessesByName(nomeProcesso);
            foreach (var processo in processos)
            {
                try
                {
                    processo.Kill();
                    processo.WaitForExit();
                    Console.WriteLine($"Processo {nomeProcesso} encerrado.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao tentar encerrar o processo {nomeProcesso}: {ex.Message}");
                }
            }
        }
    
}
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        } 
            }
}


        
