[![rg_sjis v2.3.0.1](https://img.shields.io/badge/rg_sjis-v2.3.0-6479ff.svg)](https://github.com/komiyamma/vscode_ripgrep_sjis/releases)
[![MIT License](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)](LICENSE)

# Visual Studio Code の Grep で SJIS も ヒットするように
- Visual Studio Code で grep すると、utf8とsjisが混じったファイル群だと、sjis が検知できない。
そこで検知できるようにしたもの。  
- なお、Visual Studio CodeのExtentionとして機能するものは「1. [vscode_ripgrep_sjis_extension リポジトリ](https://github.com/komiyamma/vscode_ripgrep_sjis_extension/releases) 」にて「rg-sjis-***.vsix」として提供しています。

# 動作環境
1. Windows系。Win7以降くらいじゃないかな。多分。
2. .NET Framework 4.5.2 以上

# 使い方
- **自動**で入れる場合
1. [rg_sjis.zip](https://github.com/komiyamma/vscode_ripgrep_sjis/releases) を ダウンロードして、解凍し、rg_sjis.exe を得る。  
1. 「rg_sjis.exe」をダブルクリックして実行

- **手作業**で入れる場合
1. [rg_sjis.zip](https://github.com/komiyamma/vscode_ripgrep_sjis/releases) を ダウンロードして、解凍し、rg_sjis.exe を得る。  
1. Visual Studio Code の  インストールフォルダから辿って  
「vscode\resources\app\node_modules.asar.unpacked\vscode-ripgrep\bin」 フォルダにある  
「rg.exe」→ 「rg_utf8.exe」と名前を変更。
1. 「rg_sjis.exe」を「rg.exe」へと名前を変更して 同じディレクトリへと入れる。

# 備考
- Visual Studio Code にて「ファイル」→「ユーザー設定」→「設定」で、  
検索欄に「guess」と入れて「Auto Guess Encoding」に「チェック」を入れることを推奨。  
推奨理由としては、grep 検索結果から「間違えたエンコード」で該当のファイルへとジャンプした場合、Visual Studio Code は  
「対象のファイルは最新状態だと検索対象の文字列は存在しない」と判断して候補から消してしまうため。

# Visual Studio Code のバージョンを更新すると...
- 更新する度に、「rg.exe」が上書きされてしまうため、同じことをする必要あり。

