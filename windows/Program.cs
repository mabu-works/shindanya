using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shindanya
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }

    // Web版（shindanya）の診断ロジックをそのまま再現する。
    // シード = FNV-1a ハッシュ（名前|日付|テーマID）、乱数は mulberry32。
    // 同じ名前・同じ日・同じテーマなら Web 版と同じ結果になる。
    internal sealed class DiagnosisData
    {
        public sealed class ResultItem
        {
            public string Title = "";
            public string Desc = "";
        }

        public sealed class Theme
        {
            public string Id = "";
            public string Label = "";
            public string Question = "";
            public ResultItem[] Results = Array.Empty<ResultItem>();
        }

        private static ResultItem R(string title, string desc) => new ResultItem { Title = title, Desc = desc };

        public static readonly Theme[] Themes =
        {
            new Theme
            {
                Id = "weather", Label = "天気", Question = "あなたを天気に例えると",
                Results = new[]
                {
                    R("快晴", "雲ひとつない青空のような人。いるだけで周りの空気が明るくなります。\n今日はその明るさが誰かの一日を救うはず。笑顔を惜しまずに過ごしてみてください。"),
                    R("晴れのち笑顔", "基本は穏やかな晴れ模様、ときどき見せる笑顔が破壊力抜群の人。\n今日は小さな親切が大きく返ってくる予感。いつもの道で寄り道してみるのもおすすめです。"),
                    R("曇り", "落ち着いていて思慮深い、曇り空タイプ。派手さはないけれど、長く一緒にいると安心感がすごい人です。\n今日は焦らずじっくり考えることが正解。答えは急がなくて大丈夫です。"),
                    R("小雨", "しっとりと優しい小雨のような人。乾燥した心にそっと潤いを届けます。\n今日は誰かの話を聞く側に回ると良い日。あなたの相づちが相手の支えになります。"),
                    R("豪雨", "感情の振れ幅が大きい豪雨タイプ。やるときは一気に全力で、周りを巻き込むエネルギーがあります。\n今日は勢いに任せて進むのが吉。ただし言い過ぎには少しだけ注意を。"),
                    R("雷", "一瞬で場の空気を変える雷タイプ。ひらめきと瞬発力が武器で、退屈とは無縁の人です。\n今日は思いついたアイデアをすぐ口に出してみて。それが突破口になります。"),
                    R("雪", "静かで凛とした雪タイプ。物静かだけれど、積もれば景色を一変させる存在感があります。\n今日は一人の時間が力をくれる日。好きなことに没頭してみてください。"),
                    R("虹", "雨上がりに現れる虹のような人。つらい時期を乗り越えた先で、希望を見せるのが得意です。\n今日は誰かの「よかった」のきっかけになれる日。前向きな言葉を選んでみて。"),
                }
            },
            new Theme
            {
                Id = "color", Label = "色", Question = "あなたのイメージカラーは",
                Results = new[]
                {
                    R("情熱の赤", "行動力と熱量で周りを引っ張る赤。決めたら迷わず突き進む姿が人の心を動かします。\n今日は「やる」と宣言すると運気が上がる日。小さな一歩でも先に進みましょう。"),
                    R("誠実な青", "信頼と落ち着きの青。約束を守り、コツコツ積み上げる姿勢が評価されています。\n今日は整理整頓がツキを呼ぶ日。机の上と頭の中をすっきりさせてみて。"),
                    R("癒やしの緑", "自然体で人をほっとさせる緑。争いを好まず、場の調和を大切にする平和主義者です。\n今日は深呼吸が味方になる日。疲れたら外の空気を吸いに行きましょう。"),
                    R("元気な黄色", "好奇心と笑いの黄色。新しいものにすぐ飛びつくフットワークの軽さが魅力です。\n今日は気になったことを調べるだけでも収穫あり。メモを取るのがおすすめです。"),
                    R("神秘の紫", "感性豊かでミステリアスな紫。人と違う視点を持っていて、創作や企画で光ります。\n今日は直感を信じてよい日。理由は後からついてきます。"),
                    R("優しいピンク", "思いやりと柔らかさのピンク。相手の気持ちを先回りして考えられる人です。\n今日は「ありがとう」を多めに伝えてみて。関係が一段と温かくなります。"),
                    R("純粋な白", "まっさらで誠実な白。ごまかしが苦手で、正面から向き合う姿勢が信頼を集めます。\n今日は新しいことを始めるのに良い日。小さな習慣からどうぞ。"),
                    R("頼れる黒", "芯の強さと安定感の黒。多くを語らなくても、どっしりした存在感があります。\n今日は引き受けるより「見守る」が正解の場面も。余裕が魅力になります。"),
                }
            },
            new Theme
            {
                Id = "animal", Label = "動物", Question = "あなたを動物に例えると",
                Results = new[]
                {
                    R("猫", "マイペースで自由な猫タイプ。気分屋に見えて、実は周りをよく見ています。\n今日は無理に合わせなくて大丈夫。自分の心地よさを優先してみてください。"),
                    R("犬", "素直で人懐っこい犬タイプ。好きな人や物事には全力でまっすぐ向かっていきます。\n今日は素直な気持ちを伝えると良い日。遠慮はほどほどに。"),
                    R("狼", "一匹狼に見えて仲間思いな狼タイプ。信じた相手のためなら力を尽くします。\n今日はチームで動くと成果が出やすい日。役割分担がカギです。"),
                    R("狐", "機転が利く狐タイプ。状況を読むのが速く、ピンチをチャンスに変えるのが上手です。\n今日は先回りした準備が効く日。ひと手間かけてみましょう。"),
                    R("兎", "穏やかで警戒心も強い兎タイプ。慎重に様子を見てから、跳ぶときは大きく跳びます。\n今日は情報収集の日。決断は材料がそろってからで十分です。"),
                    R("熊", "どっしり構えた熊タイプ。普段は温厚、ここぞというときの集中力がすごい人です。\n今日は体力温存がテーマ。しっかり食べて、しっかり休みましょう。"),
                    R("鷹", "高い視点で全体を見渡す鷹タイプ。細部より大局を読むのが得意です。\n今日は俯瞰して考えることが突破口に。目先のことから一歩引いてみて。"),
                    R("亀", "着実に進む亀タイプ。速さでは負けても、続ける力では誰にも負けません。\n今日は昨日の続きを淡々と進めるのが吉。積み重ねは裏切りません。"),
                }
            },
            new Theme
            {
                Id = "station", Label = "駅", Question = "あなたを駅に例えると",
                Results = new[]
                {
                    R("高崎駅", "いろいろな路線が集まる結節点・高崎駅タイプ。人と人をつなぐ場所に自然といる人です。\n今日は誰かを紹介したり橋渡しをしたりすると、思わぬ広がりが生まれます。"),
                    R("大宮駅", "大きくて頼れるターミナル・大宮駅タイプ。たくさんの行き先を受け止める懐の深さがあります。\n今日は抱え込みすぎに注意。乗り換え案内のように、人に任せるのも大事です。"),
                    R("東京駅", "堂々とした中心・東京駅タイプ。責任ある立場でも物おじしない安定感があります。\n今日は堂々としていることが正解。迷いを見せない姿勢が信頼を呼びます。"),
                    R("新宿駅", "エネルギッシュで多面的な新宿駅タイプ。いろいろな顔を持ち、どこへでも行ける柔軟さがあります。\n今日は寄り道が収穫になる日。いつもと違う出口から出てみましょう。"),
                    R("池袋駅", "親しみやすくて活気のある池袋駅タイプ。サブカルから日常まで、守備範囲が広い人です。\n今日は好きなものの話で盛り上がれる日。趣味の共有が縁を広げます。"),
                    R("秋葉原駅", "好きを極める秋葉原駅タイプ。興味のある分野への没入感が周りを驚かせます。\n今日は専門知識が役に立つ場面がありそう。遠慮なく披露してみて。"),
                    R("横浜駅", "洗練されていて開放的な横浜駅タイプ。新しい風を取り入れるのが上手な人です。\n今日は身だしなみを少し整えると運気アップ。第一印象がものを言います。"),
                    R("上野駅", "人情と歴史のある上野駅タイプ。古いものも新しいものも大切にできるバランス感覚があります。\n今日は懐かしい連絡が吉報を運ぶかも。返信は早めがおすすめです。"),
                }
            },
            new Theme
            {
                Id = "food", Label = "食べ物", Question = "あなたを食べ物に例えると",
                Results = new[]
                {
                    R("ラーメン", "熱々で個性派なラーメンタイプ。こだわりが強く、自分の「これだ」という味を持っています。\n今日はこだわりを貫くことが評価される日。中途半端に妥協しないで。"),
                    R("寿司", "素材の良さで勝負する寿司タイプ。飾らなくても確かな実力がある、職人気質の人です。\n今日は丁寧な仕事が光る日。一つひとつ確実に仕上げていきましょう。"),
                    R("カレー", "スパイスの効いたカレータイプ。一見穏やかでも、内側に刺激と深みを秘めています。\n今日はいつもと違う選択が面白い展開を呼びます。辛さ増しでどうぞ。"),
                    R("パンケーキ", "ふわふわ甘いパンケーキタイプ。一緒にいる人をほっと幸せな気持ちにさせます。\n今日は自分へのご褒美も忘れずに。甘いものが元気をくれます。"),
                    R("焼肉", "豪快でエネルギッシュな焼肉タイプ。場を盛り上げるムードメーカーです。\n今日はみんなで集まる場面にツキあり。幹事役も悪くありません。"),
                    R("そば", "素朴で凛としたそばタイプ。無駄をそぎ落としたシンプルさに美学があります。\n今日は「減らす」ことが正解の日。持ち物も予定も軽くしてみましょう。"),
                    R("プリン", "つるんと可愛いプリンタイプ。柔らかそうに見えて、芯にはほろ苦いカラメルのような大人っぽさも。\n今日はギャップが魅力になる日。意外な一面を見せてみて。"),
                    R("おにぎり", "ほっとする定番・おにぎりタイプ。特別じゃなくても、なくてはならない存在です。\n今日は基本を大切に。当たり前のことを丁寧にやる人が一番強いです。"),
                }
            },
            new Theme
            {
                Id = "rpg", Label = "RPGジョブ", Question = "あなたのRPGジョブは",
                Results = new[]
                {
                    R("勇者", "困難から逃げない勇者タイプ。責任感が強く、みんなの期待を背負える人です。\n今日は先頭に立つ場面がありそう。迷ったら「やる」を選んでみてください。"),
                    R("魔法使い", "知識と発想で戦う魔法使いタイプ。頭の回転が速く、難しい問題ほど燃えます。\n今日は調べものや勉強にツキあり。新しい呪文（知識）を覚える日です。"),
                    R("戦士", "体力と根性の戦士タイプ。地道な努力と継続力で信頼を勝ち取る人です。\n今日は筋の通った行動が評価されます。正面突破が吉。"),
                    R("僧侶", "回復と支援の僧侶タイプ。周りの疲れにいち早く気づく、優しい人です。\n今日は「大丈夫？」の一言が誰かを救います。ただし自分の回復も忘れずに。"),
                    R("盗賊", "身軽で機敏な盗賊タイプ。チャンスを見つける嗅覚と、素早い判断が武器です。\n今日はスピード勝負の場面にツキあり。先手必勝で動きましょう。"),
                    R("賢者", "経験と知恵の賢者タイプ。物事の本質を見抜き、的確な助言ができる人です。\n今日は相談されることが増えそう。答えより「考え方」を渡すと喜ばれます。"),
                    R("踊り子", "場を華やかにする踊り子タイプ。表現力と魅力で人を惹きつけます。\n今日は見せる・聞かせる場面にツキあり。少し大げさなくらいが丁度いいです。"),
                    R("商人", "現実感覚に優れた商人タイプ。損得を見極め、資源をうまく回すのが得意です。\n今日はお金や時間の使い方を見直すと収穫あり。無駄の発見が利益になります。"),
                }
            },
        };

        // FNV-1a（Web版 hashString と同じ）
        public static uint HashString(string str)
        {
            uint h = 0x811c9dc5;
            foreach (char c in str)
            {
                h ^= c;
                h = unchecked(h * 0x01000193);
            }
            return h;
        }

        // mulberry32（Web版と同じ決定的乱数）
        public sealed class Mulberry32
        {
            private uint a;
            public Mulberry32(uint seed) { a = seed; }
            public double Next()
            {
                unchecked
                {
                    a = a + 0x6d2b79f5;
                    uint t = (a ^ (a >> 15)) * (1u | a);
                    t = (t + ((t ^ (t >> 7)) * (61u | t))) ^ t;
                    return (t ^ (t >> 14)) / 4294967296.0;
                }
            }
        }
    }

    internal sealed class MainForm : Form
    {
        private static readonly Color Bg = Color.FromArgb(10, 15, 28);
        private static readonly Color Surface = Color.FromArgb(17, 26, 44);   // --panel #111a2c
        private static readonly Color Surface2 = Color.FromArgb(24, 38, 66);  // --panel-2 #182642
        private static readonly Color TextColor = Color.FromArgb(236, 241, 248);
        private static readonly Color Muted = Color.FromArgb(157, 177, 199);
        private static readonly Color Accent = Color.FromArgb(106, 141, 255); // --accent #6a8dff
        private static readonly Color Accent2 = Color.FromArgb(45, 212, 191); // --accent-2 #2dd4bf
        private static readonly Color ErrorColor = Color.FromArgb(255, 107, 107);

        private readonly TextBox nameBox;
        private readonly ComboBox themeBox;
        private readonly Label errorLabel;
        private readonly Panel resultPanel;
        private readonly Label resultThemeLabel;
        private readonly Label resultTitleLabel;
        private readonly Label resultForLabel;
        private readonly Label resultDescLabel;
        private readonly Button copyBtn;

        private string currentShareText = "";

        public MainForm()
        {
            Text = "診断メーカー for Windows";
            ClientSize = new Size(540, 640);
            MinimumSize = new Size(556, 679);
            BackColor = Bg;
            ForeColor = TextColor;
            Font = new Font("Yu Gothic UI", 10F);

            var title = new Label
            {
                Text = "診断メーカー",
                Font = new Font("Yu Gothic UI", 16F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(20, 14),
                AutoSize = true
            };
            Controls.Add(title);

            var tagline = new Label
            {
                Text = "名前を入れると、今日のあなたを診断します。同じ名前・同じ日なら、何度やっても同じ結果です。",
                ForeColor = Muted,
                Location = new Point(20, 50),
                Size = new Size(500, 36)
            };
            Controls.Add(tagline);

            var nameLabel = new Label
            {
                Text = "名前",
                ForeColor = Muted,
                Location = new Point(20, 96),
                AutoSize = true
            };
            Controls.Add(nameLabel);

            nameBox = new TextBox
            {
                Location = new Point(20, 120),
                Width = 500,
                MaxLength = 20,
                BackColor = Surface2,
                ForeColor = TextColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            nameBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { Diagnose(); e.SuppressKeyPress = true; }
            };
            Controls.Add(nameBox);

            var themeLabel = new Label
            {
                Text = "テーマ",
                ForeColor = Muted,
                Location = new Point(20, 158),
                AutoSize = true
            };
            Controls.Add(themeLabel);

            themeBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(20, 182),
                Width = 240,
                BackColor = Surface2,
                ForeColor = TextColor
            };
            foreach (var t in DiagnosisData.Themes) themeBox.Items.Add(t.Label);
            themeBox.SelectedIndex = 0;
            Controls.Add(themeBox);

            var diagnoseBtn = new Button
            {
                Text = "診断する",
                Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold),
                BackColor = Accent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 224),
                Size = new Size(500, 40)
            };
            diagnoseBtn.FlatAppearance.BorderSize = 0;
            diagnoseBtn.Click += (s, e) => Diagnose();
            Controls.Add(diagnoseBtn);

            errorLabel = new Label
            {
                Text = "名前を入力してください。",
                ForeColor = ErrorColor,
                Location = new Point(20, 272),
                AutoSize = true,
                Visible = false
            };
            Controls.Add(errorLabel);

            resultPanel = new Panel
            {
                BackColor = Surface,
                Location = new Point(20, 300),
                Size = new Size(500, 272),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                Visible = false
            };
            Controls.Add(resultPanel);

            resultThemeLabel = new Label
            {
                ForeColor = Muted,
                Location = new Point(16, 14),
                Size = new Size(468, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };
            resultPanel.Controls.Add(resultThemeLabel);

            resultTitleLabel = new Label
            {
                Font = new Font("Yu Gothic UI", 22F, FontStyle.Bold),
                ForeColor = Accent2,
                Location = new Point(16, 40),
                Size = new Size(468, 44),
                TextAlign = ContentAlignment.MiddleCenter
            };
            resultPanel.Controls.Add(resultTitleLabel);

            resultForLabel = new Label
            {
                ForeColor = Muted,
                Location = new Point(16, 88),
                Size = new Size(468, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };
            resultPanel.Controls.Add(resultForLabel);

            resultDescLabel = new Label
            {
                ForeColor = TextColor,
                Location = new Point(16, 122),
                Size = new Size(468, 140)
            };
            resultPanel.Controls.Add(resultDescLabel);

            copyBtn = new Button
            {
                Text = "結果をコピー",
                BackColor = Surface2,
                ForeColor = TextColor,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 584),
                Size = new Size(160, 34),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Visible = false
            };
            copyBtn.Click += (s, e) =>
            {
                if (currentShareText.Length == 0) return;
                Clipboard.SetText(currentShareText);
                copyBtn.Text = "コピーしました";
            };
            Controls.Add(copyBtn);
        }

        private void Diagnose()
        {
            string name = nameBox.Text.Trim();
            if (name.Length == 0)
            {
                errorLabel.Visible = true;
                resultPanel.Visible = false;
                copyBtn.Visible = false;
                return;
            }
            errorLabel.Visible = false;

            var theme = DiagnosisData.Themes[themeBox.SelectedIndex];
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            uint seed = DiagnosisData.HashString($"{name}|{today}|{theme.Id}");
            var rand = new DiagnosisData.Mulberry32(seed);
            int idx = (int)(rand.Next() * theme.Results.Length);
            var result = theme.Results[idx];

            resultThemeLabel.Text = theme.Question;
            resultTitleLabel.Text = result.Title;
            resultForLabel.Text = $"{name} さんの診断結果（{today}）";
            resultDescLabel.Text = result.Desc;
            resultPanel.Visible = true;

            currentShareText = $"{name}さんを{theme.Label}に例えると「{result.Title}」でした！ #診断メーカー";
            copyBtn.Text = "結果をコピー";
            copyBtn.Visible = true;
        }
    }
}
