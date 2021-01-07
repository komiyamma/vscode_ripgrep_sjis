# Visual Studio Code 用 SJISも検索するGrep
vscode で grep すると、utf8とsjisが混じったファイル群だと、sjis が検知できない。
そこで検知できるようにしたもの。

# 動作環境
Windows系。Win7以降くらいじゃないかな。多分。

# 使い方
rg_sjis.zip を ダウンロードして、解凍し、rg_sjis.exe を得る。  
- 「vscode\resources\app\node_modules.asar.unpacked\vscode-ripgrep\bin」フォルダにて、  
「rg.exe」→ 「rg_utf8.exe」  
と名前を変更。
- 「rg_sjis.exe」を「rg.exe」へと名前を変更して 同じディレクトリへと入れる。
　

