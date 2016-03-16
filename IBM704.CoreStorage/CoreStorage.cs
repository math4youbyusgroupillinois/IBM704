using System;
using System.Collections.Generic;
using System.Text;

namespace IBM704.CoreStorage
{
    public class CoreStorage
    {
        private Word[] _words;

        public CoreStorage()
        {
            _words = new Word[Settings.CORE_STORAGE_SIZE];

            for (int i = 0; i < _words.Length; i++)
            {
                _words[i] = new Word();
            }
        }

        public Word[] Words
        {
            get
            {
                return _words;
            }
        }
    }
}
