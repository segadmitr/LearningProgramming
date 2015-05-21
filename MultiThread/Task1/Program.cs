using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Addons;
using BufferWorkers;

namespace Task1
{
    class Program
    {
        static List<Reader> readerList = new List<Reader>();
        static List<Writer> writerList = new List<Writer>();

        static int writersCount;
        static int readersCount;
        static int messagesCount;

        static void Main(string[] args)
        {
            writersCount = getCountWriters();
            readersCount = getCountReaders();
            messagesCount = getCountMessages();

            initializeWriters(writersCount, messagesCount);
            initializeReaders(readersCount);


            var taskList = new List<Task>();

            readerList.ForEach(s =>
                {
                    var readTask = Task.Factory.StartNew(s.Read);
                    taskList.Add(readTask);
                });
            writerList.ForEach(s =>
                {
                    var writeTask = Task.Factory.StartNew(s.Write);
                    taskList.Add(writeTask);
                });

            Task.WaitAll(taskList.ToArray());
            
            validate();
            
            Console.WriteLine("Для завершения нажмите на любую клавишу...");
        }

        #region Validating

        static void validate()
        {
            var messagesList = new List<string>();
            var expectedCountMessages = writersCount*messagesCount;

            var messageCounter = 0;
            foreach (var reader in readerList)
            {
                foreach (var readerMessage in reader.Messages)
                {
                    if (messagesList.Contains(readerMessage))
                    {
                        Console.WriteLine(string.Format("Дублирование сообения {0}", readerMessage));
                    }
                    else
                    {
                        messagesList.Add(readerMessage);
                    }
                    messageCounter++;
                }
            }
            if (expectedCountMessages != messageCounter)
                Console.WriteLine("Не совпадает количество ожидаемых и фактических сообщений");
        }

        #endregion

        #region initialize

        static void initializeReaders(int readersCount)
        {
            for (var i = 0; i < readersCount; i++)
            {
                readerList.Add(new Reader());
            }
        }

        static void initializeWriters(int writersCount, int messagesCount)
        {
            for (var i = 0; i < writersCount; i++)
            {
                var writer = new Writer(messagesCount, string.Format("Писатель {0}", i));
                writer.Finished+=writer_Finished;
                writerList.Add(writer);
            }
        }

        static void writer_Finished(object sender, EventArgs e)
        {
            var allWriterFinished = writerList.All(s => s.State == StateWorker.Finish);
            BufferWorkers.Buffer.IsClosed = allWriterFinished;
        }

        #endregion

        #region getUIParams

        /// <summary>
        /// Получает количество писателей
        /// </summary>
        /// <returns></returns>
        private static int getCountWriters()
        {
            try
            {
                return ConsoleAdons.GetIntFromConsole("Введитите количество писателей");
            }
            catch (InvalidOperationException)
            {
                return getCountWriters();
            }
        }

        /// <summary>
        /// Получает количество читателей
        /// </summary>
        /// <returns></returns>
        private static int getCountReaders()
        {
            try
            {
                return ConsoleAdons.GetIntFromConsole("Введитите количество читателей");
            }
            catch (InvalidOperationException)
            {
                return getCountReaders();
            }
        }

        /// <summary>
        /// Количество генерируемых сообщений
        /// </summary>
        /// <returns></returns>
        private static int getCountMessages()
        {
            try
            {
                return ConsoleAdons.GetIntFromConsole("Сколько сообщений должны генерировать писатели");
            }
            catch (InvalidOperationException)
            {
                return getCountMessages();
            }
        }

        #endregion
    }
}
