using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Addons;
using BufferWorkers;

namespace Task1
{
    class Program
    {
        static List<Reader> readerList = new List<Reader>();
        static List<Writer> writerList = new List<Writer>();

        static void Main(string[] args)
        {
            var writersCount = getCountWriters();
            var readersCount = getCountReaders();
            var messagesCount = getCountMessages();

            initializeWriters(writersCount, messagesCount);
            initializeReaders(readersCount);
        }

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
            var allWriterFinished = writerList.Any(s => s.State != StateWorker.Finish);
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
