MIT License

Copyright (c) 2018 Leopotam <leopotam@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

# GoogleDocs Data Downloader
[Downloader of google docs sheets in csv format with optional converting it to json](https://github.com/Leopotam/googledocs-import).

## How to use
* Open `Window` / `Leopotam` / `GoogleDocs Downloader` window.
* Add required urls (direct link from `SHARE` wizard of GoogleDocs) and paths to files at current project. Paths should be relative to `Assets` folder.
* Press `Download data` button.
* Enjoy.

## GoogleDocs sheet headers for JSON translation
* First line - always will be recognized as JSON field names.
* Second line - JSON data wrappers. For example, strings should be wrapped with "", arrays should be wrapped with [] and so on.
    > Any column with `IGNORE` value in second line will be skipped. Can be used for comments.
* First column can be recognized as keys in JSON mode "To dictionary" (will be excluded from data).

> Tested on unity 2018.2 (dependent on it) and contains assembly definition for compiling to separate assembly file for performance reason.

# License
The software released under the terms of the [MIT license](./LICENSE). Enjoy.

# Donate
Its free opensource software, but you can buy me a coffee:

<a href="https://www.buymeacoffee.com/leopotam" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>