using System.Collections.Generic;

namespace MaxSumProject {
    public class FileData {

        public string FilePath { get; set; }
        public List<int> DefectedRows { get; private set; } = new List<int>();
        public List<int> RowsWithMaxSum { get; set; } = new List<int>();
    }

}
