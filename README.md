Audiobook-Splitter
==================
This program allows you to extract all chapters of an audio file. The name of the final files is _chaptername_.mp3.
Up to now, this program only works under Linux. The name files will be in the root directory of this application. They are named in following manner: _originalFileName_._extension_
The audio files be in an extra _output_ directory in the program root.
Dependencies
------------
<ul>
    <li>mono</li>
    <li>ffmpeg with libmp3lame</li>
</ul>
Usage
-----
For complete extraction of the chapters as audio files.
```bash
mono SplittingM4bToMp3.exe firstFile secondFile...
```

For only extracting the names of the chapters in a file
```bash
mono SplittingM4bToMp3.exe -x firstFile secondFile...
```

License
-------
This program is released under the GPLv2.
