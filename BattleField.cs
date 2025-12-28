using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace OOP_RPG
{
    internal partial class FrmBattleField : Form, ICommand
    {
        private Callback _callback = static (string a, string t) => { };

        private readonly string[] _actions = [.. CommandList.Commands.Keys];
        //コマンド選択以外のリストボックスを選択できないようにtarFlgで制御
        public bool tarFlg { get; private set; } = false;

        public FrmBattleField()
        {
            InitializeComponent();
            //EnemyMotion enemy = new EnemyMotion();

            this.Timer.Start();
        }

        public void Timing_Tick(object sender, EventArgs e)
        {
            GameMaster.Run();
        }

        //コマンドリスト生成
        public void ActivateCommand(ICallback callbackObj)
        {
            this.Timer.Stop();
            lstTarget.Items.Clear();
            tarFlg = false;

            _callback = callbackObj.Callback;
            //引数をCharacterとして変数に代入
            _currentChar = (Character)callbackObj;


            lblName.Text = ((Character)callbackObj).Name;

            foreach (var action in _actions)
            {
                lstAction.Items.Add(action);
            }
        }

        private readonly string[] _spells = [.. CommandList.Spells.Keys];   //呪文名の配列を作成

        //アクションコマンド作成
        private void ActivateActionCommand()
        {
            tarFlg = false;
            lstAction.Items.Clear();
            lstTarget.Items.Clear();
            //アクションからコマンドリストを作成
            foreach (var action in _actions)
            {
                lstAction.Items.Add(action);
            }

        }
        //呪文コマンド作成
        private void ActivateSpellCommand()
        {
            tarFlg = false;
            lstAction.Items.Clear();
            lstTarget.Items.Clear();
            //呪文からコマンドリストを作成
            foreach (var spell in _spells)
            {
                lstAction.Items.Add(spell);
            }

        }

        //ActivateCommandを起動したHeroのインスタンスを保存する用、ActivateCommandで代入
        private Character? _currentChar;

        private void lstAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            var nextTarget = (string)lstAction.SelectedItem!;
            if (nextTarget == null) return;

            //呪文が選択された場合、呪文をコマンドリストに表示
            if (nextTarget == "呪文")
            {
                ActivateSpellCommand();
                return;
            }
            //戻るが選択された場合、通常のコマンドリストを表示
            if (nextTarget == "戻る")
            {
                ActivateActionCommand();
                return;
            }

            //BattleAction commandTarget = CommandList.Spells[nextTarget];
            BattleAction? commandTarget= null;

            //選択されたコマンドによってターゲットの参照先を決定
            if (_actions.Contains(nextTarget)) commandTarget = CommandList.Commands[nextTarget];
            if (_spells.Contains(nextTarget)) commandTarget = CommandList.Spells[nextTarget];


            bool AbleToMagic = false;   //呪文が使用可能か
            if ((commandTarget?.Kind == "Magic") && _currentChar != null && _currentChar.Mp >= commandTarget.UseMp)
            {
                AbleToMagic = true;
            }
            //else
            //{
            //    AbleToMagic = false;    //いらなそう
            //}

                //選択されたのが呪文以外場合
                if (commandTarget?.Kind != "Magic")
                {
                    tarFlg = true;
                    lstTarget.Items.Clear();

                    //Command c = (Command)CommandList.Commands[nextTarget];

                    switch (commandTarget?.Target)
                    {
                        case "味方":
                            foreach (var tar in CharacterList.Heroes)
                            {
                                if (tar.Value.Hp > 0 && tar.Value != _currentChar)
                                {
                                    lstTarget.Items.Add(tar.Key);
                                }
                            }
                            break;

                        case "敵方":
                            foreach (var tar in CharacterList.Enemies)
                            {
                                if (tar.Value.Hp > 0)
                                {
                                    lstTarget.Items.Add(tar.Key);
                                }
                            }
                            break;

                        case "自分":
                            lstTarget.Items.Add(lblName.Text);
                            break;
                    }
                }
                //選択されたのが呪文の場合
                else if (AbleToMagic)
                {
                    tarFlg = true;
                    lstTarget.Items.Clear();

                    //Command c = (Command)CommandList.Commands[nextTarget];

                    switch (commandTarget?.Target)
                    {
                        case "味方":
                            foreach (var tar in CharacterList.Heroes)
                            {
                                if (tar.Value.Hp > 0)
                                {
                                    lstTarget.Items.Add(tar.Key);
                                }
                            }
                            break;

                        case "敵方":
                            foreach (var tar in CharacterList.Enemies)
                            {
                                if (tar.Value.Hp > 0)
                                {
                                    lstTarget.Items.Add(tar.Key);
                                }
                            }
                            break;

                        case "自分":
                            lstTarget.Items.Add(lblName.Text);
                            break;
                    }
                }
                else
                {
                    TraceLog.Write($"{nextTarget}を使用するにはMPが足りない！");
                    lstTarget.Items.Clear();
                    AbleToMagic = false;
                }
            }

        private void lstTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tar = (string)lstTarget.SelectedItem!;
            if (tar == null) return;

            if (tarFlg)
            {
                string act = (string)lstAction.SelectedItem!;
                //if (_spells.Contains(act)) act = "呪文";  //呪文選択時は呪文で返す

                _callback(act, tar);

                _callback = static (string a, string t) => { }; //クリア

                lstAction.Items.Clear();
                lstTarget.Items.Clear();

                this.Timer.Start();
            }

        }


        public void MsgLog(string text)
        {
            string sep = txtTraceLog.Text.Length == 0 ? "" : "\r\n";
            txtTraceLog.AppendText(sep + text);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (Timer.Enabled)
            {
                Timer.Stop();
            }
            else
            {
                Timer.Start();
            }
        }

        private void btnLogClear_Click(object sender, EventArgs e)
        {
            txtTraceLog.Clear();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //HeroステータスUI表示
        private int heroCount = 0;
        public void HeroLabel(string name, uint hp, uint maxHp, uint mp, uint maxMp)
        {
            //Hero用の名前とステータスラベルを生成
            Label heroname = new Label();
            Label heroStatus = new Label();
            int x = heroCount * 200　+ 25;
            heroCount++;

            // 名前ラベルのプロパティ設定
            heroname.Text = $"{name}";
            heroname.Location = new Point(x, 20); // 位置
            heroname.AutoSize = true; // テキストに合わせてサイズ調整
            heroname.Font = new Font("Meiryo", 16, FontStyle.Bold); //フォントの調整
            heroname.ForeColor = SystemColors.HighlightText;

            // ステータスラベルのプロパティ設定
            heroStatus.Text = $"HP：{hp}/{maxHp}\r\n" +
                            　$"MP：{mp}/{maxMp}";
            heroStatus.Location = new Point(x, 70); // 位置
            heroStatus.AutoSize = true; // テキストに合わせてサイズ調整
            heroStatus.Font = new Font("Meiryo", 16, FontStyle.Bold); //フォントの調整
            heroStatus.ForeColor = SystemColors.HighlightText;

            // フォームに追加
            this.panelHero.Controls.Add(heroname);
            this.panelHero.Controls.Add(heroStatus);

            _heroLabels[name] = (heroname, heroStatus);
        }

        private readonly Dictionary<string, (Label nameLabel, Label statusLabel)> _heroLabels = new();

        //HP増減時、ステータスラベル更新
        public void UpdateHeroStatus(string name, uint hp, uint maxHp, uint mp, uint maxMp)
        {
                if (_heroLabels.TryGetValue(name, out var labels))
            {
                labels.statusLabel.Text = $"HP：{hp}/{maxHp}\r\n" +
                                          $"MP：{mp}/{maxMp}";
                //HP状況によって色変更
                if (hp == 0)
                {
                    labels.nameLabel.ForeColor = Color.Red;
                    labels.statusLabel.ForeColor = Color.Red;
                }
                else if (100 * hp / maxHp < 50)
                {
                    labels.nameLabel.ForeColor = Color.Yellow;
                    labels.statusLabel.ForeColor = Color.Yellow;
                }
                else
                {
                    labels.nameLabel.ForeColor = SystemColors.HighlightText;
                    labels.statusLabel.ForeColor = SystemColors.HighlightText;
                }
            }
        }
        //敵画像の配列
        //private readonly List<PictureBox> _enemyBoxes = new();
        //private readonly Dictionary<string, PictureBox> _enemyPictureBoxes = new();
        private readonly Dictionary<Enemy, PictureBox> _enemyPBs = new();
        private readonly List<PictureBox> _effectPBs = new List<PictureBox>();


        //敵画像生成
        public void CreatePB(string[] files)    //filesは画像の相対パスが格納されているもの
        {
            // 既存 PictureBox を破棄
            foreach (var pb in _enemyPBs.Values)
            {
                pb.Image?.Dispose();//Disposeは破棄、多分
                pb.Dispose();
            }
            _enemyPBs.Clear();
            panel1.Controls.Clear();

            //大きさ調整
            int margin = (int)(10 * currentDpiScale);
            int panelWidth = panel1.ClientSize.Width;
            int panelHeight = panel1.ClientSize.Height;
            int maxH = (int)(400 * currentDpiScale);

            var enemies = CharacterList.Enemies.Values.ToList();

            int count = Math.Min(files.Length, enemies.Count);//Math.Minは比較して小さい方選ぶ、多分

            for (int i = 0; i < count; i++)
            {
                Image img;
                try
                {
                    using var fs = new FileStream(files[i], FileMode.Open, FileAccess.Read);    //filesは画像の相対パスが格納されているもの
                    img = Image.FromStream(fs);
                }
                catch
                {
                    continue;
                }

                int maxW = Math.Min((int)(400 * currentDpiScale),
                    (panelWidth - (count + 1) * margin) / count);

                double scale = Math.Min((double)maxW / img.Width, (double)maxH / img.Height);
                int newW = (int)(img.Width * scale);
                int newH = (int)(img.Height * scale);

                PictureBox pb = new PictureBox
                {
                    Image = img,
                    Size = new Size(newW, newH),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(
                        margin + i * (newW + margin),
                        (panelHeight - newH) / 2
                    )
                };

                panel1.Controls.Add(pb);
                _enemyPBs[enemies[i]] = pb;
            }
        }
        //public void HideEnemy(Enemy enemy)
        //{
        //    if (_enemyPBs.TryGetValue(enemy, out var pb))
        //        pb.Visible = false;
        //}

        //public void ShowEnemy(Enemy enemy)
        //{
        //    if (_enemyPBs.TryGetValue(enemy, out var pb))
        //        pb.Visible = true;
        //}

        // 現在の DPI スケール
        private float currentDpiScale = 1.0f;
        private string[]? currentFiles; // CreatePB に渡す画像リストを保持

        private void Form1_Load(object sender, EventArgs e)
        {
            using (Graphics g = this.CreateGraphics())
            {
                currentDpiScale = g.DpiX / 96.0f; // 96 DPI を基準に倍率を計算
            }

            // 例：敵画像をロードして表示
            currentFiles = CharacterList.GetEnemyImages().ToArray();
            CreatePB(currentFiles);
        }

        // DPI が変更されたときに再描画
        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);

            currentDpiScale = e.DeviceDpiNew / 96.0f;
            if (currentFiles != null)
            {
                CreatePB(currentFiles); // DPI に合わせて再描画
            }
        }

        //敵死亡時画像を非表示に
        public void UpdateEnemyStatus(Enemy enemy)
        {
            if (_enemyPBs.TryGetValue(enemy, out var pb))
            {
                pb.Visible = enemy.Hp > 0; // HPが0なら非表示、1以上なら表示
            }
        }
        //攻撃時のモーション再生
        public void AttackMotion(Enemy target, string? attackGif)
        {
            if (attackGif == null) return;  //gifが存在しなければ処理終了
            if (!_enemyPBs.TryGetValue(target, out var basePb)) return;

            // 既に同じ攻撃エフェクトがある場合は削除してから追加
            var existing = _effectPBs.FirstOrDefault(pb =>
                pb.ImageLocation == attackGif &&
                pb.Location == basePb.Location
            );
            if (existing != null)
            {
                panel1.Controls.Remove(existing);
                _effectPBs.Remove(existing);
                existing.Dispose();
            }

            // PictureBox を生成して攻撃エフェクト表示、敵画像の位置を取得してそこに表示
            PictureBox effect = new PictureBox
            {
                ImageLocation = attackGif,
                Size = basePb.Size,
                Location = basePb.Location,
                SizeMode = PictureBoxSizeMode.Zoom,
                Enabled = true,
                BackColor = Color.Transparent
            };

            panel1.Controls.Add(effect);
            effect.BringToFront();
            _effectPBs.Add(effect);

            // 自動削除タイマー
            var t = new System.Windows.Forms.Timer();
            t.Interval = GetGifDuration(attackGif); // GIFの再生時間をミリ秒で取得
            t.Tick += (s, e) =>
            {
                t.Stop();
                panel1.Controls.Remove(effect);
                _effectPBs.Remove(effect);
                effect.Dispose();
                t.Dispose();
            };
            t.Start();
        }

        //gif再生時間取得
        private int GetGifDuration(string gifPath)
        {
            Image img = Image.FromFile(gifPath);
            int duration = 0;
            foreach (var prop in img.PropertyItems)
            {
                // 0x5100 は GIF のフレームごとのディレイ
                if (prop.Id == 0x5100)
                {
                    byte[]? values = prop.Value;
                    if (values != null)
                    {
                        for (int i = 0; i < values.Length; i += 4)
                        {
                            int frameDelay = BitConverter.ToInt32(values, i) * 10; // 単位は ms
                            duration += frameDelay;
                        }
                    }
                }
            }
            img.Dispose();
            return Math.Max(duration, 500); // 最低0.5秒
        }

    }
}