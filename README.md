<!DOCTYPE html>
<html lang="ja">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Shader Replacer 取扱説明書</title>
    <style>
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }
        body {
            font-family: 'Helvetica Neue', Arial, 'Hiragino Kaku Gothic ProN', 'Hiragino Sans', Meiryo, sans-serif;
            background-color: #f0f4f8;
            color: #1e293b;
            line-height: 1.7;
            padding: 40px 20px;
        }
        .container {
            max-width: 800px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
            overflow: hidden;
        }
        /* ヘッダー */
        .header {
            background: linear-gradient(135deg, #1e3a8a, #0369a1);
            color: #ffffff;
            padding: 40px;
            text-align: center;
        }
        .header h1 {
            font-size: 28px;
            font-weight: 700;
            margin-bottom: 10px;
            letter-spacing: 0.5px;
        }
        .header p {
            font-size: 15px;
            color: #e0f2fe;
            max-width: 600px;
            margin: 0 auto 15px auto;
        }
        .meta-info {
            font-size: 13px;
            color: #bae6fd;
            display: block;
        }
        .meta-info a {
            color: #bae6fd;
            text-decoration: none;
            border-bottom: 1px dashed #bae6fd;
        }
        .meta-info a:hover {
            color: #ffffff;
            border-bottom-style: solid;
        }

        /* コンテンツエリア */
        .content {
            padding: 40px;
        }

        /* 見出し */
        h2 {
            font-size: 20px;
            color: #1e3a8a;
            padding: 10px 16px;
            background-color: #f0f7ff;
            border-left: 5px solid #2563eb;
            border-radius: 0 6px 6px 0;
            margin-top: 40px;
            margin-bottom: 20px;
        }
        h2:first-of-type {
            margin-top: 0;
        }

        /* 段落 */
        p {
            margin-bottom: 16px;
            font-size: 15px;
        }

        /* 画像コンテナ */
        .tool-image-container {
            text-align: center;
            margin: 25px 0 35px 0;
            background-color: #f8fafc;
            border: 1px solid #e2e8f0;
            border-radius: 10px;
            padding: 15px;
        }
        .tool-image {
            max-width: 323px; /* 元の幅に合わせる */
            width: 100%;
            height: auto;
            border-radius: 6px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }
        .tool-image-caption {
            font-size: 13.5px;
            color: #64748b;
            margin-top: 10px;
            font-style: italic;
        }

        /* ステップリスト (Flexを使わないブロック配置) */
        .step-list {
            margin-bottom: 25px;
        }
        .step-item {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 8px;
            padding: 24px;
            margin-bottom: 16px;
            position: relative;
        }
        .step-number {
            display: inline-block;
            background-color: #2563eb;
            color: #ffffff;
            font-weight: bold;
            font-size: 12px;
            padding: 2px 10px;
            border-radius: 20px;
            margin-bottom: 12px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        .step-title {
            font-size: 16px;
            font-weight: bold;
            color: #0f172a;
            margin-bottom: 8px;
        }
        .step-text {
            font-size: 14.5px;
            color: #475569;
        }
        
        /* 注意書き・コールアウト */
        .note-box {
            background-color: #fffbeb;
            border-left: 4px solid #d97706;
            color: #78350f;
            padding: 16px;
            border-radius: 0 8px 8px 0;
            font-size: 14px;
            margin-top: 12px;
        }

        /* 特徴・機能セクション */
        .feature-container {
            margin-top: 15px;
        }
        .feature-card {
            background-color: #f8fafc;
            border: 1px solid #e2e8f0;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 16px;
        }
        .feature-title {
            font-size: 16px;
            font-weight: bold;
            color: #1e3a8a;
            margin-bottom: 6px;
            display: table;
        }
        .feature-title::before {
            content: "✦";
            color: #3b82f6;
            margin-right: 8px;
            display: table-cell;
        }
        .feature-desc {
            font-size: 14px;
            color: #475569;
            padding-left: 18px;
        }

        /* 強調表現 */
        .highlight {
            font-weight: bold;
            color: #0f172a;
            background: linear-gradient(transparent 60%, #dbeafe 40%);
            padding: 0 2px;
        }
        .badge {
            background-color: #e0f2fe;
            color: #0369a1;
            padding: 2px 6px;
            border-radius: 4px;
            font-size: 13px;
            font-weight: bold;
            font-family: monospace;
        }
        /* キーボードショートカット */
        kbd {
            background: #e2e8f0;
            padding: 1px 5px;
            border-radius: 4px;
            font-size: 13px;
            border-bottom: 1px solid #cbd5e1;
        }
    </style>
</head>
<body>

    <div class="container">
        <!-- ヘッダー -->
        <div class="header">
            <h1>Shader Replacer</h1>
            <p>指定したフォルダ内にある全てのマテリアルを検索し、割り当てられているシェーダーを別のシェーダーへ一括で置換・管理するUnityエディタ拡張ツールです。</p>
            <span class="meta-info">Developer: <a href="https://github.com/MintakaSeiran" target="_blank">MintakaSeiran</a></span>
        </div>

        <!-- コンテンツ -->
        <div class="content">
            
            <h2>1. ツールの起動方法</h2>
            <p>Unityエディタを起動し、上部メニューバーにある <span class="highlight">Tools ＞ Shader Replacer</span> をクリックします。クリックすると、操作用の専用ウィンドウが立ち上がります。</p>

            <div class="tool-image-container">
                <img src="https://github.com/user-attachments/assets/8faa6dfb-7d06-4cd4-97a8-a7d7fb22108f" alt="Unityプロジェクト内でのパッケージ構成と配置場所" class="tool-image">
                <div class="tool-image-caption">パッケージの構成と配置場所（Projectビュー）</div>
            </div>

            <h2>2. 操作手順</h2>
            <div class="step-list">
                <!-- STEP 1 -->
                <div class="step-item">
                    <div class="step-number">Step 1</div>
                    <div class="step-title">変更元・変更先シェーダーの指定</div>
                    <div class="step-text">
                        ウィンドウの「シェーダー置換設定」欄にて、以下の2つのシェーダーを設定します。
                        <p style="margin-top: 8px; margin-bottom: 0; font-size: 14px; color: #64748b;">
                            ・<span class="highlight">変更元のシェーダー</span>: 現在マテリアルに適用されている、置き換えたいシェーダー<br>
                            ・<span class="highlight">変更先のシェーダー</span>: 新しく適用したいターゲットシェーダー
                        </p>
                        <p style="margin-top: 6px; font-size: 13.5px; color: #64748b;">※プロジェクトウィンドウから直接ドラッグ＆ドロップするか、右側の丸いアイコンから選択可能です。</p>
                    </div>
                </div>

                <!-- STEP 2 -->
                <div class="step-item">
                    <div class="step-number">Step 2</div>
                    <div class="step-title">対象フォルダの選択</div>
                    <div class="step-text">
                        置換を行いたいマテリアルが保存されているフォルダを指定します。
                        <br><span class="badge">参照...</span> ボタンをクリックするとフォルダ選択ダイアログが開きます。
                        <div class="note-box">
                            <strong>【注意】</strong><br>
                            必ずプロジェクト内（<span class="badge">Assets</span> フォルダ以下）のフォルダを選択してください。プロジェクト外のフォルダを指定した場合はエラーダイアログが表示されます。
                        </div>
                    </div>
                </div>

                <!-- STEP 3 -->
                <div class="step-item">
                    <div class="step-number">Step 3</div>
                    <div class="step-title">置換の実行</div>
                    <div class="step-text">
                        設定が完了したら、下部にある水色の <span class="highlight">「指定フォルダ内のシェーダーを置換する」</span> ボタンをクリックします。
                        処理が完了すると、変更されたマテリアル数がポップアップで通知されます。
                    </div>
                </div>
            </div>

            <h2>3. 便利な標準機能</h2>
            <div class="feature-container">
                <!-- 機能 1 -->
                <div class="feature-card">
                    <div class="feature-title">プログレスバー & キャンセル機能</div>
                    <div class="feature-desc">大量のマテリアルが含まれる巨大なフォルダを指定した場合でも、処理の進捗がプログレスバーでリアルタイムに表示されます。Unityがフリーズするのを防ぐほか、途中で処理を安全に中断することも可能です。</div>
                </div>

                <!-- 機能 2 -->
                <div class="feature-card">
                    <div class="feature-title">安心の Undo（元に戻す）対応</div>
                    <div class="feature-desc">万が一、誤ったフォルダやシェーダーを指定して一括置換を行ってしまった場合でも、キーボードの <kbd>Ctrl + Z</kbd>（Macは <kbd>Cmd + Z</kbd>）を押すことで、瞬時に置換前の状態に復元することができます。</div>
                </div>
            </div>

        </div>
    </div>

</body>
</html>
