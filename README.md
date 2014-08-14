dbf2csv
=======

Converts FoxPro data files to csv format.

** You will need the Microsoft FoxPro OLEDB driver installed: http://www.microsoft.com/en-us/download/details.aspx?id=14839

** target platform in build must be X86 (it will still run on 64 bit hardware, though)

** drop your .dbf and .fpt files into the same directory as the executable and kick it off. It will process all of them and save them as .csv files.

** this code is ugly and to the point. It does what I need it to do. If you would like to change anything (arguments for specifying a directory and/or a single file name maybe?), please do so and issue a pull request.
