# Console2HTML

## Overview

Console2HTML is a simple utility that converts console output with colored text into an HTML file, preserving the color formatting. This tool is particularly useful when you need to share console output that includes color-coded text.

## Requirements

-   .NET Framework 4.8
- 
## Features

-   Preserves console text colors
    
-   Monospace font for consistent display
    
-   Black background to mimic console appearance
    

## Usage
```
Console2HTML.exe --target OutputFileName

```

Example:

```
Console2HTML.exe --target .\test.html

```



    

## Output Example

```
<html>
<head>
<style>
body { background-color: black; }
.defaultColor { color: #808080; font-family: monospace; }
.colorDarkGreen { color: #006400; font-family: monospace; }
.colorDarkRed { color: #8B0000; font-family: monospace; }
</style>
</head>
<body>
<pre>
<span class='defaultColor'>C:\WorkingSet\sources\Console2HTML\bin\Release&gt;git</span> <span class='defaultColor'>branch</span> <span class='defaultColor'>-a</span>
<span class='defaultColor'>*</span> <span class='colorDarkGreen'>master</span>
<span class='defaultColor'>master2</span>
<span class='colorDarkRed'>remotes/origin/main</span>
<span class='colorDarkRed'>remotes/origin/master</span>

<span class='defaultColor'>C:\WorkingSet\sources\Console2HTML\bin\Release&gt;Console2HTML.exe</span> <span class='defaultColor'>--target</span> <span class='defaultColor'>.\test.html</span>
</pre>
</body>
</html>

```

## Screenshot

![Console2HTML Output](https://private-user-images.githubusercontent.com/7032867/422719038-9116eef6-7e57-46ef-93ea-14bfe0abc818.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTUiLCJleHAiOjE3NDE5NDU1MjEsIm5iZiI6MTc0MTk0NTIyMSwicGF0aCI6Ii83MDMyODY3LzQyMjcxOTAzOC05MTE2ZWVmNi03ZTU3LTQ2ZWYtOTNlYS0xNGJmZTBhYmM4MTgucG5nP1gtQW16LUFsZ29yaXRobT1BV1M0LUhNQUMtU0hBMjU2JlgtQW16LUNyZWRlbnRpYWw9QUtJQVZDT0RZTFNBNTNQUUs0WkElMkYyMDI1MDMxNCUyRnVzLWVhc3QtMSUyRnMzJTJGYXdzNF9yZXF1ZXN0JlgtQW16LURhdGU9MjAyNTAzMTRUMDk0MDIxWiZYLUFtei1FeHBpcmVzPTMwMCZYLUFtei1TaWduYXR1cmU9N2Y0OTc0Mzk3NzBlZDg3ZjQ3OWZlYTkwMjhjMGVhMjI5Zjg2YjQzNTI3NGZlNjY5OTViZDc3NjczYjA0Yzc5YiZYLUFtei1TaWduZWRIZWFkZXJzPWhvc3QifQ.hSS2lJ6KRVnaA0SjHVFGoSLTITFeZ2lsE0ECfs8k_QE)

## Contribution

Contributions are welcome! Feel free to open issues or submit pull requests.

## License

MIT License: Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction.....

## Credits

-   Developed with assistance from YandexGPT 5 Pro and **Qwen**: https://github.com/alibabacloud/qwen