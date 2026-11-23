
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1234
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Задание 1 запуск потока 
        private void StartProgressButton_Click(object sender, RoutedEventArgs e)
        {
            StartProgressButton.IsEnabled = false;

            ProgressBar1.Value = 0;
            ProgressText.Text = "Прогресс: 0%";

            Thread thread = new Thread(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    int value = i;

                    Dispatcher.Invoke(() =>
                    {
                        ProgressBar1.Value = value;
                        ProgressText.Text = $"Прогресс: {value}%";
                    });

                    Thread.Sleep(50);
                }

                Dispatcher.Invoke(() =>
                {
                    ProgressText.Text = "Работа завершена!";
                    StartProgressButton.IsEnabled = true;
                });
            });

            thread.IsBackground = true;
            thread.Start();
        }

        // Задание 1 изменение приоритета потока
        private void StartPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            PriorityText.Text = "Поток запущен с приоритетом Normal";

            Thread thread = new Thread(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    if (i == 40)
                    {
                        Thread.CurrentThread.Priority = ThreadPriority.Highest;

                        Dispatcher.Invoke(() =>
                        {
                            PriorityText.Text =
                                "Приоритет изменён на Highest!";
                        });
                    }

                    Thread.Sleep(50);
                }

                Dispatcher.Invoke(() =>
                {
                    PriorityText.Text = "Поток завершил работу.";
                });
            });

            thread.Priority = ThreadPriority.Normal;
            thread.IsBackground = true;
            thread.Start();
        }

        // Задание 1 попытка обновить UI без Dispatcher
        private void WithoutDispatcherButton_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    ErrorText.Text = "Обновление из другого потока";
                }
                catch (InvalidOperationException ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ErrorText.Text =
                            "Ошибка: " + ex.Message;
                    });
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        // Задание 2 метод подсчёта от 1 до 100
        private void CountNumbers(TextBlock textBlock)
        {
            for (int i = 1; i <= 100; i++)
            {
                int number = i;

                Dispatcher.Invoke(() =>
                {
                    textBlock.Text = $"Значение: {number}";
                });

                Thread.Sleep(100);
            }
        }

        // Задание 2 запуск трёх потоков
        private void StartThreeThreadsButton_Click(object sender, RoutedEventArgs e)
        {
            ThreadsResultText.Text = "Потоки работают...";

            LowestText.Text = "Значение: 0";
            NormalText.Text = "Значение: 0";
            HighestText.Text = "Значение: 0";

            Thread lowestThread = new Thread(() =>
            {
                CountNumbers(LowestText);
            });

            Thread normalThread = new Thread(() =>
            {
                CountNumbers(NormalText);
            });

            Thread highestThread = new Thread(() =>
            {
                CountNumbers(HighestText);
            });

            lowestThread.Priority = ThreadPriority.Lowest;
            normalThread.Priority = ThreadPriority.Normal;
            highestThread.Priority = ThreadPriority.Highest;

            lowestThread.IsBackground = true;
            normalThread.IsBackground = true;
            highestThread.IsBackground = true;

            Thread waitThread = new Thread(() =>
            {
                lowestThread.Start();
                normalThread.Start();
                highestThread.Start();

                lowestThread.Join();
                normalThread.Join();
                highestThread.Join();

                Dispatcher.Invoke(() =>
                {
                    ThreadsResultText.Text =
                        "Все три потока завершили работу!";
                });
            });

            waitThread.IsBackground = true;
            waitThread.Start();
        }
    }
}
