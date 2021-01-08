# Visual Studio Code の Grep で SJIS も ヒットするように
- vscode で grep すると、utf8とsjisが混じったファイル群だと、sjis が検知できない。
そこで検知できるようにしたもの。  


# 動作環境
1. Windows系。Win7以降くらいじゃないかな。多分。
2. .NET Framework 4.5 以上

# 使い方
1. rg_sjis.zip を ダウンロードして、解凍し、rg_sjis.exe を得る。  
1. Visual Studio Code の  インストールフォルダから辿って  
「vscode\resources\app\node_modules.asar.unpacked\vscode-ripgrep\bin」 フォルダにある  
「rg.exe」→ 「rg_utf8.exe」と名前を変更。
1. 「rg_sjis.exe」を「rg.exe」へと名前を変更して 同じディレクトリへと入れる。

# Visual Studio Code のバージョンを更新すると...
- 更新する度に、「rg.exe」が上書きされてしまうため、同じことをする必要あり。

# 備考
- Visual Studio Code にて「ファイル」→「ユーザー設定」→「設定」で、  
検索欄に「guess」と入れて「Auto Guess Encoding」に「チェック」を入れることを推奨。  
推奨理由としては、grep 検索結果から「間違えたエンコード」で該当のファイルへとジャンプした場合、Visual Studio Code は  
「対象のファイルは最新状態だと検索対象の文字列は存在しない」と判断して候補から消してしまうため。
