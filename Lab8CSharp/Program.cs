using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab8Sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n=================================");
                Console.WriteLine("Лабораторна робота №8. Варіант 6");
                Console.WriteLine("1 - Завдання 1.6 (Пошук web-сайтів edu.ua)");
                Console.WriteLine("2 - Завдання 2.6 (Видалення українських слів на голосну)");
                Console.WriteLine("3 - Завдання 3.6 (Вилучення попередніх входжень останньої літери)");
                Console.WriteLine("4 - Завдання 4.6 (Двійкові файли - інтервал чисел)");
                Console.WriteLine("5 - Завдання 5 (Робота з файловою системою d:\\temp)");
                Console.WriteLine("0 - Вихід");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                switch (choice)
                {
                    case "1": Task1(); break;
                    case "2": Task2(); break;
                    case "3": Task3(); break;
                    case "4": Task4(); break;
                    case "5": Task5(); break;
                    default: Console.WriteLine("Неправильний вибір."); break;
                }
            }
        }

        // =======================================================
        // ЗАВДАННЯ 1.6: Пошук та заміна адрес web-сайтів edu.ua
        // =======================================================
        static void Task1()
        {
            Console.WriteLine("\n--- Завдання 1.6 ---");
            string inputFile = "task1_in.txt";
            string outputFile = "task1_out.txt";

            // 1. Створюємо тестовий файл
            File.WriteAllText(inputFile, "Університет має сайт http://chnu.edu.ua та портал dist.chnu.edu.ua. Також є сайт google.com.");

            string text = File.ReadAllText(inputFile);
            Console.WriteLine($"\nВихідний текст:\n{text}");

            // Регулярний вираз для сайтів *.edu.ua
            string pattern = @"\b(?:https?://)?(?:www\.)?[a-zA-Z0-9.-]+\.edu\.ua\b";
            MatchCollection matches = Regex.Matches(text, pattern);

            Console.WriteLine($"\nЗнайдено сайтів домену edu.ua: {matches.Count}");
            foreach (Match m in matches)
            {
                Console.WriteLine($"- {m.Value}");
            }

            Console.Write("\nВведіть текст для заміни цих сайтів (наприклад, [ВИЛУЧЕНО]): ");
            string replacement = Console.ReadLine();

            string newText = Regex.Replace(text, pattern, replacement);
            File.WriteAllText(outputFile, newText);

            Console.WriteLine($"\nРезультат записано у файл {outputFile}:");
            Console.WriteLine(newText);
        }

        // =======================================================
        // ЗАВДАННЯ 2.6: Видалення укр. слів на голосну літеру
        // =======================================================
        static void Task2()
        {
            Console.WriteLine("\n--- Завдання 2.6 ---");
            string inputFile = "task2_in.txt";
            string outputFile = "task2_out.txt";

            // Створюємо тестовий файл
            File.WriteAllText(inputFile, "Абрикос дуже смачний. Осінь прийшла. Собака гавкає. Іван навчається. Єнот біжить.");

            string text = File.ReadAllText(inputFile);
            Console.WriteLine($"\nВихідний текст:\n{text}");

            // Регулярний вираз: шукає межу слова \b, потім голосну укр. літеру, потім будь-які букви кирилиці
            string pattern = @"\b[АЕЄИІЇОУЮЯаеєиіїоуюя][а-яА-ЯіІїЇєЄґҐ]*\b";

            // Замінюємо знайдені слова на порожній рядок
            string newText = Regex.Replace(text, pattern, "");

            // Прибираємо зайві пробіли, що могли залишитися після видалення
            newText = Regex.Replace(newText, @"\s+", " ").Trim();

            File.WriteAllText(outputFile, newText);
            Console.WriteLine($"\nРезультат запису у файл {outputFile} (видалено слова на голосну):");
            Console.WriteLine(newText);
        }

        // =======================================================
        // ЗАВДАННЯ 3.6: Вилучення попередніх входжень останньої літери
        // =======================================================
        static void Task3()
        {
            Console.WriteLine("\n--- Завдання 3.6 ---");
            string inputFile = "task3_in.txt";
            string outputFile = "task3_out.txt";

            // Створюємо тестовий файл
            File.WriteAllText(inputFile, "Слово 'радар' зміниться. Слово 'мама' також. Авіакомпанія працює.");

            string text = File.ReadAllText(inputFile);
            Console.WriteLine($"\nВихідний текст:\n{text}");

            // Використовуємо MatchEvaluator для обробки кожного слова окремо
            string newText = Regex.Replace(text, @"\b[A-Za-zА-Яа-яІіЇїЄєҐґ]+\b", match =>
            {
                string word = match.Value;
                if (word.Length <= 1) return word;

                char lastChar = word[word.Length - 1]; // Остання літера
                string prefix = word.Substring(0, word.Length - 1); // Слово без останньої літери

                // Вилучаємо з префікса всі входження останньої літери (ігноруючи регістр)
                prefix = Regex.Replace(prefix, lastChar.ToString(), "", RegexOptions.IgnoreCase);

                return prefix + lastChar; // Повертаємо склеєне слово
            });

            File.WriteAllText(outputFile, newText);
            Console.WriteLine($"\nРезультат запису у файл {outputFile}:");
            Console.WriteLine(newText);
        }

        // =======================================================
        // ЗАВДАННЯ 4.6: Двійкові файли (Інтервал)
        // =======================================================
        static void Task4()
        {
            Console.WriteLine("\n--- Завдання 4.6 (Двійкові файли) ---");
            string binFileAll = "numbers_all.dat";
            string binFileFiltered = "numbers_filtered.dat";

            Console.Write("Введіть кількість випадкових чисел (n): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0) return;

            Console.Write("Введіть початок інтервалу (a): ");
            int a = int.Parse(Console.ReadLine());

            Console.Write("Введіть кінець інтервалу (b): ");
            int b = int.Parse(Console.ReadLine());

            Random rnd = new Random();

            // 1. Записуємо всі числа у двійковий файл
            using (BinaryWriter bw = new BinaryWriter(File.Open(binFileAll, FileMode.Create)))
            {
                Console.Write("Згенеровані числа: ");
                for (int i = 0; i < n; i++)
                {
                    int num = rnd.Next(-50, 50);
                    Console.Write(num + " ");
                    bw.Write(num);
                }
                Console.WriteLine();
            }

            // 2. Читаємо, фільтруємо і записуємо в новий двійковий файл
            using (BinaryReader br = new BinaryReader(File.Open(binFileAll, FileMode.Open)))
            using (BinaryWriter bw = new BinaryWriter(File.Open(binFileFiltered, FileMode.Create)))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    int num = br.ReadInt32();
                    if (num >= a && num <= b)
                    {
                        bw.Write(num);
                    }
                }
            }

            // 3. Виводимо результат на екран
            Console.Write($"Числа, що потрапили в інтервал [{a}, {b}]: ");
            using (BinaryReader br = new BinaryReader(File.Open(binFileFiltered, FileMode.Open)))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    Console.Write(br.ReadInt32() + " ");
                }
            }
            Console.WriteLine("\nДані успішно збережено у двійковий файл.");
        }

        // =======================================================
        // ЗАВДАННЯ 5: Робота з файловою системою
        // =======================================================
        static void Task5()
        {
            Console.WriteLine("\n--- Завдання 5 (Файлова система) ---");

            // Перевіряємо чи є диск D. Якщо ні - створюємо у локальній папці проєкту (щоб не було помилок)
            string rootPath = @"d:\temp";
            if (!Directory.Exists(@"d:\"))
            {
                rootPath = Path.Combine(Environment.CurrentDirectory, "temp");
                Console.WriteLine($"Увага! Диск D:\\ відсутній, використовуємо шлях: {rootPath}");
            }

            try
            {
                string studentName = "Student"; // Замість <прізвище_студента>
                string dir1 = Path.Combine(rootPath, $"{studentName}1");
                string dir2 = Path.Combine(rootPath, $"{studentName}2");
                string dirAll = Path.Combine(rootPath, "ALL");

                // Очищення перед початком (якщо папки вже існують від попереднього запуску)
                if (Directory.Exists(dir1)) Directory.Delete(dir1, true);
                if (Directory.Exists(dir2)) Directory.Delete(dir2, true);
                if (Directory.Exists(dirAll)) Directory.Delete(dirAll, true);
                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);

                // 1. Створення папок
                Directory.CreateDirectory(dir1);
                Directory.CreateDirectory(dir2);

                // 2. Створення файлів t1.txt та t2.txt
                string t1 = Path.Combine(dir1, "t1.txt");
                string t2 = Path.Combine(dir1, "t2.txt");

                File.WriteAllText(t1, "Шевченко Степан Іванович, 2001 року народження, місце проживання м. Суми");
                File.WriteAllText(t2, "Комар Сергій Федорович, 2000 року народження, місце проживання м. Київ");

                // 3. Створення t3.txt у другій папці
                string t3 = Path.Combine(dir2, "t3.txt");
                File.WriteAllText(t3, File.ReadAllText(t1) + Environment.NewLine + File.ReadAllText(t2));

                // 4. Виведення інформації про створені файли
                Console.WriteLine("\nІнформація про створені файли до переміщення:");
                PrintDirectoryInfo(rootPath);

                // 5. Переміщення t2.txt та копіювання t1.txt
                File.Move(t2, Path.Combine(dir2, "t2.txt"));
                File.Copy(t1, Path.Combine(dir2, "t1.txt"));

                // 6. Перейменування dir2 на ALL та видалення dir1
                Directory.Move(dir2, dirAll);
                Directory.Delete(dir1, true);

                // 7. Виведення фінальної інформації
                Console.WriteLine("\nФайлові операції завершено! Фінальний вміст папки ALL:");
                PrintDirectoryInfo(dirAll);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при роботі з файлами: {ex.Message}");
            }
        }

        // Допоміжний метод для Завдання 5
        static void PrintDirectoryInfo(string path)
        {
            if (!Directory.Exists(path)) return;

            foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                FileInfo info = new FileInfo(file);
                Console.WriteLine($"Файл: {info.Name} | Каталог: {info.Directory.Name} | Розмір: {info.Length} байт | Створено: {info.CreationTime}");
            }
        }
    }
}