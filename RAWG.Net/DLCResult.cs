using System;

namespace RAWG.Net
{
    public class DLCResult : Result
    {
        public string Name { get; private set; }
        public int? ID { get; private set; }
        public DateTime? Released { get; private set; }

        private int count;
        private dynamic data;

        public DLCResult(int count = 0, int index = 0, params dynamic[] results)
        {
            if (count <= 0)
                return;

            this.count = count;
            data = results;

            Name = data[index].name;
            ID = data[index].id;
            Released = DateTime.TryParse(data[index].released.ToString(), out DateTime time) ? time as DateTime? : null;
        }

        internal DLCResult[] Initialize()
        {
            if (count <= 0)
                return null;

            var dlcs = new DLCResult[count];
            dlcs[0] = this;
            for (int i = 1; i < count; i++)
                dlcs[i] = new DLCResult(count, i, data);
            return dlcs;
        }

        public override string ToString()
        {
            string release = GetValue(Released);

            return $"Title: {Name}\n" +
                $"ID: {ID}\n" +
                $"Released: {release}\n\n";
        }
    }
}
